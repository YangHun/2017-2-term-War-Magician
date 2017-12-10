using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AIR : MonoBehaviour {
    public GameObject target;
    public float speed;
    bool isInitialized = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!isInitialized && target != null)
        {
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetTrigger("Come Closer");
            }
            isInitialized = true;
        }
        /*
        if (Vector3.Distance(target.transform.position, transform.position) < 2f)
        {
            return;
        }
        else
        {
            GetComponent<CharacterController>().Move((target.transform.position - transform.position) * speed * Time.deltaTime);

        }*/
    }
    public void Init()
    {

    }
}
