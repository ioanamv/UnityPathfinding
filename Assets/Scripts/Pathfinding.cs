using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ComponentModel;
using UnityEngine.SceneManagement;

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
    private bool endGame;
    private bool printOnce = true;
    private int selectedAlgorithm;
    public static bool noPlayer=true;
    public string sceneName;

    private void Awake()
    {
        grid = GetComponent<Grid2D>();
    }

    private void Start()
    {
        if (noPlayer)
        {
            player.gameObject.SetActive(false);
        }   
        
        bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        selectedAlgorithm = PathfindingSelector.GetSelectedAlgorithm();
        nearestBonus = FindNearestBonus(bonuses);

        if (nearestBonus != null)
        {
            if (selectedAlgorithm ==0)
            {
                path = FindPathAstar(opponent.position, nearestBonus.transform.position);
                print("a*");
            }
            else if (selectedAlgorithm ==1)
            {
                path = FindPathDijkstra(opponent.position, nearestBonus.transform.position);
                print("dijkstra");
            }
        }
        playerTurn=true;
    }

    private void Update()
    {
        endGame = EndGame();

        if (!endGame)
        {
            if (playerTurn)
            {
                if (noPlayer == false)
                {
                    MovePlayer();
                }
                else
                {
                    playerTurn = false;
                }
            }
            else
            {
                MoveOpponent(path);
            }
        }
        else if (printOnce)
        {
            print("Player score: " + ScoreManager.PlayerScore + "\nOpponent score: " + ScoreManager.OpponentScore);
            printOnce = false;
            if (ScoreManager.PlayerScore > ScoreManager.OpponentScore)
            {
                ScoreManager.PrintFinalState(0);
            }
            else if (ScoreManager.PlayerScore < ScoreManager.OpponentScore)
            {
                ScoreManager.PrintFinalState(1);
            }
            else
            {
                ScoreManager.PrintFinalState(2);
            }
            ScoreManager.ResetScoresForNextLevel();

            if (noPlayer)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void MoveOpponent(List<Node> path)
    {
        if (noPlayer)
        {
            if (path == null || path.Count == 0)
            {
                nearestBonus = FindNearestBonus(bonuses);
                if (nearestBonus != null)
                {
                    if (selectedAlgorithm == 0)
                    {
                        path = FindPathAstar(opponent.position, nearestBonus.transform.position);
                    }
                    else
                    {
                        path = FindPathDijkstra(opponent.position, nearestBonus.transform.position);
                    }
                }
            }
        }

        if (path != null && path.Count > 0)
        {
            Node nextNode = path[0];
            if (nextNode.walkable)
            {
                MoveOpponentOneStep(nextNode);
            }
            else
            {
                RecalcultatePath();
            }
            playerTurn = true;
        }
    }

    private void MoveOpponentOneStep(Node nextNode)
    {
        Vector3 direction = (nextNode.worldPosition - opponent.position).normalized;
        Vector3 newPosition = opponent.position + direction;

        if (Vector3.Distance(opponent.position, nextNode.worldPosition) < 0.1f)
        {
            path.RemoveAt(0);
        }
        opponent.position = newPosition;
    }

    private void RecalcultatePath()
    {
        if (selectedAlgorithm == 0)
        {
            path = FindPathAstar(opponent.position, nearestBonus.transform.position);
        }
        else if (selectedAlgorithm == 1)
        {
            path = FindPathDijkstra(opponent.position, nearestBonus.transform.position);
        }

        if (path != null && path.Count > 0)
        {
            Node nextNode = path[0];
            MoveOpponentOneStep(nextNode);
        }
    }

    private void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
            MovePlayerOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
            MovePlayerOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
            MovePlayerOneStep();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;
            MovePlayerOneStep();
        }
    }

    private void MovePlayerOneStep()
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
            if (selectedAlgorithm == 0)
            {
                path = FindPathAstar(opponent.position, nearestBonus.transform.position);
            }
            else if (selectedAlgorithm == 1)
            {
                path = FindPathDijkstra(opponent.position, nearestBonus.transform.position);
            }
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

    List<Node> FindPathAstar(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("path found: " + sw.ElapsedMilliseconds + " ms");

                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbour;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
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
        return new List<Node>();
    }

    List<Node> FindPathDijkstra(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.gCost = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("path found: " + sw.ElapsedMilliseconds + " ms");

                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbour;
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

    private bool EndGame()
    {
        if (bonuses.All(b => b.activeSelf == false))
        {
            return true;
        }
        else return false;
    }
}                                   
