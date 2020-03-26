using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Animator animator;

    public void SetSuccessfulArrow(bool successful)
    {
        if (successful)
            animator.SetTrigger("success");
        else
            animator.SetTrigger("fail");
    }

    public void Reset()
    {
        this.transform.rotation = Quaternion.identity;
        this.animator.SetTrigger("reset");
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
;        this.gameObject.Kill();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
