using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneral : MonoBehaviour
{
    public float speed = 10.0f;
    Vector3 position;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Object Catcher")
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        position = transform.position;
        position.z -= speed * Time.deltaTime;
        transform.position = position;
    }
}
