using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelDelMenuInicial;       
    public GameObject panelDeSeleccionPersonaje; 

    // Este es el método que asignarás al evento OnClick() del botón "Jugar".
    public void AccionBotonJugar()
    {
        // 1. Desactiva el panel del menú inicial.
        // Se verifica si la variable está asignada para evitar errores.
        if (panelDelMenuInicial != null)
        {
            panelDelMenuInicial.SetActive(false);
            Debug.Log("Panel del Menú Inicial desactivado.");
        }
        else
        {
            Debug.LogError("¡ERROR! 'Panel del Menu Inicial' no asignado en el Inspector de MenuPrincipalManager.");
        }

        // 2. Activa el panel de selección de personaje.
        if (panelDeSeleccionPersonaje != null)
        {
            panelDeSeleccionPersonaje.SetActive(true); 
            Debug.Log("Panel de Selección de Personaje activado.");
        }
        else
        {
            Debug.LogError("¡ERROR! 'Panel de Seleccion Personaje' no asignado en el Inspector de MenuPrincipalManager.");
        }
    }
    public void AccionBotonSalir()
    {        
        Application.Quit();

        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ActiveTutorial()
    {
        PlayerSelect.Instance.tutorial = true;
    }
}
