using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour {

    LineRenderer _renderer;
    public float maxdistance;

	// Use this for initialization
	void Start () {
        _renderer = GetComponentInChildren<LineRenderer>();
        _renderer.SetPosition(1, _renderer.GetPosition(0));
	}

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit))
        {
            _renderer.SetPosition(1, Vector3.forward * (transform.position - hit.point).magnitude);
        }
        else
        {
            _renderer.SetPosition(1, Vector3.forward * maxdistance);
        }
    }

}
