using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {
    public GameObject GFM;

    public int HP = 100;
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(HP <= 0)
        {
            GFM.GetComponent<GameFlowManager>().Transition(HelenaStateType.MainGame_GameOver);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FieldMonster"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Dead");
            HP--;
        }
    }
}
