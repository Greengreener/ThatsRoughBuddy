using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Environment environment;
    private Transform _spawnPos;
    private float _despawnPos;
    private float _speed;
    void Start()
    {
        _speed = environment.ObjectSpeed;
        _spawnPos = environment.SpawnPos;
        _despawnPos = environment.DespawnPos;
    }
    void FixedUpdate()
    {
        _speed = environment.ObjectSpeed;
        _despawnPos = environment.DespawnPos;
        _spawnPos = environment.SpawnPos;

        this.transform.Translate(new Vector3(0, 0, _speed));
        if (this.gameObject.transform.position.z <= _despawnPos)
        {
            this.gameObject.transform.SetPositionAndRotation(_spawnPos.transform.position, Quaternion.identity);
        }
    }
}
