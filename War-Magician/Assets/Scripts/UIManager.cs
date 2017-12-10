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
    public Text Timer;
    public Text BossGage;
    public Text CastleHP;

    public GameObject[] Manas = new GameObject[4];

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

    public void ChangeText(string name, string text)
    {
        switch (name)
        {
            case "Timer": Timer.text = text;
                break;
            case "BossGage": BossGage.text = text;
                break;
            case "CastleHP": CastleHP.text = text;
                break;
        }
    }

    public void UpdateManaIcon(int n, bool on)
    {
        if (on)
        {
            Manas[n].transform.Find("On").gameObject.SetActive(true);
            Manas[n].transform.Find("Off").gameObject.SetActive(false);
        }
        else
        {
            Manas[n].transform.Find("On").gameObject.SetActive(false);
            Manas[n].transform.Find("Off").gameObject.SetActive(true);
        }
    }

}
