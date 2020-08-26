using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Environment environment;
    private Transform _spawnPos;
    private Transform _despawnPos;
    private float _speed;
    void Start()
    {
        environment = GetComponentInParent<Environment>();
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
        if (this.gameObject.transform.position.z <= _despawnPos.transform.position.z)
        {
            this.gameObject.transform.SetPositionAndRotation(_spawnPos.transform.position, Quaternion.identity);
        }
    }
}
