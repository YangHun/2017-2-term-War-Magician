using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETop : MonoBehaviour {

    public GameObject Target;

    private void OnEnable()
    {
        //Init

        Target.SetActive(true);
        Target.transform.position = transform.position + transform.forward * 10.0f;
    }

    // Use this for initialization
    void Start () {
	}
	
    public void OnLThumbStick()
    {

    }

    public void OnLIndexTriggerDown()
    {

    }

	// Update is called once per frame
	void Update () {
		
	}
}
