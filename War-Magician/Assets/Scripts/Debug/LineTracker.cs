using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineTracker : MonoBehaviour
{

    public Transform RightIndex;

    const int maxPoint = 500;

    LineRenderer _renderer;

    List<Vector3> positions;

    bool LeftTriggerButtonDown = false;
    bool canDraw = false;

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

       

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) >= 0.9f || Input.GetKey(KeyCode.LeftControl))
        {
            // for debug
            //dest_line.SetPosition(0, RightIndex.transform.position);
            //dest_line.SetPosition(1, RightIndex.transform.position + destination);


            if (!canDraw)
                return;

            Debug.Log("Enter?");
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
        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) > 0 || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (LeftTriggerButtonDown == false)
            {
                ClearLine();
                LeftTriggerButtonDown = true;
            }
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) == 0 || Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (LeftTriggerButtonDown == true)
            {
                canDraw = false;
                LeftTriggerButtonDown = false;
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

    public void StartDrawLine()
    {
        canDraw = true;
    }

}
