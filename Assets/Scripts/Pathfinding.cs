using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class Pathfinding : MonoBehaviour
{
    Grid2D grid;
    public Transform opponent;

    private GameObject nearestBonus;
    private GameObject[] bonuses;

    private Vector2 _direction = Vector2.zero;
    public Transform player;

    private bool playerTurn;
    private List<Node> path;

    private void Awake()
    {
        grid = GetComponent<Grid2D>();
    }

    private void Start()
    {
        bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        nearestBonus = FindNearestBonus(bonuses);
        if (nearestBonus != null)
        {
            path = FindPath(opponent.position, nearestBonus.transform.position);
        }
        playerTurn=true;
    }

    private void Update()
    {
        if (playerTurn)
        {
            MovePlayer();
        }
        else
        {
            MoveOpponent(path);
        }
    }

    private void MoveOpponent(List<Node> path)
    {
        if (path!=null && path.Count > 0)
        {
            float moveSpeed = 5f;

            Node nextNode = path[0]; 
            Vector3 direction=(nextNode.worldPosition - opponent.position).normalized;
            Vector3 newPosition = opponent.position + direction * moveSpeed * Time.deltaTime;
            
            if (Vector3.Distance(opponent.position, nextNode.worldPosition)<0.1f)
            {
                path.RemoveAt(0);
            }
            opponent.position = newPosition;

            if (path.Count ==0)
            {
                playerTurn = true;
            }
        }
    }

    private void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
            MoveOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
            MoveOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
            MoveOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;
            MoveOneStep();
        }
    }

    private void MoveOneStep()
    {
        Vector3 newPosition = player.position + new Vector3(_direction.x, _direction.y, 0);

        Collider2D[] colliders = Physics2D.OverlapPointAll(newPosition);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Obstacle") || collider.CompareTag("Opponent"))
                return;
        }
        player.position = newPosition;

        nearestBonus = FindNearestBonus(bonuses);
        if (nearestBonus != null)
        {
            path = FindPath(opponent.position, nearestBonus.transform.position);
        }
        playerTurn = false;
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
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("path found: " + sw.ElapsedMilliseconds + " ms");

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
