using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    
}
public class ControllerteamDancing : MonoBehaviour
{
    public GameObject dancer1;
    public GameObject dancer2;
    public GameObject dancer3;   
    Vector3 tempPos1;
    Vector3 tempPos2;
    Vector3 tempPos3;
    public float timemove=1.5f;
    IEnumerator SwapDancer1WithDancer2()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / timemove;
            dancer1.transform.position = Vector3.Lerp(dancer1.transform.position, tempPos2, i);
            dancer2.transform.position = Vector3.Lerp(dancer2.transform.position, tempPos1, i);
            yield return 0;
        }
        tempPos1 = dancer1.transform.position;
        tempPos2 = dancer2.transform.position;
    }
    IEnumerator SwapDancer1WithDancer3()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / timemove;
            dancer1.transform.position = Vector3.Lerp(dancer1.transform.position, tempPos3, i);
            dancer3.transform.position = Vector3.Lerp(dancer3.transform.position, tempPos1, i);
            yield return 0;
        }
        tempPos1 = dancer1.transform.position;
        tempPos3 = dancer3.transform.position;

    }


    // Start is called before the first frame update
    void Start()
    {
        
        tempPos1 = dancer1.transform.position;
        tempPos2 = dancer2.transform.position;
        tempPos3 = dancer3.transform.position;
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("press sssssssssssssssssssssssssssssssssssssssss");
            ///  dancer1.transform.localPosition = Vector3.move(dancer1.transform.localPosition, tempPos2, t);
            //dancer1.transform.position = Vector3.MoveTowards(dancer1.transform.position, new Vector3(0,2,0),1f);
            StartCoroutine(SwapDancer1WithDancer2());
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SwapDancer1WithDancer3 ());
        }
        

    }
}
