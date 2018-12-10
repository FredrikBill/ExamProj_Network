using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopParticles : MonoBehaviour {

	//the player who spawned us
	private PlayerBase player;

	private List<ParticleSystem.MainModule> particleMains = new List<ParticleSystem.MainModule>();

	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<ParticleSystem>())
				particleMains.Add(transform.GetChild(i).GetComponent<ParticleSystem>().main);
		}
	}

	public void SetSpawnedByPlayer(PlayerBase p)
	{
		player = p;
		player.PController.HitStop.onUpdateSpeed += UpdateParticleSpeed;
	}

	private void UpdateParticleSpeed(float newSpeed)
	{
		ParticleSystem.MainModule main;
		//set the particle systems speed on this transform
		if (transform.GetComponent<ParticleSystem>())
		{
			main = transform.GetComponent<ParticleSystem>().main;
			main.simulationSpeed = newSpeed;
		}

		//set all the particle systems speed in this transforms children
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<ParticleSystem>())
			{
				main = transform.GetChild(i).GetComponent<ParticleSystem>().main;
				main.simulationSpeed = newSpeed;
			}
		}
	}

	private void OnDisable()
	{
		if(player != null)
			player.PController.HitStop.onUpdateSpeed -= UpdateParticleSpeed;
	}
}
