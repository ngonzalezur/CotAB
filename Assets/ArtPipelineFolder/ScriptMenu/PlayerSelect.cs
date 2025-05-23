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
    public BaseUnit currentSelectedUnitPlayer2 = null; // Variable para la selección de J2
    public bool isPlayer2Ready = false;

    // --- Referencias a los botones para Jugar 2 (para la navegación del control) ---
    // Asignar en el Inspector los GameObjects de los botones de cada personaje
    [Header("Botones de Personaje (para J2 - Navegación UI)")]
    public Button druidaButton;
    public Button robotButton;
    public Button artificeButton;

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

    void Update()
    {
        // --- Lógica de Navegación y Borde para Jugador 2 (con control) ---
        // Este es un ejemplo básico para detectar qué botón está "resaltado" por el EventSystem.
        // Solo si J2 no ha confirmado su selección
        if (!isPlayer2Ready)
        {
            // Obtener el GameObject actualmente seleccionado por el EventSystem
            GameObject currentlySelectedUIObject = EventSystem.current.currentSelectedGameObject;

            if (currentlySelectedUIObject != null)
            {
                Button selectedButton = currentlySelectedUIObject.GetComponent<Button>();

                if (selectedButton != null)
                {
                    // Determinar qué personaje corresponde al botón seleccionado para J2
                    // y actualizar el borde azul
                    if (selectedButton == druidaButton)
                    {
                        HandleDruidaBorderForPlayer2();
                    }
                    else if (selectedButton == robotButton)
                    {
                        HandleRobotBorderForPlayer2();
                    }
                    else if (selectedButton == artificeButton)
                    {
                        HandleArtificeBorderForPlayer2();
                    }
                    // Si el control se mueve a un botón que no es de personaje, los bordes azules se desactivarán
                    // porque cada Handle...BorderForPlayer2() desactiva los otros.
                }
            }
            // Si no hay ningún objeto de UI seleccionado (ej. el control no está sobre ningún botón)
            // se desactivarán todos los bordes azules.
            else
            {
                // Esto podría ser opcional si prefieres que el último borde activo se quede.
                // Sin embargo, si el control se mueve fuera de los botones de personaje,
                // la lógica de cada Handle...BorderForPlayer2() ya desactivará los otros.
                // Si J2 no ha seleccionado nada y el control no está sobre ningún botón,
                // podemos asegurarnos de que no hay bordes azules visibles.
                if (bordeAzulDruida != null && bordeAzulDruida.activeSelf ||
                    bordeAzulRobot != null && bordeAzulRobot.activeSelf ||
                    bordeAzulArtifice != null && bordeAzulArtifice.activeSelf)
                {
                    DeactivateAllBlueBorders(); // Desactivamos si no hay nada seleccionado
                }
            }


            // --- Lógica para CONFIRMAR la selección de J2 con su input específico ---
            // IMPORTANTE: Esto es un PLACEHOLDER. Debes adaptar 'Player2_ConfirmButton'
            // a tu configuración de input (Input Manager o New Input System).
            // Por ejemplo: Input.GetButtonDown("Submit_P2") si tienes un eje "Submit_P2"
            // O una Input Action para J2 con el New Input System.

            // Ejemplo conceptual (¡ADAPTAR A TU SISTEMA DE INPUT REAL!):
            // if (Input.GetKeyDown(KeyCode.Return) && currentlySelectedUIObject == druidaButton) { SaveDruidaForPlayer2(); Debug.Log("J2 Confirmed Druid"); }
            // else if (Input.GetKeyDown(KeyCode.Return) && currentlySelectedUIObject == robotButton) { SaveRobotForPlayer2(); Debug.Log("J2 Confirmed Robot"); }
            // else if (Input.GetKeyDown(KeyCode.Return) && currentlySelectedUIObject == artificeButton) { SaveArtificeForPlayer2(); Debug.Log("J2 Confirmed Artifice"); }

            // Un enfoque más robusto para la confirmación de J2:
            // if (Input.GetButtonDown("Player2_Confirm") && currentlySelectedUIObject != null)
            // {
            //     Button confirmedButton = currentlySelectedUIObject.GetComponent<Button>();
            //     if (confirmedButton == druidaButton)
            //     {
            //         SaveDruidaForPlayer2();
            //     }
            //     else if (confirmedButton == robotButton)
            //     {
            //         SaveRobotForPlayer2();
            //     }
            //     else if (confirmedButton == artificeButton)
            //     {
            //         SaveArtificeForPlayer2();
            //     }
            //     // Podrías poner lógica aquí para deshabilitar el botón de confirmación
            //     // o cambiar el borde a un estado "confirmado" si J2 ya no debe poder cambiar.
            // }
        }
    }


    // ===============================================
    //  MÉTODOS PARA GUARDAR EL PERSONAJE SELECCIONADO (J1 - Clic de Ratón)
    // ===============================================
    // Estos son los mismos que antes, para la selección por clic de ratón de J1
    // Asignar al PRIMER evento OnClick de cada botón de personaje (J1)

    public void SaveDruidaForPlayer1() { SaveCharacter(druidaPrefabUnit, 1); }
    public void SaveRobotForPlayer1() { SaveCharacter(robotPrefabUnit, 1); }
    public void SaveArtificeForPlayer1() { SaveCharacter(artificePrefabUnit, 1); }

    // ===============================================
    //  MÉTODOS PARA GUARDAR EL PERSONAJE SELECCIONADO (J2 - Confirmación por Control)
    // ===============================================
    // Estos métodos deberían ser llamados por la lógica de input del Jugador 2
    // cuando CONFIRME su selección (ej. presionando un botón específico en el gamepad).

    public void SaveDruidaForPlayer2() { SaveCharacter(druidaPrefabUnit, 2); }
    public void SaveRobotForPlayer2() { SaveCharacter(robotPrefabUnit, 2); }
    public void SaveArtificeForPlayer2() { SaveCharacter(artificePrefabUnit, 2); }


    // --- Método genérico para guardar selección ---
    private void SaveCharacter(BaseUnit selectedUnit, int playerNumber)
    {
        if (playerNumber == 1)
        {
            if (currentSelectedUnitPlayer1 != selectedUnit)
            {
                currentSelectedUnitPlayer1 = selectedUnit;
                isPlayer1Ready = true;
                Debug.Log($"[PlayerSelect] Jugador 1 HA GUARDADO a {selectedUnit.name}.");
            }
            else
            {
                Debug.Log($"[PlayerSelect] Jugador 1 ya tenía a {selectedUnit.name} guardado.");
            }
        }
        else if (playerNumber == 2)
        {
            if (currentSelectedUnitPlayer2 != selectedUnit)
            {
                currentSelectedUnitPlayer2 = selectedUnit;
                isPlayer2Ready = true;
                Debug.Log($"[PlayerSelect] Jugador 2 HA GUARDADO a {selectedUnit.name}.");
                // Una vez que J2 confirma, podrías querer desactivar su borde de navegación
                // o cambiarlo a un borde de "confirmado" si ya no puede navegar.
                // DeactivateAllBlueBorders(); // Ejemplo: Desactivar todos los bordes azules una vez confirmado
                // O: HandleConfirmedBlueBorder(selectedUnit);
            }
            else
            {
                Debug.Log($"[PlayerSelect] Jugador 2 ya tenía a {selectedUnit.name} guardado.");
            }
        }
    }


    // ===============================================
    //  MÉTODOS PARA GESTIONAR LA VISUALIZACIÓN DE LOS BORDES (J1 - Clic de Ratón)
    // ===============================================
    // Asignar al SEGUNDO evento OnClick de cada botón de personaje (J1)
    // Cada método activa su borde y desactiva los otros del mismo color.

    public void HandleDruidaBorderForPlayer1()
    {
        // 1. Desactivar los otros bordes rojos
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);

        // 2. Activar el borde rojo de este personaje
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
        // 1. Desactivar los otros bordes rojos
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoArtifice != null) bordeRojoArtifice.SetActive(false);

        // 2. Activar el borde rojo de este personaje
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
        // 1. Desactivar los otros bordes rojos
        if (bordeRojoDruida != null) bordeRojoDruida.SetActive(false);
        if (bordeRojoRobot != null) bordeRojoRobot.SetActive(false);

        // 2. Activar el borde rojo de este personaje
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

    // ===============================================
    //  MÉTODOS PARA GESTIONAR LA VISUALIZACIÓN DE LOS BORDES (J2 - Navegación por Control)
    // ===============================================
    // Estos métodos son llamados por la lógica de navegación de J2 en Update(),
    // para mostrar el borde azul del personaje actualmente "resaltado" por el control.

    public void HandleDruidaBorderForPlayer2()
    {
        // 1. Desactivar los otros bordes azules
        if (bordeAzulRobot != null) bordeAzulRobot.SetActive(false);
        if (bordeAzulArtifice != null) bordeAzulArtifice.SetActive(false);

        // 2. Activar el borde azul de este personaje
        if (bordeAzulDruida != null)
        {
            bordeAzulDruida.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde AZUL de Druida ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeAzulDruida no asignado en el Inspector.");
        }
    }

    public void HandleRobotBorderForPlayer2()
    {
        // 1. Desactivar los otros bordes azules
        if (bordeAzulDruida != null) bordeAzulDruida.SetActive(false);
        if (bordeAzulArtifice != null) bordeAzulArtifice.SetActive(false);

        // 2. Activar el borde azul de este personaje
        if (bordeAzulRobot != null)
        {
            bordeAzulRobot.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde AZUL de Robot ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeAzulRobot no asignado en el Inspector.");
        }
    }

    public void HandleArtificeBorderForPlayer2()
    {
        // 1. Desactivar los otros bordes azules
        if (bordeAzulDruida != null) bordeAzulDruida.SetActive(false);
        if (bordeAzulRobot != null) bordeAzulRobot.SetActive(false);

        // 2. Activar el borde azul de este personaje
        if (bordeAzulArtifice != null)
        {
            bordeAzulArtifice.SetActive(true);
            Debug.Log($"[PlayerSelect] Borde AZUL de Artifice ACTIVADO. Otros desactivados.");
        }
        else
        {
            Debug.LogError("[PlayerSelect] bordeAzulArtifice no asignado en el Inspector.");
        }
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

