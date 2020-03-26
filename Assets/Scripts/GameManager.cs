using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;
    public GameObject Player3Prefab;

    GameObject Player1;
    GameObject Player2;
    GameObject Player3;
    void Start()
    {
        InitPlayer();
        ArrowController.instance.BuildArrowList(5, 5);
        ArrowController.instance.StartTrackKey();
        ArrowController.OnSessionEnd += onFinish;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        ArrowController.OnSessionEnd -= onFinish;
    }

    public void onFinish(ArrowController.SessionResult sessionResult)
    {
        if (sessionResult != ArrowController.SessionResult.MISS)
        {
            int rand = Random.Range(1, 6);
            string temp = "Dancing" + rand.ToString();
            Player1.GetComponent<Animator>().SetTrigger(temp);
            Player2.GetComponent<Animator>().SetTrigger(temp);
            Player3.GetComponent<Animator>().SetTrigger(temp);
            ArrowController.instance.BuildArrowList(5, 5);
            ArrowController.instance.StartTrackKey();
        }
    }

    void InitPlayer()
    {

        Player1 = Instantiate(Player1Prefab, new Vector3(-3, 0, -3),Quaternion.identity);
        
        Player2 = Instantiate(Player2Prefab, new Vector3(3, 0, -3), Quaternion.identity);
        
        Player3 = Instantiate(Player3Prefab, new Vector3(0, 0, -7), Quaternion.identity);
        RefreshRotate();
       }

    public void RefreshRotate()
    {
        Player1.transform.eulerAngles = new Vector3(Player1.transform.eulerAngles.x, Player1.transform.eulerAngles.y + 180, Player1.transform.eulerAngles.z);
        Player2.transform.eulerAngles = new Vector3(Player2.transform.eulerAngles.x, Player2.transform.eulerAngles.y + 180, Player2.transform.eulerAngles.z);
        Player3.transform.eulerAngles = new Vector3(Player3.transform.eulerAngles.x, Player3.transform.eulerAngles.y + 180, Player3.transform.eulerAngles.z);
    }
}
