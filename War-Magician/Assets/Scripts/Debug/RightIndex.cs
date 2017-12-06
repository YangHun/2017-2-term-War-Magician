using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightIndex : MonoBehaviour {

    public EulerLineTracker line;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider col)
    {
        if(col.tag == "Point")
        {
            MagicCircleDrawManager.I.TriggerAction(col);
        }
           
        if(col.name == "Points")
        {
            GetComponentInChildren<ParticleSystem>().Play();
        }
    
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Points")
        {
            GetComponentInChildren<ParticleSystem>().Stop();
        }

    }

}
