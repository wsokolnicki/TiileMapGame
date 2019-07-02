using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;

    private void OnTriggerEnter2D(Collider2D player)
    {
        AudioSource.PlayClipAtPoint(coinPickUpSFX, /*Camera.main.transform.position*/player.transform.position);
        Destroy(gameObject);
    }
}
