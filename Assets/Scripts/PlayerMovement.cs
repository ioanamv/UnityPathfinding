using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero; 

    private void Update()
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
        Vector3 newPosition = transform.position + new Vector3(_direction.x, _direction.y, 0);

        Collider2D[] colliders = Physics2D.OverlapPointAll(newPosition);
        foreach(var collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
               return;
        }
        transform.position= newPosition;
    }
}