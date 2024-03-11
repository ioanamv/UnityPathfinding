using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    //private Renderer rendererComponent;

    //private void Start()
    //{
    //    rendererComponent = GetComponent<Renderer>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            //rendererComponent.enabled = false;
            //Destroy(gameObject);
        }
    }
}
