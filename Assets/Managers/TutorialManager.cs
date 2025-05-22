using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class TutorialManager : MonoBehaviour
{
    // Mostrar teclas de movimiento
    public TutoState GameState;
    int Counter = 0;

    Tile RandomTile()
    {
        var rx = UnityEngine.Random.Range(0, 4);
        var ry = UnityEngine.Random.Range(0, 4);
        var randomTile = GridManager.Instance.GetTileAtPosition(new Vector2(rx,ry));
        return randomTile;
    }

    // Highlight casilla aleatoria
    // Empezar timer

    void TimerHighlith(Tile tile)
    {
        tile._highlight.SetActive(true);
        timerCoroutine = StartCoroutine(Timer1());
    }

    Coroutine timerCoroutine = null;
    IEnumerator Timer1()
    {
        yield return new WaitForSeconds(20f);
        Debug.Log("Se imaginan ser 5 en el equipo, una locura");
        timerCoroutine = null;
    }

    bool DetectCharacterInTile(Tile tile)
    {
        var unit = tile.OccupiedUnit;
        if (unit != null)
        {
            if (unit.Faction == Faction.Hero)
            {
                Debug.Log("El jugador ha pisado la casilla correcta");
                tile._highlight.SetActive(false);
                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                    timerCoroutine = null;
                }
                return true;
            }
        }
        return false;
    }

    void TurnOfTile(Tile tile)
    {
        tile._highlight.SetActive(false);
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
    IEnumerator CheckIfInTile(Tile tile)
    {
        while (!DetectCharacterInTile(tile))
        {
            //DetectCharacterInTile(tile);
            yield return new WaitForSeconds(1f);
        }
        Counter++;
        if (Counter < 3)
        {
            TutoirallStates(TutoState.Tutorial1);
            yield break;
        }
        else
        {
            Counter = 0;
            TutoirallStates(TutoState.Tutorial2);
        }
    }
   
    public enum TutoState
    {
        Tutorial1,
        Tutorial2,
        Tutorial3,
        Tutorial4,
        EndTutorial
    }
    void TutoirallStates(TutoState state)
    {
        GameState = state;
        switch (state)
        {
            case TutoState.Tutorial1:
                // Mostrar teclas de movimiento
                // Highlight casilla aleatoria
                // Empezar timer
                var tile = RandomTile();
                TimerHighlith(tile);
                StartCoroutine(CheckIfInTile(tile));
                break;
            case TutoState.Tutorial2:
                var tile2 = RandomTile();
                TimerHighlith(tile2);
                StartCoroutine(CheckIfNoStamina(tile2));
                // Acceder a la casilla del jugador
                //DetectCharacterInTile(tile);
                break;
            case TutoState.Tutorial3:
                // Mostrar UI hechizos
                break;
            case TutoState.Tutorial4:
                // Dummy lanza proyectiles
                break;
        }
    }


    // Acceder a la casilla del jugador
    // Si casilla highlight == casilla jugador: contador +1
    // Si contador == 3, siguiente tutorial
    // Si timer > 30s: play sonido explicativo

    // Mostrar teclas de movimiento
    // Highlight casilla aleatoria
    // Empezar timer
    // Acceder a la casilla del jugador
    // Si casilla highlight == casilla jugador contador: Higlight casilla aleatoria
    // Si Estamina == 0: Tachas teclas de movimiento y mostrar barra vacia
    // Si timer > 30s: play sonido explicativo

    bool CheckStamina()
    {
        return UnitManager.Instance.Heroes[0].MoveCooldown <= 0;
    }

    IEnumerator CheckIfNoStamina(Tile tile)
    {
        while (!DetectCharacterInTile(tile) || !CheckStamina())
        {
            //DetectCharacterInTile(tile);
            yield return new WaitForSeconds(1f);
            if(CheckStamina())
            {
                break;
            }
        }
        if (CheckStamina())
        {
            Debug.Log("Tacansao");
            TurnOfTile(tile);
            TutoirallStates(TutoState.Tutorial2);
            yield break;
        }
        else
        {
            TutoirallStates(TutoState.Tutorial3);
        }
    }

    // Mostarr UI hechizos
    // Mostrar boton presionado 
    // Si casilla del dummy == casilla hechizo: Contador2 +1
    // Si contador2 == 2: siguiente tutorial
    // Si casilla del dummy != casilla hechizo: PLay sonido explicativo
    // Si Presiona tecla hechizo en CD: Play sonido explicativo y mostrar UI tachada

    bool CheckDummyHit()
    {
        return UnitManager.Instance.Enemies[0].Health
    }

    IEnumerable CheckDummyHit()
    {
        while (!CheckDummyHit())
        {
            //DetectCharacterInTile(tile);
            yield return new WaitForSeconds(1f);
            if (CheckDummyHit())
            {
                break;
            }
            Counter++;
            if (Counter < 3)
            {
                TutoirallStates(TutoState.Tutorial3);
                yield break;
            }
            else
            {
                Counter = 0;
                TutoirallStates(TutoState.Tutorial4);
            }
        }
    }

    // Dummy lanza proyectiles
    // Proyectil desaparece: Contador +1
    // Contador == 3: fin de tutorial


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(StartTutorial), 0.1f); // Se ejecuta después de 0.1 segundo
    }

    void StartTutorial()
    {
        TutoirallStates(TutoState.Tutorial1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
