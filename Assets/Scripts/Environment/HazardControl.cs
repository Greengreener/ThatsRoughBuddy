﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardControl : MonoBehaviour
{
    public Environment environment;
    [Header("Hazards")]
    public GameObject hazGroup;      // Hazard grouping
    public GameObject sStepHazMesh;  // Side step hazard
    public GameObject jumpHazMesh;   // Jump hazard
    public GameObject crouchHazMesh; // Crouch hazard
    [Header("Movement")]
    private float _speed;
    private Transform _despawnPos;
    private Transform _spawnPos;
    void Start()
    {
        environment = GetComponentInParent<Environment>();
        _speed = environment.ObjectSpeed;
        _despawnPos = environment.DespawnPos;
        _spawnPos = environment.SpawnPos;
    }

    // Update is called once per frame
    void FixedUpdate()  
    {
        _speed = environment.ObjectSpeed;
        _despawnPos = environment.DespawnPos;
        _spawnPos = environment.SpawnPos;
        hazGroup.transform.Translate(new Vector3(0, 0, _speed));

        if (hazGroup.transform.position.z <= _despawnPos.transform.position.z)
        {
            ResetAndRandom();
            hazGroup.transform.SetPositionAndRotation(_spawnPos.transform.position, Quaternion.identity);
        }
    }
    void ResetAndRandom()
    {
        sStepHazMesh.SetActive(false);
        jumpHazMesh.SetActive(false);
        crouchHazMesh.SetActive(false);
        switch (Random.Range(0,4))
        {
            case 1:
                sStepHazMesh.SetActive(true);
                break;
            case 2:
                jumpHazMesh.SetActive(true);
                break;
            case 3:
                crouchHazMesh.SetActive(true);
                break;
        }
    }
}
