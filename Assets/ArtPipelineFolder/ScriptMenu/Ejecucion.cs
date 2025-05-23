using UnityEngine;
using UnityEngine.SceneManagement;

public class Ejecucion : MonoBehaviour
{
    public GameObject panelDeSeleccionPersonaje;
    public GameObject panelDelMenuInicial;

    public void Regresar()
    {
        Debug.Log("Botón 'Regresar' presionado. Volviendo al Menú Inicial.");

        if (panelDeSeleccionPersonaje != null) panelDeSeleccionPersonaje.SetActive(false);
        else Debug.LogError("¡ERROR! 'Panel De Seleccion Personaje' no asignado en Ejecucion.");

        if (panelDelMenuInicial != null) panelDelMenuInicial.SetActive(true);
        else Debug.LogError("¡ERROR! 'Panel Del Menu Inicial' no asignado en Ejecucion.");

        if (PlayerSelect.Instance != null)
        {
            PlayerSelect.Instance.ResetUIState(); // Resetea la UI de selección (bordes y variables)
        }
        else
        {
            Debug.LogError("¡ERROR! No se encontró una instancia de PlayerSelect. No se puede resetear la UI de selección.");
        }

        if (PlayerStorage.Instance != null)
        {
            PlayerStorage.Instance.ResetPlayerSelections(); // Resetea las selecciones guardadas
        }
        else
        {
            Debug.LogError("¡ERROR! No se encontró una instancia de PlayerStorage.");
        }
    }

    public void Confirmar()
    {
        Debug.Log("--- Botón 'Confirmar' presionado. Iniciando validación de ambos jugadores. ---");

        //if (PlayerStorage.Instance != null && PlayerSelect.Instance != null)
        //{
        //    PlayerStorage.Instance.ObtainSelectedCharactersFromUI(); // Asegura que PlayerStorage tiene las últimas selecciones

        //    // Validar que ambos jugadores hayan seleccionado un personaje
        //    if (PlayerSelect.Instance.currentSelectedUnitPlayer1 == null)
        //    {
        //        Debug.LogWarning("¡Error! El Jugador 1 debe seleccionar un personaje.");
        //        return; // Detiene la ejecución aquí
        //    }
        //    if (PlayerSelect.Instance.currentSelectedUnitPlayer2 == null)
        //    {
        //        Debug.LogWarning("¡Error! El Jugador 2 debe seleccionar un personaje.");
        //        return; // Detiene la ejecución aquí
        //    }

        //    Debug.Log($"[Ejecucion] Ambos jugadores han seleccionado personajes. J1: {PlayerSelect.Instance.currentSelectedUnitPlayer1.name}, J2: {PlayerSelect.Instance.currentSelectedUnitPlayer2.name}.");
        //}
        //else
        //{
        //    Debug.LogError("¡ERROR! No se pudo acceder a PlayerStorage o PlayerSelect. Asegúrate de que estén configurados correctamente en la escena.");
        //    return; // Detiene la ejecución aquí
        //}

        Debug.Log("¡Validación exitosa! Cargando escena 'Tutorial'.");
        SceneManager.LoadScene("Level easy"); // Asegúrate de que la escena "Tutorial" esté añadida en File -> Build Settings.
    }
}
