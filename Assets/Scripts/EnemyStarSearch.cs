using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStarSearch : MonoBehaviour {

    // Node class for A* algorithm
    private class PathNode : IComparer<PathNode> {
        public Vector3 position;
        public float fCost; // total cost (gCost + hCost)
        public float gCost; // cost from start to current node
        public float hCost; // heuristic cost from current node to destination
        public PathNode parent;

        public PathNode(Vector3 pos, float h, PathNode par) {
            position = pos;
            hCost = h;
            parent = par;
            gCost = 0;
            fCost = gCost + hCost;
        }

        public int Compare(PathNode x, PathNode y) {
            if (x.fCost < y.fCost)
                return -1;
            else if (x.fCost > y.fCost)
                return 1;
            else
                return 0;
        }
    }


    private Transform basePoint;
    private NavMeshAgent navMeshAgent;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject baseObject = GameObject.Find("Base");

        // set base point
        basePoint = baseObject.transform;

        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            return;
        }

        if (basePoint == null) {
            Debug.LogError("Base object not found in the scene.");
            return;
        }

        // find path using A* algorithm
        FindPathAStar(basePoint.position);
    }

    void Update() {
        // Check if the agent has reached the base
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            // Enemy has reached the base
            OnReachBase();
        }
    }

    // A* algorithm to find path to destination
    private void FindPathAStar(Vector3 destination) {
        // Initialize priority queue for open nodes
        PriorityQueue<PathNode> openNodes = new PriorityQueue<PathNode>();
        HashSet<PathNode> closedNodes = new HashSet<PathNode>();

        // Start node is the current position of the enemy
        PathNode startNode = new PathNode(transform.position, CalculateHeuristic(transform.position, destination), null);
        openNodes.Enqueue(startNode);

        while (openNodes.Count > 0) {
            // Dequeue the node with the lowest cost
            PathNode currentNode = openNodes.Dequeue();

            // If the current node is the destination, reconstruct and set path
            if (Vector3.Distance(currentNode.position, destination) < navMeshAgent.stoppingDistance) {
                SetPath(currentNode);
                return;
            }

            // Add current node to closed list
            closedNodes.Add(currentNode);

            // Get neighbors (using NavMesh to find walkable positions)
            List<Vector3> neighbors = GetWalkableNeighbors(currentNode.position);
            
            foreach (Vector3 neighbor in neighbors) {
                // Create a node for the neighbor
                PathNode neighborNode = new PathNode(neighbor, CalculateHeuristic(neighbor, destination), currentNode);

                // Skip if neighbor is in closed list
                if (closedNodes.Contains(neighborNode))
                    continue;

                // Calculate tentative g cost
                float tentativeGCost = currentNode.gCost + Vector3.Distance(currentNode.position, neighbor);

                // Check if neighbor is not in open list or has a lower g cost
                if (!openNodes.Contains(neighborNode) || tentativeGCost < neighborNode.gCost) {
                    // Update g and f cost
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.parent = currentNode;

                    // Enqueue or update the neighbor node in the open list
                    if (!openNodes.Contains(neighborNode))
                        openNodes.Enqueue(neighborNode);
                    else
                        openNodes.UpdatePriority(neighborNode);
                }
            }
        }

        // If no path found, log error
        Debug.LogError("No path found to destination.");
    }

    // Calculate heuristic (using Euclidean distance)
    private float CalculateHeuristic(Vector3 from, Vector3 to) {
        return Vector3.Distance(from, to);
    }

    // Get walkable neighbors using NavMesh
    private List<Vector3> GetWalkableNeighbors(Vector3 position) {
        NavMeshPath navMeshPath = new NavMeshPath();
        List<Vector3> neighbors = new List<Vector3>();

        NavMesh.SamplePosition(position, out NavMeshHit hit, 5f, NavMesh.AllAreas);
        position = hit.position;

        if (NavMesh.CalculatePath(position, basePoint.position, NavMesh.AllAreas, navMeshPath)) {
            foreach (Vector3 corner in navMeshPath.corners) neighbors.Add(corner);
        }

        return neighbors;
    }

    // Set path to destination
    private void SetPath(PathNode targetNode) {
        List<Vector3> path = new List<Vector3>();

        // Trace back the path from target node to start node
        PathNode currentNode = targetNode;
        while (currentNode != null) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse(); // Reverse the path to start from the beginning
        NavMeshPath navMeshPath = new NavMeshPath();
        navMeshAgent.CalculatePath(path[path.Count - 1], navMeshPath);
        navMeshAgent.path = navMeshPath;
    }

    // enemy reached the base
    private void OnReachBase() {
        Debug.Log("Enemy reached the base!");
        Destroy(gameObject);
    }


    
}
