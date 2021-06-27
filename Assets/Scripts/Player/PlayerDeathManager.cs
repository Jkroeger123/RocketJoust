using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour {
	
	public static event Action<GameObject> onDeath;
	
	public void Die () 
	{
		onDeath?.Invoke(gameObject);
	}
}
