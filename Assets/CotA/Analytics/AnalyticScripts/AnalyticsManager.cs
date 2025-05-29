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

    public void SendCustomEventEndGame(
    float druidAbility1Acc, float druidAbility2Acc, float druidAbility3Acc, float druidAbility4Acc, float druidAbility5Acc,
    float interactionOverTime, float robotAbility1Acc, float robotAbility2Acc,
    float robotAbility3Acc, float robotAbility4Acc, float robotAbility5Acc,
    int countDruidAbility1, int countDruidAbility2, int countDruidAbility3, int countDruidAbility4, int countDruidAbility5,
    int countRobotAbility1,int countRobotAbility2, int countRobotAbility3, int countRobotAbility4, int countRobotAbility5,
    string characterPicks)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myEvent = new CustomEvent("Death_Win")
    {
        { "accDruidAbility1", druidAbility1Acc }, //Veces que la habilidad 1 de la druida hace da;o / veces que se castea
        { "accDruidAbility2", druidAbility2Acc }, //Veces que la habilidad 2 de la druida hace da;o / veces que se castea
        { "accDruidAbility3", druidAbility3Acc }, //Veces que la habilidad 3 de la druida hace da;o / veces que se castea
        { "accDruidAbility4", druidAbility4Acc }, //Veces que la habilidad 4 de la druida hace da;o / veces que se castea
        { "accDruidAbility5", druidAbility5Acc }, //Veces que la habilidad 5 de la druida hace da;o / veces que se castea
        { "interactionsOverTime", interactionOverTime }, //Veces que el jugador se mueve o usa una habilidad / tiempo de partida
        { "accRobotAbility1", robotAbility1Acc }, //Veces que la habilidad 1 del robot hace da;o / veces que se castea
        { "accRobotAbility2", robotAbility2Acc }, //Veces que la habilidad 2 del robot hace da;o / veces que se castea
        { "accRobotAbility3", robotAbility3Acc }, //Veces que la habilidad 3 del robot hace da;o / veces que se castea
        { "accRobotAbility4", robotAbility4Acc }, //Veces que la habilidad 4 del robot hace da;o / veces que se castea
        { "accRobotAbility5", robotAbility5Acc }, //Veces que la habilidad 5 del robot hace da;o / veces que se castea
        { "countDruidAbility1", countDruidAbility1 }, //Veces que la druida usa la habilidad 1
        { "countDruidAbility2", countDruidAbility2 }, //Veces que la druida usa la habilidad 2
        { "countDruidAbility3", countDruidAbility3 }, //Veces que la druida usa la habilidad 3
        { "countDruidAbility4", countDruidAbility4 }, //Veces que la druida usa la habilidad 4
        { "countDruidAbility5", countDruidAbility5 }, //Veces que la druida usa la habilidad 5
        { "countRobotAbility1", countRobotAbility1 }, //Veces que el robot usa la habilidad 1
        { "countRobotAbility2", countRobotAbility2 }, //Veces que el robot usa la habilidad 2
        { "countRobotAbility3", countRobotAbility3 }, //Veces que el robot usa la habilidad 3
        { "countRobotAbility4", countRobotAbility4 }, //Veces que el robot usa la habilidad 4
        { "countRobotAbility5", countRobotAbility5 }, //Veces que el robot usa la habilidad 5
        { "characterPickRate", characterPicks } //Un string que dice que personaje se eligio "Druid" o "Robot" por ejemplo
    };

        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log($"Evento 'Death_Win' registrado con los valores proporcionados.");
    }
    public void SendCustomEventMovement(
    string characterTileAndTime)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myEvent = new CustomEvent("MovementEvent")
    {
        { "characterTileTime", characterTileAndTime }, //numero de personaje, tile en el que se encuentra(si estas coordenadas tienen un solo digito poner 0 delante[(1,2) -> (01,02)]) y tiempo que pasa en ese tile
    };

        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log($"Evento 'MovementEvent' registrado con los valores proporcionados.");
    }
}