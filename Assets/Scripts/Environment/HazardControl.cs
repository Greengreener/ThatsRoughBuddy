using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardControl : MonoBehaviour
{
    private GameObject _player;
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
        _player = environment.Player;
    }
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
        if (_player.activeSelf == false)
        {
            Stopped();
        }
    }
    void ResetAndRandom() //Resets active hazards and sets a random hazard active
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
    private void Stopped() //Stops all Hazards from spawning
    {
        sStepHazMesh.SetActive(false);
        jumpHazMesh.SetActive(false);
        crouchHazMesh.SetActive(false);
    }
}
