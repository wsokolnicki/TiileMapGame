using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //config
    [SerializeField] float moveSpeed = 1.5f;

    //Cache
    Rigidbody2D rigidBody;
    Collider2D enemyCollider;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        rigidBody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D ground)
    {
        moveSpeed = -1 * moveSpeed;
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed), 1f);
    }

}
