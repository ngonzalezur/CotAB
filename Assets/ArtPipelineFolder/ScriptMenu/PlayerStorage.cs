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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player1Prefab = null;
        player2Prefab = null;
    }

    public void ObtainSelectedCharactersFromUI()
    {
        if (PlayerSelect.Instance != null) // Usa PlayerSelect.Instance
        {
            player1Prefab = PlayerSelect.Instance.currentSelectedUnitPlayer1;
            player2Prefab = null; // J2 es nulo por ahora
            Debug.Log($"[PlayerStorage] Selecciones obtenidas de UI. J1: {(player1Prefab != null ? player1Prefab.name : "Ninguno")}.");
        }
        else
        {
            Debug.LogError("[PlayerStorage] No se encontró una instancia de PlayerSelect. Asegúrate de que esté en la escena de selección.");
        }
    }

    public void InstanciarJugadores()
    {
        Debug.Log("[PlayerStorage] Iniciando instanciación de jugadores en la escena de juego...");

        Vector3 player1SpawnPos = new Vector3(-3, 0, 0);
        Vector3 player2SpawnPos = new Vector3(3, 0, 0);

        if (player1Prefab != null)
        {
            var newUnit = Instantiate(player1Prefab, player1SpawnPos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject);

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

        if (player2Prefab != null)
        {
            var newUnit = Instantiate(player2Prefab, player2SpawnPos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject);

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
            Debug.Log("No se seleccionó un personaje para el Jugador 2. No se instanciará.");
        }
    }

    public void ResetPlayerSelections()
    {
        player1Prefab = null;
        player2Prefab = null;
        Debug.Log("[PlayerStorage] Selecciones de jugadores en PlayerStorage reseteadas.");
    }
}
