using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBase : MonoBehaviour {

	protected Transform targetHole;
	public Transform TargetHole { get { return targetHole; } }

	protected Vector3 spawnPos;

	protected Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
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
		transform.position = targetHole.position;
		//Play animation
		animator.SetBool("RiseUp", true);
	}

	public void Retract()
	{
		animator.SetBool("Retract", true);
	}

	public virtual void Reset()
	{

	}
}
