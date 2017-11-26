using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTD : MonoBehaviour {

    public Transform rightIndexBone3;
    private LineRenderer _renderer;
    public float distance;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<LineRenderer>();		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = rightIndexBone3.forward;

        _renderer.SetPosition(0, rightIndexBone3.position);
        _renderer.SetPosition(1, pos * distance + rightIndexBone3.position);

	}
}
