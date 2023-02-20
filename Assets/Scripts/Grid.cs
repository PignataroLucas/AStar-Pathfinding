using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    private LayerMask walkableMask;
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    private Node[,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX,_gridSizeY;

    public bool displayGridGizmos;

    public GroundType[] walkableRegion;
    

    private void Awake()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        foreach (GroundType region in walkableRegion)
        {
            walkableMask.value  |= region.roadMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.roadMask.value,2),region.roadPenalty);
        }
        CreateGird();
    }

    public int MaxSize
    {
        get
        {
            return _gridSizeX * _gridSizeY;
        }
    }
    private void CreateGird()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft =
            transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) 
                                                     + Vector3.forward * (y * _nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius , unwalkableMask));
                int movementPenalty = 0;
                
                //Raycast Logic
                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray,out hit , 100 , walkableMask))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }
                _grid[x,y] = new Node(walkable,worldPoint,x,y,movementPenalty);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float perfectX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float perfectY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        perfectX = Mathf.Clamp01(perfectX);
        perfectY = Mathf.Clamp01(perfectY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * perfectX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * perfectY);

        return _grid[x,y];
    }
    private void OnDrawGizmos()
    {
       Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
       if (_grid != null && displayGridGizmos)
       {
           foreach (Node n in _grid)
           {
               Gizmos.color = (n.Walkable) ? Color.white : Color.red;
               Gizmos.DrawCube(n.WorldPosition,Vector3.one * (_nodeDiameter-.1f));
           }    
       }
    }

    [System.Serializable]
    public class GroundType
    {
        public LayerMask roadMask;
        public int roadPenalty;  
    }
    
}
