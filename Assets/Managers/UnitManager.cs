using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Rendering; // Necesario para Task.Delay
using UnityEngine.InputSystem;
using CotA.Configuration;
using System.Reflection;
using System;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;
using CotA.Sound;
using System.Linq;
//using System;


public class UnitManager : MonoBehaviour
{
    public ConfigurationData configuration;

    public SoundManager SoundManager;
    public UIFeedback ui;

    public static UnitManager Instance;

    public List<BaseUnit> Heroes = new List<BaseUnit>();
    public List<BaseUnit> Enemies = new List<BaseUnit>();

    public List<BaseUnit> Invocaciones = new List<BaseUnit>();


    public ObjectPool poolHero1;
    public ObjectPool poolHero2;
    public ObjectPool poolEnemies;

    public List<BaseAttack> AttacksinPlay = new List<BaseAttack>();


    public bool CanPlay = false;

    public bool SecondPlayer = false;

    Gamepad Mando = null;
    bool hasPersist = false;

    private Coroutine corrutinaInvocaciones;

    public void SetHeroUnit(BaseUnit unit)
    {
        //Agrego una intancia y esa sera quien se modifique en escena
        var newUnit = unit;
        if (!hasPersist)
        {
            newUnit = Instantiate(unit, new Vector3(0, 0, -1), Quaternion.identity);
        }       
        
        
        int cont = 0;
        foreach (BaseAttack attack in unit.Attacks)
        {
            newUnit.Attacks[cont] = Instantiate(unit.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
            newUnit.Attacks[cont].gameObject.SetActive(false);
            DontDestroyOnLoad(newUnit.Attacks[cont]);
            cont++;
        }
        Heroes.Add(newUnit);
    }

    public void SetEnemyUnit(BaseUnit unit)
    {
        //Agrego una referencia de la unidad para modificar la referencia y no el prefab
        var newUnit = Instantiate(unit, new Vector3(0, 0, -1), Quaternion.identity);
        int cont = 0;
        foreach (BaseAttack attack in unit.Attacks)
        {
            newUnit.Attacks[cont] = Instantiate(unit.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
            newUnit.Attacks[cont].gameObject.SetActive(false);
            cont++;
        }
        Enemies.Add(newUnit);
        cont = 0;
    }

    public void Awake()
    {
        Instance = this;

        BaseUnit persistentHero = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);
        
        if(persistentHero != null)
        {
            GameManager.character = persistentHero.UnitName;
            configuration.Heroes[0].prefab = persistentHero;
            Heroes.Add(persistentHero);
            Heroes[0] = persistentHero;
            hasPersist = true;
        }
        
        //Referencio los heroes del data maanger y los guardo en la Lista Heroes
        foreach (ConfigurationData.UnitData unit in configuration.Heroes)
        {
            BaseUnit hero = unit.prefab;
            SetHeroUnit(hero);
        }

        //Referencio los enemigos del data maanger y los guardo en la Lista Enemies
        foreach (ConfigurationData.UnitData unit in configuration.Enemies)
        {
            BaseUnit enemy = unit.prefab;
            SetEnemyUnit(enemy);
        }
        SetAttackDictionary();

        Mando = InputSystem.GetDevice<Gamepad>();

        //emepzar corrutinas
        StartCoroutine(AttackMove());
    }

    //codigo que hace aparecer los personajes
    public void SpawnHeroes()
    {
        //Cantidad de personajes aliaos que voy a hacer spawn, si hay dos jugadores le agrega uno
        
            var heroCount = 1;
            if (SecondPlayer)
            {
                heroCount++;
            }

            for (int i = 0; i < heroCount; i++)
            {
                var x = UnityEngine.Random.Range(0, 5);
                var y = UnityEngine.Random.Range(0, 5);
                var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
                var hero = Heroes[i];
                hero.OccupiedTile = randomSpawnTile;
                randomSpawnTile.SetUnit(hero);
                hero.GetHighlightHero()._highlight.SetActive(true);
            }

            StartCoroutine(RestoreStamina(Heroes[0]));
            StartCoroutine(RestoreMana(Heroes[0]));
            //StartCoroutine(AtaqueInvocaciones());
            if (SecondPlayer)
            {
                StartCoroutine(RestoreStamina(Heroes[1]));
                StartCoroutine(RestoreMana(Heroes[1]));
            }      
        

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    //codigo que hace aparecer los enemigos
    public void SpawnEnemies()
    {
        var enemyCount = Enemies.Count;

        for (int i = 0; i < enemyCount; i++)
        {
            var x = UnityEngine.Random.Range(6, 11);
            var y = UnityEngine.Random.Range(0, 5);
            var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
            var enemy = Enemies[i];
            enemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(Enemies[i]);
        }

        GameManager.Instance.ChangeState(GameState.GenerateUI);
    }

    public void ContarKPIAtaque(BaseUnit unit, int i)
    {
        //si es druida
        if(unit.UnitName == "Druid")
        {
            if(i == 0)
            {
                GameManager.AttDruid1++;
            }
            if (i == 1)
            {
                GameManager.AttDruid2++;
            }
            if (i == 2)
            {
                GameManager.AttDruid3++;
            }
            if (i == 3)
            {
                GameManager.AttDruid4++;
            }
            if (i == 4)
            {
                GameManager.MeleeDruid++;
            }
        }

        //si es robot

        if (unit.UnitName == "Robot")
        {
            if (i == 0)
            {
                GameManager.AttRobot1++;
            }
            if (i == 1)
            {
                GameManager.AttRobot2++;
            }
            if (i == 2)
            {
                GameManager.AttRobot3++;
            }
            if (i == 3)
            {
                GameManager.AttRobot4++;
            }
            if (i == 4)
            {
                GameManager.MeleeRobot++;
            }
        }
    }

    public void CanCastAttack(BaseUnit unit, int i)
    {
        if (unit == null) return;

        if (unit.CastMana - unit.Attacks[i].ManaCost > 0 && Time.time - unit.Attacks[i].LastCast1 >= unit.Attacks[i].CoolDown)
        {
            if (unit.Attacks[i].type == BaseAttack.AttType.invocacion && Invocaciones.Count >= 2  && Invocaciones[0].UnitName == unit.Attacks[i].invocacion.UnitName)
            {
                //no hacer nada
            }
            else
            {
                ContarKPIInteracciones();
                ContarKPIAtaque(unit,i);
                unit.CastMana -= unit.Attacks[i].ManaCost;
                StartCoroutine(ui.HandleCooldown(i));
                CastAttack(unit, unit.Attacks[i]);
                unit.Attacks[i].LastCast1 = Time.time;
            

                if(unit.animator != null)
                {
                    unit.animator.SetTrigger("Attack");
                }
                //sound effects de cada ataque
                if(unit.Attacks[i].type == BaseAttack.AttType.muro)
                {
                    SoundManager.PlayFirewallDruid();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.parry)
                {
                    SoundManager.PlayParryRobot();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.area)
                {
                    if(unit.Attacks[i].DoVeneno > 0)
                    {
                        SoundManager.PlayPoisonDruid();
                    }
                    else
                    {
                        SoundManager.PlaySmiteRobot();
                    }
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.dashMelee)
                {
                    SoundManager.PlayMeleeDruid();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.cambiarFaction)
                {
                    SoundManager.PlayGridDruid();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.invocacion)
                {
                    SoundManager.PlayBroteRobot();
                }
            }
            
            if(Invocaciones.Contains(unit))
            {
                unit.Health = 0;
                Invocaciones.Remove(unit);
                Debug.Log(Invocaciones.Count);
                unit.Destroy();
            }
        }
    }
    public delegate void Ataques(BaseUnit unit, BaseAttack attack);

    Dictionary<int, Ataques> Ataque = new Dictionary<int, Ataques>();

    public void SetAttackDictionary()
    {
        Ataque[0] = Proyectil;
        Ataque[1] = Cast;
        Ataque[2] = Area;
        Ataque[3] = Muro;
        Ataque[4] = Fila;
        Ataque[5] = AllEnemiesDamage;
        Ataque[6] = RandomCast;
        Ataque[7] = Melee;
        Ataque[8] = Invocacion;
        Ataque[9] = DashMelee;
        Ataque[10] = Parry;
        Ataque[11] = CambiarFaccion;
        Ataque[12] = AllHerosDamage;

    }

    public void CastAttack(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;

        if (Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            att(unit, attack);
        }
        else
        {
            Debug.Log("Acción no encontrada");
        }
    }


    public void Proyectil(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var tile = unit.OccupiedTile.RightTile();
        var tempAttack = Instantiate(attack, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
        tempAttack.gameObject.SetActive(true);
        tile.SetAttack(tempAttack);
        AttacksinPlay.Add(tempAttack);
    }
    public void Cast(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = unit.GetHighlightHero();
        var tempAttack = Instantiate(attack, new Vector3(target.x, target.y, -1), Quaternion.identity);
        tempAttack.gameObject.SetActive(true);
        target.SetAttack(tempAttack);
        StartCoroutine(Destruir(tempAttack));
    }

    public void SetAttacksInTiles(List<Tile> target, BaseAttack attack)
    {
        if (target == null || attack == null) return;
        foreach (Tile tile in target)
        {
            var att = Instantiate(attack, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
            att.gameObject.SetActive(true);
            tile.SetAttack(att);
            StartCoroutine(Destruir(att));
            if(attack.DoBurn > 0)
            {
                tile.StartCoroutineBurning(att.DoBurn);
            }
        }
    }
    public void Area(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        target.Add(unit.GetHighlightHero());
        //buscar vecinas
        AgregarSiNoNull(target, target[0].UpTile());
        AgregarSiNoNull(target, target[0].DownTile());
        AgregarSiNoNull(target, target[0].LeftTile());
        AgregarSiNoNull(target, target[0].RightTile());

        // Diagonales
        AgregarSiNoNull(target, target[0].UpTile().LeftTile());
        AgregarSiNoNull(target, target[0].UpTile().RightTile());
        AgregarSiNoNull(target, target[0].DownTile().LeftTile());
        AgregarSiNoNull(target, target[0].DownTile().RightTile());

        SetAttacksInTiles(target, attack);
    }
    //Funcion para ver si una tile es nula o agregarla a una lista
    public void AgregarSiNoNull(List<Tile> lista, Tile tile)
    {
        if (tile != null)
            lista.Add(tile);
    }

    public void Muro(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        target = AllYTiles(unit.GetHighlightHero());

        SetAttacksInTiles(target, attack);
    }

    //Funcion para conseguir todas las tiles de una columna
    public List<Tile> AllYTiles(Tile tile)
    {
        if (tile == null) return null;
        var target = new List<Tile>();
        for (int i = 0; i < 6; i++)
        {
            var tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(tile.x, i));
            AgregarSiNoNull(target, tempTile);
        }
        return target;
    }
    public void Fila(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        target = AllXTiles(unit.OccupiedTile);
        SetAttacksInTiles(target, attack);
    }

    //Funcion para conseguir todas las tiles de una fila a la derecha del heroe
    public List<Tile> AllXTiles(Tile tile)
    {
        if (tile == null) return null;
        var target = new List<Tile>();
        for (int i = tile.x + 1; i <= 11; i++)
        {
            var tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(i, tile.y));
            AgregarSiNoNull(target, tempTile);
        }
        return target;
    }
    public void AllEnemiesDamage(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        foreach (BaseUnit enemy in Enemies)
        {
            var tempTile = enemy.OccupiedTile;
            AgregarSiNoNull(target, tempTile);
        }
        SetAttacksInTiles(target, attack);
    }

    public void AllHerosDamage(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        foreach (BaseUnit hero in Heroes)
        {
            var tempTile = hero.OccupiedTile;
            AgregarSiNoNull(target, tempTile);
        }
        SetAttacksInTiles(target, attack);
    }

    public void RandomCast(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        // no se definir cuantos randoms van a hacer, queda pendiente
    }
    public void Melee(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        AgregarSiNoNull(target, unit.OccupiedTile.RightTile());
        SetAttacksInTiles(target, attack);
    }



    public void Invocacion(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        target.Add(unit.OccupiedTile.LeftTile());
        if(Invocaciones.Count <= 1)
        {
            if (Invocaciones.Count == 0)
            {
                InstanciarInvocacion(attack, target);
            }
            else if (Invocaciones[0] != null && Invocaciones[0].UnitName != attack.invocacion.UnitName)
            {
                InstanciarInvocacion(attack, target);
            }
            
        }
    }

    public void InstanciarInvocacion(BaseAttack attack, List<Tile> target)
    {
        if (target == null || target[0] == null || attack.invocacion == null || target[0].OccupiedUnit != null) return;
        var tile = target[0];
        var invocacion = Instantiate(attack.invocacion, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
        var ataqueInvocacion = Instantiate(invocacion.Attacks[0], new Vector3(tile.x, tile.y, -1), Quaternion.identity);
        ataqueInvocacion.LastCast1 = Time.time;
        ataqueInvocacion.gameObject.SetActive(false);
        invocacion.Attacks[0] = ataqueInvocacion;
        tile.SetUnit(invocacion);
        Invocaciones.Add(invocacion);
    }

    IEnumerator AtaqueInvocaciones(BaseUnit inovocaciones)
    {
        if(inovocaciones == null)
        {
            corrutinaInvocaciones = null;
            yield break;
        }
        
            if (inovocaciones == null || inovocaciones.Attacks[0] == null)
            {
                //literalmente no hacer nada porque al parecer se sale de la corrutina
            }
            else
            {
            Debug.Log(Invocaciones.Count);
            CanCastAttack(inovocaciones, 0);
                //yield return new WaitForSeconds(1f);
                //Debug.Log("soy un planta que ataca");
                //Invocaciones.Remove(unit);                
            }
        
        yield return new WaitForSeconds(2f);
        corrutinaInvocaciones = null;
    }

    public void AtaqueInvoacion()
    {
        //llamar corrutina si se puede
        if(corrutinaInvocaciones == null && Invocaciones.Count > 0)
        {
            corrutinaInvocaciones = StartCoroutine(AtaqueInvocaciones(Invocaciones[0]));
        }
        
    }
    public void DashMelee(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        AgregarSiNoNull(target, unit.GetHighlightHero());
        StartCoroutine(TeleportMeleeDash(unit, attack, unit.OccupiedTile));
    }

    IEnumerator TeleportMeleeDash(BaseUnit unit, BaseAttack attack, Tile baseTile)
    {
        //unit.GetHighlightHero().LeftTile().InstantSetUnit(unit);
        var newTile = DashThrouhTiles(unit, unit.OccupiedTile);
        var target = new List<Tile>();
        AgregarSiNoNull(target, newTile.RightTile());
        SetAttacksInTiles(target, attack);
        yield return new WaitForSeconds(1);
        baseTile.InstantSetUnit(unit);
    }

    public Tile DashThrouhTiles(BaseUnit unit, Tile tile)
    {
        var nextTile = tile.RightTile();
        if(nextTile == null || nextTile.OccupiedUnit != null)
        {
            //StartCoroutine(DeOccupiedTile(tile));
            return tile;
        }
        //tile.OccupiedUnit = null;
        
        nextTile.InstantSetUnit(unit);
        //nextTile.OccupiedUnit = unit;
        StartCoroutine(OccupiedTile(tile, unit));
        StartCoroutine(DeOccupiedTile(tile));
        return DashThrouhTiles(unit, nextTile);
    }

    IEnumerator OccupiedTile(Tile tile, BaseUnit unit)
    {
        if(tile != null)
        {
           tile.OccupiedUnit = unit;
            if(tile.OccupiedAttack != null)
            {
                tile.OccupiedAttack.DoDamage(unit);
                tile.OccupiedAttack.Destroy();
            }
        }
        
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator DeOccupiedTile(Tile tile)
    {
        tile.OccupiedUnit = null;
        yield return new WaitForSeconds(0.1f);
    }
    public void Parry(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        StartCoroutine(ActivateParry(unit));
        //algo visaul del parry, una corrutina con while true depronto y que al finalizar activate parry se detenga
    }

    IEnumerator ActivateParry(BaseUnit unit)
    {
        unit.parry = true;
        yield return new WaitForSeconds(2);
        unit.parry = false;
    }
    public void CambiarFaccion(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        target = PrimeraColumnaEnemiga(target);
        StartCoroutine(CambiarFactionToHero(target));
    }

    IEnumerator CambiarFactionToHero(List<Tile> target)
    {
        foreach(Tile tile in target)
        {
            tile.HeroTile.gameObject.SetActive(true);
            tile.EnemyTile.gameObject.SetActive(false);
            tile.Faction = Faction.Hero;
        }
        yield return new WaitForSeconds(10);
        foreach (Tile tile in target)
        {
            tile.HeroTile.gameObject.SetActive(false);
            tile.EnemyTile.gameObject.SetActive(true);
            tile.Faction = Faction.Enemy;
        }
    }

    public List<Tile> PrimeraColumnaEnemiga(List<Tile> target)
    {        
        for(int i = 0; i < 6; i++)
        {
            var tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(GridManager.Instance._width / 2, i));
            AgregarSiNoNull(target, tempTile);
        }
        return target;
    }

    public void AttackHero2(BaseUnit unit, int player)
    {
        if (unit == null) return;

        if (player == 0)
        {
            if ((Input.GetKeyDown(KeyCode.I) || (Mando != null && Mando.buttonSouth.wasPressedThisFrame)))
            {
                CanCastAttack(unit, 0);
            }
            if ((Input.GetKeyDown(KeyCode.J) || (Mando != null && Mando.buttonNorth.wasPressedThisFrame)))
            {
                CanCastAttack(unit, 1);
            }
            if ((Input.GetKeyDown(KeyCode.K) || (Mando != null && Mando.buttonEast.wasPressedThisFrame)))
            {
                CanCastAttack(unit, 2);
            }
            if ((Input.GetKeyDown(KeyCode.L) || (Mando != null && Mando.buttonWest.wasPressedThisFrame)))
            {
                CanCastAttack(unit, 3);
            }
        }
        if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                CanCastAttack(unit, 0);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                CanCastAttack(unit, 1);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                CanCastAttack(unit, 2);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                CanCastAttack(unit, 3);
            }
        }
    }

    IEnumerator AttackMove()
    {
        while (true)
        {
            for (int i = AttacksinPlay.Count - 1; i >= 0; i--)
            {
                if (AttacksinPlay[i] == null) break;
                if (AttacksinPlay[i].Faction == Faction.Hero)
                {
                    //if (Attacks[i] != null && Attacks[i].OccupiedTile.x >= GridManager.Instance._width - 1)
                    //{
                    //    //Attacks.Remove(Attacks[i]);
                    //    Attacks[i].Destroy();

                    //}
                    if (AttacksinPlay[i] == null)
                    {
                        break;
                    }
                    if (AttacksinPlay[i] != null && AttacksinPlay[i].OccupiedTile != null && AttacksinPlay[i].OccupiedTile.x < GridManager.Instance._width - 1)
                    {
                        var nextTile = AttacksinPlay[i].OccupiedTile.RightTile();
                        nextTile.SetAttack(AttacksinPlay[i]);
                    }
                    else if (AttacksinPlay[i] != null)
                    {
                        AttacksinPlay[i].Destroy();
                    }
                }
                else if (AttacksinPlay[i].Faction == Faction.Enemy)
                {
                    //if (Attacks[i] != null && Attacks[i].OccupiedTile.x <= 0)
                    //{
                    //    //Attacks.Remove(Attacks[i]);
                    //    Attacks[i].Destroy();

                    //}
                    if (AttacksinPlay[i] != null && AttacksinPlay[i].OccupiedTile.x > 0)
                    {
                        var nextTile = AttacksinPlay[i].OccupiedTile.LeftTile();
                        nextTile.SetAttack(AttacksinPlay[i]);
                    }
                    else if (AttacksinPlay[i] != null)
                    {
                        AttacksinPlay[i].Destroy();
                    }
                }


            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void MoveHero(BaseUnit hero, int player)
    {
        if (hero == null) return;
        if (player == 0)
        {
            if ((Input.GetKeyDown(KeyCode.W) || (Mando != null && Mando.dpad.up.wasPressedThisFrame)))
            {
                CanMove(hero, 0);
            }
            if ((Input.GetKeyDown(KeyCode.A) || (Mando != null && Mando.dpad.left.wasPressedThisFrame)))
            {
                CanMove(hero, 1);
            }
            if ((Input.GetKeyDown(KeyCode.S) || (Mando != null && Mando.dpad.down.wasPressedThisFrame)))
            {
                CanMove(hero, 2);
            }
            if ((Input.GetKeyDown(KeyCode.D) || (Mando != null && Mando.dpad.right.wasPressedThisFrame)))
            {
                CanMove(hero, 3);
            }
        }
        if (player == 1)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow)))
            {
                CanMove(hero, 0);
            }
            if ((Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                CanMove(hero, 1);
            }
            if ((Input.GetKeyDown(KeyCode.DownArrow)))
            {
                CanMove(hero, 2);
            }
            if ((Input.GetKeyDown(KeyCode.RightArrow)))
            {
                CanMove(hero, 3);
            }
        }
    }

    public void FalseHighlight(BaseUnit unit)
    {
        if (unit.Faction == Faction.Hero && unit.GetHighlightHero() != null)
        {
            unit.GetHighlightHero()._highlight.SetActive(false);
            //Debug.Log("Me apague");
        }
        else
        {
            unit.GetHighlightEnemy()._highlight.SetActive(false);
        }
    }
    public void TrueHighlight(BaseUnit unit)
    {
        if (unit.Faction == Faction.Hero)
        {
            unit.GetHighlightHero()._highlight.SetActive(true);
            //Debug.Log("Me prendi");
        }
        else
        {
            unit.GetHighlightEnemy()._highlight.SetActive(true);
        }
    }

    public void CanMove(BaseUnit unit, int direction)
    {
        //para direccion tendremos 0 arriba, 1 izquierda, 2 abajo, 3 derecha
        if (direction == 0 && CheckFaction(unit, unit.OccupiedTile.UpTile()))
        {
            //FalseHighlight(unit);
            //moverse
            MoveUnit(unit, unit.OccupiedTile.UpTile());
            //TrueHighlight(unit);
        }
        if (direction == 1 && CheckFaction(unit, unit.OccupiedTile.LeftTile()))
        {
            //FalseHighlight(unit);
            //moverse
            MoveUnit(unit, unit.OccupiedTile.LeftTile());
            if(unit.MoveCooldown > 0)
            {
                unit.animator.SetTrigger("MoveBack");
            }
            //unit.animator.SetTrigger("MoveBack");
            //TrueHighlight(unit);
        }
        if (direction == 2 && CheckFaction(unit, unit.OccupiedTile.DownTile()))
        {
            //FalseHighlight(unit);
            //moverse
            MoveUnit(unit, unit.OccupiedTile.DownTile());
            //TrueHighlight(unit);
        }
        if (direction == 3 && CheckFaction(unit, unit.OccupiedTile.RightTile()))
        {
            //FalseHighlight(unit);
            //moverse
            MoveUnit(unit, unit.OccupiedTile.RightTile());
            if (unit.MoveCooldown > 0)
            {
                unit.animator.SetTrigger("MoveFoward");
            }
            //unit.animator.SetTrigger("MoveFoward");
            //TrueHighlight(unit);
        }
    }

    public void MoveUnit(BaseUnit unit, Tile tile)
    {
        if (unit == null || tile == null) return;
        if(unit.MoveCooldown > 0)
        {
            ContarKPIInteracciones();
            FalseHighlight(unit);
            unit.MoveCooldown--;
            tile.SetUnit(unit);
            tile.OccupiedUnit.GetHighlightHero()._highlight.SetActive(true);
        }        
    }
   void ContarKPIInteracciones()
    {
        GameManager.interactionTotal++;
    }

    public bool CheckFaction(BaseUnit unit, Tile tile)
    {
        if (unit == null || tile == null) return false;
        if (unit.Faction == tile.Faction || tile.Faction == Faction.Neutral)
        {
            return true;
        }
        return false;
    }





    //Codigo de movimiento que SI se esta suando ahorita para mover y atacar del enemigo
    IEnumerator MoverEnemigo()
    {
        foreach (BaseUnit Enemy1 in Enemies)
        {
            var newTile = Enemy1.OccupiedTile;
            //Esto hace que el enemigo se mueva de forma aleatoria
            int randomMove = UnityEngine.Random.Range(1, 6);
            //Vector2 nuevaPosicion = new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y);

            //if (randomMove == 1 && Enemy1.OccupiedTile.y < GridManager.Instance._height - 1)
            //{
            //    if(Enemy1.OccupiedTile.UpTile().OccupiedUnit == null)
            //    {
            //        newTile = Enemy1.OccupiedTile.UpTile();
            //    }

            //}
            //else if (randomMove == 2 && Enemy1.OccupiedTile.x > GridManager.Instance._width / 2)
            //{
            //    if (Enemy1.OccupiedTile.LeftTile().OccupiedUnit == null)
            //    {
            //        newTile = Enemy1.OccupiedTile.LeftTile();
            //    }                
            //}
            //else if (randomMove == 3 && Enemy1.OccupiedTile.y > 0)
            //{
            //    if (Enemy1.OccupiedTile.DownTile().OccupiedUnit == null)
            //    {
            //        newTile = Enemy1.OccupiedTile.DownTile();
            //    }

            //}
            //else if (randomMove == 4 && Enemy1.OccupiedTile.x < GridManager.Instance._width - 1)
            //{
            //    if(Enemy1.OccupiedTile.RightTile().OccupiedUnit == null)
            //    {
            //        newTile = Enemy1.OccupiedTile.RightTile();
            //    }
            //}


            //if (newTile != null)
            //{
            //    newTile.SetUnit(Enemy1);
            //}


            //Aqui esta codigo de ataque enemigo
            var randomAtt = UnityEngine.Random.Range(0, 100);

            if (randomAtt < 50)
            {
                //AttackEnemy(Enemy1);
            }
            // ataque especial
            if (Enemy1.EnemyType.Special && randomAtt < 5)
            {
                //SpecialAttackEnemy(Enemy1.Attacks[1], Enemy1.GetHighlightEnemy());

            }


        }
        yield return new WaitForSeconds(2f);
    }

    public void AttackEnemy(BaseUnit enemy)
    {
        //Ataque basico enemigo
        var prefab = enemy.Attacks[0];
        var attackSpawned = poolEnemies.GetObjectInPool();
        attackSpawned.gameObject.SetActive(true);
        var spawnTileAtt = GridManager.Instance.GetTileAtPosition(new Vector2(enemy.OccupiedTile.x - 1, enemy.OccupiedTile.y));
        spawnTileAtt.SetAttack(attackSpawned);
        AttacksinPlay.Add(attackSpawned);

    }


    public void Start()
    {
        //StartCoroutine(RestoreStamina(Heroes[0]));
        //StartCoroutine(RestoreMana(Heroes[0]));
        //if (SecondPlayer)
        //{
        //    StartCoroutine(RestoreStamina(Heroes[1]));
        //    StartCoroutine(RestoreMana(Heroes[1]));
        //}
    }

    IEnumerator RestoreMana(BaseUnit hero)
    {
        while (true)
        {
            if (hero.MaxMana > hero.CastMana)
            {
                hero.CastMana += hero.regenMana;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void Update()
    {
        //Debug.Log(CanPlay);
        if (CanPlay)
        {
            MoveHero(Heroes[0], 0);
            AttackHero2(Heroes[0], 0);
            if(SecondPlayer && Heroes[1] != null)
            {
                MoveHero(Heroes[1], 1);
                AttackHero2(Heroes[1], 1);
            }
            //TrueHighlight(Heroes[0]);
            //StartCoroutine(AttackMove());
            TakeDamage();
            AtaqueInvoacion();
            if (SoundManager != null && Heroes[0] != null && Heroes[0].Health < Heroes[0].MaxHealth / 2)
            {
                SoundManager.ChangeSoundtrackToMidLifeMode();
            }
        }

    }

    //codigo para tirar un poder especial



    IEnumerator Destruir(BaseAttack att)
    {
        yield return new WaitForSeconds(1f);
        att.Destroy();
    }
    public void DestruirProyectil(BaseAttack att)
    {
        att.Destroy();
    }

    public void TakeDamage()
    {
        var AllUnits = new List<BaseUnit>();
        AllUnits.AddRange(Heroes);
        AllUnits.AddRange(Enemies);
        AllUnits.AddRange(Invocaciones);
        foreach (BaseUnit unit in AllUnits)
        {
            if (unit == null || unit.OccupiedTile == null) continue;
            if (unit.OccupiedTile.OccupiedAttack == null)
            {

            }
            else if (unit.OccupiedTile.OccupiedAttack != null)
            {
                if (unit.Faction != unit.OccupiedTile.OccupiedAttack.Faction)
                {
                    unit.OccupiedTile.OccupiedAttack.DoDamage(unit);
                    unit.OccupiedTile.OccupiedAttack.Destroy();
                }
                else if (unit.OccupiedTile.OccupiedAttack.Faction == unit.Faction && (unit.OccupiedTile.OccupiedAttack.Heal > 0 || unit.OccupiedTile.OccupiedAttack.Stamina > 0))
                {
                    unit.OccupiedTile.OccupiedAttack.DoHeal(unit);
                    unit.OccupiedTile.OccupiedAttack.DoStamina(unit);
                    unit.OccupiedTile.OccupiedAttack.Destroy();
                }
            }
            if (UnitManager.Instance.Invocaciones.Contains(unit) && unit.Health <= 0)
            {
                Invocaciones.Remove(unit);
                unit.Destroy();
            }
            if (unit.Health <= 0 && unit.Faction == Faction.Hero)
            {
                Heroes.Remove(unit);
                unit.Destroy();
                if (Heroes.Count == 0)
                {
                    GameManager.Instance.ChangeState(GameState.EndFight);
                }
            }
            if (unit.Health <= 0 && unit.Faction == Faction.Enemy)
            {
                Enemies.Remove(unit);
                unit.Destroy();
                if (Enemies.Count == 0)
                {
                    GameManager.Instance.ChangeState(GameState.EndFight);
                }
            }

        }
    }




    IEnumerator RestoreStamina(BaseUnit hero)
    {
        while (true)
        {
            if (hero.MaxStamina > hero.MoveCooldown)
            {
                hero.MoveCooldown += hero.regenStamina;
            }
            yield return new WaitForSeconds(1f);
        }
    }



    IEnumerator VenenoDoDamage()
    {
        var AllUnits = new List<BaseUnit>();
        AllUnits.AddRange(Heroes);
        AllUnits.AddRange(Enemies);
        foreach (BaseUnit unit in AllUnits)
        {
            unit.VenenoDamage();
        }
        yield return new WaitForSeconds(2f);
    }
}
