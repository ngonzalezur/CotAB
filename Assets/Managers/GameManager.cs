using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TutoState GameState;

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
        ChangeState(TutoState.GenerateGrid);        
    }
    public void ResetAndLoadScene()
    {
        AnaliticSender();
        StartCoroutine(DestroyDontDestroyOnLoadObjectsAndLoadScene());
    }

    private System.Collections.IEnumerator DestroyDontDestroyOnLoadObjectsAndLoadScene()
    {
        // Creamos un objeto temporal para acceder a la escena DontDestroyOnLoad
        GameObject temp = new GameObject("TempDDOL");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        // Buscamos todos los GameObjects que viven en esa escena especial
        List<GameObject> dontDestroyObjects = new List<GameObject>();
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>(true))
        {
            if (go.scene == ddolScene && go != temp)
            {
                dontDestroyObjects.Add(go);
            }
        }

        // Destruimos únicamente esos objetos
        foreach (GameObject go in dontDestroyObjects)
        {
            Destroy(go);
        }

        // Destruimos el temporal
        Destroy(temp);

        // Esperamos 1 frame para que Unity termine de destruir
        yield return null;

        // Cargamos la escena deseada


        // Cargar la nueva escena
        SceneManager.LoadScene("Menus");
    }

    public void AnaliticSender()
    {
        //llamar la funcion que manda los datos
        if (AttDruid1 == 0)
        {
            AttDruid1 = 1;
        }
        if (AttDruid2 == 0)
        {
            AttDruid2 = 1;
        }
        if (AttDruid3 == 0)
        {
            AttDruid3 = 1;
        }
        if (AttDruid4 == 0)
        {
            AttDruid4 = 1;
        }
        if (MeleeDruid == 0)
        {
            MeleeDruid = 1;
        }

        if (AttRobot1 == 0)
        {
            AttRobot1 = 1;
        }
        if (AttRobot2 == 0)
        {
            AttRobot2 = 1;
        }
        if (AttRobot3 == 0)
        {
            AttRobot3 = 1;
        }
        if (AttRobot4 == 0)
        {
            AttRobot4 = 1;
        }
        if (MeleeRobot == 0)
        {
            MeleeRobot = 1;
        }

        AnalyticsManager.Instance.SendCustomEventEndGame(AttDruid1/HitAttDruid1,AttDruid2/HitAttDruid2,AttDruid3/HitAttDruid3,AttDruid4/HitAttDruid4,MeleeDruid/HitMeleeDruid,interactionTotal/Time.time,AttRobot1/HitAttRobot1, AttRobot2/HitAttRobot2, AttRobot3/HitAttRobot3, AttRobot4/HitAttRobot4, MeleeRobot/HitMeleeRobot, (int)AttDruid1, (int)AttDruid2, (int)AttDruid3, (int)AttDruid4, (int)MeleeDruid, (int)AttRobot1, (int)AttRobot2, (int)AttRobot3, (int)AttRobot4, (int)MeleeRobot,character);
    }
    public void ChangeState(TutoState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case TutoState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case TutoState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                //MauriManager.Instance.SpawnHeroes();
                break;
            case TutoState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                //MauriManager.Instance.SpawnEnemies();
                break;
            case TutoState.GenerateUI:
                //CanvaManager.Instance.AssignAttack();
                CanvaManager.Instance.PutSprites();
                this.ChangeState(TutoState.HeroesTurn);
                CanvaManager.Instance.CanSprites = true;
                break;
            case TutoState.HeroesTurn:
                UnitManager.Instance.CanPlay = true;
                //MauriManager.Instance.CanPlay = true;
                break;
            case TutoState.EndFight:
                UnitManager.Instance.CanPlay = false;
                CanvaManager.Instance.CanSprites = false;
                Debug.Log("GG");
                this.ChangeState(TutoState.End);
                break;
            case TutoState.End:
                var scene = SceneManager.GetActiveScene().name;
                if (scene == "Level 1")
                {
                    if (PlayerSelect.Instance.currentSelectedUnitPlayer1.UnitName == "Druid")
                    {
                        SceneManager.LoadScene("Cine02Druid");
                    }
                    else
                    {
                        SceneManager.LoadScene("Cine02Robot");
                    }
                    
                }
                else if (scene == "Level 2")
                {
                    if (PlayerSelect.Instance.currentSelectedUnitPlayer1.UnitName == "Druid")
                    {
                        SceneManager.LoadScene("Cine03Druid");
                    }
                    else
                    {
                        SceneManager.LoadScene("Cine03Robot");
                    }

                }
                else if (scene == "Level 3")
                {                    
                    SceneManager.LoadScene("Level 4");
                }
                else if (scene == "Level 4")
                {
                    SceneManager.LoadScene("Level 5");
                }
                else if (scene == "Level 5")
                {
                    if (PlayerSelect.Instance.currentSelectedUnitPlayer1.UnitName == "Druid")
                    {
                        SceneManager.LoadScene("Cine04Druid");
                    }
                    else
                    {
                        SceneManager.LoadScene("Cine04Robot");
                    }

                }
                else if (scene == "Level 6")
                {

                    ResetAndLoadScene();
                }

                if (scene == "Level hard")
                {
                    //llamar la funcion que manda los datos
                    if(AttDruid1 == 0)
                    {
                        AttDruid1 = 1;
                    }
                    if (AttDruid2 == 0)
                    {
                        AttDruid2 = 1;
                    }
                    if (AttDruid3 == 0)
                    {
                        AttDruid3 = 1;
                    }
                    if (AttDruid4 == 0)
                    {
                        AttDruid4 = 1;
                    }
                    if (MeleeDruid == 0)
                    {
                        MeleeDruid = 1;
                    }

                    if (AttRobot1 == 0)
                    {
                        AttRobot1 = 1;
                    }
                    if (AttRobot2 == 0)
                    {
                        AttRobot2 = 1;
                    }
                    if (AttRobot3 == 0)
                    {
                        AttRobot3 = 1;
                    }
                    if (AttRobot4 == 0)
                    {
                        AttRobot4 = 1;
                    }
                    if (MeleeRobot == 0)
                    {
                        MeleeRobot = 1;
                    }
                    //AnalyticsManager.Instance.SendCustomEvent(HitMeleeDruid/MeleeDruid,HitMeleeRobot/MeleeRobot,HitAttDruid1/ AttDruid1, HitAttDruid2/AttDruid2,HitAttDruid3/AttDruid3, HitAttDruid4/AttDruid4,interactionTotal/Time.time,HitAttRobot1/AttRobot1,HitAttRobot2/AttRobot2,HitAttRobot3/AttRobot3,HitAttRobot4/AttRobot4,(int)MeleeDruid,(int)MeleeRobot,(int)combo,(int)AttDruid1,(int)AttDruid2,(int)AttDruid3,(int)AttDruid4,(int)AttRobot2,(int)AttRobot3,(int)AttRobot4,(int)AttRobot1,character);
                    Debug.Log(HitAttDruid4);                    
                    var persist = (GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero));
                    //Destroy(persist.gameObject);
                    //SceneManager.LoadScene("Elegir");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}


public enum TutoState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EndFight = 4,
    End = 5,
    GenerateUI = 6
}