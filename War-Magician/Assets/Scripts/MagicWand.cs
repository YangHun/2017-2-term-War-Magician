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

        RaycastHit[] hit = Physics.RaycastAll(transform.position, transform.forward);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.gameObject.layer != 9)
                {
                    _renderer.SetPosition(1, Vector3.forward * (transform.position - hit[i].point).magnitude);
                    break;
                }
            }
            
        }
        else
        {
            _renderer.SetPosition(1, Vector3.forward * maxdistance);
        }
    }

}
