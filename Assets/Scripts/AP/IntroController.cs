using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public static IntroController instance;

    public delegate void PlayEndedAction();
    public event PlayEndedAction OnPlayEnded;

    private PlayableDirector director;
    private bool isFinished;

    private void Awake()
    {
        IntroController.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFinished = false;
        director = this.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished && director.state != PlayState.Playing)
        {
            if (OnPlayEnded != null)
                OnPlayEnded();
            isFinished = true;
        }
    }
}
