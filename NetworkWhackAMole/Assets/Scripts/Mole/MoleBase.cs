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
		spawnPos = transform.position;
		MoleManager.Instance.onRaiseMoles += RiseUp;
		MoleManager.Instance.onRetractMoles += Retract;
		animator = GetComponent<Animator>();
	}

	public virtual void SetTargetHole(Transform hole)
	{
		targetHole = hole;
	}

	public virtual void Whacked()
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
}
