using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    
    public GameObject bordeRojoDruida;
    public GameObject bordeRojoRobot;
    public GameObject bordeRojoArtifice;

    
    public GameObject bordeAzulDruida;
    public GameObject bordeAzulRobot;
    public GameObject bordeAzulArtifice;

    
    public BaseUnit druidaPrefabUnit;
    public BaseUnit robotPrefabUnit;
    public BaseUnit artificePrefabUnit;

    
    // PlayerStorage leerá directamente estas variables.
    public BaseUnit currentSelectedUnitPlayer1 = null;
    public BaseUnit currentSelectedUnitPlayer2 = null;

    // Booleano para saber si el Jugador 2 ha hecho una selección
    public bool secondPlayerSelected = false;

    // --- Referencias a los botones de personaje (compartidos por J1 y J2) 
    // este hay que asignarlo ya que no se cuales seran los botones, esta fue la unica forma que vi sin el joystick ya que habria que agregar un recuadro mas o cambiar las condiciones del recuadro del segundo jugador en cada personaje
    public Button druidaButton;
    public Button robotButton;
    public Button artificeButton;

    // Mapeo interno para gestionar la interactividad de botones
    private Dictionary<BaseUnit, Button> _unitToButtonMap;

    // Arrays y Dictionaries para la gestión eficiente de bordes
    private GameObject[] _allRedBorders;
    private GameObject[] _allBlueBorders;
    private Dictionary<BaseUnit, GameObject> _unitToRedBorderMap;
    private Dictionary<BaseUnit, GameObject> _unitToBlueBorderMap;


    void Awake()
    {
        _allRedBorders = new GameObject[] { bordeRojoDruida, bordeRojoRobot, bordeRojoArtifice };
        _allBlueBorders = new GameObject[] { bordeAzulDruida, bordeAzulRobot, bordeAzulArtifice };

        _unitToRedBorderMap = new Dictionary<BaseUnit, GameObject>
        {
            { druidaPrefabUnit, bordeRojoDruida },
            { robotPrefabUnit, bordeRojoRobot },
            { artificePrefabUnit, bordeRojoArtifice }
        };
        _unitToBlueBorderMap = new Dictionary<BaseUnit, GameObject>
        {
            { druidaPrefabUnit, bordeAzulDruida },
            { robotPrefabUnit, bordeAzulRobot },
            { artificePrefabUnit, bordeAzulArtifice }
        };

        _unitToButtonMap = new Dictionary<BaseUnit, Button>
        {
            { druidaPrefabUnit, druidaButton },
            { robotPrefabUnit, robotButton },
            { artificePrefabUnit, artificeButton }
        };
    }

    void Start()
    {
        DeactivateAllBorders();
        currentSelectedUnitPlayer1 = null;
        currentSelectedUnitPlayer2 = null;
        secondPlayerSelected = false;

        // Deshabilita los botones para el Jugador 2 al inicio. J1 debe seleccionar primero.
        SetPlayerButtonsInteractable(false);
    }

    
    public void SelectDruidaPlayer1()
    {
        if (currentSelectedUnitPlayer1 == druidaPrefabUnit && currentSelectedUnitPlayer1 != null) { return; }

        if (druidaPrefabUnit == currentSelectedUnitPlayer2 && currentSelectedUnitPlayer2 != null)
        {
            Debug.LogWarning("Druida está bloqueado por el Jugador 2 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Druida seleccionado para Jugador 1.");
        DeactivateRedBorders();
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(true);
        currentSelectedUnitPlayer1 = druidaPrefabUnit;

        
        SetPlayerButtonsInteractable(true);
    }

    public void SelectRobotPlayer1()
    {
        if (currentSelectedUnitPlayer1 == robotPrefabUnit && currentSelectedUnitPlayer1 != null) { return; }
        if (robotPrefabUnit == currentSelectedUnitPlayer2 && currentSelectedUnitPlayer2 != null)
        {
            Debug.LogWarning("Robot está bloqueado por el Jugador 2 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Robot seleccionado para Jugador 1.");
        DeactivateRedBorders();
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(true);
        currentSelectedUnitPlayer1 = robotPrefabUnit;
        SetPlayerButtonsInteractable(true);
    }

    public void SelectArtificePlayer1()
    {
        if (currentSelectedUnitPlayer1 == artificePrefabUnit && currentSelectedUnitPlayer1 != null) { return; }
        if (artificePrefabUnit == currentSelectedUnitPlayer2 && currentSelectedUnitPlayer2 != null)
        {
            Debug.LogWarning("Artífice está bloqueado por el Jugador 2 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Artífice seleccionado para Jugador 1.");
        DeactivateRedBorders();
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(true);
        currentSelectedUnitPlayer1 = artificePrefabUnit;
        SetPlayerButtonsInteractable(true);
    }

    // --- MÉTODOS PARA LOS BOTONES DE SELECCIÓN DE PERSONAJES (JUGADOR 2) ---
    public void SelectDruidaPlayer2()
    {
        if (currentSelectedUnitPlayer2 == druidaPrefabUnit && currentSelectedUnitPlayer2 != null) { return; }

        if (druidaPrefabUnit == currentSelectedUnitPlayer1 && currentSelectedUnitPlayer1 != null)
        {
            Debug.LogWarning("Druida está bloqueado por el Jugador 1 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Druida seleccionado para Jugador 2.");
        DeactivateBlueBorders();
        if (bordeAzulDruida != null) bordeAzulDruida.SetActive(true);
        currentSelectedUnitPlayer2 = druidaPrefabUnit;
        UpdateSecondPlayerSelectedStatus();
        SetPlayerButtonsInteractable(true); // Re-evalúa los bloqueos después de la selección de J2
    }

    public void SelectRobotPlayer2()
    {
        if (currentSelectedUnitPlayer2 == robotPrefabUnit && currentSelectedUnitPlayer2 != null) { return; }
        if (robotPrefabUnit == currentSelectedUnitPlayer1 && currentSelectedUnitPlayer1 != null)
        {
            Debug.LogWarning("Robot está bloqueado por el Jugador 1 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Robot seleccionado para Jugador 2.");
        DeactivateBlueBorders();
        if (bordeAzulRobot != null) bordeAzulRobot.SetActive(true);
        currentSelectedUnitPlayer2 = robotPrefabUnit;
        UpdateSecondPlayerSelectedStatus();
        SetPlayerButtonsInteractable(true);
    }

    public void SelectArtificePlayer2()
    {
        if (currentSelectedUnitPlayer2 == artificePrefabUnit && currentSelectedUnitPlayer2 != null) { return; }
        if (artificePrefabUnit == currentSelectedUnitPlayer1 && currentSelectedUnitPlayer1 != null)
        {
            Debug.LogWarning("Artífice está bloqueado por el Jugador 1 (ya lo ha seleccionado).");
            return;
        }

        Debug.Log("Artífice seleccionado para Jugador 2.");
        DeactivateBlueBorders();
        if (bordeAzulArtifice != null) bordeAzulArtifice.SetActive(true);
        currentSelectedUnitPlayer2 = artificePrefabUnit;
        UpdateSecondPlayerSelectedStatus();
        SetPlayerButtonsInteractable(true);
    }

    

    private void DeactivateAllBorders()
    {
        DeactivateRedBorders();
        DeactivateBlueBorders();
    }

    private void DeactivateRedBorders()
    {
        foreach (GameObject border in _allRedBorders)
        {
            if (border != null) border.SetActive(false);
        }
    }

    private void DeactivateBlueBorders()
    {
        foreach (GameObject border in _allBlueBorders)
        {
            if (border != null) border.SetActive(false);
        }
    }

    private void UpdateSecondPlayerSelectedStatus()
    {
        secondPlayerSelected = (currentSelectedUnitPlayer2 != null);
        Debug.Log($"Estado de secondPlayerSelected: {secondPlayerSelected}");
    }

    // Controla la interactividad de los botones para AMBOS jugadores
    // 'canPlayer2Interact' es true una vez que J1 ha hecho una selección.
    private void SetPlayerButtonsInteractable(bool canPlayer2Interact)
    {
        //Aqui hay que poner los botones (no se cuales son)
        // Primero, establece la interactividad de todos los botones según si están seleccionados o no.
        if (druidaButton != null) druidaButton.interactable = !((currentSelectedUnitPlayer1 == druidaPrefabUnit) || (currentSelectedUnitPlayer2 == druidaPrefabUnit));
        if (robotButton != null) robotButton.interactable = !((currentSelectedUnitPlayer1 == robotPrefabUnit) || (currentSelectedUnitPlayer2 == robotPrefabUnit));
        if (artificeButton != null) artificeButton.interactable = !((currentSelectedUnitPlayer1 == artificePrefabUnit) || (currentSelectedUnitPlayer2 == artificePrefabUnit));

        // Si el Jugador 2 aún no puede interactuar (es decir, J1 no ha seleccionado nada todavía)
        if (!canPlayer2Interact)
        {
            // Deshabilita todos los botones para J2.
            // Para J1, los botones deben estar siempre activos al inicio (o gestionados por un sistema de turnos más complejo).
            // Pero como esta función se llama en Start(false), simplemente desactiva todos.
            // Los métodos SelectXPlayer1() los volverán a activar y deshabilitarán correctamente después.
            if (druidaButton != null) druidaButton.interactable = false;
            if (robotButton != null) robotButton.interactable = false;
            if (artificeButton != null) artificeButton.interactable = false;
        }

        // Caso especial: después de que J1 selecciona (canPlayer2Interact = true),
        // queremos que el botón que J1 eligió quede deshabilitado para J2.
        // Y los botones restantes habilitados para J2.
        // Esto ya lo maneja la primera parte de la función:
        // si un personaje es currentSelectedUnitPlayer1, su interactable será false.
        // Los demás, si no son currentSelectedUnitPlayer2, serán true.
    }
}
