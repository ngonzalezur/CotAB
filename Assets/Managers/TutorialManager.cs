using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnitManager;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


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
                CheckDummyHealth();
                StartCoroutine(CountDummyHits(ActualHealthDummy));
                // Mostrar UI hechizos
                break;
            case TutoState.Tutorial4:
                StartCoroutine(DummyTriAttack());
                // Dummy lanza proyectiles
                break;
            case TutoState.EndTutorial:
                // Fin de tutorial
                if (UnitManager.Instance.Heroes[0].UnitName == "Druid")
                {
                    SceneManager.LoadScene("Cine01Druid");
                }
                else
                {
                    SceneManager.LoadScene("Cine01Robot");
                }
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
            else if (DetectCharacterInTile(tile))
            {
                break;
            }
        }
        if (CheckStamina())
        {
            Debug.Log("Tacansao");
            TurnOfTile(tile);
            TutoirallStates(TutoState.Tutorial3);
            yield break;
        }
        else
        {
            TutoirallStates(TutoState.Tutorial2);
        }
    }

    // Mostarr UI hechizos
    // Mostrar boton presionado 
    // Si casilla del dummy == casilla hechizo: Contador2 +1
    // Si contador2 == 2: siguiente tutorial
    // Si casilla del dummy != casilla hechizo: PLay sonido explicativo
    // Si Presiona tecla hechizo en CD: Play sonido explicativo y mostrar UI tachada

    public int ActualHealthDummy;

    void CheckDummyHealth()
    {
        ActualHealthDummy = UnitManager.Instance.Enemies[0].Health;
    }
    bool CheckDummyHit(int i)
    {
        return UnitManager.Instance.Enemies[0].Health !=  i;
    }

    IEnumerator CountDummyHits(int i)
    {
        while (!CheckDummyHit(i))
        {
            //DetectCharacterInTile(tile);
            yield return new WaitForSeconds(1f);
             
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

    // Dummy lanza proyectiles
    void DummyAttack()
    {
        var Dummy = UnitManager.Instance.Enemies[0];
        Dummy.CastAttack(0);
    }
    // Proyectil desaparece: Contador +1
    // Contador == 3: fin de tutorial

    IEnumerator DummyTriAttack()
    {
        yield return new WaitForSeconds(2f);

        var target = SetTarget(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0]);
        ShowPrecast(target);
        yield return new WaitForSeconds(1f);
        DeletePrecast(target);
        DoAttack(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0], target);
        yield return new WaitForSeconds(2.5f);

        target = SetTarget(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0]);
        ShowPrecast(target);
        yield return new WaitForSeconds(1f);
        DeletePrecast(target);
        DoAttack(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0], target);
        yield return new WaitForSeconds(2.5f);

        target = SetTarget(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0]);
        ShowPrecast(target);
        yield return new WaitForSeconds(1f);
        DeletePrecast(target);
        DoAttack(UnitManager.Instance.Enemies[0], UnitManager.Instance.Enemies[0].Attacks[0], target);
        yield return new WaitForSeconds(1.5f);

        TutoirallStates(TutoState.EndTutorial);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(StartTutorial), 0.1f); // Se ejecuta después de 0.1 segundo
        uManager = UnitManager.Instance;
    }
    public UnitManager uManager;
    void StartTutorial()
    {
        TutoirallStates(TutoState.Tutorial1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Tile> SetTarget(BaseUnit unit, BaseAttack attack)
    {
        var target = new List<Tile>();
        if (uManager.Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            target = att(unit, attack);
        }
        return target;
    }
    public void ShowPrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(true);
        }
    }
    public void DeletePrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(false);
        }
    }

    void DoAttack(BaseUnit unit, BaseAttack attack, List<Tile> target)
    {
        if (target != null)
        {
            if (attack.type == BaseAttack.AttType.dashMelee)
            {
                uManager.StartCoroutine(uManager.TeleportMeleeDash(unit, attack, unit.OccupiedTile));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.parry)
            {
                uManager.StartCoroutine(uManager.ActivateParry(unit));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.gancho)
            {
                uManager.HabilidadGancho(unit, attack);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.atractor)
            {
                uManager.HabilidadAtraer(unit, attack);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.areaLast2Columns)
            {
                uManager.MoveFront(attack, target);
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.barridofilainverso)
            {
                uManager.StartCoroutine(uManager.Barrer(target, attack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.barridocolumnainverso)
            {
                uManager.StartCoroutine(uManager.Barrer(target, attack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.areadelay)
            {
                uManager.SetAttacksInTiles(target, attack);
                uManager.StartCoroutine(uManager.ExtraAttack(target, attack.ExtraAttack));
                uManager.PrecastDelete(target);
            }
            else if (attack.type == BaseAttack.AttType.invocacion)
            {
                if (uManager.Invocaciones.Count <= 1)
                {
                    if (uManager.Invocaciones.Count == 0)
                    {
                        uManager.InstanciarInvocacion(attack, target);
                    }
                    else if (uManager.Invocaciones[0] != null && uManager.Invocaciones[0].UnitName != attack.invocacion.UnitName)
                    {
                        uManager.InstanciarInvocacion(attack, target);
                    }

                }
                uManager.PrecastDelete(target);
            }
            else
            {
                uManager.SetAttacksInTiles(target, attack);
                uManager.PrecastDelete(target);
            }
        }
    }
}
