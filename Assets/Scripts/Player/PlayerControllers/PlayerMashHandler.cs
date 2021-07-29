using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMashHandler : MonoBehaviour
{
    public float status = 0;
    private float target = 1f;

    private GameObject _gumObject;

    private GameObject _hitEffect;
    
    public static event Action<GameObject> ONItemEffectStart; 
    public static event Action<GameObject> ONItemEffectEnd;

    private void Awake()
    {
        _hitEffect = transform.Find("HitEffects").gameObject;
        
        foreach (Transform child in transform)
        {
            if (!child.gameObject.CompareTag("gum")) continue;
            _gumObject = child.gameObject;
            break;
        }
    }

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
        _hitEffect.GetComponent<MMFeedbacks>().PlayFeedbacks();
    }

    public void InitializeMash()
    {
        GetComponent<PlayerInputController>().InitializeMashControls(OnMashPress);
        ONItemEffectStart?.Invoke(gameObject);
    }

    public void ShowGum() => _gumObject.SetActive(true);

    public void HideGum()
    {
        if (!_gumObject.activeInHierarchy) return;

        _gumObject.GetComponent<SpriteRenderer>().DOColor(Color.clear, 1f)
            .OnComplete(() =>
            {
                _gumObject.SetActive(false);
                _gumObject.GetComponent<SpriteRenderer>().color = Color.white;
            });
    }
    

    private void Unsubscribe()
    {
        GetComponent<PlayerInputController>().DestroyMashControls(OnMashPress);
        GetComponent<PlayerInputController>().EnableGameplayControls();
        ONItemEffectEnd?.Invoke(gameObject);
    }

    
    private void OnDestroy()
    {
        HideGum();
        Unsubscribe();
    }
}
