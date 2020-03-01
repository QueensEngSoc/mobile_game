using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionCube : MonoBehaviour
{
    public VehicleBuilder vehicle;
    Renderer renderer;
    public Color ogColor;
    bool selected;
    GameObject cube;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        
    }
    private void OnMouseEnter()
    {
        if (vehicle.state == State.deletingNode)
        {
            renderer.material.color = Color.red;
        }


    }
    private void OnMouseExit()
    {
         renderer.material.color = ogColor;
    }

}
