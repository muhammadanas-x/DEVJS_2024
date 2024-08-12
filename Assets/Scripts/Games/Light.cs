using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Light : MonoBehaviour 
{

    bool isLighted;
    new Renderer renderer;

    public bool IsLighted => isLighted;
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        UnLight();
    }

    public void LightUp()
    {
        isLighted = true;
        renderer.material.color = Color.yellow;
    }

    public bool IsLightedUp()
    {
        return isLighted;
    }


    public void UnLight()
    {
        isLighted = false;
        renderer.material.color = Color.white;
    }
}
