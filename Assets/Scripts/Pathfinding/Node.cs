using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The nodes of the grid, containing properties and methods necessary for pathfinding.
/// </summary>
public class Node : IHeap<Node>
{
    public int FCost => GCost + HCost;
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int GridX;
    public int GridY;
    public Vector2 nodeWorldPosition;
    public bool IsWalkable;
    public int GCost;
    public int HCost;
    public Node parent;

    private int heapIndex;

    
    public Node(int gridX, int gridY, Vector2 inputPosition, bool walkable)
    {
        GridX = gridX;
        GridY = gridY;
        nodeWorldPosition = inputPosition;
        IsWalkable = walkable;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compareResult = FCost.CompareTo(nodeToCompare.FCost);

        if  (compareResult == 0)
        {
            compareResult = HCost.CompareTo(nodeToCompare.HCost);
        }

        return -compareResult;
    }
}
