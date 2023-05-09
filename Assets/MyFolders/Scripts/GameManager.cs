using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int maxBallsOfColor;
    private Dictionary<PathColor, int> ballsColorCount = new Dictionary<PathColor, int>();
    Path[] paths;

    [SerializeField] GameObject youWonMenu, pauseMenu, pauseButton;
    public enum PathColor
    {
        red,
        blue,
        green,
        yellow
    }
    void Awake()
    {
        ballsColorCount.Add(PathColor.red, 0);
        ballsColorCount.Add(PathColor.blue, 0);
        ballsColorCount.Add(PathColor.green, 0);
        ballsColorCount.Add(PathColor.yellow, 0);

        paths = GameObject.FindObjectsOfType<Path>(); 
    }

    public bool IsAllowedNewBall(GameObject ball)
    {
        PathColor color = ball.GetComponent<Ball>().color;
        if (ballsColorCount[color] < maxBallsOfColor)
        {
            ballsColorCount[color]++;
            return true;           
        }            
        else
            return false;
    }

    public void CheckWin()
    {
        foreach(var path in paths)
        {
            if (path.isNullPath)
                continue;

            if(!path.isDone)
            {
                return;
            }
        }
        Win();
    }
    private void Win()
    {
        youWonMenu.SetActive(true);        
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void Unpause()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
