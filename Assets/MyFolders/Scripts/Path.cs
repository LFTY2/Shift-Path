using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    [SerializeField] private BallHolder[] ballContainers;
    [SerializeField] public BallHolder[] ballContainersAdditional;

    [SerializeField] private GameManager.PathColor color;

    private GameManager gameManager;
    private Path pathScript;

    public bool isDone;
    public bool isNullPath;
    public bool isPathFull = true;

    [SerializeField] BallContainersFillType ballContainersFillType;

    private enum BallContainersFillType
    {       
        Automatically,
        ByHand
    }
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        pathScript = gameObject.GetComponent<Path>();
        
        if(ballContainersFillType == BallContainersFillType.Automatically)
        {
            GetContiners();
        }      
        
    }

    void GetContiners()
    {
        ballContainers = transform.GetComponentsInChildren<BallHolder>();
    }


    public void Rotate(bool rotateDirection)
    {
        if (isNullPath||!isPathFull) return;

        BallHolder[] ballContainersAll;

        if (ballContainersAdditional != null)
        {
            ballContainersAll = ballContainers.Concat(ballContainersAdditional).ToArray();
        }
        else
        {
            ballContainersAll = ballContainers;
        }

        int count = ballContainersAll.Length;
        BallHolder firstContainer;
        BallHolder lastContainer;
        GameObject tempFirstBall;

        if (rotateDirection)
        {
            firstContainer = ballContainersAll[0];
            tempFirstBall = firstContainer.ball;

            for (int i = 0; i < count - 1; i++)
            {
                SwapBalls(ballContainersAll[i], ballContainersAll[i + 1]);
            }

            lastContainer = ballContainersAll[count - 1];
        }
        else
        {
            firstContainer = ballContainersAll[ballContainersAll.Length - 1];
            tempFirstBall = firstContainer.ball;

            for (int i = count - 1; i > 0; i--)
            {
                SwapBalls(ballContainersAll[i], ballContainersAll[i - 1]);
            }

            lastContainer = ballContainersAll[0];
        }
        Ball tempFirstBallScript = tempFirstBall.GetComponent<Ball>();
        tempFirstBallScript.MoveToPos(lastContainer.transform.position);
        lastContainer.ball = tempFirstBall;
        tempFirstBallScript.ChangePath(lastContainer.path);

        CheckWin();
    }
    private void SwapBalls(BallHolder currentContainer, BallHolder nextContainer)
    {
        GameObject nextBall = nextContainer.ball;

        Ball nextBallScript = nextBall.GetComponent<Ball>();
        nextBallScript.MoveToPos(currentContainer.transform.position);
        currentContainer.ball = nextBall;
        nextBallScript.ChangePath(currentContainer.path);
        
    }

    public void CheckWin()
    {
        if (isNullPath||!isPathFull) return;
        foreach(var container in ballContainers)
        {
            if(container.path.isNullPath)
            {
                continue;
            }
            GameObject ball = container.ball;
            GameManager.PathColor ballColor = ball.GetComponent<Ball>().color;
            if (ballColor != color)
            {
                return;
            }
        }
        this.isDone = true;
        gameManager.CheckWin();
    }

   
}
