using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleState : StateMachineBehaviour {

	//Turns the anim parameter to false when exiting the state

	[SerializeField, Tooltip("The animation parameter it will turn false when exiting")]
	private MoleStates onExit;


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool(onExit.ToString(), false);

		if(animator.GetComponent<BigMole>() && onExit == MoleStates.Retract)
		{
			if(PhotonNetwork.isMasterClient || PhotonNetwork.offlineMode)
			{
				animator.GetComponent<BigMole>().InvokeOnRetracted();
			}
		}
		//else if (animator.GetComponent<DynamiteMole>() && onExit == MoleStates.Retract)
		//{
		//	animator.transform.position = animator.GetComponent<MoleBase>().SpawnPos;
		//}
	}
}

public enum MoleStates
{
	Idle, RiseUp, Retract, Whacked
}