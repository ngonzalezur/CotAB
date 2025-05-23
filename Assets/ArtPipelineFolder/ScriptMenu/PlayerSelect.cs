using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public BaseUnit druidaPrefabUnit;
    public BaseUnit robotPrefabUnit;
    public BaseUnit artificePrefabUnit;

    // --- Bordes de Selección (Asignar en el Inspector) ---
    [Header("Bordes de Selección Jugador 1 (Rojo)")]
    public GameObject bordeRojoDruida;
    public GameObject bordeRojoRobot;
    public GameObject bordeRojoArtifice;

    // --- VARIABLES PÚBLICAS CON LA SELECCIÓN FINAL DEL JUGADOR 1 ---
    public BaseUnit currentSelectedUnitPlayer1 = null;
    public bool isPlayer1Ready = false; // Indica si J1 ha hecho una selección válida

    // Singleton para fácil acceso desde otros scripts (Ejecucion, PlayerStorage)
    public static PlayerSelect Instance { get; private set; }


    void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetUIState(); // Asegura un estado limpio al iniciar la escena.
    }

    // Método para resetear la UI de selección (llamado al inicio o al volver al menú)
    public void ResetUIState()
    {
        DeactivateAllBorders(); // Desactiva todos los bordes (útil al iniciar o al regresar al menú)
        currentSelectedUnitPlayer1 = null;
        isPlayer1Ready = false;
        Debug.Log("[PlayerSelect] UI de selección reseteada para Jugador 1.");
    }

    // ===============================================
    //  MÉTODOS PARA GUARDAR EL PERSONAJE SELECCIONADO (J1)
    // ===============================================
    // Estos métodos SÓLO actualizan la variable interna 'currentSelectedUnitPlayer1'.
    // Son llamados por el PRIMER evento OnClick de cada botón.

    public void SaveDruidaForPlayer1()
    {
        if (currentSelectedUnitPlayer1 != druidaPrefabUnit) // Solo si es una selección nueva
        {
            currentSelectedUnitPlayer1 = druidaPrefabUnit;
            isPlayer1Ready = true;
            Debug.Log($"[PlayerSelect] Jugador 1 HA GUARDADO a {druidaPrefabUnit.name}.");
        }
        else
        {
            Debug.Log($"[PlayerSelect] Jugador 1 ya tenía a {druidaPrefabUnit.name} guardado.");
        }
    }

    public void SaveRobotForPlayer1()
    {
        if (currentSelectedUnitPlayer1 != robotPrefabUnit)
        {
            currentSelectedUnitPlayer1 = robotPrefabUnit;
            isPlayer1Ready = true;
            Debug.Log($"[PlayerSelect] Jugador 1 HA GUARDADO a {robotPrefabUnit.name}.");
        }
        else
        {
            Debug.Log($"[PlayerSelect] Jugador 1 ya tenía a {robotPrefabUnit.name} guardado.");
        }
    }

    public void SaveArtificeForPlayer1()
    {
        if (currentSelectedUnitPlayer1 != artificePrefabUnit)
        {
            currentSelectedUnitPlayer1 = artificePrefabUnit;
            isPlayer1Ready = true;
            Debug.Log($"[PlayerSelect] Jugador 1 HA GUARDADO a {artificePrefabUnit.name}.");
        }
        else
        {
            Debug.Log($"[PlayerSelect] Jugador 1 ya tenía a {artificePrefabUnit.name} guardado.");
        }
    }

    // ===============================================
    //  MÉTODOS PARA GESTIONAR LA VISUALIZACIÓN DE LOS BORDES (J1)
    // ===============================================
    // Estos métodos serán llamados por el SEGUNDO evento OnClick de cada botón.
    // Cada método maneja la activación de SU propio borde y la desactivación de los OTROS dos.

    public void HandleDruidaBorderForPlayer1()
    {
        // 1. Desactivar los otros bordes
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);

        // 2. Activar el borde de este personaje
        if (bordeRojoDruida != null)
        {
            bordeRojoDruida.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde ROJO de Druida ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeRojoDruida no asignado en el Inspector.");
        }
    }

    public void HandleRobotBorderForPlayer1()
    {
        // 1. Desactivar los otros bordes
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);

        // 2. Activar el borde de este personaje
        if (bordeRojoRobot != null)
        {
            bordeRojoRobot.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde ROJO de Robot ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeRojoRobot no asignado en el Inspector.");
        }
    }

    public void HandleArtificeBorderForPlayer1()
    {
        // 1. Desactivar los otros bordes
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);

        // 2. Activar el borde de este personaje
        if (bordeRojoArtifice != null)
        {
            bordeRojoArtifice.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde ROJO de Artifice ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeRojoArtifice no asignado en el Inspector.");
        }
    }

    // --- Método Auxiliar para desactivar TODOS los bordes (solo para ResetUIState) ---
    private void DeactivateAllBorders()
    {
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);
        // No es necesario un Debug.Log aquí cada vez que se llama, ya lo hacen los Handle...
    }

}

