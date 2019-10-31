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
    public PhysicsBody physicsBody;
    public PseudoPosition pseudoPosition;

    public Node()
    {
        pseudoPosition = new PseudoPosition();
        physicsBody = new PhysicsBody(pseudoPosition);
        
    }

    List<NodeAttachment> attachments;    
}

public class NodeConnection
{
    int nodeIndex1;
    int nodeIndex2;
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
}