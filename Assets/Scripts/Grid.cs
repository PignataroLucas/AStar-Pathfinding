using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    
    private Node[,] _grid;

    private float nodeDiameter;
    private int gridSizeX,gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGird();
    }

    private void CreateGird()
    {
        _grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft =
            transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) 
                                                     + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius , unwalkableMask));
                _grid[x,y] = new Node(walkable,worldPoint);
            }
        }
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float perfectX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float perfectY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        perfectX = Mathf.Clamp01(perfectX);
        perfectY = Mathf.Clamp01(perfectY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * perfectX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * perfectY);

        return _grid[x,y];
    }
    
    
    private void OnDrawGizmos()
    {
       Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

       if (_grid != null)
       {
           foreach (Node n in _grid)
           {
               Gizmos.color = (n.Walkable) ? Color.white : Color.red;
               Gizmos.DrawCube(n.WorldPosition,Vector3.one * (nodeDiameter-.1f));
           }
       }
    }

}
