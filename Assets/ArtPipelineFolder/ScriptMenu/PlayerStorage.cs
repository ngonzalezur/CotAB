using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStorage : MonoBehaviour
{
    public BaseUnit player1Prefab;
    public BaseUnit player2Prefab;

    public static PlayerStorage Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Asegura que este GameObject persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        player1Prefab = null;
        player2Prefab = null;
    }

    // Este método será llamado desde otro script (ej. Ejecucion.cs)
    // justo antes de cargar la escena de juego, para recoger las selecciones.
    public void ObtainSelectedCharactersFromUI()
    {
        if (PlayerSelect.Instance != null)
        {
            player1Prefab = PlayerSelect.Instance.currentSelectedUnitPlayer1;
            player2Prefab = PlayerSelect.Instance.currentSelectedUnitPlayer2; // Obtenemos la selección de J2

            Debug.Log($"[PlayerStorage] Selecciones obtenidas de UI. " +
                      $"J1: {(player1Prefab != null ? player1Prefab.name : "Ninguno")}. " +
                      $"J2: {(player2Prefab != null ? player2Prefab.name : "Ninguno")}.");
        }
        else
        {
            Debug.LogError("[PlayerStorage] No se encontró una instancia de PlayerSelect. Asegúrate de que esté en la escena de selección.");
        }
    }

    // Este método es llamado en la escena de juego para instanciar los personajes.
    public void InstanciarJugadores()
    {
        Debug.Log("[PlayerStorage] Iniciando instanciación de jugadores en la escena de juego...");

        Vector3 player1SpawnPos = new Vector3(-3, 0, 0);
        Vector3 player2SpawnPos = new Vector3(3, 0, 0);

        // Instanciar Jugador 1
        if (player1Prefab != null)
        {
            var newUnit = Instantiate(player1Prefab, player1SpawnPos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject); // Persiste el personaje instanciado

            // Instanciar ataques si existen
            if (newUnit.Attacks != null)
            {
                for (int i = 0; i < newUnit.Attacks.Length; i++)
                {
                    if (newUnit.Attacks[i] != null)
                    {
                        var newAttack = Instantiate(newUnit.Attacks[i], new Vector3(0, 0, -1), Quaternion.identity);
                        newAttack.gameObject.SetActive(false);
                        DontDestroyOnLoad(newAttack.gameObject);
                        newUnit.Attacks[i] = newAttack;
                    }
                }
            }
            Debug.Log($"Instanciado Jugador 1: {newUnit.name}");
        }
        else
        {
            Debug.LogWarning("No se seleccionó un personaje para el Jugador 1. No se instanciará.");
        }

        // Instanciar Jugador 2
        if (player2Prefab != null)
        {
            var newUnit = Instantiate(player2Prefab, player2SpawnPos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject); // Persiste el personaje instanciado

            // Instanciar ataques si existen
            if (newUnit.Attacks != null)
            {
                for (int i = 0; i < newUnit.Attacks.Length; i++)
                {
                    if (newUnit.Attacks[i] != null)
                    {
                        var newAttack = Instantiate(newUnit.Attacks[i], new Vector3(0, 0, -1), Quaternion.identity);
                        newAttack.gameObject.SetActive(false);
                        DontDestroyOnLoad(newAttack.gameObject);
                        newUnit.Attacks[i] = newAttack;
                    }
                }
            }
            Debug.Log($"Instanciado Jugador 2: {newUnit.name}");
        }
        else
        {
            Debug.LogWarning("No se seleccionó un personaje para el Jugador 2. No se instanciará.");
        }
    }

    // Método para resetear las selecciones guardadas (útil al regresar al menú principal o similar)
    public void ResetPlayerSelections()
    {
        player1Prefab = null;
        player2Prefab = null;
        Debug.Log("[PlayerStorage] Selecciones de jugadores en PlayerStorage reseteadas.");
    }
}
