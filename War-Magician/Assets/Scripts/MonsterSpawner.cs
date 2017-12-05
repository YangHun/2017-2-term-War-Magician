using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterSpawner : MonoBehaviour {
    public GameObject[] point;
    public Spawnpoint[] Spawnpoint = new Spawnpoint[10];

    float SpawnTime_NORMAL = 1.0f;
    float SpawnTime_TOTEM = 3.0f;
    float SpawnTime_SWARM = 5.0f;
    float TimeCounter_NORMAL = 0.0f;
    float TimeCounter_TOTEM = 0.0f;
    float TimeCounter_SWARM = 0.0f;
    // Use this for initialization
    void Start () {
		for(int i = 0; i < 10; i++)
        {
            Spawnpoint[i] = new Spawnpoint(point[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        TimeCounter_NORMAL += Time.deltaTime;
        TimeCounter_TOTEM += Time.deltaTime;
        TimeCounter_SWARM += Time.deltaTime;
        if (TimeCounter_NORMAL >= SpawnTime_NORMAL)
        {
            for (int i = 0; i < 5; i++)
            {

                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint[i], MonsterPool.Category.NORMAL);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }

            TimeCounter_NORMAL = 0;
        }
        if(TimeCounter_TOTEM >= SpawnTime_TOTEM)
        {
            for(int i = 5; i < 10; i++)
            {
                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint[i], MonsterPool.Category.TOTEM);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            TimeCounter_TOTEM = 0;
        }
        if(TimeCounter_SWARM >= SpawnTime_SWARM)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint[i], MonsterPool.Category.SWARM);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            TimeCounter_SWARM = 0;
        }
    }
}

public class Spawnpoint
{
    private Transform pivot;
    public Transform Pivot
    {
        get
        {
            return pivot;
        }
    }
    public Spawnpoint(GameObject p)
    {
        pivot = p.transform;
    }
}