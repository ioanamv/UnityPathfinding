using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grid2D : MonoBehaviour
{
    public Transform player, opponent;
    public Transform[] bonuses;
    public LayerMask unwalkableMask;
    public Vector2Int gridSize;
    public Vector2 nodeSize;

    Node[,] grid;

    public List<Node> path;

    public BoxCollider2D gridArea;
    public int numberOfBonuses = 10;
    public GameObject PointPrefab;

    private Vector3 previousPlayerPosition;
    private int selectedDistr;

    public void GenerateItemUniform(int numberOfItems, GameObject itemPrefab, string tag)
    {
        Bounds bounds = this.gridArea.bounds;

        bonuses = new Transform[numberOfItems];
        for (int i = 0; i < numberOfItems; i++)
        {
            bool positionOccupied = true;
            Vector3 newPosition = Vector3.zero;

            while (positionOccupied)
            {
                float x = Random.Range(bounds.min.x, bounds.max.x);
                float y = Random.Range(bounds.min.y, bounds.max.y);

                newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

                Collider2D[] colliders = Physics2D.OverlapBoxAll(newPosition, itemPrefab.GetComponent<SpriteRenderer>().bounds.size, 0); 
                positionOccupied = colliders.Any(c => c.CompareTag("Player") || c.CompareTag("Bonus") || c.CompareTag("Obstacle") || c.CompareTag("Opponent"));
            }

            GameObject item = Instantiate(itemPrefab, newPosition, Quaternion.identity);
            item.tag = tag;
            item.transform.position = newPosition;

            bonuses[i] = item.transform;
        }
    }

    public void GenerateItemNormal(int numberOfItems, GameObject itemPrefab, string tag)
    {
        Bounds bounds = this.gridArea.bounds;

        bonuses = new Transform[numberOfItems];
        for (int i = 0; i < numberOfItems; i++)
        {
            bool positionOccupied = true;
            Vector3 newPosition = Vector3.zero;

            while (positionOccupied)
            {
                float x, y;
                do
                {
                    x = RandomNormal(bounds.center.x, bounds.size.x / 6);
                    y = RandomNormal(bounds.center.y, bounds.size.y / 6);
                }
                while (x < bounds.min.x || x>bounds.max.x || y<bounds.min.y || y>bounds.max.y);

                newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

                Collider2D[] colliders = Physics2D.OverlapBoxAll(newPosition, itemPrefab.GetComponent<SpriteRenderer>().bounds.size, 0);
                positionOccupied = colliders.Any(c => c.CompareTag("Player") || c.CompareTag("Bonus") || c.CompareTag("Obstacle") || c.CompareTag("Opponent"));
            }

            GameObject item = Instantiate(itemPrefab, newPosition, Quaternion.identity);
            item.tag = tag;
            item.transform.position = newPosition;

            bonuses[i] = item.transform;
        }
    }

    private float RandomNormal(float mean, float standardDeviation)
    {
        float u1 = Random.value;
        float u2= Random.value;
        float z=Mathf.Sqrt(-2.0f*Mathf.Log(u1))*Mathf.Sin(2.0f*Mathf.PI*u2); // box-muller
        return mean+standardDeviation*z;
    }

    private void Start()
    {
        CreateGrid();
        selectedDistr=DistributionSelector.GetSelectedAlgorithm();
        if (selectedDistr==0)
        {
            GenerateItemUniform(numberOfBonuses, PointPrefab, "Bonus");
        }
        else if (selectedDistr==1)
        {
            GenerateItemNormal(numberOfBonuses, PointPrefab, "Bonus");
        }

        previousPlayerPosition = player.position;
    }

    private void Update()
    {
        if (previousPlayerPosition != player.position)
        {
            UpdateGrid(player.position, previousPlayerPosition);
            previousPlayerPosition = player.position;
        }
    }

    public int MaxSize
    {
        get { return gridSize.x * gridSize.y; }
    }

    private void UpdateGrid(Vector3 currentPlayerPosition, Vector3 previousPlayerPosition)
    {
        Node previousNode = NodeFromWorldPoint(previousPlayerPosition);
        Node currentNode = NodeFromWorldPoint(currentPlayerPosition);

        previousNode.walkable = true;
        currentNode.walkable = false;
    }

    private void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        Vector2 worldBottomLeft = transform.position - new Vector3(gridSize.x * nodeSize.x / 2, gridSize.y * nodeSize.y / 2);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPoint = worldBottomLeft + new Vector2(x * nodeSize.x, y * nodeSize.y) + nodeSize / 2;
                bool walkable = !Physics2D.OverlapBox(worldPoint, nodeSize / 2, 0, unwalkableMask);

                if (Vector2.Distance(worldPoint, player.position)<0.01f)               
                {
                    walkable = false;
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Vecinul de sus
        if (node.gridY < gridSize.y - 1)
        {
            neighbours.Add(grid[node.gridX, node.gridY + 1]);
        }

        // Vecinul din dreapta
        if (node.gridX < gridSize.x - 1)
        {
            neighbours.Add(grid[node.gridX + 1, node.gridY]);
        }

        // Vecinul de jos
        if (node.gridY > 0)
        {
            neighbours.Add(grid[node.gridX, node.gridY - 1]);
        }

        // Vecinul din stanga
        if (node.gridX > 0)
        {
            neighbours.Add(grid[node.gridX - 1, node.gridY]);
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //float percentX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
        //float percentY = (worldPosition.y + gridSize.y / 2) / gridSize.y;

        float percentX = (worldPosition.x - transform.position.x + gridSize.x * nodeSize.x / 2) / (gridSize.x * nodeSize.x);
        float percentY = (worldPosition.y - transform.position.y + gridSize.y * nodeSize.y / 2) / (gridSize.y * nodeSize.y);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1)); //2D
        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            Node opponentNode = NodeFromWorldPoint(opponent.position);
            Node[] bonusesNodes = new Node[bonuses.Length];

            int i = 0;
            foreach (var bonus in bonuses)
            {
                if (bonus.gameObject.activeSelf)
                {
                    bonusesNodes[i] = NodeFromWorldPoint(bonus.position);
                    i++;
                }
            }

            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (playerNode == node)
                {
                    Gizmos.color = Color.cyan;
                }

                if (opponentNode == node)
                {
                    Gizmos.color = Color.magenta;
                }

                foreach (var bonusNode in bonusesNodes)
                {
                    if (bonusNode != null && bonusNode == node)
                    {
                        Gizmos.color = Color.green;
                    }
                }

                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawCube(node.worldPosition, nodeSize - new Vector2(0.1f, 0.1f));
            }
        }
    }
}
