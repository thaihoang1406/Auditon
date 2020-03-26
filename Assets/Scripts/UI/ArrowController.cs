using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject background;
    public int goodPercentTime = 100;
    public int greatPercentTime = 80;
    public int perfectPercentTime = 50;

    private List<Arrow> arrows;
    private float sessionTimeout; //Max time can press all arrows
    private int arrowIndex = 0;
    private bool canTrackKey = false;
    private float currentTime = 0;
    [Range(3, 10)]
    private int numberOfArrow;

    public void BuildArrowList(int numberOfArrow, float sessionTimeout = 30)
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

        UpdateBackground(numberOfArrow);

        for (int i=0; i< numberOfArrow; i++)
        {
            Arrow.ArrowDirection direction = (Arrow.ArrowDirection)Random.Range(0, 3);
            Vector3 rotationEule = new Vector3(0, 0, 90 * (int)direction);
            Arrow arrow = arrowPrefab.Spawn(Vector3.zero, Quaternion.Euler(rotationEule), Vector3.one, arrowHolder.transform, true, true)
                                    .GetComponent<Arrow>();
            arrow.direction = direction;
            arrows.Add(arrow);
        }
    }


    private void UpdateBackground(int numberOfArrow)
    {
        Vector2 rootSize = this.GetComponent<RectTransform>().sizeDelta;
        Vector2 arrowSize = arrowPrefab.GetComponent<RectTransform>().sizeDelta;
        Vector2 startPieSize = background.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        Vector2 midlePieSize = background.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta;
        Vector2 endPieSize = background.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta;
        Vector2 newRootSize;
        

        newRootSize = new Vector2((arrowSize.x + 5) * numberOfArrow, rootSize.y);
        midlePieSize = new Vector2((newRootSize.x - startPieSize.x - endPieSize.x), midlePieSize.y);

        this.GetComponent<RectTransform>().sizeDelta = newRootSize;
        background.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = midlePieSize;
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
        bool successful = arrow.direction == direction;
        arrow.SetSuccessfulArrow(successful);
        if (!successful)
            DispatchSesstionResult(SessionResult.FAIL);
        else
        {
            if (arrowIndex == arrows.Count -1)
            {
                float percent = currentTime / sessionTimeout * 100;
                if (percent <= perfectPercentTime)
                {
                    DispatchSesstionResult(SessionResult.PERFECT);
                }
                else if (percent <= greatPercentTime)
                {
                    DispatchSesstionResult(SessionResult.GREAT);
                }
                else if (percent <= goodPercentTime)
                {
                    DispatchSesstionResult(SessionResult.GOOD);
                }
            }
        }
        arrowIndex++;

    }

    private void OnGUI()
    {
        
        
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
            currentTime = currentTime + Time.deltaTime;
            if (currentTime > sessionTimeout)
            {
                DispatchSesstionResult(SessionResult.MISS);
            }
        }

        if (canTrackKey/* && Event.current.isKey*/)
        {
            if (Input.GetKeyUp("left"))
                PerformArrowDown(Arrow.ArrowDirection.LEFT);
            if (Input.GetKeyUp("up"))
                PerformArrowDown(Arrow.ArrowDirection.UP);
            if (Input.GetKeyUp("right"))
                PerformArrowDown(Arrow.ArrowDirection.RIGHT);
            if (Input.GetKeyUp("down"))
                PerformArrowDown(Arrow.ArrowDirection.DOWN);

            /*switch (Event.current.keyCode)
            {
                case KeyCode.LeftArrow:
                    PerformArrowDown(Arrow.ArrowDirection.LEFT);
                    break;
                case KeyCode.UpArrow:
                    PerformArrowDown(Arrow.ArrowDirection.UP);
                    break;
                case KeyCode.RightArrow:
                    PerformArrowDown(Arrow.ArrowDirection.RIGHT);
                    break;
                case KeyCode.DownArrow:
                    PerformArrowDown(Arrow.ArrowDirection.DOWN);
                    break;
            }*/
        }
    }
}

