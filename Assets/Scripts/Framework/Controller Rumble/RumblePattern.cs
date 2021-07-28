using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rumble Pattern", menuName = "Rumble Pattern")]
public class RumblePattern : ScriptableObject
{
    public int priority;
    public float length;
    public float intensity;
    public int repeatCount;
    
    [HideIf("ShowFunc")]
    public float timeBetweenRepeats;

    private bool ShowFunc() => repeatCount == 0;
}