using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKickBack = new Vector2(25f, 5f);
    float movingTowardCenterSpeed = 1.5f;

    //State
    bool playerHasDied;
    [HideInInspector] public bool enetringPortalToNextLevel;
    //public bool playerUnderWater;


    //Cached component references
    Animator animator;
    Rigidbody2D rigidBody;
    float gravityScaleAtStart;
    float movementSpeedAtStart;
    public Vector3 exitPortalCoordinates;
    [SerializeField] Collider2D playerBodyColider;
    [SerializeField] Collider2D playerFeetColider;
    [SerializeField] Sprite gettingSuckedToPortal;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravityScaleAtStart = rigidBody.gravityScale;
        movementSpeedAtStart = movementSpeed;
    }

    void Update()
    {
        if (playerHasDied)
            return;
        else if (enetringPortalToNextLevel)
            EnetringNextLevel();
        else
        {
            MovePlayer();
            FlipSprite();
            ClimbLadder();
            Jump();
            PlayersDeath();
        }
    }

    private void MovePlayer()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * movementSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;

        bool playerMovingHorizontaly = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerMovingHorizontaly);
    }

    void FlipSprite()
    {
        bool playerMovingHorizontaly = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if (playerMovingHorizontaly)
            transform.localScale = new Vector2(Math.Sign(rigidBody.velocity.x), 1f);
    }

    private void Jump()
    {
        if (!playerFeetColider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!playerFeetColider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidBody.gravityScale = gravityScaleAtStart;
            movementSpeed = movementSpeedAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        else
        {
            bool playerIsClimbing = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", playerIsClimbing);

            movementSpeed = 2f;
            rigidBody.gravityScale = 0.5f;
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(rigidBody.velocity.x, controlThrow * climbSpeed);
            rigidBody.velocity = climbVelocity;
        }
    }

    private void PlayersDeath()
    {
        if ((playerBodyColider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || 
            playerBodyColider.IsTouchingLayers(LayerMask.GetMask("Hazard"))) && !playerHasDied)
        {
            playerHasDied = true;
            rigidBody.velocity = Vector2.zero;
            animator.SetTrigger("isDead");
            GetComponent<Rigidbody2D>().velocity = new Vector2(deathKickBack.x * -transform.localScale.x , deathKickBack.y);
            playerBodyColider.enabled = false;
            FindObjectOfType<GameSession>().GetComponent<GameSession>().ProcessPlayersDeath();
        }
    }

    public void PlayerSpawn()
    {
        playerHasDied = false;
        animator.SetTrigger("Spawned");
        playerBodyColider.enabled = true;
        transform.position = GameObject.Find("Spawn Point").transform.position;
    }

    void EnetringNextLevel()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = 0;
        animator.enabled = false;

        GetComponent<SpriteRenderer>().sprite = gettingSuckedToPortal;
        transform.Rotate(new Vector3(0, 0, 360*Time.deltaTime));
        transform.position = Vector2.MoveTowards
            (transform.position, exitPortalCoordinates, movingTowardCenterSpeed * Time.deltaTime);

        if (transform.localScale.y > 0)
            transform.localScale -= new Vector3(0.02f, 0.02f);
        if (transform.localScale == Vector3.zero)
            Destroy(gameObject);
    }

    //private void Underwater()
    //{
    //    if (!playerBodyColider.IsTouchingLayers(LayerMask.GetMask("Water")))
    //    {
    //        playerUnderWater = false;
    //        rigidBody.gravityScale = gravityScaleAtStart;
    //        return;
    //    }
    //    else
    //    {
    //        playerUnderWater = true;
    //        rigidBody.gravityScale = 0f;
    //        float controlThrowY = CrossPlatformInputManager.GetAxis("Vertical");
    //        float controlThrowX = CrossPlatformInputManager.GetAxis("Horizontal");
    //        Vector2 UnderWaterVelocity = new Vector2(controlThrowX * 3f, controlThrowY * 10f);
    //        rigidBody.velocity = UnderWaterVelocity;
    //    }
    //}
}
