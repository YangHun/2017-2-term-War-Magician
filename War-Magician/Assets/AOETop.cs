using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETop : MonoBehaviour {

    public GameObject Target;

    int count;
    [SerializeField]
    float timer;
    public const float lifetime = 15.0f;

    private void OnEnable()
    {
        //Init
        count = 0;
        timer = 0.0f;
        Target.SetActive(true);
        Target.transform.position = transform.position + transform.forward * 10.0f;
    }

    // Use this for initialization
    void Start () {
	}
	
    public void OnLThumbStick()
    {

    }

    public void OnLIndexTriggerDown()
    {
        count++;

        Debug.Log("enter?");
        MagicManager.I.GetTopAOEMagic(transform.forward);

        if (count >= 3)
        {
            
            MagicManager.I.AOETopEnd();
        }
    }

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Debug.Log("enter??");
            MagicManager.I.AOETopEnd();
        }
	}
}
