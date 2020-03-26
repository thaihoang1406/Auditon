using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    void Start()
    {
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitPlayer()
    {

        GameObject temp = Instantiate(Player1, new Vector3(-3, 0, 0),Quaternion.identity);
        temp.transform.eulerAngles = new Vector3(
            temp.transform.eulerAngles.x,
            temp.transform.eulerAngles.y + 180,
            temp.transform.eulerAngles.z);

        temp = Instantiate(Player2, new Vector3(3, 0, 0), Quaternion.identity);
        temp.transform.eulerAngles = new Vector3(
            temp.transform.eulerAngles.x,
            temp.transform.eulerAngles.y + 180,
            temp.transform.eulerAngles.z);

        temp = Instantiate(Player3, new Vector3(0, 0, -4), Quaternion.identity);
        temp.transform.eulerAngles = new Vector3(
            temp.transform.eulerAngles.x,
            temp.transform.eulerAngles.y + 180,
            temp.transform.eulerAngles.z);
    }
}
