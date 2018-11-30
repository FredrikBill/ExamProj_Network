using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrikeState : StateMachineBehaviour {

	PlayerBase player;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		player = animator.GetComponent<PlayerBase>();
		player.PMovement.SetMovementEnabled(false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		player.PAnimator.SetBool("Whack", false);
		player.PMovement.SetMovementEnabled(true);
		player.PController.DisableHammerHitbox();
	}
}
