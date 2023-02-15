using System.Runtime.InteropServices;
using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPosition;

    public Node(bool walkable, Vector3 worldPosition)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
    }
    
}
