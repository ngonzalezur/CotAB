using UnityEngine;
using UnityEngine.SceneManagement;

public class Ejecucion : MonoBehaviour
{
    public GameObject panelDeSeleccionPersonaje; 
    public GameObject panelDelMenuInicial;       

    // IMPORTANTE: referencia al script PlayerSelect para poder resetearlo.
    public PlayerSelect playerSelectScript; //  aquí va el GameObject que tiene el script PlayerSelect

    
    public void Regresar()
    {
        Debug.Log("Botón 'Regresar' presionado. Volviendo al Menú Inicial.");

        
        if (panelDeSeleccionPersonaje != null)
        {
            panelDeSeleccionPersonaje.SetActive(false); // 'false' lo desactiva
        }
        

        
        if (panelDelMenuInicial != null)
        {
            panelDelMenuInicial.SetActive(true); // 'true' lo activa
        }
        
        //este es para el reset de los personajes
        /*
        // 3. ¡RESET DE PLAYERSELECT!
        // Ahora sí, cuando regresamos al menú principal, reseteamos el estado de selección de personajes.
        if (playerSelectScript != null)
        {
            playerSelectScript.ResetUIStatePublic();
        }
        else
        {
            Debug.LogError("¡ERROR! 'Player Select Script' no asignado en el Inspector de Ejecutar. El reseteo no se realizará.");
        }
        */
    }

    // Este método es para el botón "Confirmar" (o "Comenzar Juego", etc.)
    // Por ahora estará vacío, esperando la lógica de inicio del juego.
    public void Confirmar()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
