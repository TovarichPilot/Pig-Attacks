using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Sprite[] _pigsDirections;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject spawnManager;
    [SerializeField] private int[] haveAlreadyTakenBomb = new int[12];
    private SpriteRenderer _playerCurrentSprite;
    private BoxCollider2D _boxCollider2DPlayer;
    private Rigidbody2D _playerRb;
    private Vector3 _moveDelta;
    private Vector3 firstPosition = Vector3.zero;
    private float firstTime = 0.0f;

    public float leftBorder;
    public float rightBorder;
    public float upAndDownBorder;



    private void Start()
    {
        _boxCollider2DPlayer = gameObject.GetComponent<BoxCollider2D>();
        _playerCurrentSprite = gameObject.GetComponent<SpriteRenderer>();
        _playerRb = gameObject.GetComponent<Rigidbody2D>();
        firstPosition = transform.position;
    }

    private void FixedUpdate()
    {
        //simple input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //reset MoveDelta
        _moveDelta = new Vector2(x, y);

        // Set sprite direction (left or right)
        if (_moveDelta.x > 0)
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
        }
        else if (_moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-0.25f, 0.25f, 1.0f);
        }

        // Set sprite down or up 
        if (_moveDelta.y > 0)
        {
            _playerCurrentSprite.sprite = _pigsDirections[1];

            _boxCollider2DPlayer.size = new Vector3(3.1f, 5.7f, 1.0f);
        }
        else if (_moveDelta.y < 0)
        {
            _playerCurrentSprite.sprite = _pigsDirections[2];
            _boxCollider2DPlayer.size = new Vector3(3.1f, 5.7f, 1.0f);
        }
        else
        {
            _playerCurrentSprite.sprite = _pigsDirections[0];
            _boxCollider2DPlayer.size = new Vector3(5.7f, 3.1f, 1.0f);

        }

        //Move player
        transform.Translate(_moveDelta * speed * Time.deltaTime);

        //move camera only if player is moving
        if (Time.time - firstTime > 0.05f)
        {
            if ((transform.position - firstPosition).magnitude > 0.08f)
            {
                _mainCamera.transform.Translate(_moveDelta * speed * Time.deltaTime / 1.5f);
            }
            firstTime = Time.time;
            firstPosition = transform.position;
        }

        //limit player position
        if (transform.position.x < leftBorder)
        {
            transform.position = new Vector3(leftBorder, transform.position.y, 0);
        }
        else if (transform.position.x > rightBorder)
        {
            transform.position = new Vector3(rightBorder, transform.position.y, 0);
        }
        else if (transform.position.y > upAndDownBorder)
        {
            transform.position = new Vector3(transform.position.x, upAndDownBorder, 0);
        }
        else if (transform.position.y < -upAndDownBorder)
        {
            transform.position = new Vector3(transform.position.x, -upAndDownBorder, 0);
        }

        //limit Camera Position
        if (_mainCamera.transform.position.x < -3.5f)
        {
            _mainCamera.transform.position = new Vector3(-3.5f, _mainCamera.transform.position.y, -10);
        }
        else if (_mainCamera.transform.position.x > 1.7f)
        {
            _mainCamera.transform.position = new Vector3(1.7f, _mainCamera.transform.position.y, -10);
        }
        else if (_mainCamera.transform.position.y > 0.6f)
        {
            _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, 0.6f, -10);
        }
        else if (_mainCamera.transform.position.y < -0.6f)
        {
            _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, -0.6f, -10);
        }

    }

    //if player collides with a obstacle with a bomb, then player have a bomb in limit count
    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("ObstacleWithBomb"))
        {
            bool canTakeBomb = true;
            int currentCollidedObstacleWithBomb = collision.gameObject.GetComponent<ObstacleInformation>().obstacleNumber;


            Vector3 bombPos = transform.position + new Vector3(1.05f, 0, 0);


            for (int i = 0; i < 12; i++)
            {
                //if we have already taken bomb from this obstacle we can't get another one again
                if (haveAlreadyTakenBomb[i] == currentCollidedObstacleWithBomb)
                {
                    canTakeBomb = false;
                    break;
                }
            }


            //array with contacted obstacleswithbombs
            for (int i = 0; i < 12; i++)
            {
                if (haveAlreadyTakenBomb[i] == 0)
                {
                    haveAlreadyTakenBomb[i] = currentCollidedObstacleWithBomb;
                    break;
                }
            }

            if (canTakeBomb)
            {
                Instantiate(_bomb, bombPos, Quaternion.identity);
            }
        }
    }
}
