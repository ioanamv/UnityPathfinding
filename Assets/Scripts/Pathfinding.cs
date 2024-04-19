using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid2D grid;
    public Transform seeker, target;
    public Transform opponent;

    private GameObject nearestBonus;
    private GameObject[] bonuses;

    private void Start()
    {
        bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        //nearestBonus = FindNearestBonus(bonuses);
        //if (nearestBonus != null)
        //{
        //    List<Node> path = FindPath(opponent.position, nearestBonus.transform.position);
        //}
    }

    private void Update()
    {
        //FindPath(seeker.position, target.position);
        nearestBonus = FindNearestBonus(bonuses);
        if (nearestBonus != null)
        {
            List<Node> path = FindPath(opponent.position, nearestBonus.transform.position);
        }
    }

    private void Awake()
    {
        grid = GetComponent<Grid2D>();
    }

    GameObject FindNearestBonus(GameObject[] bonuses)
    {
        GameObject nearestBonus = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject bonus in bonuses)
        {
            if (bonus.activeSelf)
            {
                float distace = Vector3.Distance(opponent.position, bonus.transform.position);
                if (distace < shortestDistance)
                {
                    shortestDistance = distace;
                    nearestBonus = bonus;
                }
            }
        }
        return nearestBonus;
    }

    List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return 10 * (distX + distY);
    }
}

//pozitia bonusurilor la start