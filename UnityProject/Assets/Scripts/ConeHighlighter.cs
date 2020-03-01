using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeHighlighter : MonoBehaviour
{
    Renderer renderer;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    private void OnMouseEnter()
    {

        renderer.material.color = Color.yellow;

    }
    private void OnMouseExit()
    {

        renderer.material.color = Color.white;

    }
}