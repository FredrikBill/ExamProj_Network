using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteMole : MoleBase {

	private bool isExploding;
	//Mole that will explode a short while after coming out of its hole

	public override void Whacked(PlayerController player)
	{
		if (isExploding)
			return;
	}
}
