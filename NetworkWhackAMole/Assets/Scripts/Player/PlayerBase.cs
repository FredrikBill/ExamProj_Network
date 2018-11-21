using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Photon.MonoBehaviour, IPunObservable {

	//Base class that is inherited by all the 'player' components.
	//Contains references to all other components.
	//Also calls on OnPhotonSerialize on the inherited components.

	private CharacterController charController;
	public CharacterController CharController
	{
		get
		{
			if (charController == null)
				charController = GetComponent<CharacterController>();

			return charController;
		}
	}

	private PlayerController pController;
	public PlayerController PController
	{
		get
		{
			if (pController == null)
				pController = GetComponent<PlayerController>();

			return pController;
		}
	}

	private PlayerMovement pMovement;
	public PlayerMovement PMovement
	{
		get
		{
			if (pMovement == null)
				pMovement = GetComponent<PlayerMovement>();

			return pMovement;
		}
	}

	private PlayerInput pInput;
	public PlayerInput PInput
	{
		get
		{
			if (pInput == null)
				pInput = GetComponent<PlayerInput>();

			return pInput;
		}
	}

	private PlayerCollision pCollision;
	public PlayerCollision PCollision
	{
		get
		{
			if (pCollision == null)
				pCollision.GetComponent<PlayerCollision>();

			return pCollision;
		}
	}

	private PlayerHealth pHealth;
	public PlayerHealth PHealth
	{
		get
		{
			if (pHealth == null)
				pHealth = GetComponent<PlayerHealth>();

			return pHealth;
		}
	}

	private Animator pAnimator;
	public Animator PAnimator
	{
		get
		{
			if (pAnimator == null)
				pAnimator = GetComponent<Animator>();

			return pAnimator;
		}
	}

	protected float lag;

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
		PMovement.SerializeState(stream, info);
		PController.SerializeState(stream, info);
	}

	public virtual void SerializeState(PhotonStream stream, PhotonMessageInfo info)
	{

	}
}
