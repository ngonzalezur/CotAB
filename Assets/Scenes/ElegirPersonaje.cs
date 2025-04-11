using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.SceneManagement;

public class ElegirPersonaje : MonoBehaviour
{
    [SerializeField] BaseUnit elegido;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanciarPersonaje()
    {
        if (elegido != null)
        {
            //ponerle al personaje que es permanente
            var newUnit = Instantiate(elegido, new Vector3(0, 0, -1), Quaternion.identity);
            int cont = 0;
            foreach (BaseAttack attack in elegido.Attacks)
            {
                newUnit.Attacks[cont] = Instantiate(elegido.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
                newUnit.Attacks[cont].gameObject.SetActive(false);
                DontDestroyOnLoad(newUnit.Attacks[cont]);
                cont++;
            }
        }
        SceneManager.LoadScene("Level easy");
    }
}
