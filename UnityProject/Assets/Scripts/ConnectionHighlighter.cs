using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionHighlighter : MonoBehaviour
{
    public VehicleBuilder vehicle;
    private Renderer renderer;
    public Color ogColor;
    bool selected;
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
