using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

	private Transform target;

	private void Awake()
	{
		target = PlayerController.LocalPlayer.transform;
		if (target == null)
			Debug.Log("Can't find the player");
	}

	private void LateUpdate()
	{

	}
}
