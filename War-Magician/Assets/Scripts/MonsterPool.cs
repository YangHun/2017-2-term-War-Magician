using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterPool : MonoBehaviour {
    public GameObject MonsterNormal;
    public GameObject MonsterFly;
    public GameObject MonsterBird;
    public GameObject MonsterTotem;
    //  public GameObject MonsterBoss;
    public GameObject MonsterSwarm;
    public GameObject MonsterShield;
    public GameObject target_field;
    public GameObject[] target_air;
    int initialPoolSize = 20;
    int addonPoolSize = 20;

    public enum Category
    {
        NORMAL = 0,
        TOTEM,
        SWARM,
        SHIELD,
        FLY,
        BIRD
    };

    Dictionary<Spawnpoint, Dictionary<GameObject, List<GameObject>>> ObjectPool;

    void Start()
    {
        int categoryLength = System.Enum.GetValues(typeof(Category)).Length;
        ObjectPool = new Dictionary<Spawnpoint, Dictionary<GameObject, List<GameObject>>>();
        for(int i = 0; i < GetComponent<MonsterSpawner>().point_field.Length; i++)
        {
            Dictionary<GameObject, List<GameObject>> ObjectPoolPool = new Dictionary<GameObject, List<GameObject>>();
            for(int j = 0; j < categoryLength; j++)
            {
                GameObject tempCategory = ClassifyEnumObject((Category)j);
                if(tempCategory == MonsterNormal || tempCategory == MonsterShield || tempCategory == MonsterTotem)
                {
                    List<GameObject> ObjectPoolPoolPool = new List<GameObject>();
                    RaycastHit hit;
                    Physics.Raycast(GetComponent<MonsterSpawner>().Spawnpoint_field[i].Pivot.position, Vector3.down, out hit);
                    for (int k = 0; k < initialPoolSize; k++)
                    {
                        GameObject g = Instantiate(tempCategory);
                        g.transform.position = hit.point + new Vector3(0, g.GetComponent<CapsuleCollider>().height, 0);
                        g.transform.rotation = Quaternion.identity;
                        g.GetComponent<AI_FIELD>().target = target_field;
                        g.SetActive(false);
                        ObjectPoolPoolPool.Add(g);
                    }
                    ObjectPoolPool.Add(tempCategory, ObjectPoolPoolPool);
                }
            }
            ObjectPool.Add(GetComponent<MonsterSpawner>().Spawnpoint_field[i], ObjectPoolPool);
        }
        for(int i = 0; i < GetComponent<MonsterSpawner>().point_air.Length; i++)
        {
            Dictionary<GameObject, List<GameObject>> ObjectPoolPool = new Dictionary<GameObject, List<GameObject>>();
            for (int j = 0; j < categoryLength; j++)
            {
                GameObject tempCategory = ClassifyEnumObject((Category)j);
                if (tempCategory == MonsterFly || tempCategory == MonsterBird) {
                    List<GameObject> ObjectPoolPoolPool = new List<GameObject>();
                    for (int k = 0; k < initialPoolSize; k++)
                    {
                        GameObject g = Instantiate(tempCategory);
                        g.transform.position = GetComponent<MonsterSpawner>().Spawnpoint_air[i].Pivot.position;
                        g.transform.rotation = Quaternion.identity;
                        g.GetComponent<AI_AIR>().target = target_air[i];
                        g.SetActive(false);
                        ObjectPoolPoolPool.Add(g);
                    }
                    ObjectPoolPool.Add(tempCategory, ObjectPoolPoolPool);
                }
            }
            ObjectPool.Add(GetComponent<MonsterSpawner>().Spawnpoint_air[i], ObjectPoolPool);
        }
        for (int i = 0; i < GetComponent<MonsterSpawner>().point_swarm.Length; i++)
        {
            Dictionary<GameObject, List<GameObject>> ObjectPoolPool = new Dictionary<GameObject, List<GameObject>>();
            for (int j = 0; j < categoryLength; j++)
            {
                GameObject tempCategory = ClassifyEnumObject((Category)j);
                if (tempCategory == MonsterSwarm)
                {
                    List<GameObject> ObjectPoolPoolPool = new List<GameObject>();
                    RaycastHit hit;
                    Physics.Raycast(GetComponent<MonsterSpawner>().Spawnpoint_swarm[i].Pivot.position, Vector3.down, out hit);
                    for (int k = 0; k < initialPoolSize; k++)
                    {
                        GameObject g = Instantiate(tempCategory);
                        g.transform.position = hit.point + new Vector3(0, g.GetComponent<CapsuleCollider>().height, 0);
                        g.transform.rotation = Quaternion.identity;
                        g.GetComponent<AI_FIELD>().target = target_field;
                        g.SetActive(false);
                        ObjectPoolPoolPool.Add(g);
                    }
                    ObjectPoolPool.Add(tempCategory, ObjectPoolPoolPool);
                }
            }
            ObjectPool.Add(GetComponent<MonsterSpawner>().Spawnpoint_swarm[i], ObjectPoolPool);
        }
    }

    public GameObject GetObject(Spawnpoint point, Category input)
    {
        GetComponent<MonsterSpawner>().NumOfMonster++;
        List<GameObject> list = ObjectPool[point][ClassifyEnumObject(input)];
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                list[i].SetActive(true);
                return list[i];
            }
        }
        int ret = list.Count;
        GameObject temp = ClassifyEnumObject(input);
        for (int i = 0; i < addonPoolSize; i++)
        {
            GameObject g = Instantiate(temp);
            g.SetActive(false);
            list.Add(g);
        }
        return list[ret];
    }

    GameObject ClassifyEnumObject(Category input)
    {
        switch (input)
        {
            case Category.NORMAL:
                return MonsterNormal;
            case Category.TOTEM:
                return MonsterTotem;
            
            case Category.SHIELD:
                return MonsterShield;
                
            case Category.FLY:
                return MonsterFly;
            case Category.SWARM:
                return MonsterSwarm;
                /*
            case Category.BOSS:
                return MonsterBoss;
                */

            case Category.BIRD:
                return MonsterBird;
        }
        return null;
    }
}
