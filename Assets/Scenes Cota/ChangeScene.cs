using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void LevelEasy()
    {
        SceneManager.LoadScene("Level easy");
    }
    public void LevelMedium()
    {
        SceneManager.LoadScene("Level medium");
    }
    public void LevelHard()
    {
        SceneManager.LoadScene("Level hard");
    }

    public void SecondPlayer()
    {
        SceneManager.LoadScene("SecondPlayer");
    }
}
