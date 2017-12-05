using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterPool : MonoBehaviour {
    public GameObject MonsterNormal;
   // public GameObject MonsterFlying;
    public GameObject MonsterTotem;
    //  public GameObject MonsterBoss;
    public GameObject MonsterSwarm;
    public GameObject target;

    int initialPoolSize = 200;
    int addonPoolSize = 40;

    public enum Category
    {
        NORMAL = 0,
        TOTEM, //,, FLYING, BOSS
        SWARM
    };

    Dictionary<Spawnpoint, Dictionary<GameObject, List<GameObject>>> ObjectPool;

    void Start()
    {
        int categoryLength = System.Enum.GetValues(typeof(Category)).Length;
        ObjectPool = new Dictionary<Spawnpoint, Dictionary<GameObject, List<GameObject>>>();
        for(int i = 0; i < 10; i++)
        {
            Dictionary<GameObject, List<GameObject>> ObjectPoolPool = new Dictionary<GameObject, List<GameObject>>();
            for(int j = 0; j < categoryLength; j++)
            {
                GameObject tempCategory = ClassifyEnumObject((Category)j);
                List<GameObject> ObjectPoolPoolPool = new List<GameObject>();
                for(int k = 0; k < initialPoolSize; k++)
                {
                    GameObject g = Instantiate(tempCategory);

                    if (g.GetComponent<AI_FIELD>() != null)
                    {
                        RaycastHit hit;
                        Physics.Raycast(GetComponent<MonsterSpawner>().Spawnpoint[i].Pivot.position, Vector3.down, out hit);
                        g.transform.position = hit.point + new Vector3(0, g.GetComponent<CapsuleCollider>().height, 0);
                        g.transform.rotation = Quaternion.identity;
                        g.GetComponent<AI_FIELD>().target = target;
                    }
                    else if(g.GetComponent<AI_AIR>() != null)
                    {
                        g.GetComponent<AI_AIR>().target = target;
                    }
                    g.SetActive(false);
                    ObjectPoolPoolPool.Add(g);
                }
                ObjectPoolPool.Add(tempCategory, ObjectPoolPoolPool);
            }
            ObjectPool.Add(GetComponent<MonsterSpawner>().Spawnpoint[i], ObjectPoolPool);
        }
    }

    public GameObject GetObject(Spawnpoint point, Category input)
    {
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
            case Category.SWARM:
                return MonsterSwarm;
                /*
            case Category.FLYING:
                return MonsterFlying;
            
            case Category.BOSS:
                return MonsterBoss;
                */

        }
        return null;
    }
}
