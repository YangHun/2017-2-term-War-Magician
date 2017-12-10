using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Field_OnDead : StateMachineBehaviour {
    public float deathWaitTime = 1f;
    float timeCounter = 0f;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<NavMeshAgent>().enabled = false;
        GameObject.Find("MonsterManager").GetComponent<MonsterSpawner>().NumOfMonster--;
        if(animator.GetComponent<Totem>() != null)
        {
            animator.GetComponent<Totem>().isDead = true;
        }
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        timeCounter += Time.deltaTime;
        if(timeCounter >= deathWaitTime)
        {
            animator.transform.localScale = new Vector3(0f, 0f, 0f);
            animator.SetTrigger("Purgatory");
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
