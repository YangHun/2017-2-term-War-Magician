using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleInputManager : MonoBehaviour {


    private string _path; //Setter

    public LineRenderer EulerLineTracker;
    public LineRenderer ContinuousLineTracker;
    





    // Static variables for singleton
    private static MagicCircleInputManager _manager = null;
    public static MagicCircleInputManager I
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
    }
	
	// Update is called once per frame
	void Update () {


	}

    public void TexToJPEG()
    {
        Camera cam = Camera.allCameras[1];
        RenderTexture rt = cam.targetTexture;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        byte[] bytes = tex.EncodeToJPG();
        string path = "Assets/Resources/Output.jpeg";
        System.IO.File.WriteAllBytes(path, bytes);

    }

    public string Path
    {
        get { return _path; }
        set { _path = value; }
    }


}
