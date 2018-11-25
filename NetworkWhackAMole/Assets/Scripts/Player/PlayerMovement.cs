using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerBase {

	//Handles movement & rotation for the player 

	[Header("Movement")]

	[SerializeField]
	private float moveSpeed = 2;
	[SerializeField]
	private AnimationCurve accelCurve;
	[SerializeField]
	private float deaccelFactor = 5;
	private Vector3 velocity;
	private float currentSpeed;
	private float runTimer;
	private float gravity = -9.81f;
	private bool isMoving;
	public bool IsMoving { get { return isMoving; } }
	private float footstepTimer;
	private bool canMove = true;
	[SerializeField]
	private float footStepRate = 2.3f;

	private AudioSource footstepAudioSource;

	[HideInInspector]
	public float targetSpeed;
	public float MoveSpeed { get { return moveSpeed; } }

	[Header("Rotation")]

	[SerializeField]
	private float turnSmoothTime = 0.1f;
	private float turnSmoothVel;

	//Network variables
	private Vector3 networkPosition;
	private Quaternion networkRotation;
	private Vector3 networkMovement;
	private float networkSpeed;

	private float lastNetworkDataRecievedTime;
	private float pingInSeconds;
	private float timeSinceLastUpdate;
	private float totalTimePassed;

	private void Awake()
	{
		footstepAudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (photonView.isMine)
		{
			ProcessRotation();
			ProcessMovement();
		}
		else
		{
			ProcessNetworkMovement();
		}

		ProcessFootsteps();
	}

	private void ProcessRotation()
	{
		if(canMove)
		{
			if (PInput.InputDir != Vector2.zero)
			{
				float targetRot = Mathf.Atan2(PInput.InputDir.x, PInput.InputDir.y) * Mathf.Rad2Deg;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVel, turnSmoothTime);
			}
		}
	}

	private void ProcessMovement()
	{
		if(canMove)
		{
			if (PInput.InputDir != Vector2.zero)
			{
				runTimer += Time.deltaTime;
				PAnimator.SetBool("IsMoving", true);
			}
			else
			{
				runTimer -= Time.deltaTime * deaccelFactor;
				PAnimator.SetBool("IsMoving", false);
			}
		}
		else
		{
			runTimer = 0;
			PAnimator.SetBool("IsMoving", false);
		}

		isMoving = PAnimator.GetBool("IsMoving");

		runTimer = Mathf.Clamp01(runTimer);

		//evaluate what our speed is according to the acceleration curve
		currentSpeed = accelCurve.Evaluate(runTimer) * moveSpeed;

		velocity = (transform.forward * currentSpeed);
		velocity += Vector3.up * gravity;

		CharController.Move(velocity * Time.deltaTime);
	}

	private void ProcessNetworkMovement()
	{
		pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
		timeSinceLastUpdate = (float)(PhotonNetwork.time - lastNetworkDataRecievedTime);
		totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		Vector3 exterpolatedTargetPosition = networkPosition + (((networkPosition - transform.position).normalized * networkSpeed) * totalTimePassed);
		Vector3 newPosition = Vector3.MoveTowards(transform.position, exterpolatedTargetPosition, networkSpeed * Time.deltaTime);

		//if distance is too far, then we go immediately to the predicted position
		if (Vector3.Distance(transform.position, exterpolatedTargetPosition) >= 2f)
			newPosition = exterpolatedTargetPosition;

		transform.position = newPosition;

		//rotation
		transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, Time.deltaTime * 500f);
	}

	private void ProcessFootsteps()
	{
		//TODO make footsteps work over network, mainly setting isMoving to true
		if (Time.time > footstepTimer && isMoving)
		{
			footstepTimer = Time.time + 1 / footStepRate;
			footstepAudioSource.pitch = Random.Range(.9f, 1.1f);
			footstepAudioSource.volume = Random.Range(.85f, .9f);
			footstepAudioSource.Play();
		}
	}

	/// <summary>
	/// Sets if the player should be able to move or not
	/// </summary>
	/// <param name="active"></param>
	public void SetMovementEnabled(bool active)
	{
		canMove = active;
	}

	public override void SerializeState(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(currentSpeed);
		}
		else
		{
			networkPosition = (Vector3)stream.ReceiveNext();
			networkRotation = (Quaternion)stream.ReceiveNext();
			networkSpeed = (float)stream.ReceiveNext();

			lastNetworkDataRecievedTime = (float)info.timestamp;

			//networkMovement = transform.position - prevPosition;
			//networkPosition += (networkMovement * lag);
		}
	}
}
