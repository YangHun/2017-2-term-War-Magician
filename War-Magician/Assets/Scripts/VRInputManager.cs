using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// About VR input managing.
/// Deal with Update(); calls methods of sub class in this behaviour.
/// This manager calls MagicManager.GetMagicCircle on Update().
/// </summary>
public class VRInputManager : MonoBehaviour {

    public bool LIndexTriggerDown = false;
    public bool LHandTriggerDown = false;
    public bool JoystickOn = false;


    public Camera LineCamera;
    public CameraFade cameramanager;

    // Continuous Casting Variables
    public bool isContinuous = true;
    public ImageProcessor machine;
    public float[] predictions;
    float[] result;
    byte[] bytes;

    // Graph Casting Variables



    


    // Static variables for singleton
    private static VRInputManager _manager = null;
    public static VRInputManager I

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
    }
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) == 0 && !LIndexTriggerDown)
        {

            // Points Update
            MagicCircleDrawManager.I.target.OnLIndexTriggerUp();

        }

        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) == 0 && LIndexTriggerDown
#if UNITY_EDITOR
            || Input.GetKeyUp(KeyCode.LeftControl)
#endif
            ){
                       
            if (isContinuous)
                PredictContunuousInput(); //This calls MagicManager.I.GetMagicCircleImageType().
            else
                MagicManager.I.GetMagicCirclePath(MagicCircleDrawManager.I.ElementName, MagicCircleDrawManager.I.Path);

            MagicCircleDrawManager.I.OnLIndexTriggreUp();

            LIndexTriggerDown = false;
            
        }
        
        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) < 0.9f
#if UNITY_EDITOR
            || Input.GetKeyDown(KeyCode.LeftControl)
#endif     
            ){
            
            if (!LIndexTriggerDown)
            {
                MagicCircleDrawManager.I.OnLIndexTriggerDown();

                if (JoystickOn)
                {
                    MagicManager.I.AOETopObject.OnLIndexTriggerDown();
                }


                LIndexTriggerDown = true;
            }

        }

        else if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) >= 0.9f //Stay
#if UNITY_EDITOR
            || Input.GetKey(KeyCode.LeftControl)
#endif
            ){

            MagicCircleDrawManager.I.OnLIndexTrigger(); 

        }

        //------------------------------------------

        if(OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.Active) == 0)
        {
             if (LHandTriggerDown)
            {
                LHandTriggerDown = false;
            }
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.Active) < 0.9)
        {
            if (!LHandTriggerDown)
            {
                MagicManager.I.Teleport();
                LHandTriggerDown = true;
            }
        }
        else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.Active) >= 0.9)
        {

        }



        if (JoystickOn) //On When topview Camera is active
        {

            if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active) != Vector2.zero)
                MagicManager.I.AOETopObject.OnLThumbStick();

        }

    }

    void PredictContunuousInput()
    {
        predictions = null;
        StartCoroutine(TexToJpegBinary());
    }

    IEnumerator TexToJpegBinary()
    {
        Camera cam = LineCamera;
        RenderTexture rt = cam.targetTexture;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        cam.Render();
        yield return null;
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        yield return null;
        bytes = tex.EncodeToJPG();

        yield return StartCoroutine(machine.StartPredict(bytes));

        if (predictions == null)
            yield break;

        yield return StartCoroutine(CallMagic());

    }

    IEnumerator CallMagic()
    {
        if (predictions == null)
        {
            result = new float[3];
        }
        result = predictions;

        float q = Mathf.Infinity * -1.0f;
        int index = -1;
        for (int i = 0; i < result.Length; i++)
        {
            if (q <= result[i])
            {
                q = result[i];
                index = i;
            }
        }

        string element = MagicCircleDrawManager.I.ElementName;

        switch (index)
        {
            case 0:
                MagicManager.I.GetMagicCircleImageType(element, MagicManager.MagicType.MAGIC_ELEMENTAL);
                break;
            case 1:
                MagicManager.I.GetMagicCircleImageType(element, MagicManager.MagicType.MAGIC_TERRAIN_DOWN);
                break;
            case 2:
                MagicManager.I.GetMagicCircleImageType(element, MagicManager.MagicType.MAGIC_TERRAIN_UP);
                break;
            default:
                Debug.Log("Nothing Called");
                break;
        }

        MagicCircleDrawManager.I.ElementName = "";

        yield return null;
    }
}
