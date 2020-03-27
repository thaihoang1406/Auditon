using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerteamDancing : MonoBehaviour
{
    public   GameObject dancer1;
    public GameObject dancer2;
    public GameObject dancer3;   
    Vector3 tempPos1;
    Vector3 tempPos2;
    Vector3 tempPos3;
    public float timemove=1.5f;

    public static ControllerteamDancing instance=null;
    
    private void Awake()
    {
        instance = this;
    }
    public void InitPositionPlayer()
    {
      //  dancer1= dancer11;
      //  dancer2 = dancer22;
       // dancer3 = dancer33;
        tempPos1 = dancer1.transform.position;
        tempPos2 = dancer2.transform.position;
        tempPos3 = dancer3.transform.position;
    }
    public void InitPositionPlayer(GameObject dancer11, GameObject dancer22, GameObject dancer33)
    {
          dancer1= dancer11;
          dancer2 = dancer22;
         dancer3 = dancer33;
        tempPos1 = dancer1.transform.position;
        tempPos2 = dancer2.transform.position;
        tempPos3 = dancer3.transform.position;
    }
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
        
        
      
    }
    public  void SwapDemo()
    {
        int rd = Random.Range(0, 2);
        if(rd==0)
            StartCoroutine(SwapDancer1WithDancer2());
        else
            StartCoroutine(SwapDancer1WithDancer3());
    }
   
    // Update is called once per frame
    void Update()
    {
        
        
        

    }
}
