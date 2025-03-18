using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Rendering; // Necesario para Task.Delay
using UnityEngine.InputSystem;
using CotA.Configuration;
using System.Reflection;
//using System;


public class UnitManager : MonoBehaviour
{
    public ConfigurationData configuration;

    public static UnitManager Instance;
    private List<BaseUnit> _heroes = new List<BaseUnit>();
    private BaseUnit _enemies;
    private ScriptableAttack _attack;
    private ScriptableAttack _attackE;
    public List<ScriptableAttack> _attackS = new List<ScriptableAttack>();
    public List<BaseUnit> AllUnits = new List<BaseUnit>();

    [SerializeField] private float pobAtt = 50;
    [SerializeField] private int HealthEnemy = 0;
    [SerializeField] private int NumEnemies = 1;

    //[SerializeField] private int DmgAttHero = 0;
    [SerializeField] private int HealthHero = 1;

    public ObjectPool poolHero1;
    public ObjectPool poolHero2;
    public ObjectPool poolEnemies;

    private List<int> movimientos = new List<int>();
    private List<int> movimientos2 = new List<int>();


    //private BaseUnit Hero1;
    //private BaseUnit Hero2;
    private List<BaseUnit> Heroes = new List<BaseUnit>();

    private List<BaseUnit> Enemies = new List<BaseUnit>();
    public List<BaseAttack> Attacks = new List<BaseAttack>();

    private float tiempoUltimaEjecucion = 1f;
    private float tiempoUltimaEjecucion2 = 1f;
    private float tiempoUltimaEjecucion3 = 0.5f;
    private float tiempoUltimaEjecucion4 = 0.5f;

    public bool CanPlay = false;

    public float TimeMoveEne = 2f;
    public float TimeAttEne = 0.5f;
    public float TimeMoveHero = 0.5f;
    public float TimeVeneno = 1f;

    public bool SecondPlayer = false;

    //public float lastAtt1 = -100f;
    //public float lastAtt2 = -100f;

    Gamepad Mando = null;

    void Awake()
    {
        Instance = this;
        
        TimeMoveEne = configuration.setup1.TimeMoveEne;
        TimeAttEne = configuration.setup1.TimeAttEne;
        TimeMoveHero = configuration.setup1.TimeMoveHero;
        TimeVeneno = configuration.setup1.TimeVeneno;

        foreach (ConfigurationData.AttData  att in configuration.basickAttack)
        {
            att.prefab.Damage = att.Damage;
            att.prefab.DoVeneno = att.DoVeneno;
            att.prefab.CoolDown = att.Cooldown;
            att.prefab.AreaOfEffect = att.AreaOfEffect;
            att.prefab.LastCast1 = -100;
        }

        //Busco todos los prefabs que necesito

        //_heroes = new List<ScriptableUnit>(Resources.LoadAll<ScriptableUnit>("Units/Heroes"));
        _heroes.Add(configuration.nina.prefab);
        _heroes.Add(configuration.robot.prefab);

        //_enemies = Resources.Load<ScriptableUnit>("Units/Enemies/Enemy1");
        _enemies = configuration.basicEnemy.prefab;
        _attack = Resources.Load<ScriptableAttack>("Units/Attacks/BasicAttackHero");
        _attackE = Resources.Load<ScriptableAttack>("Units/Attacks/BasicAttackEnemy");

        //_attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack1"));
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack1"));
        //_attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack2"));
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack2"));
        //_attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack3"));
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack3"));


        //Seteo que este tiempo no demore nada para que pueda tirar poderes inemdiatamente
        foreach (ScriptableAttack attack in _attackS)
        {
            attack.AttackPrefab.LastCast1 = -100f;
            attack.AttackPrefab.LastCast2 = -100f;
        }
         //los tiempos de cada cuanto puede pasar cierta corrutina
        tiempoUltimaEjecucion = Time.time;
        tiempoUltimaEjecucion2 = Time.time;
        tiempoUltimaEjecucion3 = Time.time;
        //la formula inicial para calcular las velocidades de los enemigos y proyectiles
        //TimeMoveEne = 1 / velEnemy;
        //TimeAttEne = 1 / velAttack;

        //obtengo als variales para las vidas de heroes y enemigos y poder mostarlas encima de ellos
        
        _enemies.MaxHealth = HealthEnemy;

       
        _heroes[0].MaxHealth = HealthHero;

        Mando = InputSystem.GetDevice<Gamepad>();
        //Debug.Log(Mando);

        foreach(BaseHero hero in _heroes)
        {
            foreach(BaseAttack att in hero.Attacks)
            {
                att.LastCast1 = -100;
                att.LastCast2 = -100;
            }
            
        }
    }

    //codigo que hace aparecer los personajes
    public void SpawnHeroes()
    {
        var heroCount = 1;
        if (SecondPlayer)
        {
            heroCount++;
        }

        for (int i = 0; i < heroCount; i++)
        {
            var heroPrefab = _heroes[i];
            var Hero1 = Instantiate(heroPrefab,Vector3.zero, Quaternion.identity);
            var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(0,0));
            while (randomSpawnTile.OccupiedUnit != null)
            {
                randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(Random.Range(0, 5), Random.Range(0, 5)));
            }
            Heroes.Add(Hero1);
            AllUnits.Add(Hero1);

            randomSpawnTile.SetUnit(Hero1);
        }
        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    //codigo que hace aparecer los enemigos
    public void SpawnEnemies()
    {
        var enemyCount = NumEnemies;

        for (int i = 0; i < enemyCount; i++)
        {
            
            var enemyPrefab = _enemies;
            var enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(8, 3));
            while (randomSpawnTile.OccupiedUnit != null)
            {
                randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(Random.Range(6, 11), Random.Range(0, 5)));
            }

            randomSpawnTile.SetUnit(enemy);
            if(i == 2)
            {
                enemy.EnemyType.Special = true;
            }
            else
            {
                enemy.EnemyType.Special = false;
            }
            Enemies.Add(enemy);
            AllUnits.Add(enemy);
        }
        
        GameManager.Instance.ChangeState(GameState.GenerateUI);
    }

    //public void MoveHeroes()
    //{
    //    //Debug.Log(Hero1.OccupiedTile.y < GridManager.Instance._height);
    //    var newTile = Hero1.OccupiedTile;
    //    var Highlight = GridManager.Instance.GetTileAtPosition(new Vector2(Hero1.OccupiedTile.x + 6, Hero1.OccupiedTile.y));
    //    Highlight._highlight.SetActive(false);



    //    if (Input.GetKeyDown(KeyCode.W) && Hero1.OccupiedTile.y < GridManager.Instance._height - 1)
    //    {
    //        newTile = Hero1.OccupiedTile.UpTile();
    //    }
    //    if (Input.GetKeyDown(KeyCode.A) && Hero1.OccupiedTile.x > 0)
    //    {
    //        newTile = Hero1.OccupiedTile.LeftTile();
    //    }
    //    if (Input.GetKeyDown(KeyCode.S) && Hero1.OccupiedTile.y > 0)
    //    {
    //        newTile = Hero1.OccupiedTile.DownTile();
    //    }
    //    if (Input.GetKeyDown(KeyCode.D) && Hero1.OccupiedTile.x < GridManager.Instance._width/2 - 1)
    //    {
    //        newTile = Hero1.OccupiedTile.RightTile();
    //    }

    //    newTile.SetUnit(Hero1);
    //    Highlight = GridManager.Instance.GetTileAtPosition(new Vector2(Hero1.OccupiedTile.x + 6, Hero1.OccupiedTile.y));
    //    Highlight._highlight.SetActive(true);


    //    //Aqui esta el ataque de heroe, separarlo (ya se separó)
    //    //AttackHero(Highlight);
    //}

    private void AttackHero(BaseUnit hero, int player)
    {

        if(player == 0)
        {
            if ((Input.GetKeyDown(KeyCode.Y) || (Mando != null && Mando.buttonSouth.wasPressedThisFrame)))
            {
                var randomPrefab = _attack.AttackPrefab;
                //var attackSpawned = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
                var attackSpawned = poolHero1.GetObjectInPool();
                attackSpawned.gameObject.SetActive(true);
                var randomSpawnTile = hero.OccupiedTile.RightTile();

                randomSpawnTile.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }

            //ataues especiales de los heroes uno con Q otro con E

            if ((Input.GetKeyDown(KeyCode.U) || (Mando != null && Mando.buttonNorth.wasPressedThisFrame)) && Time.time >= hero.Attacks[0].LastCast1 + _attackS[0].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[0] != null)
                {
                    //SpecialAttack(hero, hero.Attacks[0], hero.GetHighlightHero());
                    SpecialAttack(hero, configuration.basickAttack[0].prefab, hero.GetHighlightHero());
                    hero.Attacks[0].LastCast1 = Time.time;
                }

            }

            if ((Input.GetKeyDown(KeyCode.I) || (Mando != null && Mando.buttonEast.wasPressedThisFrame)) && Time.time >= hero.Attacks[1].LastCast1 + _attackS[1].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[1] != null)
                {
                    SpecialAttack(hero, hero.Attacks[1], hero.GetHighlightHero());
                    hero.Attacks[1].LastCast1 = Time.time;
                }
            }

            if ((Input.GetKeyDown(KeyCode.O) || (Mando != null && Mando.buttonWest.wasPressedThisFrame)) && Time.time >= hero.Attacks[2].LastCast1 + _attackS[1].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[2] != null)
                {
                    SpecialAttack(hero, hero.Attacks[2], hero.GetHighlightHero());
                    hero.Attacks[2].LastCast1 = Time.time;
                }
            }

            //if ((Input.GetKeyDown(KeyCode.P) || (Mando != null && Mando.buttonSouth.ReadValue() > 0)) && Time.time >= hero.Attacks[3].LastCast1 + _attackS[1].AttackPrefab.CoolDown)
            //{
            //    if (hero.Attacks[3] != null)
            //    {
            //        SpecialAttack(hero, hero.Attacks[3], hero.GetHighlightHero());
            //        hero.Attacks[3].LastCast1 = Time.time;
            //    }
            //}
        }

        if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                var randomPrefab = _attack.AttackPrefab;
                var attackSpawned = poolHero2.GetObjectInPool();
                attackSpawned.gameObject.SetActive(true);
                var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(hero.OccupiedTile.x + 1, hero.OccupiedTile.y));

                randomSpawnTile.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }

            //ataues especiales de los heroes uno con Q otro con E

            if (Input.GetKeyDown(KeyCode.V) && Time.time >= _attackS[0].AttackPrefab.LastCast2 + _attackS[0].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[0] != null)
                {
                    SpecialAttack(hero, hero.Attacks[0], hero.GetHighlightHero());
                    hero.Attacks[0].LastCast1 = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.B) && Time.time >= _attackS[1].AttackPrefab.LastCast2 + _attackS[1].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[1] != null)
                {
                    SpecialAttack(hero, hero.Attacks[1], hero.GetHighlightHero());
                    hero.Attacks[1].LastCast1 = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.N) && Time.time >= _attackS[1].AttackPrefab.LastCast2 + _attackS[1].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[2] != null)
                {
                    SpecialAttack(hero, hero.Attacks[2], hero.GetHighlightHero());
                    hero.Attacks[2].LastCast1 = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.M) && Time.time >= _attackS[1].AttackPrefab.LastCast2 + _attackS[1].AttackPrefab.CoolDown)
            {
                if (hero.Attacks[3] != null)
                {
                    SpecialAttack(hero, hero.Attacks[3], hero.GetHighlightHero());
                    hero.Attacks[3].LastCast1 = Time.time;
                }
            }
        }

    }

    //codigo para que los ataques basicos se muevan de por la cuadricula
    IEnumerator AttackMove()
    {
        for(int i = Attacks.Count - 1; i >= 0; i--)
        {
            if( Attacks[i].Faction == Faction.Hero)
            {
                //if (Attacks[i] != null && Attacks[i].OccupiedTile.x >= GridManager.Instance._width - 1)
                //{
                //    //Attacks.Remove(Attacks[i]);
                //    Attacks[i].Destroy();

                //}
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x < GridManager.Instance._width - 2)
                {
                    var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(Attacks[i].OccupiedTile.x + 1, Attacks[i].OccupiedTile.y));
                    nextTile.SetAttack(Attacks[i]);
                }
                else
                {
                    Attacks[i].Destroy();
                }
            }

            if (Attacks[i].Faction == Faction.Enemy)
            {
                //if (Attacks[i] != null && Attacks[i].OccupiedTile.x <= 0)
                //{
                //    //Attacks.Remove(Attacks[i]);
                //    Attacks[i].Destroy();

                //}
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x > 0)
                {
                    var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(Attacks[i].OccupiedTile.x - 1, Attacks[i].OccupiedTile.y));
                    nextTile.SetAttack(Attacks[i]);
                }
                else
                {
                    Attacks[i].Destroy();
                }
            }


        }
        

        yield return new WaitForSeconds(0.5f);
    }

    //primer intento de mover enemigos, NO se esta usando ahorita porque no esta en corrutinas
    //public void MoveEnemies()
    //{
    //    foreach(BaseUnit Enemy1 in Enemies)
    //    {
    //        var randomMove = Random.Range(1, 5);
    //        if (randomMove == 1 && Enemy1.OccupiedTile.y < GridManager.Instance._height - 1)
    //        {
    //            var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y + 1));
    //            newTile.SetUnit(Enemy1);
    //        }
    //        if (randomMove == 2 && Enemy1.OccupiedTile.x > GridManager.Instance._width / 2)
    //        {
    //            var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x - 1, Enemy1.OccupiedTile.y));
    //            newTile.SetUnit(Enemy1);
    //        }
    //        if (randomMove == 3 && Enemy1.OccupiedTile.y > 0)
    //        {
    //            var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y - 1));
    //            newTile.SetUnit(Enemy1);
    //        }
    //        if (randomMove == 4 && Enemy1.OccupiedTile.x < GridManager.Instance._width - 1)
    //        {
    //            var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x + 1, Enemy1.OccupiedTile.y));
    //            newTile.SetUnit(Enemy1);
    //        }

    //        var randomMove2 = Random.Range(0, 6);

    //        if (randomMove2 < 3)
    //        {
    //            Debug.Log("disparo");
    //            //Debug.Log(_attack);
    //            //Debug.Log(_attack.AttackPrefab);
    //            var randomPrefab = _attackE.AttackPrefab;
    //            var attackSpawned = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
    //            var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x - 1, Enemy1.OccupiedTile.y));

    //            randomSpawnTile.SetAttack(attackSpawned);
    //            Attacks.Add(attackSpawned);
    //        }
    //    }
        
    //}


    //version de mover heores pero con con restriccion

    //IEnumerator MoverHeroeSlowOG(BaseUnit hero1, int player)
    //{
    //    if(player == 0)
    //    {
    //        //var newTile = hero1.OccupiedTile;
    //        var mc = movimientos.Count;
    //        var Highlight = hero1.GetHighlightHero();
    //        Highlight._highlight.SetActive(false);


    //        if (mc < 2)
    //        {
    //            if ((Input.GetKeyDown(KeyCode.W) || (Mando != null && Mando.dpad.up.ReadValue() > 0)) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
    //            {
    //                movimientos.Add(0);
    //            }
    //            if ((Input.GetKeyDown(KeyCode.A) || (Mando != null && Mando.dpad.left.ReadValue() > 0)) && hero1.OccupiedTile.x > 0)
    //            {
    //                movimientos.Add(1);
    //            }
    //            if ((Input.GetKeyDown(KeyCode.S) || (Mando != null && Mando.dpad.down.ReadValue() > 0)) && hero1.OccupiedTile.y > 0)
    //            {
    //                movimientos.Add(2);
    //            }
    //            if ((Input.GetKeyDown(KeyCode.D) || (Mando != null && Mando.dpad.right.ReadValue() > 0)) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
    //            {
    //                movimientos.Add(3);
    //            }
    //        }

    //        //newTile.SetUnit(hero1);
    //        Highlight = hero1.GetHighlightHero();
    //        Highlight._highlight.SetActive(true);


    //        //Aqui esta el ataque de heroe, separarlo (ya se separó)
    //        //AttackHero(hero1);
    //    }else if (player == 1)
    //    {
    //        var newTile = hero1.OccupiedTile;
    //        var Highlight = hero1.GetHighlightHero();
    //        Highlight._highlight.SetActive(false);



    //        if (Input.GetKey(KeyCode.UpArrow) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
    //        {
    //            newTile = hero1.OccupiedTile.UpTile();
    //            //Debug.Log("entra arriba?");
    //        }
    //        if (Input.GetKey(KeyCode.LeftArrow) && hero1.OccupiedTile.x > 0)
    //        {
    //            newTile = hero1.OccupiedTile.LeftTile();
    //        }
    //        if (Input.GetKey(KeyCode.DownArrow) && hero1.OccupiedTile.y > 0)
    //        {
    //            newTile = hero1.OccupiedTile.DownTile();
    //        }
    //        if (Input.GetKey(KeyCode.RightArrow) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
    //        {
    //            newTile = hero1.OccupiedTile.RightTile();
    //        }

    //        newTile.SetUnit(hero1);
    //        Highlight = hero1.GetHighlightHero();
    //        Highlight._highlight.SetActive(true);


    //        //Aqui esta el ataque de heroe, separarlo (ya se separó)
    //        //AttackHero(hero1);
    //    }

    //    yield return new WaitForSeconds(2f);
    //}


    public void  MoverHeroeSlow(BaseUnit hero1, int player)
    {
        if (player == 0)
        {
            //var newTile = hero1.OccupiedTile;
            var mc = movimientos.Count;
            var Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(false);


            if (mc == 2)
            {
                if ((Input.GetKeyDown(KeyCode.W) || (Mando != null && Mando.dpad.up.wasPressedThisFrame)) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
                {
                    movimientos[1] = 0;
                }
                if ((Input.GetKeyDown(KeyCode.A) || (Mando != null && Mando.dpad.left.wasPressedThisFrame)) && hero1.OccupiedTile.x > 0)
                {
                    movimientos[1] = 1;
                }
                if ((Input.GetKeyDown(KeyCode.S) || (Mando != null && Mando.dpad.down.wasPressedThisFrame)) && hero1.OccupiedTile.y > 0)
                {
                    movimientos[1] = 2;
                }
                if ((Input.GetKeyDown(KeyCode.D) || (Mando != null && Mando.dpad.right.wasPressedThisFrame)) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
                {
                    movimientos[1] = 3;
                }
            }

            if (mc < 2)
            {
                if ((Input.GetKeyDown(KeyCode.W) || (Mando != null && Mando.dpad.up.wasPressedThisFrame)) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
                {
                    movimientos.Add(0);
                }
                if ((Input.GetKeyDown(KeyCode.A) || (Mando != null && Mando.dpad.left.wasPressedThisFrame)) && hero1.OccupiedTile.x > 0)
                {
                    movimientos.Add(1);
                }
                if ((Input.GetKeyDown(KeyCode.S) || (Mando != null && Mando.dpad.down.wasPressedThisFrame)) && hero1.OccupiedTile.y > 0)
                {
                    movimientos.Add(2);
                }
                if ((Input.GetKeyDown(KeyCode.D) || (Mando != null && Mando.dpad.right.wasPressedThisFrame)) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
                {
                    movimientos.Add(3);
                }
            }



            //newTile.SetUnit(hero1);
            Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(true);


            //Aqui esta el ataque de heroe, separarlo (ya se separó)
            //AttackHero(hero1);
        }
        else if (player == 1)
        {
            //var newTile = hero1.OccupiedTile;
            var mc = movimientos.Count;
            var Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(false);


            if (mc == 2)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || (Mando != null && Mando.dpad.up.ReadValue() > 0)) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
                {
                    movimientos2[1] = 0;
                }
                if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Mando != null && Mando.dpad.left.ReadValue() > 0)) && hero1.OccupiedTile.x > 0)
                {
                    movimientos2[1] = 1;
                }
                if ((Input.GetKeyDown(KeyCode.DownArrow) || (Mando != null && Mando.dpad.down.ReadValue() > 0)) && hero1.OccupiedTile.y > 0)
                {
                    movimientos2[1] = 2;
                }
                if ((Input.GetKeyDown(KeyCode.RightArrow) || (Mando != null && Mando.dpad.right.ReadValue() > 0)) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
                {
                    movimientos2[1] = 3;
                }
            }

            if (mc < 2)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || (Mando != null && Mando.dpad.up.ReadValue() > 0)) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
                {
                    movimientos2.Add(0);
                }
                if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Mando != null && Mando.dpad.left.ReadValue() > 0)) && hero1.OccupiedTile.x > 0)
                {
                    movimientos2.Add(1);
                }
                if ((Input.GetKeyDown(KeyCode.DownArrow) || (Mando != null && Mando.dpad.down.ReadValue() > 0)) && hero1.OccupiedTile.y > 0)
                {
                    movimientos2.Add(2);
                }
                if ((Input.GetKeyDown(KeyCode.RightArrow) || (Mando != null && Mando.dpad.right.ReadValue() > 0)) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
                {
                    movimientos2.Add(3);
                }
            }
            Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(true);


            //Aqui esta el ataque de heroe, separarlo (ya se separó)
            //AttackHero(hero1);
        }

        
    }



    //Codigo de movimiento que SI se esta suando ahorita para mover y atacar del enemigo
    IEnumerator MoverEnemigo()
    {
        foreach(BaseUnit Enemy1 in Enemies)
        {
            var newTile = Enemy1.OccupiedTile;
            //Esto hace que el enemigo se mueva de forma aleatoria
            int randomMove = Random.Range(1, 6);
            //Vector2 nuevaPosicion = new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y);

            if (randomMove == 1 && Enemy1.OccupiedTile.y < GridManager.Instance._height - 1)
            {
                if(Enemy1.OccupiedTile.UpTile().OccupiedUnit == null)
                {
                    newTile = Enemy1.OccupiedTile.UpTile();
                }
                
            }
            else if (randomMove == 2 && Enemy1.OccupiedTile.x > GridManager.Instance._width / 2)
            {
                if (Enemy1.OccupiedTile.LeftTile().OccupiedUnit == null)
                {
                    newTile = Enemy1.OccupiedTile.LeftTile();
                }                
            }
            else if (randomMove == 3 && Enemy1.OccupiedTile.y > 0)
            {
                if (Enemy1.OccupiedTile.DownTile().OccupiedUnit == null)
                {
                    newTile = Enemy1.OccupiedTile.DownTile();
                }
                
            }
            else if (randomMove == 4 && Enemy1.OccupiedTile.x < GridManager.Instance._width - 1)
            {
                if(Enemy1.OccupiedTile.RightTile().OccupiedUnit == null)
                {
                    newTile = Enemy1.OccupiedTile.RightTile();
                }
            }

            
            if (newTile != null)
            {
                newTile.SetUnit(Enemy1);
            }
            

            //Aqui esta codigo de ataque enemigo
            var randomAtt = Random.Range(0, 100);

            if (randomAtt < pobAtt)
            {
                var prefab = _attackE.AttackPrefab;
                var attackSpawned = poolEnemies.GetObjectInPool();
                attackSpawned.gameObject.SetActive(true);
                var spawnTileAtt = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x - 1, Enemy1.OccupiedTile.y));

                spawnTileAtt.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }
            // ataque especial
            if (Enemy1.EnemyType.Special && randomAtt < 5)
            {
                SpecialAttackEnemy(_attackS[1], Enemy1.GetHighlightEnemy());

            }


        }
        yield return new WaitForSeconds(2f);
    }



    public void Update()
    {
        //Debug.Log(CanPlay);
        if (CanPlay)
        {
            if (Time.time - tiempoUltimaEjecucion4 >= TimeVeneno)
            {
                StartCoroutine(VenenoDoDamage());
                tiempoUltimaEjecucion4 = Time.time;
            }
            
            TakeDamage();
            //MoveHeroes();
            MoverHeroeSlow(Heroes[0], 0);
                if (SecondPlayer)
            {
                MoverHeroeSlow(Heroes[1], 1);
            }

            if (Time.time - tiempoUltimaEjecucion >= TimeMoveEne)
            {
                StartCoroutine(MoverEnemigo());
                tiempoUltimaEjecucion = Time.time;
            }
            if (Time.time - tiempoUltimaEjecucion2 >= TimeAttEne)
            {
                StartCoroutine(AttackMove());
                tiempoUltimaEjecucion2 = Time.time;
            }
            if (Time.time - tiempoUltimaEjecucion3 >= TimeMoveHero)
            {
                StartCoroutine(LecturaMovimientos(Heroes[0],0));
                if (SecondPlayer)
                {
                    StartCoroutine(LecturaMovimientos(Heroes[1], 1));
                }
                
                tiempoUltimaEjecucion3 = Time.time;
            }
            AttackHero(Heroes[0], 0);
            if (SecondPlayer)
            {
                AttackHero(Heroes[1], 1);
            }
            //Debug.Log(Mando.dpad.up.ReadValue());
        }
        
    }

    //codigo para tirar un poder especial

    public void SpecialAttack(BaseUnit unit, BaseAttack att, Tile tile)
    {
        if (att != null && tile != null)
        {
            if (att.attackType == 0)
            {
                var area = new List<Tile>();
                var atts = new List<BaseAttack>();
                area.Add(tile);
                if (att.AreaOfEffect == 2)
                {
                    area.Add(tile.DownTile());
                    area.Add(tile.LeftTile());
                    area.Add(tile.LeftTile().DownTile());
                }
                //luego hacer un codigo para pegar en area
                foreach (Tile t in area)
                {
                    if (t != null)
                    {
                        var attackSpawned = Instantiate(att, Vector3.zero, Quaternion.identity);
                        t.SetAttack(attackSpawned);
                        Destruir(attackSpawned);
                    }

                }

                //Destruir(attackSpawned);
            }
            if(att.attackType == 1)
            {
                var area = new List<Tile>();
                for (int i = 1; i < GridManager.Instance._width - unit.OccupiedTile.x; i++)
                {
                    area.Add(GridManager.Instance.GetTileAtPosition(new Vector2(unit.OccupiedTile.x + i, unit.OccupiedTile.y)));
                }
                foreach (Tile t in area)
                {
                    if (t != null)
                    {
                        var attackSpawned = Instantiate(att, Vector3.zero, Quaternion.identity);
                        t.SetAttack(attackSpawned);
                        Destruir(attackSpawned);
                    }

                }
            }
            if(att.attackType == 2)
            {
                var attackSpawned = Instantiate(att, Vector3.zero, Quaternion.identity);
                unit.OccupiedTile.RightTile().SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }
        }
        
    }

    public async void Destruir(BaseAttack att)
    {
        await Task.Delay(500);
        att.Destroy();
    }

    public void TakeDamage()
    {
        foreach (BaseUnit unit in AllUnits)
        {
            
            if (unit.OccupiedTile.OccupiedAttack == null)
            {

            }
            else if (unit.OccupiedTile.OccupiedAttack != null)
            {
                if(unit.Faction != unit.OccupiedTile.OccupiedAttack.Faction)
                {
                    unit.OccupiedTile.OccupiedAttack.DoDamage(unit);
                    unit.OccupiedTile.OccupiedAttack.Destroy();
                }
            }
            if (unit.Health <= 0 && unit.Faction == Faction.Hero)
            {
                unit.Destroy();
                GameManager.Instance.ChangeState(GameState.EndFight);
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

    
    public void SpecialAttackEnemy(ScriptableAttack att, Tile tile)
    {
        if (att != null && tile != null)
        {
            var area = new List<Tile>();
            var atts = new List<BaseAttack>();
            area.Add(tile);
            if (att.AttackPrefab.AreaOfEffect == 2)
            {
                area.Add(tile.DownTile());
                area.Add(tile.RightTile());
                area.Add(tile.RightTile().DownTile());
            }
            //luego hacer un codigo para pegar en area
            foreach (Tile t in area)
            {
                if (t != null)
                {
                    var attackSpawned = Instantiate(att.AttackPrefab, Vector3.zero, Quaternion.identity);
                    t.SetAttack(attackSpawned);
                    Destruir(attackSpawned);
                }

            }
            //Destruir(attackSpawned);
        }
    }

    IEnumerator LecturaMovimientos(BaseUnit hero, int jugador)
    {

            if (jugador == 0)
        {
            var newTile = hero.OccupiedTile;
            if (movimientos.Count != 0)
            {
                if (movimientos[0] == 0 && hero.OccupiedTile.y < GridManager.Instance._height - 1)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.UpTile();
                }
                if (movimientos[0] == 1 && hero.OccupiedTile.x > 0)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.LeftTile();
                }
                if (movimientos[0] == 2 && hero.OccupiedTile.y > 0)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.DownTile();
                }
                if (movimientos[0] == 3 && hero.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.RightTile();
                }
                movimientos.RemoveAt(0);
            }
            newTile.SetUnit(hero);
        }

        if (jugador == 1)
        {
            var newTile = hero.OccupiedTile;
            if (movimientos2.Count != 0)
            {
                if (movimientos2[0] == 0)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.UpTile();
                }
                if (movimientos2[0] == 1)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.LeftTile();
                }
                if (movimientos2[0] == 2)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.DownTile();
                }
                if (movimientos2[0] == 3)
                {
                    hero.GetHighlightHero()._highlight.SetActive(false);
                    newTile = hero.OccupiedTile.RightTile();
                }
                movimientos2.RemoveAt(0);
            }
            newTile.SetUnit(hero);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator VenenoDoDamage()
    {
        foreach (BaseUnit unit in AllUnits)
        {
           unit.VenenoDamage();
        }
        yield return new WaitForSeconds(2f);
    }
}
