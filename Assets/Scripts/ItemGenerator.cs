using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public int numberOfBonuses = 10;
    public GameObject PointPrefab;

    public void Start()
    {
        GenerateItem(numberOfBonuses, PointPrefab, "Bonus");
    }

    public void GenerateItem(int numberOfItems, GameObject itemPrefab, string tag)
    {
        Bounds bounds = this.gridArea.bounds;
        for (int i = 0; i < numberOfItems; i++)
        {
            bool positionOccupied = true;
            Vector3 newPosition = Vector3.zero;

            while (positionOccupied)
            {
                float x = Random.Range(bounds.min.x, bounds.max.x);
                float y = Random.Range(bounds.min.y, bounds.max.y);

                newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

                Collider2D[] colliders = Physics2D.OverlapBoxAll(newPosition, itemPrefab.GetComponent<SpriteRenderer>().bounds.size, 0);  //(newPosition, 0.1f);
                positionOccupied = colliders.Any(c => c.CompareTag("Player") || c.CompareTag("Bonus") || c.CompareTag("Obstacle"));
            }

            GameObject item = Instantiate(itemPrefab, newPosition, Quaternion.identity);
            item.tag = tag;
            item.transform.position = newPosition;
        }
    }

    /////////

    //public void Start()
    //{
    //    int remainedBonuses = numberOfBonuses;
    //    int remainedHorizontalObstacles=numberOfObstacles;
    //    int remainedVerticalObstacles = numberOfObstacles;

    //    Bounds bounds = this.gridArea.bounds;
    //    int numberOfItems = numberOfBonuses + 2 * numberOfObstacles;

    //    for (int i = 0; i < numberOfItems; i++)
    //    {
    //        if (i % 3 == 0 && remainedBonuses > 0)
    //        {
    //            GenerateItem(PointPrefab, "Bonus", bounds);
    //            remainedBonuses--;
    //        }
    //        else if (i % 3 == 1 && remainedHorizontalObstacles > 0)
    //        {
    //            GenerateItem(HorizontalObstaclePrefab, "Obstacle", bounds);
    //            remainedHorizontalObstacles--;
    //        }
    //        else if (i % 3 == 2 && remainedVerticalObstacles > 0)
    //        {
    //            GenerateItem(VerticalObstaclePrefab, "Obstacle", bounds);
    //            remainedVerticalObstacles--;
    //        }
    //    }
    //}

    //private void GenerateItem(GameObject itemPrefab, string tag, Bounds bounds)
    //{
    //    Vector3 newPosition = GetRandomPosition(bounds, itemPrefab.GetComponent<SpriteRenderer>().bounds.size);

    //    GameObject item = Instantiate(itemPrefab, newPosition, Quaternion.identity);
    //    item.tag = tag;
    //    item.transform.position = newPosition;
    //}

    //private Vector3 GetRandomPosition(Bounds bounds, Vector2 itemSize)
    //{
    //    Vector3 newPosition = Vector3.zero;
    //    bool positionOccupied = true;

    //    while (positionOccupied)
    //    {
    //        float x = Random.Range(bounds.min.x, bounds.max.x);
    //        float y = Random.Range(bounds.min.y, bounds.max.y);

    //        newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

    //        Collider2D[] colliders = Physics2D.OverlapBoxAll(newPosition, itemSize, 0); 
    //        positionOccupied = colliders.Any(c => c.CompareTag("Player") || c.CompareTag("Bonus") || c.CompareTag("Obstacle"));
    //    }

    //    return newPosition;
    //}
}
