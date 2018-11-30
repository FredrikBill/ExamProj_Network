﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteMole : MoleBase {

	//Mole that will explode a short while after coming out of its hole

	private bool isExploding;
	private bool hasExploded;

	[Header("GameObjects")]

	[SerializeField]
	private GameObject explosionParticlePrefab;

	[SerializeField]
	private GameObject dynamiteFuseParticle;

	[Header("Settings")]

	[SerializeField]
	private float explosionTime = 2;
	[SerializeField]
	private float explosionRadius = 9f;

	private float timer;

	private void Update()
	{
		if(timer < explosionTime)
		{
			timer += Time.deltaTime;
		}
		else if(!isExploding)
		{
			Explode();
		}
	}

	public override void Whacked(PlayerController player)
	{
		if (isExploding)
			return;

		Explode();
	}

	private void Explode()
	{
		isExploding = true;

		HitStop.PlayHitStop();

		if(explosionParticlePrefab !=  null)
			Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

		//Iterate through everything
		Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
		for (int i = 0; i < col.Length; i++)
		{
			if(col[i].GetComponent<PlayerBase>())
			{
				col[i].GetComponent<PlayerBase>().PController.Whacked();
			}
		}

		transform.position = spawnPos;
		anim.SetBool("Retract", true);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}

	protected override void Reset()
	{
		isExploding = false;
		timer = 0;
	}
}
