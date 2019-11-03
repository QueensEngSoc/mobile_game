using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public GameObject plane;
    public float speed;
    public float zoomSpeed;
    public float rotateSpeed;
    Vector3 rotationx;
    Vector3 rotationy;


    Vector3 lastPosition;
    Vector3 deltaPosition;
    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Rotate();
        //rotatePlane();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal")*speed;
        float zoom = Input.GetAxis("Vertical")*speed;
        float y = Input.GetAxis("Zoom")*zoomSpeed;
        transform.position += Time.deltaTime*( transform.forward*zoom + transform.right*x + transform.up*y);
    }
    void Rotate()
    {
        
        if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                deltaPosition = new Vector3(); // deltaposition is 0 on first click
            } else
            {
                deltaPosition = lastPosition - Input.mousePosition;
            }

            rotationx = new Vector3(0, -deltaPosition.x) * rotateSpeed;
            rotationy = new Vector3(deltaPosition.y,0) * rotateSpeed;

            transform.Rotate(rotationx, Space.World); // to avoid rolling 
            transform.Rotate(rotationy);

            lastPosition = Input.mousePosition;

        }
        
    }
    //void rotatePlane()
    //{
    //    plane.transform.up = transform.position- plane.transform.position;
    //}
}
