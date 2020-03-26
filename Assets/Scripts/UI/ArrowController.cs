using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ArrowController : MonoBehaviour
{
    public enum SessionResult
    {
        PERFECT,
        GREAT,
        GOOD,
        FAIL,
        MISS
    };
    public delegate void SessionEndAction(SessionResult sessionResult);
    public static event SessionEndAction OnSessionEnd;


    public static ArrowController instance;

    public GameObject arrowHolder;
    public GameObject arrowPrefab;
    public Timer timer;
    public Text levelTextValue;
    [Range(0.0f, 1.0f)]
    public float maxValueOfPerfect = 0.3f;
    [Range(0.0f, 1.0f)]
    public float maxValueOfGreat = 0.5f;
    [Range(0.0f, 1.0f)]
    public float maxValueOfGood = 1.0f;

    private List<Arrow> arrows;
    private float sessionTimeout; //Max time can press all arrows
    private int arrowIndex = 0;
    private bool canTrackKey = false;
    private float currentTime = 0;
    [Range(3, 10)]
    private int numberOfArrow;

    public void BuildArrowList(int level, int numberOfArrow, float sessionTimeout = 30)
    {
        this.currentTime = 0.0f;
        this.canTrackKey = false;
        this.arrowIndex = 0;
        this.sessionTimeout = sessionTimeout;
        this.numberOfArrow = numberOfArrow;

        foreach(Arrow arrow in arrows)
        {
            arrow.Reset();
        }
        arrows.Clear();

        levelTextValue.text = level.ToString();

        for (int i=0; i< numberOfArrow; i++)
        {
            Arrow.ArrowDirection direction = (Arrow.ArrowDirection)Random.Range(0, 3);
            Vector3 rotationEule = new Vector3(0, 0, 90 * (int)direction);
            Arrow arrow = arrowPrefab.Spawn(Vector3.zero, Quaternion.Euler(rotationEule), Vector3.one, arrowHolder.transform, true, true)
                                    .GetComponent<Arrow>();
            arrow.direction = direction;
            arrows.Add(arrow);
        }

        timer.SetValue(1.0f);
    }

    public void StartTrackKey()
    {
        canTrackKey = true;
    }

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        ArrowController.instance = this;
        arrows = new List<Arrow>();
    }

    private void DispatchSesstionResult(SessionResult result)
    {
        canTrackKey = false;
        //Debug.Log(result.ToString());
        if (OnSessionEnd != null)
            OnSessionEnd(result);
    }

    private void PerformArrowDown(Arrow.ArrowDirection direction)
    {
        Arrow arrow = arrows[arrowIndex];
        arrowIndex = arrowIndex + 1;
        bool successful = arrow.direction == direction;
        arrow.SetSuccessfulArrow(successful);
        if (!successful)
            DispatchSesstionResult(SessionResult.FAIL);
        else
        {
            if (arrowIndex == arrows.Count)
            {
                float percent = (currentTime / sessionTimeout);
                if (percent <= maxValueOfPerfect)
                {
                    DispatchSesstionResult(SessionResult.PERFECT);
                }
                else if (percent <= maxValueOfGreat)
                {
                    DispatchSesstionResult(SessionResult.GREAT);
                }
                else if (percent <= maxValueOfGood)
                {
                    DispatchSesstionResult(SessionResult.GOOD);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canTrackKey)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PerformArrowDown(Arrow.ArrowDirection.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PerformArrowDown(Arrow.ArrowDirection.UP);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PerformArrowDown(Arrow.ArrowDirection.RIGHT);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PerformArrowDown(Arrow.ArrowDirection.DOWN);
            }

            currentTime = currentTime + Time.deltaTime;
            float percent = 1.0f - (currentTime / sessionTimeout);
            timer.SetValue(percent);
            if (currentTime > sessionTimeout)
            {
                DispatchSesstionResult(SessionResult.MISS);
            }
        }
    }
}

