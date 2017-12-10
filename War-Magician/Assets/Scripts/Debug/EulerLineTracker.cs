using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class EulerLineTracker : MonoBehaviour {

    LineRenderer _renderer;

    
    public Transform RightIndexBone3;
    public InnerCircle innerCircle;

    public float linedistance = 0.001f;


	// Use this for initialization
	void Start () {
        _renderer = GetComponent<LineRenderer>();

        if (_renderer == null)
            return;        
    }
	

    public void OnLIndexTrigger() //Stay
    {
        _renderer.SetPosition(_renderer.positionCount - 1, RightIndexBone3.transform.position);
    }

    public void OnLIndexTriggerDown()
    {
        _renderer.positionCount = 1;
        _renderer.materials[0].SetColor("_TintColor", Color.white);
        
    }
    
    public void SetNextPosition(Vector3 n, Vector3 d)
    {

        _renderer.SetPosition(_renderer.positionCount - 1, n - d * linedistance);
        _renderer.positionCount++;
    }

}
