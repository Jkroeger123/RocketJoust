using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerFaceAnimHandler : MonoBehaviour {
    
    private Animator _animator;
    
    
    //blink is .15-.2 seconds
    void Start() {
        _animator = gameObject.GetComponent<Animator>();
        SubscribeToEvents();
        SetAnimToIdle();
    }

    private void SubscribeToEvents () {
        PlayerController.ONBlast += OnBlast;
        PlayerItemManager.ONItemPickup += OnItemPickup;
        PlayerMashHandler.ONItemEffectStart += OnItemEffectStart;
        PlayerMashHandler.ONItemEffectEnd += OnItemEffectEnd;
    }

    private void SetTimeBetweenBlinks() {
        float timeBetweenBlinks = Random.Range(1f, 3f);
        StartCoroutine(WaitForBlink(timeBetweenBlinks));
    }

    private IEnumerator WaitForBlink (float timeBetweenBlinks) {
        yield return new WaitForSeconds(timeBetweenBlinks);
        StartCoroutine(Blink());
    }

    private IEnumerator Blink () {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("FaceIdle")) yield break;
        _animator.Play("FaceBlink");
        yield return new WaitForSeconds(0.15f);
        SetAnimToIdle();
    }

    private void SetAnimToIdle () {
        StopAllCoroutines();
        _animator.Play("FaceIdle");
        SetTimeBetweenBlinks();
    }

    private void OnBlast (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("FaceDead")) return;
        StopAllCoroutines();
        StartCoroutine(BlastFace(gameObject.GetComponent<PlayerController>()));
    }

    private IEnumerator BlastFace (PlayerController pc) {
        _animator.Play("FaceBlast");

        while (pc.isThooming) yield return null;

        SetAnimToIdle();
    }

    private void OnItemPickup (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("FaceDead")) return;
        StopAllCoroutines();
        StartCoroutine(TriumphFace());
    }

    private IEnumerator TriumphFace () {
        _animator.Play("FaceTriumph");
        yield return new WaitForSeconds(1.5f);
        SetAnimToIdle();
    }

    private void OnItemEffectStart (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        StopAllCoroutines();
        _animator.Play("FaceDead");
    }

    private void OnItemEffectEnd (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        SetAnimToIdle();
    }

    private void OnDestroy () {
        StopAllCoroutines();
        PlayerController.ONBlast -= OnBlast;
        PlayerItemManager.ONItemPickup -= OnItemPickup;
        PlayerMashHandler.ONItemEffectStart -= OnItemEffectStart;
        PlayerMashHandler.ONItemEffectEnd -= OnItemEffectEnd;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
