using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerPathfindingMovement : MonoBehaviour
{
    private BoxCollider2D _enemyBoxCollider;
    private Rigidbody2D _enemyRb;
    private Vector3 enemyVelocity;
    public SpriteRenderer _enemyCurrentSprite;
    public float delay = 1.0f;
    public float nextTime = 3.0f;
    public float currentTime = 0.0f;
    public float speed = 2.0f;

    [SerializeField]
    private Sprite[] _farmerSprites;
    private float _xOffset = 0.1f;
    private float _ySpeed = 0.5f;


    public float UpAndDownBoundary;
    public bool upMoving;
    public bool isStop;

    private void Start()
    {
        isStop = false;
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyCurrentSprite = GetComponent<SpriteRenderer>();
        enemyVelocity = new Vector3(-_xOffset, -_ySpeed, 0.0f) * speed;
    }
    private void Update()
    {
        currentTime = Time.time;

        // enemó stands at the place periodically for a while


        // move enemy up and down in definite area
        if (transform.position.y > UpAndDownBoundary)
        {
            enemyVelocity = new Vector3(-_xOffset, -_ySpeed, 0.0f) * speed;
            upMoving = false;
        }
        else if (transform.position.y < -UpAndDownBoundary)
        {
            enemyVelocity = new Vector3(_xOffset, _ySpeed, 0.0f) * speed;
            upMoving = true;
        }
        _enemyRb.velocity = enemyVelocity;

        //change sprites dependiong on the emeny direction
        if (!upMoving && !isStop)
        {
            _enemyCurrentSprite.sprite = _farmerSprites[2];
        }
        else if (upMoving && !isStop)
        {
            _enemyCurrentSprite.sprite = _farmerSprites[3];
        }
    }
}
