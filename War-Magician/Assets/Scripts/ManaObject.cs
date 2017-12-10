using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObject : MonoBehaviour {
    public GameObject Manager;
    ManaObjectManager ManaObjectManager; 
    int HP = 20;
    public int ID;
    bool isDestroyed = false;
    float RestructTime = 10.0f;
    float timeCounter = 0.0f;
	// Use this for initialization
	void Start () {
        ManaObjectManager = Manager.GetComponent<ManaObjectManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isDestroyed)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= RestructTime)
            {
                HP = 10;
                isDestroyed = false;
                ManaObjectManager.Restructed(gameObject);
                timeCounter = 0f;
            }
        }
	}
    public void GetDamaged(int damage)
    {
        
        HP -= damage;
        if (HP <= 0)
        {
            isDestroyed = true;
            ManaObjectManager.Destroyed(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isDestroyed)
            {
                PlayerManager.I.Player_MP.isRestoring = false;
            }
            else
            {

                PlayerManager.I.Player_MP.isRestoring = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.I.Player_MP.isRestoring = false;
        }
    }
}
