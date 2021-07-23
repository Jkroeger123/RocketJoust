﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMashHandler : MonoBehaviour
{
    public float status = 0;
    private float target = 1f;

    public static event Action<GameObject> ONItemEffectStart; 
    public static event Action<GameObject> ONItemEffectEnd;
    private void Update()
    {
        status = Mathf.Clamp(status - Time.deltaTime, 0, float.PositiveInfinity);

        if (status >= target)
        {
            Unsubscribe();
            GetComponent<PlayerInputController>().EnableGameplayControls();
            Destroy(this);
        }

    }

    private void OnMashPress(InputAction.CallbackContext context)
    {
        status += 0.3f;

        Vector3 randDir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);
        Tween t = transform.DOPunchPosition(randDir, 0.1f).SetLink(gameObject);
    }

    public void InitializeMash()
    {
        GetComponent<PlayerInputController>().InitializeMashControls(OnMashPress);
        ONItemEffectStart?.Invoke(gameObject);
    }

    public void FreezePlayer()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Unsubscribe()
    {
        GetComponent<PlayerInputController>().DestroyMashControls(OnMashPress);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        ONItemEffectEnd?.Invoke(gameObject);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
