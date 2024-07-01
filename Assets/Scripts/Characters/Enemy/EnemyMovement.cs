using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the enemy's movement using pathfinding requests and movement updates.
/// </summary>
public class EnemyMovement : MonoBehaviour, IMovable
{
    public bool isRunning { get; private set; } = false;
    
    [SerializeField] private float shootRadius;
    [SerializeField] private float stopRadius;
    [SerializeField] private LayerMask playerLayer;

    private PathRequestManager pathRequestManager;
    private GridClass gridClass;
    private Transform targetTransform;

    private float speed = 3f;
    private Vector3[] waypoints;
    private int targetIndex;
    private Coroutine currentPathCoroutine; 
    private Vector3 lastTargetPosition;
    private bool isDestroyed = false;

    

    private void Awake()
    {
        pathRequestManager = GameObject.FindGameObjectWithTag("Pathfinding").GetComponent<PathRequestManager>();
        gridClass = GameObject.FindGameObjectWithTag("Pathfinding").GetComponent<GridClass>();
    }

    private void Start()
    {
        targetTransform = PlayerController.Instance.transform;
        lastTargetPosition = targetTransform.position;
        pathRequestManager.RequestPath(transform.position, targetTransform.position, OnPathReceived);
    }


    private void OnDestroy()
    {
        isDestroyed = true;

        // Ensure that the coroutine is stopped when the object is destroyed
        if (currentPathCoroutine != null)
        {
            StopCoroutine(currentPathCoroutine);
            currentPathCoroutine = null;
        }
    }

    public void HandleMovement()
    {
        if (HasTargetMoved())
        {
            pathRequestManager.RequestPath(transform.position, targetTransform.position, OnPathReceived);
        }
    }

    public bool PlayerInsideShootRadius()
    {
        return Physics2D.OverlapCircle(transform.position, shootRadius, playerLayer);
    }

    public void ApplyMovementConditions()
    {
        if (Physics2D.OverlapCircle(transform.position, stopRadius, playerLayer))
        {
            speed = 0f;
            isRunning = false;
        }
        else
        {
            speed = 3f;
            isRunning = true;
        }
    }

    private bool HasTargetMoved()
    {
        float distanceMoved = Vector3.Distance(lastTargetPosition, targetTransform.position);
        if (distanceMoved > 1.0f) 
        {
            lastTargetPosition = targetTransform.position;
            return true;
        }
        return false;
    }

    /// <summary>
    /// This is the callback method that is called when the PathRequestManager class returns a path.
    /// </summary>
    private void OnPathReceived(Vector2[] waypointsReceived, bool wasPathFound)
    {
        if (isDestroyed) return; 

        if (wasPathFound)
        {
            waypoints = new Vector3[waypointsReceived.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = new Vector3(waypointsReceived[i].x, waypointsReceived[i].y, 0f);
            }

            // Stop the previous coroutine if it's running
            if (currentPathCoroutine != null)
            {
                StopCoroutine(currentPathCoroutine);
                currentPathCoroutine = null;
            }

            targetIndex = 0;
            currentPathCoroutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        if (waypoints.Length == 0) 
        {
            yield break;
        }

        Vector3 currentWaypoint = waypoints[0];

        while (true)
        {
            if (isDestroyed) yield break; 

            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= waypoints.Length)
                {
                    targetIndex = 0;
                    yield break;
                }

                currentWaypoint = waypoints[targetIndex];
            }
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
