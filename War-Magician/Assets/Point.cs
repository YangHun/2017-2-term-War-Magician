using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Point : MonoBehaviour {
    
    public Transform RightIndexBone3;
    public Transform RightIndexBone2;

    public Canvas canvas;
    private GraphicRaycaster gr;
    private PointerEventData ped;

    Dictionary<string, Color> BloomDict = new Dictionary<string, Color>();

    public float distance = 0.0f;
    public float fadeSpeed = 2.0f;
    
    List<bool> isFading = new List<bool>();

    // Use this for initialization
    void Start()
    {
        if (canvas == null)
            return;

        BloomDict.Add("Thunder", new Color(186 / 255.0f, 130 / 255.0f, 255 / 255.0f));
        BloomDict.Add("Air", new Color(111 / 255.0f, 255 / 255.0f, 54 / 255.0f));
        BloomDict.Add("Flame", new Color(255 / 255.0f, 0 / 255.0f, 0 / 255.0f));
        BloomDict.Add("Soil", new Color(167 / 255.0f, 116 / 255.0f, 62 / 255.0f));
        BloomDict.Add("Water", new Color(13 / 255.0f, 191 / 255.0f, 229 / 255.0f));
        BloomDict.Add("Ice", new Color(192 / 255.0f, 198 / 255.0f, 252/ 255.0f));
        BloomDict.Add("White", new Color(1.0f, 1.0f, 1.0f));
        BloomDict.Add("Black", new Color(0.0f, 0.0f, 0.0f));

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
	
    public void SetBloomColor(Color c, GameObject obj)
    {

        SpriteRenderer r;

        if((r = obj.GetComponent<SpriteRenderer>()) != null)
        {
            r.materials[0].SetColor("_MKGlowColor", c);
        }

    }

	// Update is called once per frame
	void Update () {
		

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {

        }
        else if ( Input.GetKeyDown(KeyCode.LeftControl))
        {
           
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {

        }
        else
        {            
            if ( !isFading.Contains(true) && RightIndexBone3.parent.parent.gameObject.activeSelf)
            {
                isFading.Clear();
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

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_MKGlowColor");

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

    public void BloomFade (GameObject obj, string key, float wait, bool bloom_on)
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

                if (timer >= fadeSpeed && (bc.r > BloomDict[key].r && bc.g > BloomDict[key].g && bc.b > BloomDict[key].b))
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
            
            yield return new WaitForSeconds(fadeSpeed * 0.25f);
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

            if (timer >= fadeSpeed && c.r <=0.0f && c.g <=0.0f && c.b <=0.0f)
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

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_MKGlowColor");

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

        while (true)
        {
            timer += fadeSpeed * Time.deltaTime;

            Color c = r.materials[0].GetColor("_Color");
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
        
        GameObject obj = transform.Find("Additional Circle").Find(name).gameObject;

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
    
}
