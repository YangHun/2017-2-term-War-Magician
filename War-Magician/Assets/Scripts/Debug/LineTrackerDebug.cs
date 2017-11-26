using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrackerDebug : MonoBehaviour {


    public Transform RightIndexBone3;
    public Transform RightIndexBone2;
    LineRenderer _renderer;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<LineRenderer>();
        
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = (RightIndexBone3.transform.position - RightIndexBone2.transform.position).normalized;
        _renderer.SetPosition(0, RightIndexBone3.transform.position);
        _renderer.SetPosition(1, pos + RightIndexBone3.transform.position);

	}
}
