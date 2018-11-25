using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	private Collider col;
	public Collider Col { get { return col; } }

	private void Awake()
	{
		col = GetComponent<Collider>();
		col.isTrigger = true;
		col.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerBase>())
		{
			PlayerBase player = other.GetComponent<PlayerBase>();
			if (player.PController.IsInvincible == false && other.transform != transform.root)
			{
				player.PController.Whacked();
			}
		}
		else if(other.GetComponent<MoleBase>())
		{
			MoleBase mole = other.GetComponent<MoleBase>();
			mole.Whacked(transform.root.GetComponent<PlayerController>());
		}
	}
}
