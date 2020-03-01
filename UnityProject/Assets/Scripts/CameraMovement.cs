using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Toggle toggle;
    public bool freeCam;

    Vector3 rotateAbout;
    public Vector3 toLook;
    public GameObject compass;
    public float speed;
    public float zoomSpeed;
    public float rotateSpeed;
    Vector3 rotationx;
    Vector3 rotationy;

    public List<GameObject> cones;


    Vector3 lastPosition;
    Vector3 deltaPosition;
    Vector3 ogCompassRight;

    Vector3 toRotateAxis;

    void Start()
    {
        toggle.isOn = false;
        toggle.onValueChanged.AddListener((value) => freeCam = value); 
        rotateAbout = new Vector3();
        freeCam = false;
        ogCompassRight = compass.transform.right;
        //SetCones();
    }

    void Update()
    {
        getInput();
        
        if (freeCam)
        {
            FreeMove();
            CheckRotate();
            //CheckCompass();
        } else
        {
            RotateAroundNode();
        }
    }
    float x, y, zoom;
    bool mouse1, mouse1down;
    void getInput()
    {
        x = Input.GetAxis("Horizontal") * speed;    // a d
        zoom = Input.GetAxis("Vertical") * speed;   // w s
        y = Input.GetAxis("Zoom") * zoomSpeed;      // q e
        mouse1 = Input.GetMouseButton(1);
        mouse1down = Input.GetMouseButtonDown(1);
    }
    Vector3 start;
    Vector3 end;
    float timeCount;
    public float moveSpeed;
    float r;
    public float rSpeed;
    void RotateAroundNode()
    {
        r = (transform.position - toLook).magnitude * rSpeed;

        //Rotate(new Vector3(-x,zoom));                                 V rotate proportianal to r    
        transform.position += Time.deltaTime * (transform.forward * y + r * (transform.right * x + transform.up * zoom));
        transform.LookAt(rotateAbout);

        if(end != toLook)
        {
            start = rotateAbout;
            end = toLook;
            timeCount = 0;
        }
        if (timeCount < 1)
        {
            rotateAbout = Vector3.Lerp(start, end, timeCount*moveSpeed);
            timeCount += Time.deltaTime;
        } 
        
    }

    void FreeMove()
    {
        transform.position += Time.deltaTime*( transform.forward*zoom + transform.right*x + transform.up*y);
    }
    void CheckRotate()
    {
        if (mouse1)
        {
            if (mouse1down)
            {
                deltaPosition = new Vector3(); // deltaposition is 0 on first click
            } else
            {
                deltaPosition = lastPosition - Input.mousePosition;
            }

            deltaPosition *= rotateSpeed;
            deltaPosition -= new Vector3(2 * deltaPosition.x, 0, 0); //flip the x 

            Rotate(deltaPosition);

            lastPosition = Input.mousePosition;
        }
    }
    //public Camera UICamera;

    /*void CheckCompass()
    
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                switch (hit.transform.name) //ewwww
                {
                    case "X":
                        toRotateAxis = Vector3.right;
                        break;
                    case "-X":
                        toRotateAxis = -Vector3.right;
                        break;
                    case "Y":
                        toRotateAxis = Vector3.up;
                        break;
                    case "-Y":
                        toRotateAxis = -Vector3.up;
                        break;
                    case "Z":
                        toRotateAxis = Vector3.forward;
                        break;
                    case "-Z":
                        toRotateAxis = -Vector3.forward;
                        break;
                }
                StartCoroutine(RotateToAxis(toRotateAxis, transform.forward));
            }
        }
    }
     public float axisSpeed;
    float step;
   Vector3 smallRotation;
     //doesn't quite work...
    IEnumerator RotateToAxis(Vector3 axis, Vector3 forward)// forward is this.forward
    {
        step = Vector3.Angle(axis, forward);
        smallRotation = (axis + forward);

        for (int i = 0; i<step; i++)
        {
            Rotate(smallRotation);
            yield return new WaitForSeconds(1/axisSpeed);
        }


        yield return null;
    }
    
    void SetCones()
    {
        foreach (Transform child in compass.transform)
        {
            cones.Add(child.gameObject); //y,-y,x,-x,z,-z order
        }
    }
    */
    void Rotate(Vector3 toRotate)
    {
        rotationx = new Vector3(0, toRotate.x);
        rotationy = new Vector3(toRotate.y, 0);

        transform.Rotate(rotationx, Space.World); // to avoid rolling 
        transform.Rotate(rotationy);

        //move compass
        compass.transform.RotateAround(compass.transform.position, compass.transform.up, -toRotate.x); //rotate around the cubes y axis
        compass.transform.RotateAround(compass.transform.position, ogCompassRight, -toRotate.y); //rotate around the UI x axix
    }
}
