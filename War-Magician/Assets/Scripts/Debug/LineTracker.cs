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

    public void OnLIndexTrigger() //Stay
    {
        if (!canDraw)
            return;

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

    public void OnLIndexTriggerDown()
    {
        ClearLine();
    }


    public void OnLIndexTriggerUp()
    {
        canDraw = false;
    }

    void ClearLine()
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
