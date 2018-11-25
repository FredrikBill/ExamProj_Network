using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : PlayerBase {

	//Handles all the collisions for the player

	private void OnTriggerEnter(Collider other)
	{
		//Potential solution for handling collisions with the hammer, to be determined

		//if(other.GetComponentInChildren<Hammer>() != null && !PController.IsInvincible)
		//{
		//	//other.GetComponentInChildren<Hammer>()
		//	Debug.Log("Got hit");
		//	PController.Whacked();
		//}

		//print(gameObject.name + " collided with: " + other.gameObject.name);

		////check if it's a hammer, if we aren't invincible and the hammer doesn't belong to us
		//if(other.tag == "Hammer" && PController.IsInvincible == false && other.transform.root != transform)
		//{
		//	Debug.Log("Got hit");
		//	PController.Whacked();
		//}
	}
}
