using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // Static variables for singleton
    private static UIManager _manager = null;
    public static UIManager I
    {
        get { return _manager; }
    }


    public Text TutorialSystemText;
    public Canvas MainCanvas;

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
		
	}

    public void ChangeTutorialSystemUI (string text)
    {
        TutorialSystemText.text = text;
    }

}
