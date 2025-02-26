using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 7f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set { 
            if(_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }

    public bool _hasTarget = false;

    public bool HasTarget { 
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        rb.linearVelocity = new Vector2(moveSpeed * walkDirectionVector.x, rb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        } else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }

    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
