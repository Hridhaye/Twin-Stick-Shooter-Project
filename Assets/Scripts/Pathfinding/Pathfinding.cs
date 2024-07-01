using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///  This class performs the main pathfinding calculations. 
///  It is coordinated with the PathRequestManager class to ensure it provides a path only when needed.
/// </summary>
public class Pathfinding : MonoBehaviour
{
    private GridClass gridClass;
    private PathRequestManager requestManager;
    private Heap<Node> openSet;
    private HashSet<Node> closedSet;

    private void Start()
    {
        requestManager = GetComponent<PathRequestManager>();
        gridClass = GetComponent<GridClass>();
        openSet = new Heap<Node>(gridClass.MaxSize);
        closedSet = new HashSet<Node>();
    }

    public void StartFindingPath(Vector2 startPos, Vector2 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }


    private int GetDistance(Node nodeA, Node nodeB)
    {
        int deltaX = Mathf.Abs(nodeB.GridX - nodeA.GridX);
        int deltaY = Mathf.Abs(nodeB.GridY - nodeA.GridY);

        if (deltaX > deltaY)
        {
            return 14 * deltaY + 10 * (deltaX - deltaY);
        }
        else
        {
            return 14 * deltaX + 10 * (deltaY - deltaX);
        }

    }

    /// <summary>
    /// We simplify the path by only returning the points that are involved in direction changes.
    /// </summary>
    private Vector2[] SimplifyPath(List<Node> path)
    {
        Vector2 oldDirection = Vector2.zero;
        List<Vector2> waypoints = new List<Vector2>();

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);

            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i - 1].nodeWorldPosition);
            }

            oldDirection = newDirection;
        }

        return waypoints.ToArray();
    }


    private Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2[] waypointsArray = SimplifyPath(path);
        Array.Reverse(waypointsArray);
        return waypointsArray;
    }


    /// <summary>
    /// The main pathfinding algorithm. We're using a coroutine to introduce a frame delay, ensuring only one calculation 
    /// is done in one frame and, thereby, optimizing performance.
    /// </summary>
    private IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = gridClass.GetNodeFromWorldPoint(startPos);
        Node targetNode = gridClass.GetNodeFromWorldPoint(targetPos);
        Vector2[] waypoints = null;
        bool isSuccessful = false;

        Node currentNode = startNode;
        openSet.Add(currentNode);

        while (openSet.Count > 0)
        {
            //Remove the node with the lowest FCost from the heap.
            currentNode = openSet.RemoveFirstItem();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                waypoints = RetracePath(startNode, targetNode);
                isSuccessful = true;
                break;
            }

            foreach (Node neighbor in gridClass.GetNeighborNodes(currentNode))
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor)) continue;

                int costToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                // Check if this path to the neighbor is cheaper or if the neighbor is not yet in the open set
                if (costToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = costToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbor);
                    }
                }

            }
        }

        yield return null;

        //Clear the sets and signal to the requestManager that a path was found, which will then request a new path if needed.
        openSet.Clear();
        closedSet.Clear();
        requestManager.FinishedProcessingPath(waypoints, isSuccessful);
    }


}
