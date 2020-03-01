using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum State
{
    idle,
    choosingNode,
    deletingNode,
}
public class VehicleBuilder : MonoBehaviour
{
    public CameraMovement camControl;
    public Node rootNode; 
    public List<GameObject> nodeGameObs;
    public List<GameObject> connecters;

    public List<NodeConnection> connections = new List<NodeConnection>(); 

    Highlighter rootHighlighter;
    public Camera camera;
    public GameObject root;
    public ConnectionCube cubePreFab;             

    Vector3 newNodePosition;
    public State state; // for switch case
    public Text Instructions;
    public float width;

    public GameObject wheel;
    public GameObject piston;
    GameObject selectedAttachment;
    public bool attaching;

    GameObject ghostObject;

    void Start()
    {
        ghostObject = null;
        oldGhost = null;
        attaching = false;
        state = State.idle;
        Instructions.text = "";
        nodeGameObs.Add(root);
        rootNode = new Node(root);
        //nodes.Add(rootNode);
    }

    void Update()
    {
        GhostFollower();

        if (Input.GetButtonDown("Fire1"))
        {
            state = State.deletingNode;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            state = State.idle;
        }

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
    GameObject ghost;
    GameObject oldGhost;
    Renderer ghostRend;
    void GhostFollower()
    {
        if (ghostObject != null)
        {
            if(ghostObject != oldGhost)
            {
                if (oldGhost != null)
                {
                    GameObject temp = oldGhost.gameObject;
                    Destroy(temp);
                }

                oldGhost = ghostObject;

                ghostObject.layer = 2; //Ignore raycast

                if (ghostObject.tag == "Node")
                {
                    ghostObject.GetComponent<Highlighter>().Ghost();
                    //ghostRend = ghostObject.GetComponent<Renderer>();
                    //ghostRend.material.color = Color.red;//new Color(ghostRend.material.color.r, ghostRend.material.color.g, ghostRend.material.color.b, .1f);
                }
            }
            RaycastHit hit2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // instantiate ray from camera origin to mouse pointer

            if (Physics.Raycast(ray, out hit2, 100f) && hit2.transform.tag == "Node")
            {
                if(ghostObject.tag == "Node")
                {
                    ghostObject.transform.position = hit2.transform.gameObject.transform.position;
                } else
                {
                    ghostObject.transform.position = hit2.point;
                    ghostObject.transform.up = hit2.transform.gameObject.transform.position - hit2.point;
                }
            } else
            {
                ghostObject.transform.position = getNewNodePosition(ray);
            }
        }
    }
    void DeleteGhost()
    {
        ghostObject = null;
        GameObject temp = oldGhost.gameObject;
        Destroy(temp);
    }
    public void ChooseRoot(bool deleting)
    {
        if (Input.GetMouseButtonDown(0)) //if left click
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // instantiate ray from camera origin to mouse pointer

            if (Physics.Raycast(ray, out hit, 100f)){
                if (hit.transform.tag == "Node") // if node is hit by ray
                {
                    //root = hit.transform.parent.gameObject;
                    root = hit.transform.gameObject;
                    if (!deleting)
                    {
                        rootHighlighter = hit.transform.GetComponent<Highlighter>();
                        if (attaching)
                        {
                            Attach(selectedAttachment, hit.point, root.transform.position - hit.point);
                            attaching = false;
                        } else
                        { 
                            ghostObject = Instantiate(root);
                            
                            //Instructions.text = "Place New Node";
                            rootHighlighter.select();
                            state = State.choosingNode;

                        }
                    }
                    else
                    {
                        DeleteNode(root);
                        root = nodeGameObs[nodeGameObs.Count-1];
                    }
                                     
                } else if (deleting && hit.transform.gameObject.tag == "connection")
                {
                    DeleteConnection(hit.transform.gameObject);
                }
                else if (deleting && hit.transform.gameObject.tag == "Wheel")
                {
                    DeleteWheel(hit.transform.gameObject);
                }
            }
        }
    }
    public void ChooseSecondNode() { //one node is already selected so make a new one or add a connection to that one
        if (Input.GetMouseButtonDown(0))
        {
            DeleteGhost();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f) && hit.transform.tag == "Node")
            {
                addConnection(hit.transform.gameObject, root);
                camControl.toLook = hit.transform.gameObject.transform.position;
            }
            else // click where nothing exists or anything other than a node
            {
                newNodePosition = getNewNodePosition(ray); //do math to put new node perpindicular to selected root wrt camera
                AddNode(newNodePosition, root);

                camControl.toLook = newNodePosition;
            }

            Instructions.text = "";
            state = State.idle;
            rootHighlighter.unSelect();
        }
    }
    void DeleteNode(GameObject toDelete)
    {
        //nodes.RemoveAt(nodeGameObs.IndexOf(toDelete));
        int index = nodeGameObs.IndexOf(toDelete);

        if (nodeGameObs.Count > 1)
        {
            foreach (NodeConnection conn in connections.ToArray()) //make connections an array so that it is a copy  of connections cuz if it is str8 connections then there are errors (i think because somehow it is modified while iterating thru but idk this works
            {
                if (conn.getNodeIndex1() == index || conn.getNodeIndex2() == index)
                {
                    DeleteConnection(conn); // this dont work
                }
                else
                {
                    conn.handleDeletion(index);
                }
            }

            nodeGameObs.Remove(toDelete);

            foreach (GameObject node in nodeGameObs)
                node.name = "Node" + nodeGameObs.IndexOf(node); //rename nodeGameObs in list, may take this out

            Destroy(toDelete);
        }
        
    }
    void AddNode(Vector3 position, GameObject connectedNode)
    {
        GameObject newNodeGameOb = Instantiate(root, newNodePosition, new Quaternion());
        //Node newNode = new Node(newNodeGameOb);
        nodeGameObs.Add(newNodeGameOb);
        newNodeGameOb.name = "Node" + nodeGameObs.IndexOf(newNodeGameOb);
        //nodes.Add(newNode);
        addConnection(newNodeGameOb, connectedNode);

    }
    void addConnection(GameObject node1, GameObject node2)
    {
        if(node1 != node2)
        {
            NodeConnection newConn = new NodeConnection(nodeGameObs.IndexOf(node1), nodeGameObs.IndexOf(node2));

            if ( !connections.Contains(newConn))
            {
                connections.Add(newConn);
                ConnectionCube newCube = Instantiate(cubePreFab);
                Vector3 n1Pos = node1.transform.position;
                Vector3 n2Pos = node2.transform.position;

                newCube.transform.position = (n1Pos + n2Pos) / 2f;
                newCube.transform.LookAt(n1Pos);
                newCube.transform.localScale = new Vector3(width, width, (n1Pos - n2Pos).magnitude);
                newCube.gameObject.name = "Connection" + connecters.Count;
                connecters.Add(newCube.gameObject);
            }  
        }
    }
    void DeleteConnection(GameObject conn)
    {
        connections.RemoveAt(connecters.IndexOf(conn)); // NodeConnections
        connecters.Remove(conn);                        // GameObjects

        Destroy(conn);
    }
    void DeleteConnection(NodeConnection conn)
    {

        GameObject temp = connecters[connections.IndexOf(conn)];

        connecters.RemoveAt(connections.IndexOf(conn));
        connections.Remove(conn);

        Destroy(temp);
    }

    void Attach(GameObject selected, Vector3 pos, Vector3 norm)
    {
        GameObject newAttachment = Instantiate(selected);
        newAttachment.transform.position = pos;
        newAttachment.transform.up = norm;
        DeleteGhost();
    }
    void DeleteWheel(GameObject wheel)
    {
        Destroy(wheel);
    }
    public void WheelButton()
    {
        selectedAttachment = wheel;
        attaching = true;
        state = State.idle;
        ghostObject = Instantiate(wheel);

    }
    Vector3 getNewNodePosition(Ray ray) // str8 math
    {
        float theta1;
        float theta2;
        float distanceToPlane;
        float distanceToNewNode;

        theta1 = Vector3.Angle(camera.transform.forward, (root.transform.position - ray.origin));
        theta2 = Vector3.Angle(camera.transform.forward, ray.direction);

        distanceToPlane = (root.transform.position - ray.origin).magnitude * Mathf.Cos(theta1 * Mathf.Deg2Rad);
        distanceToNewNode = distanceToPlane / Mathf.Cos(theta2 * Mathf.Deg2Rad);

        return ray.origin + ray.direction * distanceToNewNode;
    }
}