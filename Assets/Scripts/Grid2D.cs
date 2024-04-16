using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2Int gridSize;
    public Vector2 nodeSize;

    Node[,] grid;

    public List<Node> path;

    private void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        Vector2 worldBottomLeft = transform.position - new Vector3(gridSize.x * nodeSize.x / 2, gridSize.y * nodeSize.y / 2);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPoint = worldBottomLeft + new Vector2(x * nodeSize.x, y * nodeSize.y) + nodeSize / 2;
                bool walkable = !Physics2D.OverlapBox(worldPoint, nodeSize / 2, 0, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
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
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (playerNode == node)
                {
                    Gizmos.color = Color.cyan;
                }

                if (path!=null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.worldPosition, nodeSize-new Vector2(0.1f, 0.1f));

            }
        }
    }
}
