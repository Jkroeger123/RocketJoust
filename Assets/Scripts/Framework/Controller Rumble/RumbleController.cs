using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleController : MonoBehaviour
{
    [SerializeField] RumblePattern thrustRumble;
    [SerializeField] RumblePattern blastRumble;

    private RumblePattern _activePattern;
    private Coroutine _currentRumble;
    private PlayerInput _playerInput;
    private Gamepad _playerGamepad;
    
    private void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        _playerGamepad = GetPlayerGamepad();
        PlayerController.ONBlast += ONBlast;
        PlayerController.ONThrust += ONThrust;
    }

    private void Rumble(RumblePattern pattern) 
    {
        _currentRumble = StartRumble(pattern);
    }

    //Resets the controller rumble to 0 and stops the current pattern
    private void StopRumble() 
    {
        if (_currentRumble == null || _playerGamepad == null) return;

        StopCoroutine(_currentRumble);
        _currentRumble = null;
        if (_playerGamepad != null) _playerGamepad.SetMotorSpeeds(0f, 0f);
    }

    //This determines if the new pattern should override the old pattern and behaves accordingly 
    private Coroutine StartRumble(RumblePattern pattern)
    {

        if (_playerGamepad == null) return null;

        if (_activePattern == null)
        {
            _currentRumble = StartCoroutine(PlayRumble(pattern));
            _activePattern = pattern;
            return _currentRumble;
        }

        //if the current rumble pattern has a higher priority, dont play the new one
        if (_activePattern.priority > pattern.priority) return null;

        //if the new pattern does have a higher priority, stop the old pattern and start the new one
        StopAllCoroutines();
        _currentRumble = StartCoroutine(PlayRumble(pattern));
        _activePattern = pattern;
        return _currentRumble;
    }
    

    //This runs the actual rumble pattern, currently this only supports constant and looping
    private IEnumerator PlayRumble(RumblePattern pattern)
    {
        //If a gamepad is not connected, do not run the coroutine
        if (_playerGamepad == null) yield break;

        _playerGamepad.SetMotorSpeeds(pattern.intensity, pattern.intensity);
        yield return new WaitForSecondsRealtime(pattern.length);

        //If the pattern is set to looping, start another coroutine.
        if (pattern.looping)
        {
            _currentRumble = StartCoroutine(PlayRumble(pattern));
        }
        else
        {
            _playerGamepad.SetMotorSpeeds(0f, 0f);
            _activePattern = null;
        }
    }

    private Gamepad GetPlayerGamepad()
    {
        return Gamepad.all.FirstOrDefault(g =>
            _playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }

    //***** Events / Callbacks *****

    private void ONBlast(GameObject obj)
    {
        if (obj != gameObject) return;
        Rumble(blastRumble);
    }

    private void ONThrust(GameObject obj)
    {
        if (obj != gameObject) return;
        Rumble(thrustRumble);
    }

    private void Unsubscribe()
    {
        PlayerController.ONBlast -= ONBlast;
        PlayerController.ONThrust -= ONThrust;
    }

    private void OnDestroy()
    {
        if (_playerGamepad != null) _playerGamepad.SetMotorSpeeds(0f, 0f);
        Unsubscribe();
    }

    private void OnDisable()
    {
        if (_playerGamepad != null) _playerGamepad.SetMotorSpeeds(0f, 0f);
    }
}
