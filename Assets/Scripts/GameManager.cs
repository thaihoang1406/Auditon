﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    class PlayerInfo
    {
        public int id;
        public int Score;
        public PlayerInfo(int id)
        {
            this.id = id;
        }
    };

    public Text Player1Score;
    public Text Player2Score;
    public Text Player3Score;

    public GameObject PerfectEffectPrefab;
    public GameObject GreatEffectPrefab;
    public GameObject GoodEffectPrefab;
    public GameObject MissEffectPrefab;

    public GameObject ResultPopup;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    public GameObject arrowBox;

    private List<PlayerInfo> playerInfos;

    private void OnIntroPlayEnded()
    {
        arrowBox.SetActive(true);
        ArrowController.instance.BuildArrowList(1, 5);
        ArrowController.instance.StartTrackKey();
        ArrowController.OnSessionEnd += onFinish;
    }

    private void PlayEffect(GameObject effectPrefab, Vector3 pos)
    {
        SesstionResult sesstionResult = effectPrefab.Spawn(pos).GetComponent<SesstionResult>();
        sesstionResult.Play();
    }

    private void UpdateScore(int playerId, int score)
    {
        foreach(PlayerInfo playerInfo in playerInfos)
        {
            if (playerInfo.id == playerId)
            {
                playerInfo.Score = score;
            }
        }

        playerInfos.Sort((a, b) => { return a.Score < b.Score ? 1 : -1; });

        //print
        Player1Score.text = "Player " + playerInfos[0].id + ": " + playerInfos[0].Score;
        Player2Score.text = "Player " + playerInfos[1].id + ": " + playerInfos[1].Score;
        Player3Score.text = "Player " + playerInfos[2].id + ": " + playerInfos[2].Score;
    }

    int score1;
    int score2;
    int score3;

    int numRound;

    Vector3 posLeft = new Vector3(-3, 0, -3);
    Vector3 posRight = new Vector3(3, 0, -3);
    Vector3 posMid = new Vector3(0, 0, -7);
    void Start()
    {
        numRound = 0;
        //InitPlayer();
       
        score1 = score2 = score3 = 0;
        IntroController.instance.OnPlayEnded += OnIntroPlayEnded;

        playerInfos = new List<PlayerInfo>();
        playerInfos.Add(new PlayerInfo(1));
        playerInfos.Add(new PlayerInfo(2));
        playerInfos.Add(new PlayerInfo(3));
        ControllerteamDancing.instance.InitPositionPlayer();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinish &&Time.time - timeFinish > 10 && !isShowResultPopup)
        {
            isShowResultPopup = true;
            ResultPopup.active = true;
            ResultPopup.transform.GetChild(2).GetComponent<Text>().text = score1.ToString();
            RefreshPosPlayer();
        }
    }

    private void OnDestroy()
    {
        ArrowController.OnSessionEnd -= onFinish;
        IntroController.instance.OnPlayEnded -= OnIntroPlayEnded;
    }

    void ProcessDanceNPC(string triggerDance)
    {
        if(triggerDance == "")
        {
            int rand = Random.Range(1, 6);
            triggerDance = "Dancing" + rand.ToString();
        }
        int rand1 = Random.Range(0, 3);
        score2 += getScore(rand1);
        //Player2Score.text = "Player 2: " + score2;
        UpdateScore(2, score2);
        Player2.GetComponent<Animator>().SetTrigger(triggerDance);
        initEffect(rand1, Player2.transform.GetChild(6).transform.position);

        int rand2 = Random.Range(0, 3);
        score3 += getScore(rand2);
        //Player3Score.text = "Player 3: " + score3;
        UpdateScore(3, score3);
        Player3.GetComponent<Animator>().SetTrigger(triggerDance);
        initEffect(rand1, Player3.transform.GetChild(3).transform.position);
    }

    int getScore(int type)
    {
        switch (type)
        {
            case 0:
                return 100;
            case 1:
                return 80;
            case 2:
                return 50;
            case 3:
                return 0;
        }
        return 0;
    }

    void initEffect(int type, Vector3 pos)
    {
        switch (type)
        {
            case 0:
                //Instantiate(PerfectEffectPrefab, pos, Quaternion.identity);
                PlayEffect(PerfectEffectPrefab, pos);
                break;
            case 1:
                //Instantiate(GreatEffectPrefab, pos, Quaternion.identity);
                PlayEffect(GreatEffectPrefab, pos);
                break;
            case 2:
                //Instantiate(GoodEffectPrefab, pos, Quaternion.identity);
                PlayEffect(GoodEffectPrefab, pos);
                break;
            case 3:
                //Instantiate(MissEffectPrefab, pos, Quaternion.identity);
                PlayEffect(MissEffectPrefab, pos);
                break;

        }
    }

    public void RefreshPosPlayer()
    {
        Vector3 temp1 = Player1.transform.position;temp1.y = 0;
        Vector3 temp2 = Player2.transform.position; temp2.y = 0;
        Vector3 temp3 = Player3.transform.position; temp3.y = 0;
        Player1.transform.position = temp1;
        Player2.transform.position = temp2;
        Player3.transform.position = temp3;
        //  Player1.transform.position = posLeft;
        // Player1.transform.position = posLeft;
        // Player2.transform.position = posRight;
        // Player3.transform.position = posMid;
        Player1.transform.eulerAngles = new Vector3(Player1.transform.eulerAngles.x, 180, Player1.transform.eulerAngles.z);
        Player2.transform.eulerAngles = new Vector3(Player2.transform.eulerAngles.x, 180, Player2.transform.eulerAngles.z);
        Player3.transform.eulerAngles = new Vector3(Player3.transform.eulerAngles.x, 180, Player3.transform.eulerAngles.z);
       // ControllerteamDancing.instance.InitPositionPlayer(Player1, Player2, Player3);
    }
    float timeFinish = 0;
    bool isShowResultPopup = false;
    bool isFinish = false;
    public void ShowPosition(GameObject obj)
    {
        Debug.Log("x :"+  obj.transform.position.x);
        Debug.Log("y :" + obj.transform.position.y);
        Debug.Log("z :" + obj.transform.position.z);
    }
    public void onFinish(ArrowController.SessionResult sessionResult)
    {
        numRound++;
        
        RefreshPosPlayer();

        //Ultimate
        if (numRound == 20)
        {
            isFinish = true;
            timeFinish = Time.time;
            if (sessionResult != ArrowController.SessionResult.MISS)
            {
                Player1.GetComponent<Animator>().SetTrigger("Ultimate");
                switch (sessionResult)
                {
                    case ArrowController.SessionResult.PERFECT:
                        score1 += getScore(0);
                        initEffect(0, Player1.transform.GetChild(3).transform.position);

                        break;
                    case ArrowController.SessionResult.GREAT:
                        score1 += getScore(1);


                        initEffect(1, Player1.transform.GetChild(3).transform.position);
                        break;
                    case ArrowController.SessionResult.GOOD:
                        score1 += getScore(2);


                        initEffect(2, Player1.transform.GetChild(3).transform.position);
                        break;

                }

                //Player1Score.text = "Player 1: " + score1;
                UpdateScore(1, score1);
            }
            else
            {
                initEffect(3, Player1.transform.GetChild(3).transform.position);
                Debug.Log("MIss");
               
            }
            string temp = "Ultimate";
            ProcessDanceNPC(temp);
        }
        //Normal dancing
        else
        {
            string temp = "";
            if (sessionResult != ArrowController.SessionResult.MISS)
            {
                int rand = Random.Range(1, 6);
                temp = "Dancing" + rand.ToString();
                Player1.GetComponent<Animator>().SetTrigger(temp);

            }
            else
                initEffect(3, Player1.transform.GetChild(3).transform.position);
            ProcessDanceNPC(temp);

            switch (sessionResult)
            {
                case ArrowController.SessionResult.PERFECT:
                    score1 += getScore(0);
                    initEffect(0, Player1.transform.GetChild(3).transform.position);                    //
                        ControllerteamDancing.instance.SwapDemo();
                    break;
                case ArrowController.SessionResult.GREAT:
                    score1 += getScore(1);
                    initEffect(1, Player1.transform.GetChild(3).transform.position);
                    ControllerteamDancing.instance.SwapDemo();
                    break;
                case ArrowController.SessionResult.GOOD:
                    score1 += getScore(2);
                    initEffect(2, Player1.transform.GetChild(3).transform.position);
                    ControllerteamDancing.instance.SwapDemo();
                    break;

            }

            //Player1Score.text = "Player 1: " + score1;
            UpdateScore(1, score1);
            int lvl = numRound / 4 + 1;
            int numArrow = 5 + numRound / 4;
            ArrowController.instance.BuildArrowList(lvl, numArrow);
            ArrowController.instance.StartTrackKey();
        }
        

    }

    void InitPlayer()
    {
       RefreshPosPlayer();
    }
}
