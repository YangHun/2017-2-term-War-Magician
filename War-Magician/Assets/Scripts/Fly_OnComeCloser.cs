using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_OnComeCloser : StateMachineBehaviour {
    float speed;
    float rotateSpeed = 10f;
    GameObject Target;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Target = animator.GetComponent<AI_AIR>().target;
        speed = animator.GetComponent<AI_AIR>().speed;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector3 dir = Target.transform.position - animator.transform.position;
        if (animator.tag != "Boss")
        {
            if (dir.magnitude < 6f)
            {
                animator.SetTrigger("Attack");
                return;
            }
        }
        else
        {
            if (dir.magnitude < 0.5f)
            {
                animator.SetTrigger("Attack");
                return;
            }
        }
        animator.transform.LookAt(Target.transform.position);
        animator.GetComponent<CharacterController>().Move(dir * speed * Time.deltaTime);
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
