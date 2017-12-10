using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MP : MonoBehaviour {

    public float MaxMP;
    public float currentMP;
    public float MP
    {
        get
        {
            return currentMP;
        }
        set
        {
            currentMP = value;
        }
    }

    private bool _isRestoring = false;
    public bool isRestoring
    {
        get
        {
            return _isRestoring;
        }
        set
        {
            _isRestoring = value;
        }
    }
    private bool fullMana;
    float restoreSpeed = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(currentMP > MaxMP)
        {
            currentMP = MaxMP;
            fullMana = true;
        }
        if(fullMana && currentMP < MaxMP)
        {
            fullMana = false;
        }
        if (!fullMana && _isRestoring)
        {
            currentMP += Time.deltaTime * restoreSpeed;
        }
	}
    public bool BurnMana(float mana)
    {
        if(currentMP >= mana)
        {
            currentMP -= mana;
            return true;
        }
        else
        {
            return false;
        }
    }
}
