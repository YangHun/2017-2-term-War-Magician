﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_FIELD : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Init()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
    void OnEnable()
    {
        
    }
}