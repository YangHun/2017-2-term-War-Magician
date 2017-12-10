using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "FieldMonster")
        {
            other.GetComponent<Monster_HP>().GetDamaged(20);
        }
    }

    private void OnParticleTrigger()
    {
        
    }

}
