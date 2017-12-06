using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Point : MonoBehaviour {
    
    public Transform RightIndexBone3;
    public Transform RightIndexBone2;
    
    private GraphicRaycaster gr;
    private PointerEventData ped;
    
    public float distance = 0.0f;
    


    // Use this for initialization
    void Start()
    {

        Transform vertex = transform.Find("Vertex");

        for (int i = 0; i < vertex.childCount; i++)
        {
            SpriteRenderer r;
            if ( (r = vertex.GetChild(i).GetComponent<SpriteRenderer>()) != null)
            {
                r.materials[0].SetColor("_MKGlowColor", Color.black);
            }
        }
        
    }
    
    public void OnLIndexTriggerUp()
    {
        List<bool> isfading = MagicCircleDrawManager.I.isFading;
        if (!isfading.Contains(true) && RightIndexBone3.parent.parent.gameObject.activeSelf)
        {
            MagicCircleDrawManager.I.isFading.Clear();
            Vector3 pos = (RightIndexBone3.transform.position - Camera.main.transform.position).normalized;
            Vector3 rot = (RightIndexBone3.transform.position - RightIndexBone2.transform.position).normalized;
            pos *= 10.0f;
            pos.z = 10.0f;

            transform.LookAt(Camera.main.transform.position);
            //transform.LookAt(rot);
            transform.position = RightIndexBone3.transform.position + RightIndexBone3.forward * distance;
            //transform.position = RightIndexBone3.transform.position;
        }
    }

}
