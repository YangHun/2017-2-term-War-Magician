﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// About drawing and fading magic circle UI and Line Renderer.
/// Also have a Path string of Euler Line Tracker.
/// </summary>
public class MagicCircleDrawManager : MonoBehaviour {

    public EulerLineTracker EulerLineTracker;
    public LineTracker ContinuousLineTracker;
    public LineRenderer GuideLineRenderer;

    Dictionary<string, Color> BloomDict = new Dictionary<string, Color>();
    public List<bool> isFading;
    public float fadeSpeed = 2.0f;

    public enum PointType { POINT_NULL, POINT_THUNDER, POINT_AIR, POINT_FLAME, POINT_SOIL, POINT_WATER, POINT_ICE};
    public PointType firstPoint = PointType.POINT_NULL;

    public Point target;

    bool getKey = false;
    bool FirstTouched = false;
    public string ElementName = "";
    bool SecondTouched = false;
    string lastPointname = "";

    private string _path; //Setter

    //for Continuous
    public bool canDraw = false;

    Vector3 destination;
    
    // Static variables for singleton
    private static MagicCircleDrawManager _manager = null;
    public static MagicCircleDrawManager I
    {
        get { return _manager; }
    }

    // Use this for initialization
    void Start () {

        // Singleton
        if (I == null)
        {
            _manager = this;
        }
        else if (I != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        //Initialization
        isFading = new List<bool>();

        BloomDict.Add("Thunder", new Color(186 / 255.0f, 130 / 255.0f, 255 / 255.0f));
        BloomDict.Add("Air", new Color(111 / 255.0f, 255 / 255.0f, 54 / 255.0f));
        BloomDict.Add("Flame", new Color(255 / 255.0f, 0 / 255.0f, 0 / 255.0f));
        BloomDict.Add("Soil", new Color(167 / 255.0f, 116 / 255.0f, 62 / 255.0f));
        BloomDict.Add("Water", new Color(13 / 255.0f, 191 / 255.0f, 229 / 255.0f));
        BloomDict.Add("Ice", new Color(192 / 255.0f, 198 / 255.0f, 252 / 255.0f));
        BloomDict.Add("White", new Color(1.0f, 1.0f, 1.0f));
        BloomDict.Add("Black", new Color(0.0f, 0.0f, 0.0f));
       
        Color c = Color.white;
        c.a = 0.0f;

        for (int i = 0; i < target.transform.Find("Vertex").childCount - 1; i++)
        {
            target.transform.Find("Vertex").GetChild(i).GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
            target.transform.Find("Additional Circle").GetChild(i).GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
        }
       
        if (GuideLineRenderer != null)
        {
            c = GuideLineRenderer.materials[0].GetColor("_TintColor");
            c.a = 0.0f;
            GuideLineRenderer.materials[0].SetColor("_TintColor", c);
        }

    }
    
    public IEnumerator GuideFadeIn()
    {
        if (GuideLineRenderer == null)
            yield break;
        

        for ( ; ; )
        {
            Color c = GuideLineRenderer.materials[0].GetColor("_TintColor");
            c.a += Time.deltaTime * fadeSpeed;
            GuideLineRenderer.materials[0].SetColor("_TintColor", c);

            yield return null;

            if (c.a >= 1.0f)
                break;
        }

    }

    
    public void OnLIndexTriggreUp()
    {

        //Reset Fades
        Transform vertices = target.transform.Find("Vertex");
        for (int i = vertices.childCount - 1; i > 0; i--)
        {
            BloomFade(vertices.GetChild(i - 1).gameObject, "Black", fadeSpeed * 0.1f, true);
            //target.SetBloomColor(Color.black, vertices.GetChild(i).gameObject);
        }

        if (FirstTouched)
        {
            AdditionalCircleFade(ElementName);
            LineBloomFade(EulerLineTracker.gameObject, ElementName);
        }
        if (SecondTouched)
        {
            BloomFade(target.transform.Find("Circle").Find("Inner Circle").gameObject, ElementName, 0.0f, false);
        }
            
        getKey = false;
        FirstTouched = false;
        SecondTouched = false;
        //ElementName = "";
        lastPointname = "";
        Path = "";
        canDraw = false;

        ContinuousLineTracker.OnLIndexTriggerUp();

    }

    public void OnLIndexTriggerDown()
    {
        destination = target.transform.forward * (-1.0f);
        Vector3 initpos = target.transform.Find("Vertex").Find("Center").position;
        initpos -= destination;

        for (int i = 0; i < target.transform.Find("Vertex").childCount; i++)
        {
            FadeIn(target.transform.Find("Vertex").GetChild(i).gameObject, false);
        }
        if (!getKey)
            getKey = true;

        ContinuousLineTracker.OnLIndexTriggerDown();
        EulerLineTracker.OnLIndexTriggerDown();

    }

    public void OnLIndexTrigger() //Stay
    {
        ContinuousLineTracker.OnLIndexTrigger();
        EulerLineTracker.OnLIndexTrigger();
    }

    public void TriggerAction(Collider col)
    {
        string p = Path;

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
                    FadeIn(col.gameObject, true);
                    FirstTouched = true;
                }

                else if (!SecondTouched)
                {
                    FadeIn(target.transform.Find("Circle").Find("Inner Circle").gameObject, false);
                    SecondTouched = true;
                }

                switch (name)
                {
                    case "Thunder":
                        p = p + (int)PointType.POINT_THUNDER;
                        break;
                    case "Air":
                        p = p + (int)PointType.POINT_AIR;
                        break;
                    case "Flame":
                        p = p + (int)PointType.POINT_FLAME;
                        break;
                    case "Soil":
                        p = p + (int)PointType.POINT_SOIL;
                        break;
                    case "Water":
                        p = p + (int)PointType.POINT_WATER;
                        break;
                    case "Ice":
                        p = p + (int)PointType.POINT_ICE;
                        break;
                }

                EulerLineTracker.SetNextPosition(nextpos, destination);
                ContinuousLineTracker.StartDrawLine();
                lastPointname = name;

                Path = p;
            }
        }
    }

    // Fades
    public void SetBloomColor(Color c, GameObject obj)
    {

        SpriteRenderer r;

        if ((r = obj.GetComponent<SpriteRenderer>()) != null)
        {
            r.materials[0].SetColor("_MKGlowColor", c);
        }

    }
    public void FadeIn(GameObject obj, bool bloom)
    {
        if (bloom)
            StartCoroutine(_bloomFadeIn(obj));
        else
            StartCoroutine(_fadeIn(obj));
    }

    public void FadeOut(GameObject obj, bool bloom)
    {
        if (bloom)
            StartCoroutine(_bloomFadeOut(obj));
        else
            StartCoroutine(_fadeOut(obj));
    }


    IEnumerator _bloomFadeIn(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        if (r == null)
            yield break;

        float timer = 0.0f;

        Color c = r.materials[0].GetColor("_MKGlowColor");

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            c.r += BloomDict[obj.name].r * fadeSpeed * Time.deltaTime;
            c.g += BloomDict[obj.name].g * fadeSpeed * Time.deltaTime;
            c.b += BloomDict[obj.name].b * fadeSpeed * Time.deltaTime;

            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", c);
            yield return null;

            if (c.r >= BloomDict[obj.name].r && c.g >= BloomDict[obj.name].g && c.b >= BloomDict[obj.name].b)
                break;
        }

        isFading[index] = false;
        yield return null;
    }

    public void BloomFade(GameObject obj, string key, float wait, bool bloom_on)
    {
        StartCoroutine(_bloomFade(obj, key, wait, bloom_on));
    }

    IEnumerator _bloomFade(GameObject obj, string key, float wait, bool bloom_on)
    {
        if (obj == null)
            yield break;

        Renderer r = obj.GetComponent<SpriteRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;

        if (!bloom_on)
        {

            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", Color.black);

            while (true)
            {
                timer += fadeSpeed * Time.deltaTime;

                Color bc = r.materials[0].GetColor("_MKGlowColor");
                bc.r += BloomDict[key].r * fadeSpeed * Time.deltaTime;
                bc.g += BloomDict[key].g * fadeSpeed * Time.deltaTime;
                bc.b += BloomDict[key].b * fadeSpeed * Time.deltaTime;
                obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", bc);

                yield return null;

                if (timer >= fadeSpeed && (bc.r >= BloomDict[key].r && bc.g >= BloomDict[key].g && bc.b >= BloomDict[key].b))
                    break;
            }

        }
        else
        {
            while (true)
            {
                timer += fadeSpeed * Time.deltaTime;

                yield return null;

                if (timer >= fadeSpeed)
                    break;
            }

           // yield return new WaitForSeconds(fadeSpeed * 0.25f);
        }

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_Color");
            c.a -= fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);

            yield return null;

            if (c.a < 0.0f)
                break;

        }
        
        Color c_ = Color.white;
        c_.a = 0.0f;

        obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c_);
        obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", Color.black);

        isFading[index] = false;
        yield return null;

    }

    public void LineBloomFade(GameObject obj, string key)
    {
        StartCoroutine(LinebloomFade(obj, key));
    }

    IEnumerator LinebloomFade(GameObject obj, string key)
    {
        if (obj == null)
            yield break;

        Renderer r = obj.GetComponent<LineRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;

        LineRenderer line = obj.GetComponent<LineRenderer>();

        line.materials[0].SetColor("_TintColor", Color.white);
        line.materials[0].SetColor("_MKGlowTexColor", Color.black);

        yield return null;

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color bc = r.materials[0].GetColor("_MKGlowTexColor");
            bc.r += BloomDict[key].r * fadeSpeed * Time.deltaTime;
            bc.g += BloomDict[key].g * fadeSpeed * Time.deltaTime;
            bc.b += BloomDict[key].b * fadeSpeed * Time.deltaTime;
            obj.GetComponent<LineRenderer>().materials[0].SetColor("_MKGlowTexColor", bc);

            yield return null;

            if (timer >= fadeSpeed && bc.r >= BloomDict[key].r && bc.g >= BloomDict[key].g && bc.b >= BloomDict[key].b)
                break;
        }

        //yield return new WaitForSeconds(0.1f);

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_TintColor");
            c.r -= BloomDict["White"].r * fadeSpeed * Time.deltaTime;
            c.g -= BloomDict["White"].g * fadeSpeed * Time.deltaTime;
            c.b -= BloomDict["White"].b * fadeSpeed * Time.deltaTime;

            Color bc = r.materials[0].GetColor("_MKGlowTexColor");
            bc.r -= BloomDict[key].r * fadeSpeed * Time.deltaTime;
            bc.g -= BloomDict[key].g * fadeSpeed * Time.deltaTime;
            bc.b -= BloomDict[key].b * fadeSpeed * Time.deltaTime;
            obj.GetComponent<LineRenderer>().materials[0].SetColor("_MKGlowTexColor", bc);
            obj.GetComponent<LineRenderer>().materials[0].SetColor("_TintColor", c);

            yield return null;

            if (timer >= fadeSpeed && c.r <= 0.0f && c.g <= 0.0f && c.b <= 0.0f)
                break;

        }



        //line.positionCount = 1;
        yield return null;

        line.materials[0].SetColor("_TintColor", Color.black);
        line.materials[0].SetColor("_MKGlowTexColor", Color.black);

        isFading[index] = false;
        yield return null;

    }

    IEnumerator _bloomFadeOut(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;
        Color c = r.materials[0].GetColor("_MKGlowColor");
        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            

            c.r -= BloomDict[obj.name].r * fadeSpeed * Time.deltaTime;
            c.g -= BloomDict[obj.name].g * fadeSpeed * Time.deltaTime;
            c.b -= BloomDict[obj.name].b * fadeSpeed * Time.deltaTime;

            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", c);
            yield return null;

            if (c.r <= 0.0f && c.g <= 0.0f && c.b <= 0.0f)
                break;
        }

        obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", Color.black);

        isFading[index] = false;
        yield return null;
    }

    IEnumerator _fadeOut(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;
        Color c = r.materials[0].GetColor("_Color");
        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            
            c.a -= fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
            yield return null;

            if (c.a <= 0.0f)
                break;
        }

        Color c_ = Color.white;
        c_.a = 0.0f;
        obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c_);

        isFading[index] = false;
        yield return null;
    }

    IEnumerator _fadeIn(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_Color");
            c.a += fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);
            yield return null;

            if (c.a > 1.0f)
                break;
        }

        isFading[index] = false;

        yield return null;
    }

    public void AdditionalCircleFade(string name)
    {
        StartCoroutine(_AdditionalCircleFade(name));
    }

    IEnumerator _AdditionalCircleFade(string name)
    {
        GameObject obj = null;
        if (name != "")
            obj = GameObject.Find("Additional Circle").transform.Find(name).gameObject;
       
        if (obj == null)
            yield break;

        Renderer r = obj.GetComponent<SpriteRenderer>();
        if (r == null)
            yield break;

        isFading.Add(true);
        int index = isFading.Count - 1;
        yield return null;

        float timer = 0.0f;

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_Color");
            c.a += fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);

            Color bc = r.materials[0].GetColor("_MKGlowColor");
            bc.r += BloomDict[obj.name].r * fadeSpeed * Time.deltaTime;
            bc.g += BloomDict[obj.name].g * fadeSpeed * Time.deltaTime;
            bc.b += BloomDict[obj.name].b * fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_MKGlowColor", bc);

            yield return null;

            if (c.a >= 1.0f)
                break;
        }

        //yield return new WaitForSeconds(0.1f);

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_Color");
            c.a -= fadeSpeed * Time.deltaTime;
            obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c);

            yield return null;

            if (c.a < 0.0f)
                break;

        }

        Color c_ = Color.white;
        c_.a = 0.0f;

        obj.GetComponent<SpriteRenderer>().materials[0].SetColor("_Color", c_);
        isFading[index] = false;
        yield return null;
    }

  
    
    public string Path
    {
        get { return _path; }
        set { _path = value; }
    }


}
