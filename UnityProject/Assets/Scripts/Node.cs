using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum node_type
{
    NODE,
    STATIC_NODE // laws of physics don't apply, literally just fixed in place.
}

public class Node
{
    public node_type nodeType;
    //   public PhysicsBody physicsBody;
    // public PseudoPosition pseudoPosition;
    public Vector3 position;

    public Node(GameObject gameObject)
    {
        position = gameObject.transform.position;
        
 //       pseudoPosition = new PseudoPosition();
   //     physicsBody = new PhysicsBody(pseudoPosition);
        
    }

    List<NodeAttachment> attachments;    
}

public class NodeConnection
{
    public NodeConnection(int node1, int node2)
    {
        nodeIndex1 = node1;
        nodeIndex2 = node2;
    }

    int nodeIndex1;
    int nodeIndex2;

    public int getNodeIndex1()
    {
        return nodeIndex1;
    }
    public int getNodeIndex2()
    {
        return nodeIndex2;
    }
    override public string ToString()
    {
        return "" + nodeIndex1 + ", " + nodeIndex2;
    }
    public override bool Equals(object obj)
    {
        if(obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        } else
        {
            NodeConnection compare = (NodeConnection)obj;
            //                          V                    V                        V                    V  
            return (compare.getNodeIndex1() == this.nodeIndex1 && compare.getNodeIndex2() == this.nodeIndex2)
                || (compare.getNodeIndex1() == this.nodeIndex2 && compare.getNodeIndex2() == this.nodeIndex1);
        }
    }

    public void handleDeletion(int deletedNode)//if a node gets deleted all the ones after it in the list in vehicle builder move down 1 index so need to handle that here
    {
        if (nodeIndex1> deletedNode)nodeIndex1--;
        if (nodeIndex2> deletedNode)nodeIndex2--;
    }
}

public enum node_attachment_type
{
    NODE_ATTACHMENT_WHEEL,
    NODE_ATTACHMENT_PISTON
}

public class NodeAttachment
{
    node_attachment_type attachmentType;
    Vector3 sphereNormal;
    int nodeIndex;

    public NodeAttachment(node_attachment_type type, Vector3 norm, int node)
    {
        attachmentType = type;
        sphereNormal = norm;
        nodeIndex = node;
    }
    public void handleDeletion(int deletedNode)
    {
        if(deletedNode > nodeIndex)
        {
            nodeIndex--;
        }
    }
}