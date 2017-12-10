using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBullet : MonoBehaviour {

    ParticleSystem _system;
    bool isCollided = false;
    float timer = 0.0f;

    // Use this for initialization
    void Start () {
        _system = GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {

      if (isCollided)
        {
            timer += Time.deltaTime;

            if (timer > 10.0f)
            {
                Destroy(this.gameObject);
            }
        }

	}

    void OnParticleCollision(GameObject obj)
    {
        if (obj.tag == "FieldMonster" || obj.tag == "AirMonster" )
        {
            obj.GetComponent<Monster_HP>().GetDamaged(20);
        }
        

        Debug.Log(obj.tag);

        if (!isCollided)
        {
            isCollided = true;
        }

    }


}
