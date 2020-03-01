using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public CameraMovement camControl;
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
        else
        {
            renderer.material.color = Color.yellow;
        }
    }
    private void OnMouseExit()
    {
        if (!selected)
        {
            renderer.material.color = ogColor;
        } else
        {
            renderer.material.color = Color.green;
        }
    }
    public void unSelect(){
        selected = false;
        renderer.material.color = ogColor;
    }
    public void select()
    {
        selected = true;
        renderer.material.color = Color.green;
        camControl.toLook = transform.position;
    }
    public void Ghost()
    {
        renderer.material.color = Color.red;//new Color(ogColor.r, ogColor.g, ogColor.b, .1f);
    }

}
