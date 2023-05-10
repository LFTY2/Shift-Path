using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Connector : MonoBehaviour
{
    float speed = 5;

    [SerializeField]
    private Path pathRed, 
        pathBlue; 

    [SerializeField]
    private BallHolder[] ballHolders1,
        ballHolders2;


    [SerializeField] private Path nullPath;

    public bool isTouching;
    public bool isAlloweMoving = true;

    public Vector2 fingerDownPosition;
    private Vector2 fingerMovePosition;

    private float minDistanceForSwipe = 150f;

    Position position = Position.Midle;
    enum Position
    {
        Left,
        Right,
        Midle
    }

    void Start()
    {
        pathRed.ballContainersAdditional = ballHolders1;
        pathBlue.ballContainersAdditional = ballHolders2;
    }

    void Update()
    {
        if (!isTouching) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                fingerMovePosition = touch.position;
                float distance = Vector2.Distance(fingerDownPosition, fingerMovePosition);
                if (distance > minDistanceForSwipe)
                {                    
                    CheckSwipe();
                    isTouching = false;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }

    void CheckSwipe()
    {
        if (!isAlloweMoving) return;
        Vector2 direction = fingerMovePosition - fingerDownPosition;
        if (direction.x > 0)
        {            
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
        StartCoroutine(MoveCooldown());
    }

    public void MoveLeft()
    {
        if (position == Position.Midle)
        {
            pathBlue.isPathFull = false;
            pathRed.ballContainersAdditional = RevertArray(ballHolders2);
            position = Position.Left;
        }
        else if (position == Position.Right)
        {
            pathRed.isPathFull = true;
            pathRed.ballContainersAdditional = ballHolders1;
            pathBlue.ballContainersAdditional = ballHolders2;
            position = Position.Midle;
        }
        else 
        {
            return;
        }

        StartCoroutine(Move(transform.position - new Vector3(1.7f, 0, 0)));
        ChangePath(ballHolders1, position == Position.Midle ? pathRed : nullPath);
        ChangePath(ballHolders2, position == Position.Midle ? pathBlue : pathRed);
    }

    public void MoveRight()
    {
        if (position == Position.Midle)
        {
            pathRed.isPathFull = false;
            pathBlue.ballContainersAdditional = RevertArray(ballHolders1);
            position = Position.Right;
        }
        else if (position == Position.Left)
        {
            pathBlue.isPathFull = true;
            pathRed.ballContainersAdditional = ballHolders1;
            pathBlue.ballContainersAdditional = ballHolders2;
            position = Position.Midle;
        }
        else 
        {
            return;
        }

        StartCoroutine(Move(transform.position + new Vector3(1.7f, 0, 0)));
        ChangePath(ballHolders1, position == Position.Midle ? pathRed : pathBlue);
        ChangePath(ballHolders2, position == Position.Midle ? pathBlue : nullPath);
    }

    private void ChangePath(BallHolder[] ballHolders, Path path)
    {
        int count = ballHolders.Length;
        for(int i = 0; i < count ; i++)
        {
            Ball ballInHolder = ballHolders[i].ball.GetComponent<Ball>();
            ballInHolder.ChangePath(path);
            ballInHolder.MoveToPos(ballHolders[i].transform);
        }
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;

            yield return null;
        }
    }

    private BallHolder[] RevertArray(BallHolder[] originalArray)
    {
        BallHolder[] reversedArray = (BallHolder[])originalArray.Clone();
        Array.Reverse(reversedArray);
        return reversedArray;
    }

    IEnumerator MoveCooldown()
    {
        isAlloweMoving = false;
        yield return new WaitForSeconds(0.5f);
        isAlloweMoving = true;
    }
}
