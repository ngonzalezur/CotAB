using UnityEngine;

public class SeleccionPersonaje : MonoBehaviour
{
    public BaseUnit player1;
    public BaseUnit player2;
    public static SeleccionPersonaje Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
    }

    public void InstanciarJugadores() {
        if (player1 != null)
        {
            //ponerle al personaje que es permanente
            var newUnit = Instantiate(player1, new Vector3(0, 0, -1), Quaternion.identity);
            int cont = 0;
            foreach (BaseAttack attack in player1.Attacks)
            {
                newUnit.Attacks[cont] = Instantiate(player1.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
                newUnit.Attacks[cont].gameObject.SetActive(false);
                DontDestroyOnLoad(newUnit.Attacks[cont]);
                cont++;
            }
        }
        if (player2 != null)
        {
            //ponerle al personaje que es permanente
            var newUnit = Instantiate(player2, new Vector3(0, 0, -1), Quaternion.identity);
            int cont = 0;
            foreach (BaseAttack attack in player2.Attacks)
            {
                newUnit.Attacks[cont] = Instantiate(player2.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
                newUnit.Attacks[cont].gameObject.SetActive(false);
                DontDestroyOnLoad(newUnit.Attacks[cont]);
                cont++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
