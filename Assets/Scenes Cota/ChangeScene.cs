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

    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void Level4()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void Level5()
    {
        SceneManager.LoadScene("Level 5");
    }
    public void Level7()
    {
        SceneManager.LoadScene("Level 7");
    }
    public void Level6()
    {
        SceneManager.LoadScene("Level 6");
    }
}
