using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnParticleCollision(GameObject obj)
    {
        if (obj.tag == "FieldMonster")
        {
            obj.GetComponent<Monster_HP>().GetDamaged(20);
        }

        Debug.Log(obj.tag);
        
    }


}
