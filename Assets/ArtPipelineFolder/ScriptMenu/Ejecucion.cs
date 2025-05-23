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

        if (PlayerSelect.Instance != null) // Usa PlayerSelect.Instance
        {
            PlayerSelect.Instance.ResetUIState();
        }
        else
        {
            Debug.LogError("¡ERROR! No se encontró una instancia de PlayerSelect. No se puede resetear la UI de selección.");
        }

        if (PlayerStorage.Instance != null)
        {
            PlayerStorage.Instance.ResetPlayerSelections();
        }
        else
        {
            Debug.LogError("¡ERROR! No se encontró una instancia de PlayerStorage.");
        }
    }

    public void Confirmar()
    {
        Debug.Log("Botón 'Confirmar' presionado. Intentando iniciar el juego...");

        if (PlayerStorage.Instance != null && PlayerSelect.Instance != null) // Usa PlayerSelect.Instance
        {
            PlayerStorage.Instance.ObtainSelectedCharactersFromUI();

            if (PlayerSelect.Instance.currentSelectedUnitPlayer1 == null)
            {
                Debug.LogWarning("¡Error! El Jugador 1 debe seleccionar un personaje para poder iniciar el juego.");
                return;
            }
        }
        else
        {
            Debug.LogError("¡ERROR! No se pudo acceder a PlayerStorage o PlayerSelect. Asegúrate de que estén configurados correctamente en la escena.");
            return;
        }

        SceneManager.LoadScene("Tutorial");
    }
}
