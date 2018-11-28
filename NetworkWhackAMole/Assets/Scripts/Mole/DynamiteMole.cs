using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteMole : MoleBase {

	//Mole that will explode a short while after coming out of its hole

	private bool isExploding;

	[Header("GameObjects")]

	[SerializeField]
	private GameObject explosionParticlePrefab;

	[SerializeField]
	private GameObject dynamiteFuseParticle;

	[Header("Settings")]

	[SerializeField]
	private float explosionRadius = 9f;

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

		//Iterate through everything
		if(explosionParticlePrefab !=  null)
			Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

		Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
		for (int i = 0; i < col.Length; i++)
		{
			if(col[i].GetComponent<PlayerBase>())
			{
				col[i].GetComponent<PlayerBase>().PController.Whacked();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}

	protected override void Reset()
	{
		isExploding = false;
	}
}
