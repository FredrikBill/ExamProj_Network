using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMole : MoleBase {

	//The mole that players should be on the look out for

	private Transform previousHole;
	public Transform PreviousHole { get { return previousHole; } }

	[SerializeField]
	private int scorePerHit = 2;

	public override void SetTargetHole(Transform hole)
	{
		targetHole = hole;

		previousHole = targetHole;
	}

	public override void Whacked(PlayerController player)
	{
		if(player.photonView.isMine)
		{
			//send player ui an message to increase their score
			PlayerUI.LocalPlayerUi.IncreaseScore(scorePerHit);
			animator.SetBool("Whacked", true);
		}
	}

}
