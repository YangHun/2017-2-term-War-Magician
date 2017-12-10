using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_HP : MonoBehaviour {
    public int HP;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GetDamaged(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            GetComponent<Animator>().SetTrigger("Dead");
        }
    }
}
