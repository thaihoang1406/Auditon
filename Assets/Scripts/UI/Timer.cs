using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image uiValue;

    //public void Init(float width, float height = -1)
    //{
    //    if (height == -1)
    //        height = this.GetComponent<RectTransform>().sizeDelta.y;

    //    this.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    //}

    public void SetValue(float value)
    {
        uiValue.fillAmount = value;
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
