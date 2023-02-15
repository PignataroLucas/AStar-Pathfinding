using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    
    public int gCost;
    public int hCost;
    public Node parent;
    
    public Node(bool walkable, Vector3 worldPosition, int gridX , int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
    
}
