using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject waitPanel;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] ScoreTab scoreTab;
    [SerializeField] WaveTab waveTab;
    [SerializeField] GameOverPanel overPanel;
    [SerializeField] SkillDisplay skillDisplay;
    [SerializeField] HostLeftPanel hostLeftPanel;

    [HideInInspector] public PhotonView view;
    [HideInInspector] public PlayerHealth charater;
    [HideInInspector] public int scores = 0;

    GameManager[] clients;
    
    bool isGameOn = false;
    bool isGameOver = false;

    public static GameManager master;
    public static GameManager my;

    public void LeaveRoom()
    { 
        PhotonNetwork.LeaveRoom();
    }

    private void OnDestroy()
    {
        if (view.IsMine == true) SceneManager.LoadScene(0);
        else if (this == master) GameManager.my.HostLeft();
    }

    void HostLeft()
    {
        if (isGameOver == true) return;
        isGameOver = true;
        hostLeftPanel.gameObject.SetActive(true);
        int wave = FindObjectOfType<EnemySpawner>().wave;

        int credits = (int)(scores + scores * wave * 0.1f);
        int allCredits = PlayerPrefs.GetInt("credits");
        PlayerPrefs.SetInt("credits", allCredits + credits);

        hostLeftPanel.SetCreditText(credits);
    }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        overPanel.gameObject.SetActive(false);
        waitPanel.gameObject.SetActive(true);
        skillDisplay.gameObject.SetActive(false);
        hostLeftPanel.gameObject.SetActive(false);
        
        if (view.IsMine == false) Destroy(canvas);
        if(PhotonNetwork.IsMasterClient) StartCoroutine(CheckPlayers());
        SetupMainClients();
        if(view.IsMine == true) MusicManager.instance.SetBattleMusic();
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

    void SetupMainClients()
    {
        clients = FindObjectsOfType<GameManager>();
        for (int i = 0; i < clients.Length; i++)
        {
            if (clients[i].view.Owner.IsMasterClient == true) master = clients[i];
            if (clients[i].view.IsMine) my = clients[i];
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
        StartCoroutine(CheckCharsAlive());
    }


    [PunRPC]
    void StartGame()
    {
        Vector3 randVec = Random.insideUnitSphere * 2;
        randVec.y = 1;
        GameObject plr = PhotonNetwork.Instantiate(playerPrefab.name, randVec, Quaternion.identity);
        charater = plr.GetComponent<PlayerHealth>();
        waitPanel.SetActive(false);
        skillDisplay.gameObject.SetActive(true);
        charater.GetComponent<SoldierSkill>().display = skillDisplay;
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
            if (clients[i].view.Owner.NickName == nick)
            {
                clients[i].scores += score;
                clients[i].view.RPC("SetScoreRPC", clients[i].view.Owner, clients[i].scores);
            }
        }
        ShowScores();
    }

    [PunRPC]
    void SetScoreRPC(int score)
    {
        scores = score;

    }

    IEnumerator CheckCharsAlive()
    {
        yield return new WaitForSeconds(5);
        PlayerHealth[] characters = FindObjectsOfType<PlayerHealth>();
        if (characters.Length == 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(CheckCharsAlive());
        }

    }

    void GameOver()
    {
        string leaderString = GetLeaderString();
        int wave = FindObjectOfType<EnemySpawner>().wave;
        for (int i = 0; i < clients.Length; i++)
        {
            int s = clients[i].scores;
            clients[i].view.RPC("GameOverAll", clients[i].view.Owner, leaderString, wave, s);
        }

    }

    string GetLeaderString()
    {
        string str = "���������� ����:\n";

        for (int i = 0; i < clients.Length; i++)
        {
            GameManager maxClient = clients[i];
            int maxIndex = i;

            for (int j = i+1; j < clients.Length; j++)
            {
                if (clients[j].scores > maxClient.scores)
                {
                    maxClient = clients[j];
                    maxIndex = j;
                }
            }
            GameManager temp = clients[i];
            clients[i] = maxClient;
            clients[maxIndex] = temp;

        }

        for (int i = 0; i < clients.Length; i++)
        {
            str += clients[i].view.Owner.NickName + " : " + clients[i].scores + "\n";
        }


        return str;
    }



    [PunRPC]
    void GameOverAll(string leaderString, int wave, int score)
    {
        if (isGameOver == true) return;
        isGameOver = true;

        int credits = (int) (score + score * wave * 0.1f);
        int allCredits = PlayerPrefs.GetInt("credits");
        PlayerPrefs.SetInt("credits", allCredits + credits);


        overPanel.gameObject.SetActive(true);
        overPanel.SetTexts(leaderString, wave, credits);
    }





    
}
