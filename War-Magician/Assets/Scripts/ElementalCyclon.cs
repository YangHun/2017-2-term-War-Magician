using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalCyclon : MonoBehaviour {

    public Vector3 forward;
    public float speed;

	// Use this for initialization
	void Start () {

        if (forward == null)
            forward = transform.forward;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position +  forward * Time.deltaTime * speed;
	}
}
