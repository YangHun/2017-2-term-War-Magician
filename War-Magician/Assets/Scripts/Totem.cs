using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Totem : MonoBehaviour {
    public bool isDead = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if ( other.tag == "FieldMonster")
        {
            if (!isDead)
            {
                other.GetComponent<NavMeshAgent>().speed = other.GetComponent<AI_FIELD>().originSpeed * 3f;
                other.GetComponent<Animator>().SetTrigger("Run");
            }
            else
            {
                other.GetComponent<NavMeshAgent>().speed = other.GetComponent<AI_FIELD>().originSpeed * 1f;
                other.GetComponent<Animator>().SetTrigger("Walk");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FieldMonster")
        {
            other.GetComponent<NavMeshAgent>().speed = other.GetComponent<AI_FIELD>().originSpeed * 1f;
            other.GetComponent<Animator>().SetTrigger("Walk");
        }
    }
}
