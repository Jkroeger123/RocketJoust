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
        PlayerController.ONBlast += InitiateBlastFace;
        SetAnimToIdle();
    }

    private void SetTimeBetweenBlinks() {
        float timeBetweenBlinks = Random.Range(1f, 5f);
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

    private void InitiateBlastFace (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        StartCoroutine(BlastFace(gameObject.GetComponent<PlayerController>()));
    }

    private IEnumerator BlastFace (PlayerController pc) {
        _animator.Play("FaceBlast");

        while (pc.isThooming) yield return null;

        SetAnimToIdle();
    }

    private void OnDestroy () {
        PlayerController.ONBlast -= InitiateBlastFace;
    }
}
