using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Totem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "FieldMonster")
        {
            other.GetComponent<NavMeshAgent>().speed = other.GetComponent<AI_FIELD>().originSpeed * 1.5f;
            other.GetComponent<Animator>().SetTrigger("Run");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FieldMonster")
        {
            other.GetComponent<NavMeshAgent>().speed = other.GetComponent<AI_FIELD>().originSpeed * 1.5f;
            other.GetComponent<Animator>().SetTrigger("Run");
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
