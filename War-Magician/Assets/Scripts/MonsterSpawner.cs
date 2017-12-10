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
    public float SpawnTime_NORMAL = 2.0f;
    public float SpawnTime_TOTEM = 15.0f;
    public float SpawnTime_SWARM = 10.0f;
    public float SpawnTime_SHIELD = 10.0f;
    public float SpawnTime_FLY = 20.0f;
    public float SpawnTime_BIRD = 30.0f;
    float TimeCounter_NORMAL = 0.0f;
    float TimeCounter_TOTEM = 0.0f;
    float TimeCounter_SWARM = 0.0f;
    float TimeCounter_SHIELD = 0.0f;
    float TimeCounter_FLY = 27.0f;
    float TimeCounter_BIRD = 0.0f;
    public bool Activation_NORMAL = false;
    public bool Activation_TOTEM = false;
    public bool Activation_SWARM = false;
    public bool Activation_SHIELD = false;
    public bool Activation_FLY = false;
    public bool Activation_BIRD = false;
    int Cycle_Normal = 0;
    int Cycle_Totem = 0;
    int Cycle_Shield = 0;

    int Interval_Normal = 6;
    int Interval_Totem = 3;
    int Interval_Shield = 3;

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
        TimeIsTicking();
        if (TimeCounter_NORMAL >= SpawnTime_NORMAL)
        {
            for (int i = Cycle_Normal * Interval_Normal; i < (Cycle_Normal + 1) * Interval_Normal; i++)
            {

                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.NORMAL);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            Cycle_Normal++;
            if(Cycle_Normal == point_field.Length / Interval_Normal)
            {
                Cycle_Normal = 0;
            }
            TimeCounter_NORMAL = 0;
        }
        if(TimeCounter_TOTEM >= SpawnTime_TOTEM)
        {
            for (int i = Cycle_Totem * Interval_Totem; i < (Cycle_Totem + 1) * Interval_Totem; i++)
            {
                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.TOTEM);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            Cycle_Totem++;
            if (Cycle_Totem == point_field.Length / Interval_Totem)
            {
                Cycle_Totem = 0;
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
            for (int i = Cycle_Shield * Interval_Shield; i < (Cycle_Shield + 1) * Interval_Shield; i++)
            {

                GameObject g = GetComponent<MonsterPool>().GetObject(Spawnpoint_field[i], MonsterPool.Category.SHIELD);
                g.GetComponent<NavMeshAgent>().enabled = true;
                g.GetComponent<AI_FIELD>().Init();
            }
            Cycle_Shield++;
            if (Cycle_Shield == point_field.Length / Interval_Shield)
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
    void TimeIsTicking()
    {
        if (Activation_NORMAL)
        {
            TimeCounter_NORMAL += Time.deltaTime;
        }
        if (Activation_TOTEM)
        {
            TimeCounter_TOTEM += Time.deltaTime;
        }
        if (Activation_SWARM)
        {
            TimeCounter_SWARM += Time.deltaTime;
        }
        if (Activation_SHIELD)
        {
            TimeCounter_SHIELD += Time.deltaTime;
        }
        if (Activation_FLY)
        {
            TimeCounter_FLY += Time.deltaTime;
        }
        if (Activation_BIRD)
        {
            TimeCounter_BIRD += Time.deltaTime;
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