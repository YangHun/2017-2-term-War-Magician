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
        UIManager.I.UpdateManaIcon(obj.GetComponent<ManaObject>().ID, false);
    }
    public void Restructed(GameObject obj)
    {
        obj.GetComponent<Animator>().SetTrigger("Restruct");
        UIManager.I.UpdateManaIcon(obj.GetComponent<ManaObject>().ID, true);
    }
}
