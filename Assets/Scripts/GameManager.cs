using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;
    public GameObject Player3Prefab;

    public Text Player1Score;
    public Text Player2Score;
    public Text Player3Score;

    public GameObject PerfectEffectPrefab;
    public GameObject GreatEffectPrefab;
    public GameObject GoodEffectPrefab;

    public GameObject ResultPopup;

    GameObject Player1;
    GameObject Player2;
    GameObject Player3;

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
        InitPlayer();
        score1 = score2 = score3 = 0;
        ArrowController.instance.BuildArrowList(5, 8);
        ArrowController.instance.StartTrackKey();
        ArrowController.OnSessionEnd += onFinish;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinish &&Time.time - timeFinish > 10 && !isShowResultPopup)
        {
            isShowResultPopup = true;
            ResultPopup.active = true;
        }
    }

    private void OnDestroy()
    {
        ArrowController.OnSessionEnd -= onFinish;
    }

    void ProcessDanceNPC(string triggerDance)
    {
        if(triggerDance == "")
        {
            int rand = Random.Range(1, 6);
            triggerDance = "Dancing" + rand.ToString();
        }
        int rand1 = Random.Range(0, 2);
        score2 += getScore(rand1);
        Player2Score.text = "Player 2: " + score2;
        Player2.GetComponent<Animator>().SetTrigger(triggerDance);
        initEffect(rand1, Player2.transform.GetChild(6).transform.position);

        int rand2 = Random.Range(0, 2);
        score3 += getScore(rand2);
        Player3Score.text = "Player 3: " + score3;
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
        }
        return 0;
    }

    void initEffect(int type, Vector3 pos)
    {
        switch (type)
        {
            case 0:
                Instantiate(PerfectEffectPrefab, pos, Quaternion.identity);
                break;
            case 1:
                Instantiate(GreatEffectPrefab, pos, Quaternion.identity);
                break;
            case 2:
                Instantiate(GoodEffectPrefab, pos, Quaternion.identity);
                break;

        }
    }

    public void RefreshPosPlayer()
    {
        Player1.transform.position = posLeft;
        Player2.transform.position = posRight;
        Player3.transform.position = posMid;
        Player1.transform.eulerAngles = new Vector3(Player1.transform.eulerAngles.x, 180, Player1.transform.eulerAngles.z);
        Player2.transform.eulerAngles = new Vector3(Player2.transform.eulerAngles.x, 180, Player2.transform.eulerAngles.z);
        Player3.transform.eulerAngles = new Vector3(Player3.transform.eulerAngles.x, 180, Player3.transform.eulerAngles.z);
    }
    float timeFinish = 0;
    bool isShowResultPopup = false;
    bool isFinish = false;
    public void onFinish(ArrowController.SessionResult sessionResult)
    {
        numRound++;
        
        RefreshPosPlayer();

        //Ultimate
        if (numRound == 5)
        {
            isFinish = true;
            timeFinish = Time.time;
            if (sessionResult != ArrowController.SessionResult.MISS)
            {
                Player1.GetComponent<Animator>().SetTrigger("Ultimate");
                switch (sessionResult)
                {
                    case ArrowController.SessionResult.PERFECT:
                        score1 += getScore(0)*3;
                        initEffect(0, Player1.transform.GetChild(3).transform.position);
                        break;
                    case ArrowController.SessionResult.GREAT:
                        score1 += getScore(1)*3;
                        initEffect(1, Player1.transform.GetChild(3).transform.position);
                        break;
                    case ArrowController.SessionResult.GOOD:
                        score1 += getScore(2)*3;
                        initEffect(2, Player1.transform.GetChild(3).transform.position);
                        break;

                }

                Player1Score.text = "Player 1: " + score1;
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
            ProcessDanceNPC(temp);

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

            Player1Score.text = "Player 1: " + score1;
            ArrowController.instance.BuildArrowList(5, 8);
            ArrowController.instance.StartTrackKey();
        }
        

    }

    void InitPlayer()
    {
        Player1 = Instantiate(Player1Prefab, posLeft, Quaternion.identity);
        Player2 = Instantiate(Player2Prefab, posRight, Quaternion.identity);
        Player3 = Instantiate(Player3Prefab, posMid, Quaternion.identity);
        RefreshPosPlayer();
    }
}
