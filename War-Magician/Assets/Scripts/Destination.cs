using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {
    public GameObject GFM;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FieldMonster"))
        {
            GFM.GetComponent<GameFlowManager>().Transition(HelenaStateType.MainGame_GameOver);
        }
    }
}
