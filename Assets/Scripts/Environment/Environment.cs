using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject Player { get; set; }
    public GameObject player;
    public Transform SpawnPos { get; set; }
    public Transform spawnPos;
    public Transform DespawnPos { get; set; }
    public Transform despawnPos;
    public float ObjectSpeed { get; set; }
    public float objectSpeed;
    void Awake()
    {
        SpawnPos = spawnPos;
        DespawnPos = despawnPos;
        ObjectSpeed = objectSpeed;
        Player = player;
    }
    void FixedUpdate()
    {
        Player = player;
        SpawnPos = spawnPos;
        DespawnPos = despawnPos;
        ObjectSpeed = objectSpeed;
    }
}
