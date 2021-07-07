using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputController _input;
    
    private float maxVelocity;

    [Header("Rotation")]
    public float rotationSpeed = 250f; 
    public float superRotationMultiplier = 1.5f;
    [Header("Thrust")]
    public float velocityPerThrust = 1f;
    public float thrusterMaxVelocity = 10f;
    [Header("Blast")]
    public float thoomMaxVelocity = 65f;
    public float thoomTime = 0.5f; 
    public float thoomSlowdownDuration = 1f;
    public float blastDuration;
    [NonSerialized]
    public bool isThooming = false;
    
    public GameObject blast;
    
    
    public static event Action<GameObject> ONDeath;
    public static event Action<GameObject> ONThrust;
    public static event Action<GameObject> ONBlast; 

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _input = gameObject.GetComponent<PlayerInputController>();
        _input.ONRocketPress += OnBlast;
        _input.ONThrusterPress += OnThrust;
        maxVelocity = thrusterMaxVelocity;
    }

    private void Start () {
        SetBlastActive(false);
    }
    
    private void Update()
    {
        GetRotationInput();
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxVelocity);
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
        _rb.velocity += (Vector2)transform.up * velocityPerThrust;
        ONThrust?.Invoke(gameObject);
    }

    private void OnBlast() {
        if (isThooming) return;
        isThooming = true;
        maxVelocity = thoomMaxVelocity;
        StartCoroutine(EnableBlast());
        StartCoroutine(Thoom());
        ONBlast?.Invoke(gameObject);
    }

    private IEnumerator EnableBlast () {
        SetBlastActive(true);
        yield return new WaitForSeconds(blastDuration);
        SetBlastActive(false);
    }
    
    private IEnumerator Thoom() {
        _rb.velocity = transform.up * thoomMaxVelocity;
        yield return new WaitForSeconds(thoomTime);
        Tween tween = DOTween.To(() => 
                maxVelocity, value => maxVelocity = value, thrusterMaxVelocity, thoomSlowdownDuration)
            .OnComplete(() => isThooming = false);
    }
    
    private void SetBlastActive (bool b) { 
        blast.SetActive(b);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform)) return;

        if (other.CompareTag("Beam")) ONDeath?.Invoke(gameObject);
    }
    
    private void OnDestroy () {
        _input.ONRocketPress -= OnBlast;
        _input.ONThrusterPress -= OnThrust;
    }
}
