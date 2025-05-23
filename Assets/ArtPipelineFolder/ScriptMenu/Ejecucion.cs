using UnityEngine;
using UnityEngine.SceneManagement;

public class Ejecucion : MonoBehaviour
{
    public GameObject panelDeSeleccionPersonaje;
    public GameObject panelDelMenuInicial;

    public void Regresar()
    {
        

        if (panelDeSeleccionPersonaje != null) panelDeSeleccionPersonaje.SetActive(false);
        

        if (panelDelMenuInicial != null) panelDelMenuInicial.SetActive(true);
        

        if (PlayerSelect.Instance != null)
        {
            PlayerSelect.Instance.ResetUIState(); // Resetea la UI de selección (bordes y variables)
        }
        

        if (PlayerStorage.Instance != null)
        {
            PlayerStorage.Instance.ResetPlayerSelections(); // Resetea las selecciones guardadas
        }
        
    }

    public void Confirmar()
    {
        

        if (PlayerStorage.Instance != null && PlayerSelect.Instance != null)
        {
            PlayerStorage.Instance.ObtainSelectedCharactersFromUI(); // Asegura que PlayerStorage tiene las últimas selecciones
            /*
            // Validar que ambos jugadores hayan seleccionado un personaje
            if (PlayerSelect.Instance.currentSelectedUnitPlayer1 == null)
            {
                
                return; // Detiene la ejecución aquí
            }
            if (PlayerSelect.Instance.currentSelectedUnitPlayer2 == null)
            {
                
                return; // Detiene la ejecución aquí
            }

            Debug.Log($"[Ejecucion] Ambos jugadores han seleccionado personajes. J1: {PlayerSelect.Instance.currentSelectedUnitPlayer1.name}, J2: {PlayerSelect.Instance.currentSelectedUnitPlayer2.name}.");
            */
        }
        else
        {
            
            return; // Detiene la ejecución aquí
        }

        
        SceneManager.LoadScene("Tutorial"); 
    }
}
