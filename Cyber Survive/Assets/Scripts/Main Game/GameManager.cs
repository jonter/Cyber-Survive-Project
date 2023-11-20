using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject waitPanel;
    [SerializeField] GameObject playerPrefab;

    public PhotonView view;

    GameManager[] clients;

    bool isGameOn = false;

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void OnDestroy()
    {
        if (view.IsMine == true) SceneManager.LoadScene(0);
    }


    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) Destroy(canvas);
        if(PhotonNetwork.IsMasterClient) StartCoroutine(CheckPlayers());
    }

    IEnumerator CheckPlayers()
    {
        yield return new WaitForSeconds(1);
        if (isGameOn == true) yield break;
        clients = FindObjectsOfType<GameManager>();
        string info = "Игроки в сессии: \n";
        for (int i = 0; i < clients.Length; i++)
        {
            info += (i+1) +". " + clients[i].view.Owner.NickName + "\n";
        }

        for (int i = 0; i < clients.Length; i++)
        {
            PhotonView client = clients[i].view;
            client.RPC("DisplayInfoForAll", client.Owner, info);
        }
        StartCoroutine(CheckPlayers());
    }

    [PunRPC]
    void DisplayInfoForAll(string info)
    {
        waitPanel.GetComponent<WaitPanel>().DisplayInfo(info);
    }

    public void OnPlayButton()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (isGameOn == true) return;
        isGameOn = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        FindObjectOfType<EnemySpawner>().StartSpawning();
        for (int i = 0; i < clients.Length; i++)
        {
            PhotonView client = clients[i].GetComponent<PhotonView>();
            client.RPC("StartGame", client.Owner);
        }
    }


    [PunRPC]
    void StartGame()
    {
        Vector3 randVec = Random.insideUnitSphere * 2;
        randVec.y = 1;
        PhotonNetwork.Instantiate(playerPrefab.name, randVec, Quaternion.identity);
        waitPanel.SetActive(false);
    }




    
}
