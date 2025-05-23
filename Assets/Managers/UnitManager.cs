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
using Unity.Behavior;
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

    //Gamepad Mando = null;
    //Gamepad Mando2 = null;
    bool hasPersist = false;

    private List<Tile>[] currentPrecasts = new List<Tile>[2] { new List<Tile>(), new List<Tile>() };
    private int[] currentPrecastIndex = new int[2] { -1, -1 }; // -1 = ning�n precast activo



    private Coroutine corrutinaInvocaciones;
    private Coroutine corrutinaInvocaciones2;


    private bool hasChangedMusic = false; // Boolean para controlar el cambio de música

    public Gamepad Mando;
    public Gamepad Mando2;

    void OnEnable()
    {
        // Escucha conexiones y desconexiones de dispositivos
        InputSystem.onDeviceChange += OnDeviceChange;

        // Asignar los controles que ya estén conectados al iniciar
        foreach (var device in Gamepad.all)
        {
            AsignarGamepad(device);
        }
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                    AsignarGamepad(gamepad);
                    break;

                case InputDeviceChange.Removed:
                case InputDeviceChange.Disconnected:
                    RemoverGamepad(gamepad);
                    break;
            }
        }
    }

    void AsignarGamepad(Gamepad gamepad)
    {
        if (Mando == null)
        {
            Mando = gamepad;
            Debug.Log("Asignado a mando1: " + gamepad.deviceId);
        }
        else if (Mando2 == null && gamepad != Mando)
        {
            Mando2 = gamepad;
            Debug.Log("Asignado a mando2: " + gamepad.deviceId);
        }
    }

    void RemoverGamepad(Gamepad gamepad)
    {
        if (Mando == gamepad)
        {
            Debug.Log("mando1 desconectado");
            Mando = null;
        }
        else if (Mando2 == gamepad)
        {
            Debug.Log("mando2 desconectado");
            Mando2 = null;
        }
    }

    public void SetHeroUnit(BaseUnit unit)
    {
        //Agrego una intancia y esa sera quien se modifique en escena
        var newUnit = unit;
        if (!hasPersist)
        {
            newUnit = Instantiate(unit, new Vector3(0, 0, -1), Quaternion.identity);
        }

        SetAttacks(unit);
        //int cont = 0;
        //foreach (BaseAttack attack in unit.Attacks)
        //{
        //    newUnit.Attacks[cont] = Instantiate(unit.Attacks[cont], new Vector3(0, 0, -1), Quaternion.identity);
        //    newUnit.Attacks[cont].gameObject.SetActive(false);
        //    //ui.abilities[cont].cooldown = newUnit.Attacks[cont].CoolDown;
        //    DontDestroyOnLoad(newUnit.Attacks[cont]);
        //    cont++;
        //}
        Heroes.Add(newUnit);
    }

    public void SetAttacks(BaseUnit unit)
    {
        var pref1 = configuration.Heroes[0].prefab;
        var pref2 = configuration.Heroes[1].prefab;
        var i = 0;
        var reference = new BaseAttack[0];
        Debug.Log(pref1.UnitName);
        Debug.Log(pref2.UnitName);
        Debug.Log(unit.UnitName);
        if (unit.UnitName == pref1.UnitName)
        {
            i = configuration.Heroes[0].Attacks.Length;
            reference = configuration.Heroes[0].Attacks;
            Debug.Log(configuration.Heroes[0].Attacks[0]);
            Debug.Log("entre");
        }
        else
        {
            i = configuration.Heroes[1].Attacks.Length;
            reference = configuration.Heroes[1].Attacks;
        }
        int cont = 0;
        var newAtts = new BaseAttack[i];
        Debug.Log(reference);
        foreach (BaseAttack attack in reference)
        {
            newAtts[cont] = Instantiate(reference[cont], new Vector3(0, 0, -1), Quaternion.identity);
            newAtts[cont].gameObject.SetActive(false);
            //ui.abilities[cont].cooldown = newUnit.Attacks[cont].CoolDown;
            DontDestroyOnLoad(newAtts[cont]);
            cont++;
        }
        unit.Attacks = newAtts;
        Debug.Log(unit.Attacks[0]);
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
        hasChangedMusic = false; //Reiniciar Música
        Instance = this;

        BaseUnit persistentHero = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);

        if (persistentHero != null)
        {
            GameManager.character = persistentHero.UnitName;
            //configuration.Heroes[0].prefab = persistentHero;
            Heroes.Add(persistentHero);
            Heroes[0] = persistentHero;
            hasPersist = true;
        }
        var c = 0;
        if (SecondPlayer)
        {
            c++;
        }
        //Referencio los heroes del data maanger y los guardo en la Lista Heroes
        foreach (ConfigurationData.UnitData unit in configuration.Heroes)
        {
            BaseUnit hero = unit.prefab;
            SetHeroUnit(hero);
            if(c == 0)
            {
                break;
            }
        }

        //Referencio los enemigos del data maanger y los guardo en la Lista Enemies
        foreach (ConfigurationData.UnitData unit in configuration.Enemies)
        {
            BaseUnit enemy = unit.prefab;
            SetEnemyUnit(enemy);
        }
        SetAttackDictionary();

        //Mando = InputSystem.GetDevice<Gamepad>();

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
            SetAttacks(hero);
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

    void ContarKPIAtaque(BaseUnit unit, int i)
    {
        //si es druida
        if (unit.UnitName == "Druid")
        {
            if (i == 0)
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


    public void ShowPrecast(BaseUnit unit, int i, int player)
    {
        if (unit == null || unit.Attacks[i] == null) return;

        if (Ataque.TryGetValue((int)unit.Attacks[i].type, out Ataques att))
        {
            var target = att(unit, unit.Attacks[i]);
            PrecastAppear(target, player);
            currentPrecastIndex[player] = i; // Guarda el ataque actual
        }
        else
        {
            Debug.Log("Acci�n no encontrada");
        }
    }


    public void CanCastAttack(BaseUnit unit, int i)
    {
        if (unit == null) return;

        if (unit.CastMana - unit.Attacks[i].ManaCost > 0 && Time.time - unit.Attacks[i].LastCast1 >= unit.Attacks[i].CoolDown)
        {
            var checkInvo = false;
            if (unit.Attacks[i].type == BaseAttack.AttType.invocacion)
            {
                foreach (BaseUnit invocacion in Invocaciones)
                {
                    if (invocacion != null && invocacion.UnitName == unit.Attacks[i].invocacion.UnitName)
                    {
                        checkInvo = true;
                    }
                }
            }

            if (checkInvo)
            {
                //no hacer nada                
            }
            else
            {
                ContarKPIInteracciones();
                ContarKPIAtaque(unit, i);

                unit.CastMana -= unit.Attacks[i].ManaCost;
                StartCoroutine(ui.HandleCooldown(i));
                CastAttack(unit, unit.Attacks[i]);
                unit.Attacks[i].LastCast1 = Time.time;


                if (unit.animator != null)
                {
                    unit.animator.SetTrigger("Attack");
                }
                //sound effects de cada ataque
                if (unit.Attacks[i].type == BaseAttack.AttType.muro)
                {
                    SoundManager.PlayFirewallDruid();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.parry)
                {
                    SoundManager.PlayParryRobot();
                }
                if (unit.Attacks[i].type == BaseAttack.AttType.area)
                {
                    if (unit.Attacks[i].DoVeneno > 0)
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

            if (Invocaciones.Contains(unit))
            {
                unit.Health = 0;
                Invocaciones.Remove(unit);
                //Debug.Log(Invocaciones.Count);
                var anim = unit.GetComponentInChildren<Animator>();
                anim.SetTrigger("Death");
                StartCoroutine(KillInvo(unit));
            }
        }
        else if (Ataque.TryGetValue((int)unit.Attacks[i].type, out Ataques att))
        {
            var target = att(unit, unit.Attacks[i]);
            PrecastDelete(target);
        }
    }

    public IEnumerator KillInvo(BaseUnit unit)
    {
        yield return new WaitForSeconds(2f);
        unit.Destroy();
    }
    public delegate List<Tile> Ataques(BaseUnit unit, BaseAttack attack);

    public Dictionary<int, Ataques> Ataque = new Dictionary<int, Ataques>();

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

        //nueva tanda de habilidades
        Ataque[13] = AreaMelee2x3;
        Ataque[14] = Fila3x1;
        Ataque[15] = Gancho;
        Ataque[16] = AreaLast2Columns;
        Ataque[17] = ArearDelay;
        Ataque[18] = AreaMelee1x3;
        Ataque[19] = AreaGlobal;
        Ataque[20] = Atractor;
        Ataque[21] = BarridoFilaInverso;
        Ataque[22] = BarridoColumnaInverso;
    }

    public List<Tile> BarridoColumnaInverso(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        var rh = UnityEngine.Random.Range(0, Heroes.Count);
        var r = Heroes[rh].OccupiedTile.x;
        var dir = UnityEngine.Random.Range(0, 2);
        if (dir == 0)
        {
            for (int i = 0; i < GridManager.Instance._height; i++)
            {
                var tile = GridManager.Instance.GetTileAtPosition(new Vector2(r, i));
                AgregarSiNoNull(target, tile);
            }
        }
        if (dir == 1)
        {
            for (int i = GridManager.Instance._height - 1; i >= 0; i--)
            {
                var tile = GridManager.Instance.GetTileAtPosition(new Vector2(r, i));
                AgregarSiNoNull(target, tile);
            }
        }


        return target;
    }
    public List<Tile> BarridoFilaInverso(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        var rh = UnityEngine.Random.Range(0, Heroes.Count);
        var r = Heroes[rh].OccupiedTile.y;
        for (int i = 0; i < GridManager.Instance._width / 2; i++)
        {
            var tile = GridManager.Instance.GetTileAtPosition(new Vector2(i, r));
            AgregarSiNoNull(target, tile);
        }
        return target;
    }
    public IEnumerator Barrer(List<Tile> target, BaseAttack attack)
    {
        for (int i = 0; i < target.Count; i++)
        {
            var auxTarget = new List<Tile>();
            auxTarget.Add(target[i]);
            SetAttacksInTiles(auxTarget, attack);
            yield return new WaitForSeconds(0.7f);
            auxTarget.Clear();
        }
        yield return new WaitForSeconds(0.1f);
    }

    public List<Tile> AreaGlobal(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            var r = UnityEngine.Random.Range(0, Enemies.Count);
            var tile = Enemies[r].OccupiedTile;
            AgregarSiNoNull(target, tile);
            AgregarSiNoNull(target, target[0].UpTile());
            AgregarSiNoNull(target, target[0].DownTile());
            AgregarSiNoNull(target, target[0].LeftTile());
            AgregarSiNoNull(target, target[0].RightTile());

            // Diagonales
            AgregarSiNoNull(target, target[0].UpTile().LeftTile());
            AgregarSiNoNull(target, target[0].UpTile().RightTile());
            AgregarSiNoNull(target, target[0].DownTile().LeftTile());
            AgregarSiNoNull(target, target[0].DownTile().RightTile());
        }
        else if (unit.Faction == Faction.Enemy)
        {
            var r = UnityEngine.Random.Range(0, Heroes.Count);
            var tile = Heroes[r].OccupiedTile;
            AgregarSiNoNull(target, tile);
            AgregarSiNoNull(target, target[0].UpTile());
            AgregarSiNoNull(target, target[0].DownTile());
            AgregarSiNoNull(target, target[0].LeftTile());
            AgregarSiNoNull(target, target[0].RightTile());

            // Diagonales
            AgregarSiNoNull(target, target[0].UpTile().LeftTile());
            AgregarSiNoNull(target, target[0].UpTile().RightTile());
            AgregarSiNoNull(target, target[0].DownTile().LeftTile());
            AgregarSiNoNull(target, target[0].DownTile().RightTile());
        }
        //SetAttacksInTiles(target, attack);
        return target;
    }
    public List<Tile> AreaMelee1x3(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().DownTile());
        }
        else if (unit.Faction == Faction.Enemy)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().DownTile());
        }

        return target;
    }
    public List<Tile> AreaMelee2x3(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().DownTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().RightTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().RightTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().RightTile().DownTile());
        }
        else if (unit.Faction == Faction.Enemy)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().DownTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().LeftTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().LeftTile().UpTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().LeftTile().DownTile());
        }
        return target;
    }

    public List<Tile> Fila3x1(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().RightTile());
            AgregarSiNoNull(target, unit.OccupiedTile.RightTile().RightTile().RightTile());
        }
        else if (unit.Faction == Faction.Enemy)
        {
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().LeftTile());
            AgregarSiNoNull(target, unit.OccupiedTile.LeftTile().LeftTile().LeftTile());
        }

        return target;
    }
    public List<Tile> Atractor(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = AllXTiles(unit);
        return target;
    }

    public void HabilidadAtraer(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        var newtarget = new List<Tile>();
        target = AllXTiles(unit);
        foreach (Tile tile in target)
        {
            newtarget.Add(tile);
            SetAttacksInTiles(newtarget, attack);
            if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction != unit.Faction)
            {
                if (tile.OccupiedUnit.Faction == Faction.Hero)
                {
                    tile.OccupiedUnit.GetHighlightHero()._highlight?.SetActive(false);
                    tile.OccupiedUnit.MoveToXandY(5, tile.PositionYTile());
                }
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                {
                    tile.OccupiedUnit.MoveToXandY(6, tile.PositionYTile());
                }
                break;
            }
            newtarget.Clear();
        }
    }

    public List<Tile> Gancho(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = AllXTiles(unit);
        return target;
    }

    public void HabilidadGancho(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;
        var target = new List<Tile>();
        var newtarget = new List<Tile>();
        target = AllXTiles(unit);
        foreach (Tile tile in target)
        {
            newtarget.Add(tile);
            SetAttacksInTiles(newtarget, attack);
            if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction != unit.Faction)
            {
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Hero)
                {
                    tile.OccupiedUnit.GetHighlightHero()._highlight?.SetActive(false);
                    tile.OccupiedUnit.MoveToXandY(5, tile.PositionYTile());
                    unit.MoveToXandY(6, unit.OccupiedTile.PositionYTile());
                }
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                {
                    tile.OccupiedUnit.MoveToXandY(6, tile.PositionYTile());
                    unit.MoveToXandY(5, unit.OccupiedTile.PositionYTile());
                }

                //tile.OccupiedUnit.MoveToXandY(6, tile.PositionYTile());
                break;
            }
            newtarget.Clear();
        }
    }
    public List<Tile> AreaLast2Columns(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            target.AddRange(AllYTilesInXColumn(GridManager.Instance._width - 1));
            target.AddRange(AllYTilesInXColumn(GridManager.Instance._width - 2));
        }
        else if (unit.Faction == Faction.Enemy)
        {
            target.AddRange(AllYTilesInXColumn(0));
            target.AddRange(AllYTilesInXColumn(1));
        }

        return target;
    }

    public List<Tile> AllYTilesInXColumn(int x)
    {
        var tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, 0));
        if (tile == null) return null;
        var target = new List<Tile>();
        for (int i = 0; i < 6; i++)
        {
            var tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(x, i));
            AgregarSiNoNull(target, tempTile);
        }
        return target;
    }
    public void MoveFront(BaseAttack attack, List<Tile> target)
    {
        if (attack == null) return;
        //var auxtarget = new List<Tile>();
        var units = new List<BaseUnit>();
        //target.AddRange(AllYTilesInXColumn(GridManager.Instance._width - 1));
        //target.AddRange(AllYTilesInXColumn(GridManager.Instance._width - 2));
        SetAttacksInTiles(target, attack);
        foreach (Tile tile in target)
        {
            if (tile.OccupiedUnit != null)
            {
                if (tile.OccupiedUnit.Faction != attack.Faction)
                {
                    if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Hero)
                    {
                        tile.OccupiedUnit.GetHighlightHero()._highlight?.SetActive(false);
                    }
                    attack.DoDamage(tile.OccupiedUnit);
                }
                units.Add(tile.OccupiedUnit);
            }
        }
        foreach (BaseUnit unit in units)
        {
            if (unit.Faction == Faction.Hero)
            {
                unit.MoveToXandY(5, unit.OccupiedTile.y);
            }
            else if (unit.Faction == Faction.Enemy)
            {
                unit.MoveToXandY(6, unit.OccupiedTile.y);
                //Debug.Log("debi moverme wtf");
            }
        }
    }

    public List<Tile> ArearDelay(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            AgregarSiNoNull(target, unit.GetHighlightHero());
        }
        else if (unit.Faction == Faction.Enemy)
        {
            AgregarSiNoNull(target, unit.GetHighlightEnemy());
        }

        return target;
    }

    public IEnumerator ExtraAttack(List<Tile> target, BaseAttack attack)
    {
        var extraTarget = new List<Tile>();
        //agrego el ataque extra
        AgregarSiNoNull(extraTarget, target[0].RightTile());
        AgregarSiNoNull(extraTarget, target[0].LeftTile());
        AgregarSiNoNull(extraTarget, target[0].UpTile());
        AgregarSiNoNull(extraTarget, target[0].DownTile());
        AgregarSiNoNull(extraTarget, target[0].DownTile().RightTile());
        AgregarSiNoNull(extraTarget, target[0].UpTile().RightTile());
        AgregarSiNoNull(extraTarget, target[0].DownTile().LeftTile());
        AgregarSiNoNull(extraTarget, target[0].UpTile().LeftTile());

        PrecastAppear(extraTarget, -1);

        yield return new WaitForSeconds(0.7f);

        PrecastDelete(extraTarget);
        SetAttacksInTiles(extraTarget, attack);
    }


    public void CastAttack(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return;

        if (Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            var target = att(unit, attack);
            if (target != null)
            {
                if (attack.type == BaseAttack.AttType.dashMelee)
                {
                    StartCoroutine(TeleportMeleeDash(unit, attack, unit.OccupiedTile));
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.parry)
                {
                    StartCoroutine(ActivateParry(unit));
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.gancho)
                {
                    HabilidadGancho(unit, attack);
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.atractor)
                {
                    HabilidadAtraer(unit, attack);
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.areaLast2Columns)
                {
                    MoveFront(attack, target);
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.barridofilainverso)
                {
                    StartCoroutine(Barrer(target, attack));
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.barridocolumnainverso)
                {
                    StartCoroutine(Barrer(target, attack));
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.areadelay)
                {
                    SetAttacksInTiles(target, attack);
                    StartCoroutine(ExtraAttack(target, attack.ExtraAttack));
                    PrecastDelete(target);
                }
                else if (attack.type == BaseAttack.AttType.invocacion)
                {
                    if (Invocaciones.Count <= 1)
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
                    PrecastDelete(target);
                }
                else
                {
                    SetAttacksInTiles(target, attack);
                    PrecastDelete(target);
                }
            }
        }
        else
        {
            Debug.Log("Acci�n no encontrada");
        }
    }


    public List<Tile> Proyectil(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var tile = unit.OccupiedTile.RightTile();
        var tempAttack = Instantiate(attack, new Vector3(tile.x, tile.y, -1), Quaternion.identity);
        tempAttack.gameObject.SetActive(true);
        tile.SetAttack(tempAttack);
        AttacksinPlay.Add(tempAttack);
        return null;
    }
    public List<Tile> Cast(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target.Add(unit.GetHighlightHero());
        var tempAttack = Instantiate(attack, new Vector3(target[0].x, target[0].y, -1), Quaternion.identity);
        tempAttack.gameObject.SetActive(true);
        target[0].SetAttack(tempAttack);
        StartCoroutine(Destruir(tempAttack));
        return target;
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
            if (attack.DoBurn > 0)
            {
                tile.StartCoroutineBurning(att.DoBurn);
            }
        }
    }
    public List<Tile> Area(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        if (unit.Faction == Faction.Hero)
        {
            target.Add(unit.GetHighlightHero());
        }
        else
        {
            target.Add(unit.GetHighlightEnemy());
        }
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

        //SetAttacksInTiles(target, attack);
        return target;
    }
    //Funcion para ver si una tile es nula o agregarla a una lista
    public void AgregarSiNoNull(List<Tile> lista, Tile tile)
    {
        if (tile != null)
            lista.Add(tile);
    }

    public List<Tile> Muro(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = AllYTiles(unit.GetHighlightHero());

        //SetAttacksInTiles(target, attack);
        return target;
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
    public List<Tile> Fila(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = AllXTiles(unit);
        //SetAttacksInTiles(target, attack);
        return target;
    }

    //Funcion para conseguir todas las tiles de una fila a la derecha del heroe
    public List<Tile> AllXTiles(BaseUnit unit)
    {
        if (unit == null) return null;
        var tile = unit.OccupiedTile;
        if (tile == null) return null;
        var target = new List<Tile>();
        var Gmanager = GridManager.Instance;
        if (unit.Faction == Faction.Hero)
        {
            for (int i = tile.x + 1; i <= Gmanager._width - 1; i++)
            {
                var tempTile = Gmanager.GetTileAtPosition(new Vector2(i, tile.y));
                AgregarSiNoNull(target, tempTile);
            }
        }
        else if (unit.Faction == Faction.Enemy)
        {
            for (int i = tile.x - 1; i >= 0; i--)
            {
                var tempTile = Gmanager.GetTileAtPosition(new Vector2(i, tile.y));
                AgregarSiNoNull(target, tempTile);
            }
        }

        return target;
    }
    public List<Tile> AllEnemiesDamage(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        foreach (BaseUnit enemy in Enemies)
        {
            var tempTile = enemy.OccupiedTile;
            AgregarSiNoNull(target, tempTile);
        }
        //SetAttacksInTiles(target, attack);
        return target;
    }

    public List<Tile> AllHerosDamage(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        foreach (BaseUnit hero in Heroes)
        {
            var tempTile = hero.OccupiedTile;
            AgregarSiNoNull(target, tempTile);
        }
        //SetAttacksInTiles(target, attack);
        return target;
    }

    public List<Tile> RandomCast(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        // no se definir cuantos randoms van a hacer, queda pendiente
        if (unit.Faction == Faction.Enemy)
        {
            for (int i = 0; i < attack.numRandomTiles; i++)
            {
                var rx = UnityEngine.Random.Range(0, GridManager.Instance._width / 2);
                var ry = UnityEngine.Random.Range(0, GridManager.Instance._height);
                var tile = GridManager.Instance.GetTileAtPosition(new Vector2(rx, ry));
                if (!target.Contains(tile))
                {
                    AgregarSiNoNull(target, tile);
                }
                else
                {
                    i--;
                }

            }
        }
        else if (unit.Faction == Faction.Hero)
        {
            for (int i = 0; i < attack.numRandomTiles; i++)
            {
                var rx = UnityEngine.Random.Range(6, GridManager.Instance._width);
                var ry = UnityEngine.Random.Range(0, GridManager.Instance._height);
                var tile = GridManager.Instance.GetTileAtPosition(new Vector2(rx, ry));
                if (!target.Contains(tile))
                {
                    AgregarSiNoNull(target, tile);
                }
                else
                {
                    i--;
                }

            }
        }

        return target;
    }
    public List<Tile> Melee(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        AgregarSiNoNull(target, unit.OccupiedTile.RightTile());
        //SetAttacksInTiles(target, attack);
        return target;
    }



    public List<Tile> Invocacion(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target.Add(unit.OccupiedTile.LeftTile());
        //if(Invocaciones.Count <= 1)
        //{
        //    if (Invocaciones.Count == 0)
        //    {
        //        InstanciarInvocacion(attack, target);
        //    }
        //    else if (Invocaciones[0] != null && Invocaciones[0].UnitName != attack.invocacion.UnitName)
        //    {
        //        InstanciarInvocacion(attack, target);
        //    }

        //}
        return target;
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
        StartCoroutine(AtaqueInvocaciones(invocacion));
    }

    public IEnumerator AtaqueInvocaciones(BaseUnit inovocaciones)
    {
        if (inovocaciones == null || inovocaciones.Attacks[0] == null)
        {
            yield break;
        }
        while (!MayCastAttack(inovocaciones, inovocaciones.Attacks[0]))
        {
            yield return new WaitForSeconds(0.1f);
        }
        CanCastAttack(inovocaciones, 0);
        yield return new WaitForSeconds(0.1f);
    }

    public void AtaqueInvoacion()
    {
        ////llamar corrutina si se puede
        //if(corrutinaInvocaciones == null && Invocaciones.Count < 0)
        //{
        //    corrutinaInvocaciones = StartCoroutine(AtaqueInvocaciones(Invocaciones[0]));
        //}
        //f(corrutinaInvocaciones2 == null && Invocaciones.Count > 1)
        //{
        //    corrutinaInvocaciones2 = StartCoroutine(AtaqueInvocaciones(Invocaciones[0]));
        //}
    }
    public List<Tile> DashMelee(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = AllXTiles(unit);
        //AgregarSiNoNull(target, unit.GetHighlightHero());
        //StartCoroutine(TeleportMeleeDash(unit, attack, unit.OccupiedTile));
        return target;
    }

    public IEnumerator TeleportMeleeDash(BaseUnit unit, BaseAttack attack, Tile baseTile)
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
        if (nextTile == null || nextTile.OccupiedUnit != null)
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

    public IEnumerator OccupiedTile(Tile tile, BaseUnit unit)
    {
        if (tile != null)
        {
            tile.OccupiedUnit = unit;
            if (tile.OccupiedAttack != null)
            {
                tile.OccupiedAttack.DoDamage(unit);
                tile.OccupiedAttack.Destroy();
            }
        }

        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator DeOccupiedTile(Tile tile)
    {
        tile.OccupiedUnit = null;
        yield return new WaitForSeconds(0.1f);
    }
    public List<Tile> Parry(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        //StartCoroutine(ActivateParry(unit));
        var target = new List<Tile>();
        target.Add(unit.OccupiedTile);
        //algo visaul del parry, una corrutina con while true depronto y que al finalizar activate parry se detenga
        return target;
    }

    public IEnumerator ActivateParry(BaseUnit unit)
    {
        unit.parry = true;
        yield return new WaitForSeconds(2);
        unit.parry = false;
    }
    public List<Tile> CambiarFaccion(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return null;
        var target = new List<Tile>();
        target = PrimeraColumnaEnemiga(target);
        StartCoroutine(CambiarFactionToHero(target));
        return target;
    }

    public IEnumerator CambiarFactionToHero(List<Tile> target)
    {
        foreach (Tile tile in target)
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
        for (int i = 0; i < 6; i++)
        {
            var tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(GridManager.Instance._width / 2, i));
            AgregarSiNoNull(target, tempTile);
        }
        return target;
    }

    bool MayCastAttack(BaseUnit unit, BaseAttack attack)
    {
        if (unit == null || attack == null) return false;

        if (unit.CastMana - attack.ManaCost > 0 && Time.time - attack.LastCast1 >= attack.CoolDown)
        {
            if (attack.type == BaseAttack.AttType.invocacion)
            {
                foreach (BaseUnit invocacion in Invocaciones)
                {
                    if (invocacion.UnitName == attack.invocacion.UnitName)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public void AttackHero2(BaseUnit unit, int player)
    {
        if (unit == null) return;


        if (player == 0)
        {
            if ((Input.GetKey(KeyCode.I) || (Mando != null && Mando.buttonSouth.wasPressedThisFrame)) && unit.Attacks.Length > 0 && MayCastAttack(unit, unit.Attacks[0]))
            {
                //CanCastAttack(unit, 0);
                ShowPrecast(unit, 0, player);
            }
            if ((Input.GetKey(KeyCode.J) || (Mando != null && Mando.buttonNorth.wasPressedThisFrame)) && unit.Attacks.Length > 1 && MayCastAttack(unit, unit.Attacks[1]))
            {
                //CanCastAttack(unit, 1);
                ShowPrecast(unit, 1, player);
            }
            if ((Input.GetKey(KeyCode.K) || (Mando != null && Mando.buttonEast.wasPressedThisFrame)) && unit.Attacks.Length > 2 && MayCastAttack(unit, unit.Attacks[2]))
            {
                //CanCastAttack(unit, 2);
                ShowPrecast(unit, 2, player);
            }
            if ((Input.GetKey(KeyCode.L) || (Mando != null && Mando.buttonWest.wasPressedThisFrame)) && unit.Attacks.Length > 3 && MayCastAttack(unit, unit.Attacks[3]))
            {
                //CanCastAttack(unit, 3);
                ShowPrecast(unit, 3, player);
            }
            if ((Input.GetKey(KeyCode.O) || (Mando != null && Mando.rightTrigger.wasPressedThisFrame)) && unit.Attacks.Length > 4 && MayCastAttack(unit, unit.Attacks[4]))
            {
                //CanCastAttack(unit, 4);
                ShowPrecast(unit, 4, player);
            }

            //lo de arriba sera le precast y este de abajo el cast

            if ((Input.GetKeyUp(KeyCode.I) || (Mando != null && Mando.buttonSouth.wasReleasedThisFrame)) && unit.Attacks.Length > 0)
            {
                CanCastAttack(unit, 0);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.J) || (Mando != null && Mando.buttonNorth.wasReleasedThisFrame)) && unit.Attacks.Length > 1)
            {
                CanCastAttack(unit, 1);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.K) || (Mando != null && Mando.buttonEast.wasReleasedThisFrame)) && unit.Attacks.Length > 2)
            {
                CanCastAttack(unit, 2);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.L) || (Mando != null && Mando.buttonWest.wasReleasedThisFrame)) && unit.Attacks.Length > 3)
            {
                CanCastAttack(unit, 3);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.O) || (Mando != null && Mando.rightTrigger.wasReleasedThisFrame)) && unit.Attacks.Length > 4)
            {
                CanCastAttack(unit, 4);
                currentPrecastIndex[0] = -1;
            }
        }
        if (player == 1)
        {
            if ((Input.GetKey(KeyCode.I) || (Mando2 != null && Mando2.buttonSouth.wasPressedThisFrame)) && unit.Attacks.Length > 0 && MayCastAttack(unit, unit.Attacks[0]))
            {
                //CanCastAttack(unit, 0);
                ShowPrecast(unit, 0, player);
            }
            if ((Input.GetKey(KeyCode.J) || (Mando2 != null && Mando2.buttonNorth.wasPressedThisFrame)) && unit.Attacks.Length > 1 && MayCastAttack(unit, unit.Attacks[1]))
            {
                //CanCastAttack(unit, 1);
                ShowPrecast(unit, 1, player);
            }
            if ((Input.GetKey(KeyCode.K) || (Mando2 != null && Mando2.buttonEast.wasPressedThisFrame)) && unit.Attacks.Length > 2 && MayCastAttack(unit, unit.Attacks[2]))
            {
                //CanCastAttack(unit, 2);
                ShowPrecast(unit, 2, player);
            }
            if ((Input.GetKey(KeyCode.L) || (Mando2 != null && Mando2.buttonWest.wasPressedThisFrame)) && unit.Attacks.Length > 3 && MayCastAttack(unit, unit.Attacks[3]))
            {
                //CanCastAttack(unit, 3);
                ShowPrecast(unit, 3, player);
            }
            if ((Input.GetKey(KeyCode.O) || (Mando2 != null && Mando2.rightTrigger.wasPressedThisFrame)) && unit.Attacks.Length > 4 && MayCastAttack(unit, unit.Attacks[4]))
            {
                //CanCastAttack(unit, 4);
                ShowPrecast(unit, 4, player);
            }

            //lo de arriba sera le precast y este de abajo el cast

            if ((Input.GetKeyUp(KeyCode.G) || (Mando2 != null && Mando2.buttonSouth.wasReleasedThisFrame)) && unit.Attacks.Length > 0)
            {
                CanCastAttack(unit, 0);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.V) || (Mando2 != null && Mando2.buttonNorth.wasReleasedThisFrame)) && unit.Attacks.Length > 1)
            {
                CanCastAttack(unit, 1);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.B) || (Mando2 != null && Mando2.buttonEast.wasReleasedThisFrame)) && unit.Attacks.Length > 2)
            {
                CanCastAttack(unit, 2);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.N) || (Mando2 != null && Mando2.buttonWest.wasReleasedThisFrame)) && unit.Attacks.Length > 3)
            {
                CanCastAttack(unit, 3);
                currentPrecastIndex[0] = -1;
            }
            if ((Input.GetKeyUp(KeyCode.H) || (Mando2 != null && Mando2.rightTrigger.wasReleasedThisFrame)) && unit.Attacks.Length > 4)
            {
                CanCastAttack(unit, 4);
                currentPrecastIndex[0] = -1;
            }
        }
    }

    public void PrecastAppear(List<Tile> target, int player)
    {
        if (player > -1)
        {
            PrecastDelete(currentPrecasts[player]);
            currentPrecasts[player] = target;
        }

        foreach (Tile tile in target)
        {
            tile._precast.SetActive(true);
        }
    }
    public void PrecastDelete(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(false);
        }
    }

    public IEnumerator AttackMove()
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

        bool moved = false;

        if (player == 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || (Mando != null && Mando.dpad.up.wasPressedThisFrame))
            {
                CanMove(hero, 0);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.A) || (Mando != null && Mando.dpad.left.wasPressedThisFrame))
            {
                CanMove(hero, 1);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.S) || (Mando != null && Mando.dpad.down.wasPressedThisFrame))
            {
                CanMove(hero, 2);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.D) || (Mando != null && Mando.dpad.right.wasPressedThisFrame))
            {
                CanMove(hero, 3);
                moved = true;
            }
        }

        if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || (Mando2 != null && Mando2.dpad.up.wasPressedThisFrame))
            {
                CanMove(hero, 0);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Mando2 != null && Mando2.dpad.left.wasPressedThisFrame))
            {
                CanMove(hero, 1);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || (Mando2 != null && Mando2.dpad.down.wasPressedThisFrame))
            {
                CanMove(hero, 2);
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || (Mando2 != null && Mando2.dpad.right.wasPressedThisFrame))
            {
                CanMove(hero, 3);
                moved = true;
            }
        }

        if (moved)
        {
            PrecastDelete(currentPrecasts[player]);

            // Si hab�a un ataque precasteado, lo volvemos a mostrar
            if (currentPrecastIndex[player] != -1)
            {
                ShowPrecast(hero, currentPrecastIndex[player], player);
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
            if (unit.MoveCooldown > 0)
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
        if (unit.MoveCooldown > 0)
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
    public IEnumerator MoverEnemigo()
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

    public IEnumerator RestoreMana(BaseUnit hero)
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
            if (SecondPlayer && Heroes[1] != null)
            {
                MoveHero(Heroes[1], 1);
                AttackHero2(Heroes[1], 1);
            }
            //TrueHighlight(Heroes[0]);
            //StartCoroutine(AttackMove());
            TakeDamage();
            AtaqueInvoacion();

            //Agregue el hasChangedMusic porque me llamaba los eventos de wwise cada frame y me generaba distorsión
            if (SoundManager != null && Heroes[0] != null && Heroes[0].Health < Heroes[0].MaxHealth / 2 && !hasChangedMusic)
            {
                SoundManager.ChangeSoundtrackToMidLifeMode();
                hasChangedMusic = true; // Marcamos que ya hicimos el cambio
            }
        }

    }

    //codigo para tirar un poder especial



    public IEnumerator Destruir(BaseAttack att)
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
                var anim = unit.GetComponentInChildren<Animator>();
                anim.SetTrigger("Death");
                StartCoroutine(KillInvo(unit));
                //unit.Destroy();
            }else if (unit.Health <= 0 && unit.Faction == Faction.Hero && !Invocaciones.Contains(unit))
            {
                Heroes.Remove(unit);
                //unit.Destroy();
                if (Heroes.Count == 0)
                {
                    GameManager.Instance.ChangeState(GameState.EndFight);
                }
            }
            if (unit.Health <= 0 && unit.Faction == Faction.Enemy)
            {
                Enemies.Remove(unit);
                var anim = unit.GetComponentInChildren<Animator>();
                anim.SetTrigger("Death");
                //unit.Destroy();
                var ia = unit.GetComponent<BehaviorGraphAgent>();
                ia.enabled = false;
            }

        }
        if (Enemies.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.EndFight);
        }
    }




    public IEnumerator RestoreStamina(BaseUnit hero)
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
