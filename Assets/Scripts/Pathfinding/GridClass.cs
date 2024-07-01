using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create and manage a grid for various purposes like pathfinding and enemy spawning.
/// </summary>
public class GridClass : MonoBehaviour
{
    public int GridSizeX { get; private set; }
    public int GridSizeY { get; private set; }
    public int MaxSize => GridSizeX * GridSizeY;

    public Node[,] Grid;
    public Vector2 GridWorldSize;

    [SerializeField] private LayerMask unwalkableLayerMask;

    private float nodeRadius = 0.5f;
    private float nodeDiameter;
    private Pathfinding pathfindingClass;


    private void Awake()
    {
        CreateGrid();
    }

    private void Start()
    {
        pathfindingClass = GetComponent<Pathfinding>();
    }

    public void CreateGrid()
    {
        nodeDiameter = nodeRadius * 2f;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        Grid = new Node[GridSizeX, GridSizeY];

        Vector2 gridBottomLeft = transform.position - Vector3.right * (GridWorldSize.x / 2f) - Vector3.up * (GridWorldSize.y / 2f);

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector2 nodePosition = gridBottomLeft + Vector2.right * (nodeDiameter * x + nodeRadius) + Vector2.up * (nodeDiameter * y + nodeRadius);
                bool walkable = false;
                if (!Physics2D.OverlapCircle(nodePosition, nodeRadius * 0.9f, unwalkableLayerMask))
                {
                    walkable = true;
                }

                Grid[x, y] = new Node(x, y, nodePosition, walkable);
            }
        }
    }


    public Node GetNodeFromWorldPoint(Vector2 worldPoint)
    {
        // 15 is the amount of the x grid that falls in the negative side. GridWorldSize.x is 20, with 15 being in the negative side and 5 in the positive side. 
        //Adding 15 brings the line completely into the positive side, giving us a line with the same underlyin quantity but in
        //figures that allow us to calculate the fraction. Note: Must be refactored and generalized later.
        float fractionX = (worldPoint.x + 15f) / GridWorldSize.x;
        float fractionY = (worldPoint.y + 5f) / GridWorldSize.y;

        fractionX = Mathf.Clamp01(fractionX);
        fractionY = Mathf.Clamp01(fractionY);

        int x = Mathf.RoundToInt((GridSizeX - 1) * fractionX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * fractionY);

        return Grid[x, y];
    }


    public List<Node> GetNeighborNodes(Node n)
    {
        List<Node> neighborNodes = new List<Node>();

        //There can be eight nodes adjacent to n from bottom-left to top-right. Get those that are within the grid.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    //The node at the center will be n itself.
                    continue;
                }

                int checkX = n.GridX + x;
                int checkY = n.GridY + y;

                if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY)
                {
                    neighborNodes.Add(Grid[checkX, checkY]);
                }
            }
        }

        return neighborNodes;
    }


}
