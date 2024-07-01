using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Characters request a path from this class, which then coordinates with the 
/// Pathfinding class to provide a path when possible.
/// </summary>
public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private bool isProcessingPath = false;
    private Pathfinding pathfindingClass;
    private PathRequest currentPathRequest;

    private void Awake()
    {
        pathfindingClass = GetComponent<Pathfinding>();
    }

    public void RequestPath(Vector2 startPos, Vector2 targetPos, Action<Vector2[], bool> callback)
    {
        //Initialize the PathRequest and add it to the queue.
        PathRequest newPathRequest = new PathRequest(startPos, targetPos, callback);
        pathRequestQueue.Enqueue(newPathRequest);
        TryProcessingPath();
    }

    public void FinishedProcessingPath(Vector2[] waypoints, bool isSuccessful)
    {
        currentPathRequest.callback(waypoints, isSuccessful);
        isProcessingPath = false;
        TryProcessingPath();
    }

    private void TryProcessingPath()
    {
        //Request a path if no other request is being processed or if this is the first request added to the queue.
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            pathfindingClass.StartFindingPath(currentPathRequest.startPos, currentPathRequest.targetPos);
            isProcessingPath = true;
        }
    }


    public struct PathRequest
    {
        public Vector2 startPos;
        public Vector2 targetPos;
        public Action<Vector2[], bool> callback;

        //The delegate field (callback) is for requesting classes to provide a method that will be called once a path is found.
        public PathRequest(Vector2 startPos, Vector2 targetPos, Action<Vector2[], bool> callback)
        {
            this.startPos = startPos;
            this.targetPos = targetPos;
            this.callback = callback;
        }
    }


}
