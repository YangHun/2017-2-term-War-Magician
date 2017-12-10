using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_HP : MonoBehaviour {
    public int HP;
    public bool alreadyDead = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!alreadyDead)
        {
            if (HP <= 0)
            {
                GetComponent<Animator>().SetTrigger("Dead");
                alreadyDead = true;
            }
        }
    }
    public void GetDamaged(int damage)
    {
        HP -= damage;
        
    }
}
