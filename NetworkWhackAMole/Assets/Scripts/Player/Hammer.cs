using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	private PlayerBase player;

	[SerializeField]
	private GameObject hitEffectPrefab;

	private Collider col;
	public Collider Col { get { return col; } }

	private void Awake()
	{
		player = transform.root.GetComponent<PlayerBase>();
		col = GetComponent<Collider>();
		col.isTrigger = true;
		col.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		//check if we hit a player
		if(other.GetComponent<PlayerBase>())
		{
			PlayerBase otherPlayer = other.GetComponent<PlayerBase>();
			//make sure the collided player isn't invincible and isn't ourselves
			if (otherPlayer.PController.IsInvincible == false && other.transform != transform.root)
			{
				SpawnHitEffect();
				otherPlayer.PController.Whacked();
				player.PController.HitStop.PlayHitStop();
			}
		}
		//check if we hit a mole
		else if(other.GetComponent<MoleBase>())
		{
			SpawnHitEffect();
			MoleBase mole = other.GetComponent<MoleBase>();
			mole.Whacked(transform.root.GetComponent<PlayerController>());
			player.PController.HitStop.PlayHitStop();
		}
	}

	/// <summary>
	/// Spawns the the hit particle system and sets a reference to the player that spawned it
	/// </summary>
	private void SpawnHitEffect()
	{
		if(PhotonNetwork.offlineMode)
		{
			GameObject go = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
			go.GetComponent<HitStopParticles>().SetSpawnedByPlayer(player);
		}
		else if(player.photonView.isMine)
		{
			GameObject go = PhotonNetwork.Instantiate(hitEffectPrefab.name, transform.position, Quaternion.identity, 0);
			go.GetComponent<HitStopParticles>().SetSpawnedByPlayer(player);
		}
	}
}
