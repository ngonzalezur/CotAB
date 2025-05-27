using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PausaManager : MonoBehaviour
{
    public static PausaManager Instance;

    public GameObject pauseMenuCanvas;
    private List<Canvas> otherCanvases = new List<Canvas>();
    private bool isPaused = false;

    [Header("Nombre de la escena de selección de personaje")]
    public string sceneSeleccionPersonaje = "SeleccionPersonaje";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // Guardar y ocultar otros Canvas
        otherCanvases.Clear();
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject != pauseMenuCanvas)
            {
                otherCanvases.Add(canvas);
                canvas.gameObject.SetActive(false);
            }
        }

        pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenuCanvas.SetActive(false);

        foreach (Canvas canvas in otherCanvases)
        {
            if (canvas != null)
                canvas.gameObject.SetActive(true);
        }
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuCanvas.SetActive(false);

        StartCoroutine(DestroyDontDestroyOnLoadObjectsAndLoadScene());
    }

    private IEnumerator DestroyDontDestroyOnLoadObjectsAndLoadScene()
    {
        // Crear objeto temporal para acceder a la escena DontDestroyOnLoad
        GameObject temp = new GameObject("TempDDOL");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        // Guardamos una referencia a este GameObject
        GameObject self = this.gameObject;

        // Buscar todos los objetos en la escena DDOL
        List<GameObject> ddolObjects = new List<GameObject>();
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>(true))
        {
            if (go.scene == ddolScene && go != temp && go != self)
            {
                ddolObjects.Add(go);
            }
        }

        // Destruir los objetos encontrados
        foreach (GameObject go in ddolObjects)
        {
            Destroy(go);
        }

        // Destruir el objeto temporal
        Destroy(temp);

        yield return null; // Esperar destrucción

        // Cargar la escena deseada
        SceneManager.LoadScene(sceneSeleccionPersonaje);

        // Esperar un frame para que la escena cargue
        yield return null;

        // Ahora destruir este objeto (PauseManager)
        Destroy(self);
    }

        public void SalirDelJuego()
    {
        Application.Quit();
    }
}
