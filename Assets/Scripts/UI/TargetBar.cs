using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetBar : MonoBehaviour
{

    public Image target;
    public Image perfectZone;
    public Image greatZone;
    public Image goodZone;
    [Range(0.0f, 1.0f)]
    public float value = 0.0f;

    private RectTransform rectTarget;
    private float targetBarRectSizeX;

    public bool MeetPerfectZone()
    {
        return MeetZone(perfectZone);
    }

    public bool MeetGreatZone()
    {
        return MeetZone(greatZone);
    }

    public bool MeetGoodZone()
    {
        return MeetZone(goodZone);
    }

    private bool MeetZone(Image zone)
    {
        RectTransform rectZone = zone.GetComponent<RectTransform>();

        return target.transform.localPosition.x >= zone.transform.localPosition.x - rectZone.sizeDelta.x/2
            && target.transform.localPosition.x <= zone.transform.localPosition.x + rectZone.sizeDelta.x/2;
    }

    public void SetValue(float value)
    {
        this.value = value;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        targetBarRectSizeX = this.GetComponent<RectTransform>().sizeDelta.x;
        rectTarget = target.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.localPosition = new Vector3(targetBarRectSizeX * value, 0, 0);
    }
}
