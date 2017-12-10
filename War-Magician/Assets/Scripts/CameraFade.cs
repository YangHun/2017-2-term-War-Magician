using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour {

    public Camera mainCamera;
    public Camera topviewCamera;

    public Canvas maincanvas;
    public Canvas topviewcanvas;

    public RenderTexture mainrt;
    public RenderTexture topviewrt;

    public float fadeTime;
    
    void Start()
    {
        Color c;
        Material m = maincanvas.GetComponentInChildren<Image>().material;
        c = m.GetColor("_TintColor");
        c.a = 0.0f;
        m.SetColor("_TintColor", c);
        m = topviewcanvas.GetComponentInChildren<Image>().material;
        m.SetColor("_TintColor", c);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MainToTop();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TopToMain();
        }
    }

    public void MainToTop()
    {
        Material m = maincanvas.GetComponentInChildren<Image>().material;
        m.SetTexture("_MainTex", topviewrt);
        StartCoroutine(ImageFadeIn(m, "main"));
    }

    public void TopToMain()
    {
        Material m = topviewcanvas.GetComponentInChildren<Image>().material;
        m.SetTexture("_MainTex", mainrt);
        StartCoroutine(ImageFadeIn(m, "top"));
    }

    IEnumerator ImageFadeIn(Material m, string cam)
    {
        float timer = 0.0f;

        Color c = m.GetColor("_TintColor");

        yield return null;
        
        while (true) {

            timer += Time.deltaTime;

            c.a = timer / fadeTime;
            
            m.SetColor("_TintColor", c);

            yield return null;

            if (c.a > 1.0f)
            {
                c.a = 1.0f;
                m.SetColor("_TintColor", c);
                yield return null;
                break;
            }
        }
        
        if (cam.Equals("main"))
        {
            Debug.Log("?");
            m = topviewcanvas.GetComponentInChildren<Image>().material;
            m.SetTexture("_MainTex", topviewrt);
            topviewCamera.targetDisplay = 0;
            mainCamera.targetDisplay = 2;
            yield return null;
        }
        else if (cam.Equals("top"))
        {
            Debug.Log("?");
            m = maincanvas.GetComponentInChildren<Image>().material;
            m.SetTexture("_MainTex", mainrt);
            mainCamera.targetDisplay = 0;
            topviewCamera.targetDisplay = 2;
            yield return null;
        }
        
        yield return StartCoroutine(ImageFadeout(m));
    }

    IEnumerator ImageFadeout (Material m)
    {
        float timer = 0.0f;

        Color c = m.GetColor("_TintColor");

        while (true)
        {

            timer += Time.deltaTime;

            c.a = 1.0f - timer / fadeTime;
            m.SetColor("_TintColor", c);

            yield return null;

            if (c.a < 0.0f)
            {
                c.a = 0.0f;
                m.SetColor("_TintColor", c);
                yield return null;
                break;
            }

        }
    }

}
