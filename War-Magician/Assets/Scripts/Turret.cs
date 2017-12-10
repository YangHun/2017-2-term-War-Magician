using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject Bullet;


    List<GameObject> enemies = new List<GameObject>();

    GameObject Target;
    public Transform forward;

    float timer = 0.0f;
    const float lifetime = 30.0f;

    float skilltimer = 0.0f;
    const float skillcooltime = 3.0f;

	// Use this for initialization
	void Start () {
        forward = transform.Find("forward");
        if (forward == null)
            forward = transform;        
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(skilltimer < skillcooltime)
            skilltimer += Time.deltaTime;

        if (skilltimer >= skillcooltime && FindTarget())
        {
            Attack();
            skilltimer = 0.0f;
        }

        if (timer >= lifetime)
            Destroy(this.gameObject);
	}

    bool FindTarget()
    {
        if (enemies.Count > 0)
        {
            float min = 500f;

            for (int i =0; i < enemies.Count; i++) {

                if(enemies[i] == null)
                {
                    continue;
                }
                else if ((enemies[i].transform.position - transform.position).magnitude < min)
                 {
                    min = (enemies[i].transform.position - transform.position).magnitude;
                    Target = enemies[i];
                }
            }

            if (Target != null)
                return true;
        }

        return false;
    }

    void Attack()
    {
        Vector3 dir;
        if (Target == null)
        {
            dir = transform.forward + forward.position;
        }
        else
            dir = Target.transform.position;

        GameObject b = (GameObject)Instantiate(Bullet, forward.position, Quaternion.identity);
        b.transform.LookAt(dir);
        GetComponent<Animator>().SetTrigger("Attack");
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FieldMonster")
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains (other.gameObject))
        {
            enemies.Remove(other.gameObject);
        }
    }

}
