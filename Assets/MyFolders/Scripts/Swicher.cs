using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swicher : MonoBehaviour
{
    [SerializeField] BallHolder[] ballContainers1, ballContainers2;
    [SerializeField]private bool isAlowedToSwitch = true;
    
    private void OnMouseDown()
    {
        Switch();      
    }

    void Switch()
    {
        if (!isAlowedToSwitch) return;

        int count = ballContainers1.Length;

        for (int i = 0; i < count; i++)
        {
            BallHolder ballContainer1 = ballContainers1[i];
            BallHolder ballContainer2 = ballContainers2[i];

            GameObject ball1 = ballContainer1.ball;
            GameObject ball2 = ballContainer2.ball;

            Ball ballScript1 = ball1.GetComponent<Ball>();
            Ball ballScript2 = ball2.GetComponent<Ball>();

            ballScript1.MoveToPos(ballContainer2.transform);
            ballScript2.MoveToPos(ballContainer1.transform);

            ballContainer1.ball = ball2;
            ballContainer2.ball = ball1;

            Path path1 = ballScript1.path;
            Path path2 = ballScript2.path;

            ballScript1.ChangePath(path2);
            ballScript2.ChangePath(path1);

            path1.CheckWin();
            path2.CheckWin();
        }

        StartCoroutine(SwitchCooldown());
    }

    IEnumerator SwitchCooldown()
    {
        isAlowedToSwitch = false;
        yield return new WaitForSeconds(0.5f);
        isAlowedToSwitch = true;
    }
}
