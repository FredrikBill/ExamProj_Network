using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerBase {

	//Handles movement & rotation for the player 


	[Header("Movement")]

	[SerializeField]
	private float moveSpeed = 2;
	[SerializeField]
	private float stunnedMoveSpeed = 1;
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
	[SerializeField]
	private float footStepRate = 2.3f;

	private AudioSource footstepAudioSource;

	[HideInInspector]
	public float targetSpeed;
	public float MoveSpeed { get { return moveSpeed; } }
	public float StunnedMoveSpeed { get { return stunnedMoveSpeed; } }

	[Header("Rotation")]

	[SerializeField]
	private float turnSmoothTime = 0.1f;
	private float turnSmoothVel;

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

			ProcessFootsteps();
	}

	private void ProcessRotation()
	{
		if (PInput.InputDir != Vector2.zero)
		{
			float targetRot = Mathf.Atan2(PInput.InputDir.x, PInput.InputDir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVel, turnSmoothTime);
		}
	}

	private void ProcessMovement()
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

		isMoving = PAnimator.GetBool("IsMoving");

		runTimer = Mathf.Clamp01(runTimer);

		currentSpeed = accelCurve.Evaluate(runTimer) * moveSpeed;

		velocity = (transform.forward * currentSpeed);
		velocity += Vector3.up * gravity;

		CharController.Move(velocity * Time.deltaTime);
	}

	private void ProcessFootsteps()
	{
		if (Time.time > footstepTimer && isMoving)
		{
			footstepTimer = Time.time + 1 / footStepRate;
			footstepAudioSource.Play();
		}
	}

	public override void SerializeState(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{

		}
		else
		{

		}
	}
}
