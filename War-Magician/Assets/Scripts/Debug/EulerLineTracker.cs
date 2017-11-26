using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class EulerLineTracker : MonoBehaviour {

    LineRenderer _renderer;

    Vector3 destination;
    public Transform RightIndexBone3;
    public Point target;
    public InnerCircle innerCircle;

    public float linedistance = 0.001f;

    public enum PointType { POINT_THUNDER, POINT_AIR, POINT_FLAME, POINT_SOIL, POINT_WATER, POINT_ICE, POINT_NULL};

    public PointType firstPoint = PointType.POINT_NULL;
    string lastPointname = "";

    bool getKey = false;
    bool FirstTouched = false;
    string ElementName = "";
    bool SecondTouched = false;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<LineRenderer>();

        if (_renderer == null)
            return;
        
        Color c = Color.white;
        c.a = 0.0f;

        for (int i = 0; i < target.transform.Find("Vertex").childCount - 1 ; i++)
        {
            target.transform.Find("Vertex").GetChild(i).GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
            target.transform.Find("Additional Circle").GetChild(i).GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MagicCircleInputManager.I.TexToJPEG();

            Transform vertices = target.transform.Find("Vertex");
            for (int i = vertices.childCount - 1; i > 0; i--)
            {
                target.BloomFade(vertices.GetChild(i - 1).gameObject, "Black", target.fadeSpeed * 0.1f, true);
                //target.SetBloomColor(Color.black, vertices.GetChild(i).gameObject);
            }

            if (FirstTouched)
            {
                target.AdditionalCircleFade(ElementName);
                target.LineBloomFade(this.gameObject, ElementName);
            }
            if (SecondTouched)
            {
                target.BloomFade(target.transform.Find("Circle").Find("Inner Circle").gameObject, ElementName, 0.0f, false);
            }
            
            getKey = false;
            FirstTouched = false;
            SecondTouched = false;
            ElementName = "";
            lastPointname = "";
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _renderer.positionCount = 1;
            _renderer.materials[0].SetColor("_TintColor", Color.white);
            destination = target.transform.forward * (-1.0f);
            //_renderer.SetPosition(0, target.transform.Find("Vertex").Find("Center").position);

            Vector3 initpos = target.transform.Find("Vertex").Find("Center").position;
            initpos -= destination * linedistance;

            //_renderer.SetPosition(0, initpos);

            for (int i = 0; i < target.transform.Find("Vertex").childCount; i++)
            {
                target.FadeIn(target.transform.Find("Vertex").GetChild(i).gameObject, false);
            }

            _renderer.materials[0].SetColor("_TintColor", Color.white);

            if (!getKey)
                getKey = true;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {

            //_renderer.SetPosition(_renderer.positionCount - 1, RightIndexBone3.position);
            /*
            RaycastHit[] hits;
            //hits = Physics.RaycastAll(RightIndexBone3.position, destination);
            hits = Physics.RaycastAll(RightIndexBone3.position, RightIndexBone3.position - Camera.main.transform.position);
                */
            _renderer.SetPosition(_renderer.positionCount - 1, RightIndexBone3.transform.position);
        }
        else
        {

        }

	}

    public void TriggerAction(Collider col)
    {
        string path = MagicCircleInputManager.I.Path;

        if (!getKey)
            return;

        if (col.tag == "Point")
        {
            string name = col.name;

            //Debug.Log(hits[i].collider.name);

            if (lastPointname != name)
            {
                Vector3 nextpos = col.transform.position;

                if (!FirstTouched)
                {
                    ElementName = name;
                    target.FadeIn(col.gameObject, true);
                    FirstTouched = true;
                }

                else if (!SecondTouched)
                {
                    target.FadeIn(target.transform.Find("Circle").Find("Inner Circle").gameObject, false);
                    SecondTouched = true;
                }

                switch (name)
                {
                    case "Thunder":
                        path = path + (int)PointType.POINT_THUNDER;
                        break;
                    case "Air":
                        path = path + (int)PointType.POINT_AIR;
                        break;
                    case "Flame":
                        path = path + (int)PointType.POINT_FLAME;
                        break;
                    case "Soil":
                        path = path + (int)PointType.POINT_SOIL;
                        break;
                    case "Water":
                        path = path + (int)PointType.POINT_WATER;
                        break;
                    case "Ice":
                        path = path + (int)PointType.POINT_ICE;
                        break;
                }

                _renderer.SetPosition(_renderer.positionCount - 1, nextpos - destination * linedistance);
                _renderer.positionCount++;

                lastPointname = name;

                MagicCircleInputManager.I.Path = path;
            }
        }

       

    }

}
