using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlastUI : MonoBehaviour
{

    public GameObject blastElement;
    public GameObject target;
    public float yOffset = 0.3f;
    
    private RectTransform _transform;
    private Canvas _parentCanvas;

    private void Start()
    {
        _parentCanvas = transform.parent.GetComponent<Canvas>();
        _transform = GetComponent<RectTransform>();
    }

    public void Initialize(int cap, GameObject t)
    {
        target = t;
        
        for (int i = 0; i < cap; i++)
        {
            Instantiate(blastElement, transform);
        }
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 uiPos = WorldToUISpace(_parentCanvas, target.transform.position);
        uiPos.y += yOffset;

        _transform.position = uiPos;
    }

    public void UpdateBlastUI(int blastCount, float step)
    { 
        for (int i = 0; i < blastCount-1; i ++ )
        {
            transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
        }

        transform.GetChild(blastCount-1).GetComponent<Image>().fillAmount = 1-step;

        for (int i = blastCount; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
        }

    }

    private Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out Vector2 movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
