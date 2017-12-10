using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObjectManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Destroyed(GameObject obj)
    {
        obj.GetComponent<Animator>().SetTrigger("Destroyed");
        Debug.Log("ha");
    }
    public void Restructed(GameObject obj)
    {
        obj.GetComponent<Animator>().SetTrigger("Restruct");
    }
}
