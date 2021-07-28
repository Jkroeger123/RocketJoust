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
        if (!gameObject)
        {
            print("JAmeE");
        }

        status += 0.3f;

        Vector3 randDir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);
        //Tween t = transform.DOPunchPosition(randDir, 0.1f).SetLink(gameObject);
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
        HideGum();
        Unsubscribe();
    }
}
