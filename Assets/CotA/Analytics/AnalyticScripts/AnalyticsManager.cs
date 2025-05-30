using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;
    private async void Awake()
    {
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
        }
        else
        {
            Debug.LogError("Unity Services no se inicializaron correctamente");
        }
    }
    public void SendCustomEvent(
    float basicAtackDruidAcc, float basicAtackRobotAcc,
    float druidAbility1Acc, float druidAbility2Acc, float druidAbility3Acc, float druidAbility4Acc,
    float interactionOverTime, float robotAbility1Acc, float robotAbility2Acc,
    float robotAbility3Acc, float robotAbility4Acc,
    int countBasicAtackDruid, int countBasicAtackRobot, int countBurnPoisonCombo,
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
        { "BasicAtackDruidAcc", basicAtackDruidAcc }, //Veces que el ataque basico de la druida hace da;o / veces que se castea
        { "BasicAtackRobotAcc", basicAtackRobotAcc }, //Veces que el ataque basico del robot hace da;o / veces que se castea
        { "DruidAbility1Acc", druidAbility1Acc }, //Veces que la habilidad 1 de la druida hace da;o / veces que se castea
        { "DruidAbility2Acc", druidAbility2Acc }, //Veces que la habilidad 2 de la druida hace da;o / veces que se castea
        { "DruidAbility3Acc", druidAbility3Acc }, //Veces que la habilidad 3 de la druida hace da;o / veces que se castea
        { "DruidAbility4Acc", druidAbility4Acc }, //Veces que la habilidad 4 de la druida hace da;o / veces que se castea
        { "interactionOverTime", interactionOverTime }, //Veces que el jugador se mueve o usa una habilidad / tiempo de partida
        { "RobotAbility1Acc", robotAbility1Acc }, //Veces que la habilidad 1 del robot hace da;o / veces que se castea
        { "RobotAbility2Acc", robotAbility2Acc }, //Veces que la habilidad 2 del robot hace da;o / veces que se castea
        { "RobotAbility3Acc", robotAbility3Acc }, //Veces que la habilidad 3 del robot hace da;o / veces que se castea
        { "RobotAbility4Acc", robotAbility4Acc }, //Veces que la habilidad 4 del robot hace da;o / veces que se castea
        { "CountBasicAtackDruid", countBasicAtackDruid }, //Veces que la druida usa el ataque basico
        { "CountBasicAtackRobot", countBasicAtackRobot }, //Veces que el robot usa el ataque basico
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
    public void SendCustomEvent2(
   string movementParameter2)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myEvent2 = new CustomEvent("Playtest4Event")
    {
        { "movementParameter", movementParameter2 }, //Enviar numero de perosnaje, casillas y tiempo en ella
    };

        AnalyticsService.Instance.RecordEvent(myEvent2);
        Debug.Log($"Evento 'Playtest4Event' registrado con los valores proporcionados.");
    }
}