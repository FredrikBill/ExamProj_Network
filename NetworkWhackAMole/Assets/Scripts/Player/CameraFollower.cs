using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float followSpeed = 2f;
	private float rotationSpeed = 2;
	[SerializeField]
	private Vector3 positionOffset;
	private Vector3 targetPos;
	private float playerHalfHeight = 1f;
	private Vector3 relativePos;
	Quaternion lookRot;
	private float cameraHeight;
	private float zoom;
	private Vector3 velocity;

	private void Awake()
	{
		//GameManager.Instance.onPlayerSpawned += SetTarget;
		//target = PlayerController.LocalPlayer.transform;
		//if (target == null)
		//	Debug.Log("Couldn't find the player");
		//else if(target.GetComponent<CharacterController>())
		//	playerHalfHeight = target.GetComponent<CharacterController>().height / 2;
	}

	private void OnEnable()
	{
		GameManager.Instance.onPlayerSpawned += SetTarget;
	}

	private void SetTarget()
	{
		print("it happen");
		target = PlayerController.LocalPlayer.transform;
		if (target.GetComponent<CharacterController>())
			playerHalfHeight = target.GetComponent<CharacterController>().height / 2;
	}

	private void Update()
	{
		if (target == null)
			target = PlayerController.LocalPlayer.transform;
	}

	private void LateUpdate()
	{
		relativePos = target.position - transform.position;


		targetPos = target.transform.position - (target.transform.forward * zoom) + (Vector3.up * cameraHeight) + positionOffset;

		transform.position = Vector3.SmoothDamp(
		transform.position,
		targetPos,
		ref velocity,
		followSpeed);
	}
}
