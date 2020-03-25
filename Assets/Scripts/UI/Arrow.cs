using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum ArrowDirection
    {
        NODE = -1,
        LEFT,
        UP,
        RIGHT,
        DOWN,
    };

    public ArrowDirection direction = ArrowDirection.NODE;

    private Animator animator;

    public void SetSuccessfulArrow(bool successful)
    {
        if (successful)
            animator.SetTrigger("Successful");
        else
            animator.SetTrigger("Failure");
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
