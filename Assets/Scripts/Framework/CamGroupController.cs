using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CamGroupController : MonoBehaviour
{
    private CinemachineTargetGroup _camGroup;
    private Tween _smoothTween;
    
    private void Awake()
    {
        _camGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void AddObject(GameObject obj)
    {
        _camGroup.AddMember(obj.transform, 1, 0);
    }

    public void RemoveObject(GameObject obj)
    {
        _camGroup.RemoveMember(obj.transform);
    }

    
    //Removes an object from the camera
    //Slowly focus away from where the object was
    //Any addition to the cam group interrupts this lerp
    public void SmoothRemove(GameObject obj)
    {
        
        _camGroup.RemoveMember(obj.transform);
        
        GameObject standIn = new GameObject("CamStandIn");
        standIn.transform.position = obj.transform.position;
        
        _camGroup.AddMember(standIn.transform, 1, 0);
        
        if(_smoothTween != null) _smoothTween.Kill();
        
        _smoothTween = DOTween.To(
            () => _camGroup.m_Targets[_camGroup.FindMember(standIn.transform)].weight,
            (x) => _camGroup.m_Targets[_camGroup.FindMember(standIn.transform)].weight = x, 0, 3f)
            .OnKill(() => RemoveObject(standIn)).OnComplete(() => RemoveObject(standIn)).SetEase(Ease.InOutCubic);
    }

}
