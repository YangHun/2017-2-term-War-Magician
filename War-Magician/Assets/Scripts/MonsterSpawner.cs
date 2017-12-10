using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterSpawner : MonoBehaviour {
    public GameObject[] point_field;
    public GameObject[] point_air;
    public GameObject[] point_swarm;
    public GameObject point_boss;
    public Spawnpoint[] Spawnpoint_field;
    public Spawnpoint[] Spawnpoint_air;
    public Spawnpoint[] Spawnpoint_swarm;
    public Spawnpoint Spawnpoint_boss;
    float SpawnTime_NORMAL = 2.0f;
    float SpawnTime_TOTEM = 15.0f;
    float SpawnTime_SWARM = 10.0f;
    float SpawnTime_SHIELD = 10.0f;
    float SpawnTime_FLY = 20.0f;
    float SpawnTime_BIRD = 30.0f;
    float TimeCounter_NORMAL = 0.0f;
    float TimeCounter_TOTEM = 0.0f;
    float TimeCounter_SWARM = 0.0f;
    float TimeCounter_SHIELD = 0.0f;
    float TimeCounter_FLY = 0.0f;
    float TimeCounter_BIRD = 0.0f;
    int Cycle_Normal = 0;
    int Cycle_Shield = 0;
    public int NumOfMonster = 0;
    public GameObject bossMonster;
    public GameObject bossTarget;
    // Use this for initialization
    void Start () {
        Spawnpoint_field = new Spawnpoint[point_field.Length];
        Spawnpoint_air = new Spawnpoint[point_air.Length];
        Spawnpoint_swarm = new Spawnpoint[point_swarm.Length];
        for (int i = 0; i < point_field.Length; i++)
        {
            Spawnpoint_field[i] = new Spawnpoint(point_field[i]);
        }
        for (int i = 0; i < point_air.Length; i++)
        {
            Spawnpoint_air[i] = new Spawnpoint(point_air[i]);
        }
        for (int i = 0; i < point_swarm.Length; i++)
        {
            Spawnpoint_swarm[i] = new Spawnpoint(point_swarm[i]);
        }
        Spawnpoint_boss = new Spawnpoint(point_boss);
    }
	
	// Update is called once per frame
	void Update () {
        TimeCounter_NORMAL += Time.deltaTime;
        TimeCounter_TOTEM += Time.deltaTime;
        TimeCounter_SWARM += Time.deltaTime;
        TimeCounter_SHIELD += Time.deltaTime;
        TimeCounter_FLY += Time.deltaTime;
        TimeCounter_BIRD += Time.deltaTime;
        if (TimeCounter_NORMAL >= SpawnTime_NORMAL)
        {
            for (int i = Cycle_Normal * 6; i < (Cycle_Normal + 1) * 6; i++)
            {

                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.NORMAL);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            Cycle_Normal++;
            if(Cycle_Normal == 5)
            {
                Cycle_Normal = 0;
            }
            TimeCounter_NORMAL = 0;
        }
        if(TimeCounter_TOTEM >= SpawnTime_TOTEM)
        {
            for(int i = 0; i < point_field.Length; i++)
            {
                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.TOTEM);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            TimeCounter_TOTEM = 0;
        }

        if (TimeCounter_SWARM >= SpawnTime_SWARM)
        {
            for (int i = 0; i < point_swarm.Length; i++)
            {
                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_swarm[i], MonsterPool.Category.SWARM);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            TimeCounter_SWARM = 0;
        }

        if (TimeCounter_SHIELD >= SpawnTime_SHIELD)
        {
            for (int i = Cycle_Shield * 3; i < (Cycle_Shield + 1) * 3; i++)
            {

                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.SHIELD);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            Cycle_Shield++;
            if (Cycle_Shield == 10)
            {
                Cycle_Shield = 0;
            }
            TimeCounter_SHIELD = 0;
        }

        if(TimeCounter_FLY >= SpawnTime_FLY)
        {
            for (int i = 0; i < point_air.Length; i++)
            {
                GetComponent<MonsterPool>().GetObject(Spawnpoint_air[i], MonsterPool.Category.FLY);
            }
            TimeCounter_FLY = 0;
        }
        if (TimeCounter_BIRD >= SpawnTime_BIRD)
        {
            for (int i = 0; i < point_air.Length; i++)
            {
                GetComponent<MonsterPool>().GetObject(Spawnpoint_air[i], MonsterPool.Category.BIRD);
            }
            TimeCounter_BIRD = 0;
        }
          
        if(NumOfMonster >= 6000)
        {
            GameObject g = Instantiate(bossMonster);
            g.transform.position = Spawnpoint_boss.Pivot.position;
            g.transform.rotation = Quaternion.identity;
            g.GetComponent<AI_AIR>().target = bossTarget;
            NumOfMonster = 0;
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