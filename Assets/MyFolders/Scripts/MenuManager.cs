using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }
}
