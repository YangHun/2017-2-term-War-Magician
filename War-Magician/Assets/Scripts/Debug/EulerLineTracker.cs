using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class EulerLineTracker : MonoBehaviour {

    LineRenderer _renderer;

    
    public Transform RightIndexBone3;
    public InnerCircle innerCircle;

    public float linedistance = 0.001f;

    bool LeftTriggerButtonDown = false;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<LineRenderer>();

        if (_renderer == null)
            return;        
    }
	
	// Update is called once per frame
	void Update () {
		
        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) == 0 || Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (LeftTriggerButtonDown == true)
            {
                LeftTriggerButtonDown = false;
            }
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) < 0.9f || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (LeftTriggerButtonDown == false)
            {
                _renderer.positionCount = 1;
                _renderer.materials[0].SetColor("_TintColor", Color.white);
                LeftTriggerButtonDown = true;
            } 
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) >= 0.9f|| Input.GetKey(KeyCode.LeftControl))
        {
           
                _renderer.SetPosition(_renderer.positionCount - 1, RightIndexBone3.transform.position);
           
        }
        else
        {

        }

	}

    public void SetNextPosition(Vector3 n, Vector3 d)
    {
        _renderer.SetPosition(_renderer.positionCount - 1, n - d * linedistance);
        _renderer.positionCount++;
    }

}
