﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBase : Photon.MonoBehaviour {

	[Header("Base Defaults")]

	[SerializeField]
	private GameObject dirtEffectPrefab;

	protected Transform targetHole;
	public Transform TargetHole { get { return targetHole; } }

	protected Vector3 spawnPos;
	public Vector3 SpawnPos { get { return spawnPos; } }

	protected Animator anim;
	public Animator Anim { get { return anim; } }
	protected Collider col;

	private HitStopPlayer hitStop;
	public HitStopPlayer HitStop { get { return hitStop; } }

	private void Awake()
	{
		//Get components
		anim = GetComponent<Animator>();
		col = GetComponent<Collider>();
		hitStop = GetComponent<HitStopPlayer>();
		//set spawn position and rotate to face the camera
		spawnPos = transform.position;
		transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		//subscribe to events
		if(MoleManager.Instance)
		{
			MoleManager.Instance.onRaiseMoles += RiseUp;
			MoleManager.Instance.onRetractMoles += Retract;
		}
	}

	public virtual void SetTargetHole(Transform hole)
	{
		targetHole = hole;
	}

	public virtual void Whacked(PlayerController player)
	{

	}

	public virtual void RiseUp()
	{
		//if target hole is null, means that the manager hasn't given us one because we aren't supposed to go up yet.
		if (targetHole == null)
			return;

		transform.position = targetHole.position;
		col.enabled = true;
		Reset();
		//Play animation
		anim.SetBool("RiseUp", true);
		SpawnParticle(dirtEffectPrefab);
	}

	public virtual void Retract()
	{
		//collision disabled so that you can't hit a mole while they are retracting
		col.enabled = false;
		anim.SetBool("Retract", true);
		SpawnParticle(dirtEffectPrefab);
	}

	protected virtual void Reset()
	{

	}

	protected void SpawnParticle(GameObject particlePrefab)
	{
		if (PhotonNetwork.offlineMode)
			Instantiate(particlePrefab, transform.position, Quaternion.identity);
		else
			PhotonNetwork.InstantiateSceneObject(particlePrefab.name, transform.position, Quaternion.identity, 0, null);
	}

	[PunRPC]
	protected void RPCSetAnimWhacked()
	{
		anim.SetTrigger("W");
	}
}
