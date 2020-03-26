using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public enum ArrowDirection
    {
        NODE = -1,
        LEFT,
        DOWN,
        RIGHT,
        UP,
    };

    public ArrowDirection direction = ArrowDirection.NODE;

    //private Animator animator;

    public void SetSuccessfulArrow(bool successful)
    {
        if (successful)
            this.GetComponent<Image>().color = new Color(0, 1, 0);
        else
            this.GetComponent<Image>().color = new Color(1, 0, 0);
    }

    public void Reset()
    {
        this.transform.rotation = Quaternion.identity;
        //this.animator.SetTrigger("reset");
        this.GetComponent<Image>().color = new Color(1, 1, 1);
        this.gameObject.Kill();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
