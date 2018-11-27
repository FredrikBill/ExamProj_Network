using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleState : StateMachineBehaviour {

	[SerializeField, Tooltip("The animation parameter it will turn false when exiting")]
	private MoleStates onExit;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool(onExit.ToString(), false);
		if(animator.GetComponent<BigMole>() && onExit == MoleStates.Retract)
		{
			animator.GetComponent<BigMole>().onBigMoleRetracted.Invoke();
		}
	}
}

public enum MoleStates
{
	Idle, RiseUp, Retract, Whacked
}