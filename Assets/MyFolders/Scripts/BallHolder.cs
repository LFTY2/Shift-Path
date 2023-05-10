using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    public GameObject ball;
    [SerializeField] private GameObject[] ballPrefabs;
    private GameManager gameManager;
    public Path path;
    
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        ball = Instantiate(RandomBallIndex(), transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().ChangePath(path);
    }

    public GameObject RandomBallIndex()
    {
        int randNum = Random.Range(0, ballPrefabs.Length);
        GameObject ballToInstantiate = ballPrefabs[randNum];
        bool isAloved = gameManager.IsAllowedNewBall(ballToInstantiate);
        if (isAloved)
            return ballToInstantiate;
        else
            return RandomBallIndex();
    }
}
