using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStorage : MonoBehaviour
{
    public BaseUnit player1Prefab; //  públicos para asignación por si acaso, pero los llenará la UI de PlayerSelect
    public BaseUnit player2Prefab;

    public static PlayerStorage Instance;

    
    // Arrastra el GameObject con el script PlayerSelect aquí en el Inspector.
    // Este campo lo asignas UNA VEZ en la escena del menú de selección.
    public PlayerSelect playerSelectUI;

    private void Awake()
    {
        // Singleton
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

    // Este método será llamado por un botón en tu UI de selección 
    // Obtendrá las selecciones directamente de las variables públicas de PlayerSelect.
    public void ObtainSelectedCharactersFromUI()
    {
        if (playerSelectUI != null)
        {
            // Obtener el BaseUnit seleccionado para el jugador 1 desde PlayerSelect
            player1Prefab = playerSelectUI.currentSelectedUnitPlayer1;

            //  toma la selección de J2 si realmente seleccionó algo
            if (playerSelectUI.secondPlayerSelected)
            {
                player2Prefab = playerSelectUI.currentSelectedUnitPlayer2;
            }
            else
            {
                player2Prefab = null; // Si J2 no seleccionó, asegúrate de que sea null
            }

            Debug.Log($"[PlayerStorage] Selección Final Obtenida. J1: {(player1Prefab != null ? player1Prefab.name : "Ninguno")}, J2: {(player2Prefab != null ? player2Prefab.name : "Ninguno")}.");

            //  validación final antes de cargar la escena
            if (player1Prefab == null)
            {
                Debug.LogWarning("Jugador 1 no ha seleccionado un personaje. No se puede iniciar el juego.");
                
            }
        }
        else
        {
            Debug.LogError("[PlayerStorage] La referencia a PlayerSelectUI no está asignada. No se pudo obtener la selección de UI.");
        }
    }


    // El método InstanciarJugadores ahora solo usará los prefabs que ya ha obtenido y guardado.
    public void InstanciarJugadores()
    {
        Debug.Log("[PlayerStorage] Instanciando jugadores en la escena de juego...");

        //  ajustar las posiciones de instanciación aquí.
        Vector3 player1Pos = new Vector3(-3, 0, 0);
        Vector3 player2Pos = new Vector3(3, 0, 0);

        if (player1Prefab != null)
        {
            var newUnit = Instantiate(player1Prefab, player1Pos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject);
            Debug.Log($"Instanciado Jugador 1: {newUnit.name}");

            // Manejo de ataques
            if (newUnit.Attacks != null)
            {
                for (int i = 0; i < newUnit.Attacks.Length; i++)
                {
                    if (player1Prefab.Attacks[i] != null)
                    {
                        newUnit.Attacks[i] = Instantiate(player1Prefab.Attacks[i], new Vector3(0, 0, -1), Quaternion.identity);
                        newUnit.Attacks[i].gameObject.SetActive(false);
                        DontDestroyOnLoad(newUnit.Attacks[i].gameObject);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No hay prefab para Jugador 1 en PlayerStorage. Asegúrate de que se haya seleccionado.");
        }

        if (player2Prefab != null)
        {
            var newUnit = Instantiate(player2Prefab, player2Pos, Quaternion.identity);
            DontDestroyOnLoad(newUnit.gameObject);
            Debug.Log($"Instanciado Jugador 2: {newUnit.name}");

            // Manejo de ataques
            if (newUnit.Attacks != null)
            {
                for (int i = 0; i < newUnit.Attacks.Length; i++)
                {
                    if (player2Prefab.Attacks[i] != null)
                    {
                        newUnit.Attacks[i] = Instantiate(player2Prefab.Attacks[i], new Vector3(0, 0, -1), Quaternion.identity);
                        newUnit.Attacks[i].gameObject.SetActive(false);
                        DontDestroyOnLoad(newUnit.Attacks[i].gameObject);
                    }
                }
            }
        }
        else
        {
            Debug.Log("No hay personaje para Jugador 2 seleccionado. Instanciando solo Jugador 1.");
        }
    }

    // Este método cargará la escena. Obtendrá las selecciones primero, luego carga.
    public void LoadGameScene(string sceneName)
    {
        ObtainSelectedCharactersFromUI(); 
        SceneManager.LoadScene(sceneName);
    }

    // Método para resetear las selecciones si se vuelve al menú principal o similar
    public void ResetPlayerSelections()
    {
        player1Prefab = null;
        player2Prefab = null;
        Debug.Log("[PlayerStorage] Selecciones de jugadores reseteadas.");
    }
}
