using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    
    public int gCost;
    public int hCost;
    public Node parent;

    private int heapIndex;
    
    public Node(bool walkable, Vector3 worldPosition, int gridX , int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int fCost => gCost + hCost;

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }

    public int HeapIndex
    {
        get => heapIndex;
        set => heapIndex = value;
    }
}
