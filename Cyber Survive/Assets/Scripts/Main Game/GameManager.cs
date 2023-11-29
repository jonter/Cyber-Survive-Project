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
    [SerializeField] ScoreTab scoreTab;
    [SerializeField] WaveTab waveTab;

    [HideInInspector] public PhotonView view;
    [HideInInspector] public PlayerHealth charater;
    [HideInInspector] public int scores = 0;

    GameManager[] clients;

    bool isGameOn = false;

    public static GameManager master;

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
        waitPanel.gameObject.SetActive(true);
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) Destroy(canvas);
        if(PhotonNetwork.IsMasterClient) StartCoroutine(CheckPlayers());
        SetupMasterClient();
    }

    public void DisplayWave(int wave)
    {
        for (int i = 0; i < clients.Length; i++)
        {
            clients[i].view.RPC("DisplayWaveRPC", clients[i].view.Owner, wave);
        }

    }

    [PunRPC]
    void DisplayWaveRPC(int wave)
    {
        waveTab.gameObject.SetActive(true);
        waveTab.SetText("�����: "+ wave);
    }

    void SetupMasterClient()
    {
        clients = FindObjectsOfType<GameManager>();
        for (int i = 0; i < clients.Length; i++)
        {
            if (clients[i].view.Owner.IsMasterClient == true) master = clients[i];
        }

    }

    IEnumerator CheckPlayers()
    {
        yield return new WaitForSeconds(1);
        if (isGameOn == true) yield break;
        clients = FindObjectsOfType<GameManager>();
        string info = "������ � ������: \n";
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
        ShowScores();
    }


    [PunRPC]
    void StartGame()
    {
        Vector3 randVec = Random.insideUnitSphere * 2;
        randVec.y = 1;
        GameObject plr = PhotonNetwork.Instantiate(playerPrefab.name, randVec, Quaternion.identity);
        charater = plr.GetComponent<PlayerHealth>();
        waitPanel.SetActive(false);
    }

    public void ShowScores()
    {
        string stringScore = "������� ����:\n";
        for (int i = 0;i < clients.Length;i++)
        {
            stringScore += clients[i].view.Owner.NickName + " : " + clients[i].scores + "\n";
        }

        for (int i = 0; i < clients.Length; i++)
        {
            clients[i].view.RPC("DisplayAllScores", clients[i].view.Owner, stringScore);
        }

    }

    [PunRPC]
    void DisplayAllScores(string str)
    {
        scoreTab.gameObject.SetActive(true);
        scoreTab.SetText(str);
    }


    public void AddScore(string nick, int score)
    {
        for (int i = 0; i < clients.Length; i++)
        {
            if (clients[i].view.Owner.NickName == nick) clients[i].scores += score;
        }
        ShowScores();
    }







    
}