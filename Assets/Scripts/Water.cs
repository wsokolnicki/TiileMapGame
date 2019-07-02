using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D player)
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
}
