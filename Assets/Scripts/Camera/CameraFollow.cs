using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    private Transform _transform;
    public GameObject objectToFollow;

    private void Awake()
    {
        _transform = transform;
        
    }

    void Update()
    {
        if (objectToFollow)
            _transform.position = new Vector3(objectToFollow.GetComponent<Transform>().position.x,
                objectToFollow.GetComponent<Transform>().position.y, _transform.position.z);
    }
}
