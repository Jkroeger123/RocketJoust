using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerFaceAnimHandler : MonoBehaviour {
    private Animator _animator;
    // Start is called before the first frame update
    //blink is .15-.2 seconds
    void Start() {
        _animator = gameObject.GetComponent<Animator>();
        PlayerController.ONBlast += InitiateBlastFace;
        ReturnToIdle();
    }

    // Update is called once per frame
    void Update() {

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
        ReturnToIdle();
    }

    private void ReturnToIdle () {
        StopAllCoroutines();
        _animator.Play("FaceIdle");
        SetTimeBetweenBlinks();
    }

    private void InitiateBlastFace (GameObject gameObject) {
        if (gameObject != transform.parent.gameObject) return;
        PlayerController pc = gameObject.GetComponent<PlayerController>();
        float duration = pc.thoomTime + pc.thoomSlowdownDuration;
        StartCoroutine(BlastFace(duration));
    }

    private IEnumerator BlastFace (float duration) {
        _animator.Play("FaceBlast");
        yield return new WaitForSeconds(duration);
        ReturnToIdle();
    }

    private void OnDestroy () {
        PlayerController.ONBlast -= InitiateBlastFace;
    }
}
