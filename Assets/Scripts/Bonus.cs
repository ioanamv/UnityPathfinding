using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            ScoreManager.IncrementPlayerScore();
        }
        else if (collision.tag == "Opponent")
        {
            gameObject.SetActive(false);
            ScoreManager.IncrementOpponentScore();
        }
    }
}
