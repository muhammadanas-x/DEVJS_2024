using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public float speed = 2.5f;
    private float amplitude = .2f;

    private float startY; 

    void Start()
    {
        startY = transform.position.y; 
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * speed) * amplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
