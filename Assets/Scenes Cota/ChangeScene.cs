using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class ChangeScene : MonoBehaviour
{
    public PlayableDirector timeline; // Para usar el timeline para cambiar escenas
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void LevelEasy()
    {
        SceneManager.LoadScene("Level easy");
        Debug.Log("Puto");
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
