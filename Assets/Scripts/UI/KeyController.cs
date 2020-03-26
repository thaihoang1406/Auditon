using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeyController : MonoBehaviour
{
    public enum SessionResult
    {
        PERFECT,
        GREAT,
        GOOD,
        FAIL,
        MISS
    };

    public static KeyController instance;

    public GameObject arrowHolder;
    public GameObject arrowPrefab;
    public int goodPercentTime = 100;
    public int greatPercentTime = 80;
    public int perfectPercentTime = 50;

    private List<Arrow> arrows;
    private float keySessionTimeout;
    private int keyIndex = 0;
    private bool canTrackKey = false;
    private float currentTime = 0;

    public void BuildKeyList(int numberOfKey, float keySessionTimeout)
    {
        this.currentTime = 0.0f;
        this.canTrackKey = false;
        this.keyIndex = 0;
        this.keySessionTimeout = keySessionTimeout;

        foreach(Arrow arrow in arrows)
        {
            arrow.Reset();
        }
        arrows.Clear();

        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.size = new Vector2((arrowPrefab.GetComponent<RectTransform>().sizeDelta.x + 5) * numberOfKey, sr.size.y);
        this.GetComponent<SpriteRenderer>().size = sr.size;

        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(sr.size.x, rt.sizeDelta.y);
        this.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;

        for (int i=0; i<numberOfKey; i++)
        {
            Arrow.ArrowDirection direction = (Arrow.ArrowDirection)Random.Range(0, 3);
            Vector3 rotationEule = new Vector3(0, 0, 90 * (int)direction);
            Arrow arrow = arrowPrefab.Spawn(Vector3.zero, Quaternion.Euler(rotationEule), Vector3.one, arrowHolder.transform, true, true)
                                    .GetComponent<Arrow>();
            arrow.direction = direction;
            arrows.Add(arrow);
        }
    }

    public void StartTrackKey()
    {
        canTrackKey = true;
    }

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        KeyController.instance = this;
        arrows = new List<Arrow>();
    }

    private void DispatchSesstionResult(SessionResult result)
    {
        canTrackKey = false;
        Debug.Log(result.ToString());
    }

    private void PerformArrowDown(Arrow.ArrowDirection direction)
    {
        Arrow arrow = arrows[keyIndex];
        bool successful = arrow.direction == direction;
        arrow.SetSuccessfulArrow(successful);
        if (!successful)
            DispatchSesstionResult(SessionResult.FAIL);
        else
        {
            if (keyIndex == arrows.Count -1)
            {
                float percent = currentTime / keySessionTimeout * 100;
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
        keyIndex++;

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

            if (currentTime > keySessionTimeout)
            {
                DispatchSesstionResult(SessionResult.MISS);
            }
        }
    }
}

