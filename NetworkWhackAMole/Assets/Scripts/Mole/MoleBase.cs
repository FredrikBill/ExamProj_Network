using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBase : Photon.MonoBehaviour {

	protected Transform targetHole;
	public Transform TargetHole { get { return targetHole; } }

	protected Vector3 spawnPos;

	protected Animator anim;
	public Animator Anim { get { return anim; } }
	protected Collider col;

	private void Awake()
	{
		//Get components
		anim = GetComponent<Animator>();
		col = GetComponent<Collider>();
		//set spawn position and rotate to face the camera
		spawnPos = transform.position;
		transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		//subscribe to events
		MoleManager.Instance.onRaiseMoles += RiseUp;
		MoleManager.Instance.onRetractMoles += Retract;
	}

	public virtual void SetTargetHole(Transform hole)
	{
		targetHole = hole;
	}

	public virtual void Whacked(PlayerController player)
	{

	}

	public void RiseUp()
	{
		//if target hole is null, mans that the manager hasn't given us one because we aren't supposed to go up yet.
		if (targetHole == null)
			return;

		transform.position = targetHole.position;
		col.enabled = true;
		//Play animation
		anim.SetBool("RiseUp", true);
	}

	public void Retract()
	{
		//collision disabled so that you can't hit a mole while they are retracting
		col.enabled = false;
		anim.SetBool("Retract", true);
	}

	public virtual void Reset()
	{

	}
}
