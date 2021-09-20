using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle, _obstacleWithBomb;
    private int _coloms = 8;
    private int _lines = 4;
    private float _xOffset = 2.23f;
    private int _bombs = 12;
    private int _currentNumberOfObstacle = 1;
    private bool _needObstacle;

    public int[] _obstacleWithBombsNumbers = new int[12];

    private void Awake()
    {
        Vector3 startPosition = new Vector3(-7.52f, 2.77f, 0); //set start position of the firsgt obstacle
        Vector3 obstalceCurrentPosition = startPosition; //set cuurent position of the obstacle
        Vector3 offset = new Vector3(-0.29f, - 2.11f, 0); //set offset or distance between obstacles

        _obstacleWithBombsNumbers = FillBombsArray();

        for (int i = 0; i < _lines; i++)
        {
            for (int j = 0; j < _coloms; j++)
            {
                _needObstacle = true;

                for (int g = 0; g < _bombs; g++)
                {
                    if ((_currentNumberOfObstacle == _obstacleWithBombsNumbers[g]) && _needObstacle)
                    {
                        Instantiate(_obstacleWithBomb, obstalceCurrentPosition, Quaternion.identity); //create obstacle
                        GameObject.FindObjectOfType<ObstacleInformation>().SetMyObstacleNumber(_currentNumberOfObstacle);
                        _currentNumberOfObstacle++;
                        _needObstacle = false;
                    }
                }

                // we don't need double obstacle, if we have created with a bomb, the we go next
                if (_needObstacle)
                {
                    Instantiate(_obstacle, obstalceCurrentPosition, Quaternion.identity); //create obstacle
                    GameObject.FindObjectOfType<ObstacleInformation>().SetMyObstacleNumber(_currentNumberOfObstacle);
                    _currentNumberOfObstacle++;
                }

                obstalceCurrentPosition += new Vector3(_xOffset, 0, 0); //change position for next obstacle in this line
            }

            obstalceCurrentPosition = startPosition + offset * (i + 1); //change position for the next line 
        }
    }

    //make array with 12 different numbers of obstacles with a bomb
    private int[] FillBombsArray()
    {
        int[] currentArray = new int[12];
        bool a = false;

        for (int i = 0; i < _bombs; )
        {
            a = false;
            int NewRandomValue = Random.Range(1, 33);

            for (int j = 0; j < i; j++)
            {
                if (currentArray[j] == NewRandomValue)
                {
                    a = true;
                    break;
                } 
            }
            if (!a)
            {
                currentArray[i] = NewRandomValue;
                i++;
            }
        }
        return currentArray;
    }
}
