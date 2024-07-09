using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// enemy class with A* pathfinding
public class EnemyStarSearch : MonoBehaviour {
    private class Node {
        public Vector3 position;
        public Node parent;
        public float gCost;
        public float hCost;

        public float FCost { get { return gCost + hCost; } }

        public Node(Vector3 pos, float g, float h, Node par = null) {
            position = pos;
            gCost = g;
            hCost = h;
            parent = par;
        }
    }

    private List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos) {
        List<Node> openSet = new List<Node>();
        HashSet<Vector3> closedSet = new HashSet<Vector3>();

        Node startNode = new Node(startPos, 0, CalculateDistance(startPos, targetPos));
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.position);

            if (currentNode.position == targetPos) {
                return RetracePath(startNode, currentNode);
            }

            foreach (Vector3 neighborPos in GetNeighbors(currentNode.position)) {
                if (closedSet.Contains(neighborPos)) continue;

                float newMovementCostToNeighbor = currentNode.gCost + CalculateDistance(currentNode.position, neighborPos);
                Node neighborNode = new Node(neighborPos, newMovementCostToNeighbor, CalculateDistance(neighborPos, targetPos), currentNode);

                if (openSet.Exists(node => node.position == neighborPos && node.FCost < neighborNode.FCost)) continue;

                openSet.Add(neighborNode);
            }
        }

        return null;
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode) {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private float CalculateDistance(Vector3 a, Vector3 b) {
        return Vector3.Distance(a, b);
    }
    private List<Vector3> GetNeighbors(Vector3 center) {
        List<Vector3> neighbors = new List<Vector3>();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(center, out hit, 1f, NavMesh.AllAreas)) {
            Vector3[] directions = new Vector3[] {
                Vector3.forward,
                Vector3.back,
                Vector3.left,
                Vector3.right
            };

            foreach (Vector3 dir in directions) {
                Vector3 neighborPos = hit.position + dir;
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(center, neighborPos, NavMesh.AllAreas, path)) {
                    neighbors.Add(neighborPos);
                }
            }
        }

        return neighbors;
    }
}
