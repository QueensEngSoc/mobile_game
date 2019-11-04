using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum State
{
    idle,
    choosingNode,
    deletingNode
}
public class VehicleBuilder : MonoBehaviour
{
    public Node rootNode; 
    public List<Node> nodes;
    public List<NodeConnection> connections;

    Highlighter rootHighlighter;
    public Camera camera;
    public GameObject root;
    public GameObject cubePreFab;
    public Vector3 newNodePosition;
    public State state; // for switch case
    public Text Instructions;
    void Start()
    {
        state = State.idle;
        Instructions.text = "";
        /*
        rootNode = new Node();
        rootNode.nodeType = node_type.STATIC_NODE;
        nodes.Add(rootNode);
        */
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            state = State.deletingNode;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            state = State.idle;

        }

        Debug.Log(state);

        switch (state)
        {
            case State.idle:
                ChooseRoot(false);
                break;
            case State.choosingNode:
                ChooseSecondNode();
                break;
            case State.deletingNode:
                ChooseRoot(true);
                break;
        }
    }

    public void ChooseRoot(bool deleting)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f)){
                if (hit.transform.name == "Sphere")
                {
                    root = hit.transform.parent.gameObject;
                    if (!deleting)
                    {
                        rootHighlighter = hit.transform.GetComponent<Highlighter>();
                        rootHighlighter.select();
                        Instructions.text = "Place New Node";
                        state = State.choosingNode;
                    } else
                    {
                        Destroy(root);
                    }
                   
                } else if (deleting && hit.transform.name == "Cube(Clone)")
                {
                    Destroy(hit.transform.gameObject);

                }
            }
        }
    }
    float theta1;
    float theta2;
    float distanceToPlane;
    float distanceToNewNode;
    public void ChooseSecondNode() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.name == "Sphere") 
                {
                    addVisibleConnection(hit.transform.parent.gameObject, root);

                }
            } else
            {
                newNodePosition = getNewNodePosition(ray); //do math to put new node perpindicular to selected root
                GameObject newNode = Instantiate(root, newNodePosition, new Quaternion()); 
                addVisibleConnection(newNode, root);

            }
            Instructions.text = "";
            state = State.idle;
            rootHighlighter.unSelect();

        }
    }

    void addVisibleConnection(GameObject node1, GameObject node2)
    {
        GameObject newCube = Instantiate(cubePreFab);
        Vector3 n1Pos = node1.transform.position;
        Vector3 n2Pos = node2.transform.position;

        newCube.transform.position = (n1Pos+n2Pos)/2f;
        newCube.transform.LookAt(n1Pos);
        newCube.transform.localScale = new Vector3(width, width, (n1Pos-n2Pos).magnitude);
    }
    public float width;
 
    Vector3 getNewNodePosition(Ray ray) 
    {

        theta1 = Vector3.Angle(camera.transform.forward, (root.transform.position - ray.origin));
        theta2 = Vector3.Angle(camera.transform.forward, ray.direction);

        distanceToPlane = (root.transform.position - ray.origin).magnitude * Mathf.Cos(theta1 * Mathf.Deg2Rad);
        distanceToNewNode = distanceToPlane / Mathf.Cos(theta2 * Mathf.Deg2Rad);

        return ray.origin + ray.direction * distanceToNewNode;

    }
    //public void CreateNode(Vector3 Position, Node connectedNode) //Not in use yet
    //{
    //    Node newNode = new Node();
    //    nodes.Add(newNode);

    //    NodeConnection newConnection = new NodeConnection(nodes.IndexOf(connectedNode), nodes.IndexOf(newNode));
    //    connections.Add(newConnection);

    //}

}
