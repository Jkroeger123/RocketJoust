using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleText : MonoBehaviour
{
   
    private void Start()
    {
        Tween t = transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .6f).SetLoops(-1, LoopType.Yoyo);
    }

}
