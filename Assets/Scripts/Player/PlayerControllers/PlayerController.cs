using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 250f; 
    public float superRotationMultiplier = 1.5f;
    
    [Header("Thrust")]
    public float velocityPerThrust = 1f;
    public float thrusterMaxVelocity = 10f;
    
    [Header("Blast")]
    public int blastCap;
    public float thoomMaxVelocity = 65f;
    public float thoomTime = 0.5f; 
    public float thoomSlowdownDuration = 1f;
    public float blastDuration;
    public float blastCooldown = 0.2f;
    public GameObject blast;
    public GameObject blastUI;
    
    public bool isThooming = false;

    private BlastUI _blastUI;
    
    private Rigidbody2D _rb;
    private PlayerInputController _input;
    private bool _isInvincible;
    private float _maxVelocity;
    private float _timer;
    private int _blastCount;
    
    private Coroutine _blastRoutine;
    private Tween _blastTween;
    
    public static event Action<GameObject> ONThrust; 
    public static event Action<GameObject> ONBlast; 

    private void Awake()
    {
        _blastCount = blastCap;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _input = gameObject.GetComponent<PlayerInputController>();
        _input.ONRocketPress += OnBlast;
        _input.ONThrusterPress += OnThrust;
        _maxVelocity = thrusterMaxVelocity;
        _blastUI = Instantiate(blastUI).transform.GetChild(0).GetComponent<BlastUI>();
        _blastUI.Initialize(blastCap, gameObject);
        _blastUI.IsHidden = true;
    }

    private void Start () {
        SetBlastActive(false);
    }
    
    private void Update()
    {
        GetRotationInput();
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxVelocity);
        UpdateBlastUI();
    }

    private void UpdateBlastUI()
    {
        if (_blastCount != blastCap)
        {
            _blastUI.IsHidden = false;
        }
        
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            
            if (_blastCount == blastCap)
            {
                _blastUI.IsHidden = true;
            }
            
            _blastCount = Mathf.Clamp(_blastCount + 1, 0, blastCap);
            _timer = blastCooldown;
        }
        
        _blastUI.UpdateBlastUI(_blastCount, Mathf.Clamp(_timer/blastCooldown, 0, 1));
        
    }

    #region Rotation
    private void GetRotationInput () {
        if (_input.rotateRightSuperPressed)
        {
            RotateRightSuper();
        }else if (_input.rotateRightPressed)
        {
            RotateRight();
        }

        if (_input.rotateLeftSuperPressed)
        {
            RotateLeftSuper();
        } else if (_input.rotateLeftPressed)
        {
            RotateLeft();
        }
    }
    
    private void RotateRight () {
        _rb.rotation -= rotationSpeed * Time.deltaTime;
    }

    private void RotateRightSuper() {
        _rb.rotation -= rotationSpeed * superRotationMultiplier * Time.deltaTime;
    }

    private void RotateLeft () {
        _rb.rotation += rotationSpeed * Time.deltaTime;
    }

    private void RotateLeftSuper()
    {
        _rb.rotation += rotationSpeed * superRotationMultiplier * Time.deltaTime;
    }
    
    #endregion

    #region Action
    private void OnThrust()
    {
        if (!BattleManager.canMove) return;
        
        _rb.velocity += (Vector2)transform.up * velocityPerThrust;
        ONThrust?.Invoke(gameObject);
    }

    private void OnBlast() {
        
        if (!BattleManager.canMove) return;
        if (_blastCount <= 0) return;
        
        if(_blastRoutine != null) StopCoroutine(_blastRoutine);
        _blastTween?.Kill();

        _timer = blastCooldown;
        _blastCount--;
        
        StartCoroutine(EnableBlast());
        _blastRoutine = StartCoroutine(Thoom());
        
        ONBlast?.Invoke(gameObject);
    }

    private IEnumerator EnableBlast () {
        SetBlastActive(true);
        yield return new WaitForSeconds(blastDuration);
        SetBlastActive(false);
    }
    
    private IEnumerator Thoom()
    {

        isThooming = true;
        _maxVelocity = thoomMaxVelocity;
        
        _rb.velocity = transform.up * _maxVelocity;

        float timer = 0;
        
        while (timer <= thoomTime)
        {
            timer += Time.deltaTime;

            if (_rb.velocity.magnitude < (_maxVelocity - 3f))
            {
                _maxVelocity = thrusterMaxVelocity;
                isThooming = false;
                yield break;
            }

            yield return null;
        }

        _blastTween = DOTween.To(() => 
                _maxVelocity, value => _maxVelocity = value, thrusterMaxVelocity, thoomSlowdownDuration)
            .OnComplete(() => isThooming = false);
    }
    
    private void SetBlastActive (bool b) { 
        blast.SetActive(b);
    }
    #endregion
    

    private void OnDestroy () {
        _input.ONRocketPress -= OnBlast;
        _input.ONThrusterPress -= OnThrust;
    }
}
