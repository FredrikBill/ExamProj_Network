using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopParticles : MonoBehaviour {

	//the player who spawned us
	private PlayerBase player;

	public void SetSpawnedByPlayer(PlayerBase p)
	{
		player = p;
		player.PController.HitStop.onUpdateSpeed += UpdateParticleSpeed;
	}

	private void UpdateParticleSpeed(float newSpeed)
	{
		ParticleSystem.MainModule main;
		if (transform.GetComponent<ParticleSystem>())
		{
			main = transform.GetComponent<ParticleSystem>().main;
			main.simulationSpeed = newSpeed;
		}

		//ParticleSystem.MainModule[] mainInChildren = transform.GetComponentsInChildren<ParticleSystem.MainModule>();
		//if(mainInChildren.Length > 0)
		//{
		//	for (int i = 0; i < mainInChildren.Length; i++)
		//	{
		//		mainInChildren[i].simulationSpeed = newSpeed;
		//	}
		//}
	}

	private void OnDisable()
	{
		if(player != null)
			player.PController.HitStop.onUpdateSpeed -= UpdateParticleSpeed;
	}
}
