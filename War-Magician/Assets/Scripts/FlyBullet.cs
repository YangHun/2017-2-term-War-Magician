﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBullet : MonoBehaviour
{
    public int Damage;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ManaObject"))
        {
            collision.gameObject.GetComponent<ManaObject>().GetDamaged(Damage);

        }
        Destroy(gameObject);
    }
}
