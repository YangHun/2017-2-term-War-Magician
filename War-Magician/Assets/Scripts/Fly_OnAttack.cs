using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_OnAttack : StateMachineBehaviour {
    public GameObject bolt;
    GameObject Target;
    float delay_Attack = 10f;
    float timeCounter_Attack = 8f;
    float boltSpeed = 3f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Target = animator.GetComponent<AI_AIR>().target;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        timeCounter_Attack += Time.deltaTime;
        if(animator.tag != "Boss" && timeCounter_Attack >= delay_Attack)
        {
            GameObject tempBolt = Instantiate(bolt);
            tempBolt.transform.position = animator.transform.position + (animator.transform.forward);
            tempBolt.transform.LookAt(Target.transform.position);
            tempBolt.GetComponent<Rigidbody>().velocity = boltSpeed * tempBolt.transform.forward;
            timeCounter_Attack = 0f;
        }
        else if (animator.tag == "Boss" && timeCounter_Attack >= delay_Attack)
        {
            // GameOver trigger
            timeCounter_Attack = 0f;
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
