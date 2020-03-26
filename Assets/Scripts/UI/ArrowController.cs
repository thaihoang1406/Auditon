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
    public TargetBar targetBar;
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
    private bool hasFailure;

    public void BuildArrowList(int level, int numberOfArrow, float sessionTimeout = 5)
    {
        this.currentTime = 0.0f;
        this.canTrackKey = false;
        this.arrowIndex = 0;
        this.sessionTimeout = sessionTimeout;
        this.numberOfArrow = numberOfArrow;
        hasFailure = false;

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

        //timer.SetValue(1.0f);
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
        if (arrowIndex >= numberOfArrow)
            return;

        Arrow arrow = arrows[arrowIndex];
        arrowIndex = arrowIndex + 1;
        bool successful = arrow.direction == direction;
        arrow.SetSuccessfulArrow(successful);
        if (!successful)
            hasFailure = true;
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
            if (currentTime > sessionTimeout)
            {
                DispatchSesstionResult(SessionResult.MISS);
            }
        }

        float percent = currentTime / sessionTimeout;
        targetBar.SetValue(percent);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (arrowIndex == arrows.Count && !hasFailure)
            {
                if (targetBar.MeetPerfectZone())
                {
                    DispatchSesstionResult(SessionResult.PERFECT);
                }
                else if (targetBar.MeetGreatZone())
                {
                    DispatchSesstionResult(SessionResult.GREAT);
                }
                else if (targetBar.MeetGoodZone())
                {
                    DispatchSesstionResult(SessionResult.GOOD);
                    
                    if (percent >= 0.999f)
                    {
                        DispatchSesstionResult(SessionResult.MISS);
                    }
                }
                else
                {
                    DispatchSesstionResult(SessionResult.MISS);
                }
            }
            else
            {
                DispatchSesstionResult(SessionResult.MISS);
            }
        }
    }
}

