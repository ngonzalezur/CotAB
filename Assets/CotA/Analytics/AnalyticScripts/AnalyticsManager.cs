using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Awake()
    {
        Debug.Log("Awake ha sido llamado");
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Unity Services inicializados correctamente");
    }
    public void SendCustomEvent()
    {
        CustomEvent myEvent = new CustomEvent("TestEvent2")
        {
            { "testParameter2", 14 }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("Evento personalizado enviado correctamente");
    }

    void Start()
    {
        SendCustomEvent();
        Debug.Log("See llama la funcion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}