using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    //las 21 cosas que leen los kpis

    public static float AttDruid1 = 0;
    public static float AttDruid2 = 0;
    public static float AttDruid3 = 0;
    public static float AttDruid4 = 0;
    public static float MeleeDruid = 0;

    public static float AttRobot1 = 0;
    public static float AttRobot2 = 0;
    public static float AttRobot3 = 0;
    public static float AttRobot4 = 0;
    public static float MeleeRobot = 0;

    public static float interactionTotal = 0;

    public static float HitAttDruid1 = 0;
    public static float HitAttDruid2 = 0;
    public static float HitAttDruid3 = 0;
    public static float HitAttDruid4 = 0;
    public static float HitMeleeDruid = 0;

    public static float HitAttRobot1 = 0;
    public static float HitAttRobot2 = 0;
    public static float HitAttRobot3 = 0;
    public static float HitAttRobot4 = 0;
    public static float HitMeleeRobot = 0;

    public static float combo = 0;
    public static string character = "";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);        
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                //MauriManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                //MauriManager.Instance.SpawnEnemies();
                break;
            case GameState.GenerateUI:
                //CanvaManager.Instance.AssignAttack();
                CanvaManager.Instance.PutSprites();
                this.ChangeState(GameState.HeroesTurn);
                CanvaManager.Instance.CanSprites = true;
                break;
            case GameState.HeroesTurn:
                UnitManager.Instance.CanPlay = true;
                //MauriManager.Instance.CanPlay = true;
                break;
            case GameState.EndFight:
                UnitManager.Instance.CanPlay = false;
                CanvaManager.Instance.CanSprites = false;
                Debug.Log("GG");
                this.ChangeState(GameState.End);
                break;
            case GameState.End:
                var scene = SceneManager.GetActiveScene().name;
                if (scene == "Tutorial")
                {
                    SceneManager.LoadScene("Level easy");
                }
                else if (scene == "Level easy")
                {
                    SceneManager.LoadScene("Level medium");
                }
                else if (scene == "Level medium")
                {
                    SceneManager.LoadScene("Level hard");
                }

                if (scene == "Level hard" || UnitManager.Instance.SecondPlayer)
                {
                    //llamar la funcion que manda los datos
                    AnalyticsManager.Instance.SendCustomEvent(HitMeleeDruid/MeleeDruid,HitMeleeRobot/MeleeRobot,HitAttDruid1/ AttDruid1, HitAttDruid2/AttDruid2,HitAttDruid3/AttDruid3, HitAttDruid4/AttDruid4,interactionTotal,HitAttRobot1/AttRobot1,HitAttRobot2/AttRobot2,HitAttRobot3/AttRobot3,HitAttRobot4/AttRobot4,(int)MeleeDruid,(int)MeleeRobot,(int)combo,(int)AttDruid1,(int)AttDruid2,(int)AttDruid3,(int)AttDruid4,(int)AttRobot2,(int)AttRobot3,(int)AttRobot4,(int)AttRobot1,character);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}



public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EndFight = 4,
    End = 5,
    GenerateUI = 6
}