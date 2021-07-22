using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlastUI : MonoBehaviour
{

    public Color readyColor;
    public Color notReadyColor;
    public GameObject blastElement;
    public GameObject target;
  
    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            if(_isHidden != value) SetIconsActive(value);
            _isHidden = value;
        }
    }
    
    private float yOffset = 1.75f;
    private RectTransform _transform;
    private Canvas _parentCanvas;
    private bool _isHidden;
    
    private void Start()
    {
        _parentCanvas = transform.parent.GetComponent<Canvas>();
        _transform = GetComponent<RectTransform>();
        PlayerDeathHandler.ONDeath += PlayerDeathHandlerOnONDeath;
    }

    private void PlayerDeathHandlerOnONDeath(GameObject obj)
    {
        if(obj == target) Destroy(gameObject);
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
        UpdatePosition();
    }

    
    //Update the state of the blast display
    public void UpdateBlastUI(int blastCount, float step)
    {
        
        for (int i = 0; i < blastCount; i ++ )
        {
            transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
            transform.GetChild(i).GetComponent<Image>().color = readyColor;
        }

        if (blastCount >= transform.childCount) return;
        
        transform.GetChild(blastCount).GetComponent<Image>().fillAmount = 1-step;
        transform.GetChild(blastCount).GetComponent<Image>().color = notReadyColor;
        
        for (int i = blastCount+1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
        }

    }


    private void SetIconsActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!isActive);
        }
    }


    private void UpdatePosition()
    {
        Vector3 worldPos = Vector3.Lerp(UiToWorldSpace(transform.position), target.transform.position, 0.5f);
        worldPos.y += yOffset;
        Vector3 uiPos = WorldToUISpace(_parentCanvas, worldPos);

        _transform.position = uiPos;
    }

    //Utility functions
    
    private Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out Vector2 movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    private Vector3 UiToWorldSpace(Vector3 uiPos)
    {
        return Camera.main.ScreenToWorldPoint(uiPos);
    }


}
