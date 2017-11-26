using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineTracker : MonoBehaviour
{

    public Transform RightIndex;
    public Point target;

    const int maxPoint = 500;

    LineRenderer _renderer;

    List<Vector3> positions;

    Vector3 destination = Vector3.zero;

    

    // Use this for initialization
    void Start()
    {

        _renderer = GetComponent<LineRenderer>();
        positions = new List<Vector3>();

        if (_renderer == null)
            return;

        _renderer.positionCount = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ClearLine();
            destination = target.transform.forward * (-1.0f);
        }

        if (OVRInput.Get(OVRInput.RawButton.X, OVRInput.Controller.Active) || Input.GetKey(KeyCode.LeftControl))
        {
            // for debug
            //dest_line.SetPosition(0, RightIndex.transform.position);
            //dest_line.SetPosition(1, RightIndex.transform.position + destination);
        

            if (_renderer.positionCount < maxPoint)
            {
                _renderer.positionCount++;
                _renderer.SetPosition(_renderer.positionCount - 1, RightIndex.position);
                positions.Add(RightIndex.position);
            }
            else if (_renderer.positionCount >= maxPoint)
            {
                positions.RemoveAt(0);
                positions.Add(RightIndex.position);
                _renderer.SetPositions(positions.ToArray());
            }
        }
    }

    public void ClearLine()
    {
        if (_renderer.positionCount > 0)
        {
            _renderer.positionCount = 0;
            positions.Clear();
        }
    }

}
