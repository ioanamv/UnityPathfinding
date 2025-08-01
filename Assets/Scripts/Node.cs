using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;

    private int heapIndex;

    public Node(bool _walkeble, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkeble;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;

        gCost = int.MaxValue;
        hCost = 0;
        parent = null;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; } 
    }

    public int CompareTo(Node nodeToCompare)
    {
        if (PathfindingSelector.GetSelectedAlgorithm() == 0) //A*
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
        else //dijkstra
        {
            return -gCost.CompareTo(nodeToCompare.gCost);
        }
    }
}
