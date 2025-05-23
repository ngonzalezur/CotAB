using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public BaseUnit druidaPrefabUnit;
    public BaseUnit robotPrefabUnit;
    public BaseUnit artificePrefabUnit;

    // --- Bordes de Selección Jugador 1 (Rojo) ---
    [Header("Bordes de Selección Jugador 1 (Rojo)")]
    public GameObject bordeRojoDruida;
    public GameObject bordeRojoRobot;
    public GameObject bordeRojoArtifice;

    // --- Bordes de Selección Jugador 2 (Azul) ---
    [Header("Bordes de Selección Jugador 2 (Azul)")]
    public GameObject bordeAzulDruida;
    public GameObject bordeAzulRobot;
    public GameObject bordeAzulArtifice;

    // --- VARIABLES PÚBLICAS CON LA SELECCIÓN FINAL DE LOS JUGADORES ---
    [Header("Selecciones Finales")]
    public BaseUnit currentSelectedUnitPlayer1 = null;
    public bool isPlayer1Ready = false;
    public BaseUnit currentSelectedUnitPlayer2 = null;
    public bool isPlayer2Ready = false;

    // Singleton para fácil acceso
    public static PlayerSelect Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        ResetUIState();
    }

    // Método para resetear la UI de selección (llamado al inicio o al volver al menú)
    public void ResetUIState()
    {
        DeactivateAllBorders(); // Desactiva todos los bordes (rojos y azules)
        currentSelectedUnitPlayer1 = null;
        isPlayer1Ready = false;
        currentSelectedUnitPlayer2 = null;
        isPlayer2Ready = false;
        Debug.Log("[PlayerSelect] UI de selección reseteada para ambos jugadores.");
    }

    // El Update() ya no necesita la lógica de EventSystem para J2,
    // ya que ambos seleccionan con el mouse a través de OnClick.
    // Puedes eliminar el método Update() si no lo necesitas para nada más.
    /*
    void Update()
    {
        // ... (lógica anterior de J2 con EventSystem eliminada) ...
    }
    */


    // ===============================================
    //  MÉTODOS PARA SELECCIONAR Y VISUALIZAR (J1 Y J2 - Clic de Ratón)
    // ===============================================
    // Estos métodos serán llamados por el ÚNICO evento OnClick de CADA botón de personaje.
    // Manejarán la lógica de quién selecciona (J1 o J2) y qué borde activar.

    public void SelectDruida() { HandleSelectionAndBorders(druidaPrefabUnit); }
    public void SelectRobot() { HandleSelectionAndBorders(robotPrefabUnit); }
    public void SelectArtifice() { HandleSelectionAndBorders(artificePrefabUnit); }

    // --- Método genérico que maneja la selección y los bordes para el jugador actual ---
    private void HandleSelectionAndBorders(BaseUnit selectedUnit)
    {
        // Si J1 aún no ha seleccionado, este click es para J1
        if (!isPlayer1Ready)
        {
            // Guardar selección para J1
            currentSelectedUnitPlayer1 = selectedUnit;
            isPlayer1Ready = true;
            Debug.Log($"[PlayerSelect] Jugador 1 HA SELECCIONADO a {selectedUnit.name}.");

            // Activar borde rojo y desactivar otros rojos
            ActivateSpecificRedBorder(selectedUnit);
        }
        // Si J1 ya seleccionó y J2 aún no, este click es para J2
        else if (!isPlayer2Ready)
        {
            // Validar que J2 no seleccione el mismo personaje que J1
            if (selectedUnit == currentSelectedUnitPlayer1)
            {
                Debug.LogWarning($"[PlayerSelect] ¡Advertencia! Jugador 2 no puede seleccionar el mismo personaje que Jugador 1 ({selectedUnit.name}). Por favor, elige otro.");
                // Opcional: Podrías mostrar un mensaje en la UI para el usuario
                return; // No procesar la selección para J2
            }

            // Guardar selección para J2
            currentSelectedUnitPlayer2 = selectedUnit;
            isPlayer2Ready = true;
            Debug.Log($"[PlayerSelect] Jugador 2 HA SELECCIONADO a {selectedUnit.name}.");

            // Activar borde azul y desactivar otros azules
            ActivateSpecificBlueBorder(selectedUnit);
        }
        else
        {
            // Ambos jugadores ya seleccionaron. Ignorar clicks adicionales
            Debug.Log("[PlayerSelect] Ambos jugadores ya seleccionaron sus personajes. Clicks adicionales ignorados.");
        }
    }


    // ===============================================
    //  MÉTODOS PRIVADOS PARA GESTIONAR LA VISUALIZACIÓN DE LOS BORDES
    // ===============================================
    // Estos métodos son llamados internamente por HandleSelectionAndBorders()

    private void ActivateSpecificRedBorder(BaseUnit unitToActivate)
    {
        // Desactivar todos los bordes rojos primero
        DeactivateAllRedBorders();

        // Activar el borde rojo del personaje seleccionado
        if (unitToActivate == druidaPrefabUnit && bordeRojoDruida != null) bordeRojoDruida.SetActive(true);
        else if (unitToActivate == robotPrefabUnit && bordeRojoRobot != null) bordeRojoRobot.SetActive(true);
        else if (unitToActivate == artificePrefabUnit && bordeRojoArtifice != null) bordeRojoArtifice.SetActive(true);
        else Debug.LogError($"[PlayerSelect] Borde rojo no asignado o personaje no reconocido para {unitToActivate.name}.");
    }

    private void ActivateSpecificBlueBorder(BaseUnit unitToActivate)
    {
        // Desactivar todos los bordes azules primero
        DeactivateAllBlueBorders();

        // Activar el borde azul del personaje seleccionado
        if (unitToActivate == druidaPrefabUnit && bordeAzulDruida != null) bordeAzulDruida.SetActive(true);
        else if (unitToActivate == robotPrefabUnit && bordeAzulRobot != null) bordeAzulRobot.SetActive(true);
        else if (unitToActivate == artificePrefabUnit && bordeAzulArtifice != null) bordeAzulArtifice.SetActive(true);
        else Debug.LogError($"[PlayerSelect] Borde azul no asignado o personaje no reconocido para {unitToActivate.name}.");
    }

    // --- Métodos Auxiliares para desactivar bordes ---
    private void DeactivateAllBorders()
    {
        DeactivateAllRedBorders();
        DeactivateAllBlueBorders();
        Debug.Log("[PlayerSelect] Todos los bordes (rojos y azules) desactivados.");
    }

    private void DeactivateAllRedBorders()
    {
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);
    }

    private void DeactivateAllBlueBorders()
    {
        if (bordeAzulDruida != null) bordeAzulDruida.SetActive(false);
        if (bordeAzulRobot != null) bordeAzulRobot.SetActive(false);
        if (bordeAzulArtifice != null) bordeAzulArtifice.SetActive(false);
    }

}

