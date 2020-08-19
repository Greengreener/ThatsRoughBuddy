using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Transform SpawnPos { get; set; }
    public Transform spawnPos;
    public float DespawnPos { get; set; }
    public float despawnPos;
    public float ObjectSpeed { get; set; }
    public float objectSpeed;
    void Awake()
    {
        SpawnPos = spawnPos;
        DespawnPos = despawnPos;
        ObjectSpeed = objectSpeed;
    }
    void FixedUpdate()
    {
        SpawnPos = spawnPos;
        DespawnPos = despawnPos;
        ObjectSpeed = objectSpeed;
    }
}
