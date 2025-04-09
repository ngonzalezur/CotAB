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
            SendCustomEvent();
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
    public void SendCustomEvent()
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("TestEvent2")
        {
            {"testParameter3", 11}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("Se llamo eta vaina");
    }
}
