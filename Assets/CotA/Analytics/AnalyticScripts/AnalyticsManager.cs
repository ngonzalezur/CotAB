using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Awake()
    {
        /*
        Debug.Log("Awake ha sido llamado");
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Unity Services inicializados correctamente");
        */
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private async void Start()
    {
        Debug.Log("Awake ha sido llamado");

        await UnityServices.InitializeAsync();

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AnalyticsService.Instance.StartDataCollection();
            _isInitialized = true;
            Debug.Log("Unity Services inicializados correctamente");
            //SendCustomEvent();
        }
        else
        {
            Debug.LogError("Unity Services no se inicializaron correctamente");
        }
    }
    /*
    public void SendCustomEvent()
    {
        CustomEvent myEvent = new CustomEvent("TestEvent2")
        {
            { "testParameter3", 14 }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("Evento personalizado enviado correctamente");
    }

    void Start()
    {
        SendCustomEvent();
        Debug.Log("See llama la funcion");
    }
    */
    /*
    public void SendCustomEvent()
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("TestEvent2")
        {
            {"TestParameter3", 11}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("Se llamo eta vaina");
    }
    */
    /*
    public void SendCustomEvent(int parameterValue)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myEvent = new CustomEvent("TestEvent2")
    {
        {"TestParameter3", parameterValue}
    };

        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log($"Se llamó la función con el valor: {parameterValue}");
    }
    */

    public void SendCustomEvent(
    float basicAttackDruidAcc, float basicAttackRobotAcc,
    float druidAbility1Acc, float druidAbility2Acc, float druidAbility3Acc, float druidAbility4Acc,
    float interactionOverTime, float robotAbility1Acc, float robotAbility2Acc,
    float robotAbility3Acc, float robotAbility4Acc,
    int countBasicAttackDruid, int countBasicAttackRobot, int countBurnPoisonCombo,
    int countDruidAbility1, int countDruidAbility2, int countDruidAbility3, int countDruidAbility4,
    int countRobotAbility2, int countRobotAbility3, int countRobotAbility4, int parryCasts,
    string characterPicks)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myEvent = new CustomEvent("Playtest2Event")
    {
        { "BasicAttackDruidAcc", basicAttackDruidAcc }, //Veces que el ataque basico de la druida hace da;o / veces que se castea
        { "BasicAttackRobotAcc", basicAttackRobotAcc }, //Veces que el ataque basico del robot hace da;o / veces que se castea
        { "DruidAbility1Acc", druidAbility1Acc }, //Veces que la habilidad 1 de la druida hace da;o / veces que se castea
        { "DruidAbility2Acc", druidAbility2Acc }, //Veces que la habilidad 2 de la druida hace da;o / veces que se castea
        { "DruidAbility3Acc", druidAbility3Acc }, //Veces que la habilidad 3 de la druida hace da;o / veces que se castea
        { "DruidAbility4Acc", druidAbility4Acc }, //Veces que la habilidad 4 de la druida hace da;o / veces que se castea
        { "InteractionOverTime", interactionOverTime }, //Veces que el jugador se mueve o usa una habilidad / tiempo de partida
        { "RobotAbility1Acc", robotAbility1Acc }, //Veces que la habilidad 1 del robot hace da;o / veces que se castea
        { "RobotAbility2Acc", robotAbility2Acc }, //Veces que la habilidad 2 del robot hace da;o / veces que se castea
        { "RobotAbility3Acc", robotAbility3Acc }, //Veces que la habilidad 3 del robot hace da;o / veces que se castea
        { "RobotAbility4Acc", robotAbility4Acc }, //Veces que la habilidad 4 del robot hace da;o / veces que se castea
        { "CountBasicAttackDruid", countBasicAttackDruid }, //Veces que la druida usa el ataque basico
        { "CountBasicAttackRobot", countBasicAttackRobot }, //Veces que el robot usa el ataque basico
        { "CountBurnPoisonCombo", countBurnPoisonCombo }, //veces que se aplica el combo de fuego y veneno
        { "CountDruidAbility1", countDruidAbility1 }, //Veces que la druida usa la habilidad 1
        { "CountDruidAbility2", countDruidAbility2 }, //Veces que la druida usa la habilidad 2
        { "CountDruidAbility3", countDruidAbility3 }, //Veces que la druida usa la habilidad 3
        { "CountDruidAbility4", countDruidAbility4 }, //Veces que la druida usa la habilidad 4
        { "CountRobotAbility2", countRobotAbility2 }, //Veces que el robot usa la habilidad 2
        { "CountRobotAbility3", countRobotAbility3 }, //Veces que el robot usa la habilidad 3
        { "CountRobotAbility4", countRobotAbility4 }, //Veces que el robot usa la habilidad 4
        { "ParryCasts", parryCasts }, //Veces que el robot usa la habilidad 1
        { "CharacterPicks", characterPicks } //Un string que dice que personaje se eligio "Druid" o "Robot" por ejemplo
    };

        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log($"Evento 'Playtest2Event' registrado con los valores proporcionados.");
    }
}