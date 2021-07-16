using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rumble Pattern", menuName = "Rumble Pattern")]
public class RumblePattern : ScriptableObject
{
    public int priority;
    public bool looping;
    public float length;
    public float intensity;
}