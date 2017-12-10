using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {
    MonsterSpawner MS;

    void Start()
    {
        MS = GetComponent<MonsterSpawner>();
    }
}
