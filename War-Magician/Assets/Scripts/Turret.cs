using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject Bullet;

    [SerializeField]
    List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    GameObject Target;
    public Transform forward;

    float timer = 0.0f;
    const float lifetime = 30.0f;

    float skilltimer = 0.0f;
    public float skillcooltime = 1.5f;
    Animator _animator;

	// Use this for initialization
	void Start () {
        forward = transform.Find("forward");
        _animator = GetComponent<Animator>();
        if (forward == null)
            forward = transform;        
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(skilltimer < skillcooltime)
            skilltimer += Time.deltaTime;
        if (Target == null || (Target!= null &&Target.GetComponent<Monster_HP>().alreadyDead) )
        {
            Target = null;
            _animator.SetTrigger("LostTarget");
        }

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
                else if (enemies [i].GetComponent<Monster_HP>().alreadyDead)
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
        {
            dir = Target.transform.position;
            transform.LookAt(dir);
        }
        GameObject b = (GameObject)Instantiate(Bullet, forward.position , Quaternion.identity);
        b.transform.LookAt(dir);
       
        if (b.GetComponent<ElementalCyclon>() != null)
        {
            b.GetComponent<ElementalCyclon>().forward = forward.forward;
        }

        _animator.SetTrigger("Attack");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FieldMonster" || other.tag == "AirMonster")
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
