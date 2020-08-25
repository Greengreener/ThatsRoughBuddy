using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variables
    [Header("General Spawning")]
    public List<Transform> spawnPoints = new List<Transform>();
    [Header("Obstacles")]
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    public float obSpawnMin, obSpawnMax, obTimeStamp;
    public float obSpawnDelay;
    [Header("Coins")]
    public GameObject coinPrefab;
    public float cnSpawnMin, cnSpawnMax, cnTimeStamp;
    public float cnSpawnDelay;
    [Header("Power Up")]
    public List<GameObject> powerupPrefabs = new List<GameObject>();
    public float puSpawnMin, puSpawnMax, puTimeStamp;
    public float puSpawnDelay;
    #endregion
    void SpawnObject(GameObject prefab)
    {
        Instantiate(prefab, spawnPoints[Random.Range(0, spawnPoints.Count)]);
    }
    void ObstacleSpawn()
    {
        if(Time.time - obTimeStamp >= obSpawnDelay)
        {
            SpawnObject(obstaclePrefabs[0]);
            obTimeStamp = Time.time;
            obSpawnDelay = Random.Range(obSpawnMin, obSpawnMax);
        }
    }
    void CoinSpawn()
    {
        if (Time.time - cnTimeStamp >= cnSpawnDelay)
        {
            SpawnObject(coinPrefab);
            cnTimeStamp = Time.time;
            cnSpawnDelay = Random.Range(cnSpawnMin, cnSpawnMax);
        }
    }
    void PowerUpSpawn()
    {
        if (Time.time - puTimeStamp >= puSpawnDelay)
        {
            SpawnObject(powerupPrefabs[0]);
            puTimeStamp = Time.time;
            puSpawnDelay = Random.Range(puSpawnMin, puSpawnMax);
        }
    }
    private void Update()
    {
        if(Time.time > 5)
        {
            //ObstacleSpawn();
        }
        CoinSpawn();
        //PowerUpSpawn();
    }
}