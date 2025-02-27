using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Rendering; // Necesario para Task.Delay


public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private List<ScriptableUnit> _heroes = new List<ScriptableUnit>();
    private ScriptableUnit _enemies;
    private ScriptableAttack _attack;
    private ScriptableAttack _attackE;
    public List<ScriptableAttack> _attackS = new List<ScriptableAttack>();
    public List<BaseUnit> AllUnits = new List<BaseUnit>();
    [SerializeField] private float velEnemy;
    [SerializeField] private float velAttack;
    [SerializeField] private float pobAtt = 50;
    [SerializeField] private int HealthEnemy = 0;
    [SerializeField] private int DmgAttEnemy = 0;
    [SerializeField] private int NumEnemies = 1;

    [SerializeField] private int DmgAttHero = 0;
    [SerializeField] private int HealthHero = 1;


    //private BaseUnit Hero1;
    //private BaseUnit Hero2;
    private List<BaseUnit> Heroes = new List<BaseUnit>();

    private List<BaseUnit> Enemies = new List<BaseUnit>();
    private List<BaseAttack> Attacks = new List<BaseAttack>();

    private float tiempoUltimaEjecucion = 1f;
    private float tiempoUltimaEjecucion2 = 1f;
    private float tiempoUltimaEjecucion3 = 0.5f;

    public bool CanPlay = false;

    public float TimeMoveEne = 2f;
    public float TimeAttEne = 0.5f;
    public float TimeMoveHero = 0.5f;

    public bool SecondPlayer = false;

    //public float lastAtt1 = -100f;
    //public float lastAtt2 = -100f;

    void Awake()
    {
        Instance = this;
        //Busco todos los prefabs que necesito
        _heroes = new List<ScriptableUnit>(Resources.LoadAll<ScriptableUnit>("Units/Heroes"));
        _enemies = Resources.Load<ScriptableUnit>("Units/Enemies/Enemy1");
        _attack = Resources.Load<ScriptableAttack>("Units/Attacks/BasicAttackHero");
        _attackE = Resources.Load<ScriptableAttack>("Units/Attacks/BasicAttackEnemy");
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack1"));
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack2"));
        _attackS.Add(Resources.Load<ScriptableAttack>("Units/Attacks/SpecialAttack3"));


        //Seteo que este tiempo no demore nada para que pueda tirar poderes inemdiatamente
        foreach (ScriptableAttack attack in _attackS)
        {
            attack.AttackPrefab.LastCast = -100f;
        }
         //los tiempos de cada cuanto puede pasar cierta corrutina
        tiempoUltimaEjecucion = Time.time;
        tiempoUltimaEjecucion2 = Time.time;
        tiempoUltimaEjecucion3 = Time.time;
        //la formula inicial para calcular las velocidades de los enemigos y proyectiles
        TimeMoveEne = 1 / velEnemy;
        TimeAttEne = 1 / velAttack;

        //obtengo als variales para las vidas de heroes y enemigos y poder mostarlas encima de ellos
        _attackE.AttackPrefab.Damage = DmgAttEnemy;
        _enemies.UnitPrefab.MaxHealth = HealthEnemy;

        _attack.AttackPrefab.Damage = DmgAttHero;
        _heroes[0].UnitPrefab.MaxHealth = HealthHero;
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
            var heroPrefab = _heroes[i].UnitPrefab;
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
            
            var enemyPrefab = _enemies.UnitPrefab;
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
            if (Input.GetKey(KeyCode.Y))
            {
                var randomPrefab = _attack.AttackPrefab;
                var attackSpawned = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
                var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(hero.OccupiedTile.x + 1, hero.OccupiedTile.y));

                randomSpawnTile.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }

            //ataues especiales de los heroes uno con Q otro con E

            if (Input.GetKey(KeyCode.U) && Time.time >= _attackS[0].AttackPrefab.LastCast + _attackS[0].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[0], hero.GetHighlightHero());
                _attackS[0].AttackPrefab.LastCast = Time.time;

            }

            if (Input.GetKey(KeyCode.I) && Time.time >= _attackS[1].AttackPrefab.LastCast + _attackS[1].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[1], hero.GetHighlightHero());
                _attackS[1].AttackPrefab.LastCast = Time.time;
            }

            if (Input.GetKey(KeyCode.O) && Time.time >= _attackS[1].AttackPrefab.LastCast + _attackS[1].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[2], hero.GetHighlightHero());
                _attackS[2].AttackPrefab.LastCast = Time.time;
            }
        }

        if (player == 1)
        {
            if (Input.GetKey(KeyCode.V))
            {
                var randomPrefab = _attack.AttackPrefab;
                var attackSpawned = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
                var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(hero.OccupiedTile.x + 1, hero.OccupiedTile.y));

                randomSpawnTile.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }

            //ataues especiales de los heroes uno con Q otro con E

            if (Input.GetKey(KeyCode.B) && Time.time >= _attackS[0].AttackPrefab.LastCast + _attackS[0].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[0], hero.GetHighlightHero());
                _attackS[0].AttackPrefab.LastCast = Time.time;

            }

            if (Input.GetKey(KeyCode.N) && Time.time >= _attackS[1].AttackPrefab.LastCast + _attackS[1].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[1], hero.GetHighlightHero());
                _attackS[1].AttackPrefab.LastCast = Time.time;
            }

            if (Input.GetKey(KeyCode.M) && Time.time >= _attackS[1].AttackPrefab.LastCast + _attackS[1].AttackPrefab.CoolDown)
            {
                SpecialAttack(_attackS[2], hero.GetHighlightHero());
                _attackS[2].AttackPrefab.LastCast = Time.time;
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
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x + 1 >= GridManager.Instance._width)
                {
                    //Attacks.Remove(Attacks[i]);
                    Attacks[i].Destroy();

                }
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x < GridManager.Instance._width - 1)
                {
                    var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(Attacks[i].OccupiedTile.x + 1, Attacks[i].OccupiedTile.y));
                    nextTile.SetAttack(Attacks[i]);
                }
            }

            if (Attacks[i].Faction == Faction.Enemy)
            {
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x - 1 < 0)
                {
                    //Attacks.Remove(Attacks[i]);
                    Attacks[i].Destroy();

                }
                if (Attacks[i] != null && Attacks[i].OccupiedTile.x > 0)
                {
                    var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(Attacks[i].OccupiedTile.x - 1, Attacks[i].OccupiedTile.y));
                    nextTile.SetAttack(Attacks[i]);
                }
            }


        }
        

        yield return new WaitForSeconds(0.5f);
    }

    //primer intento de mover enemigos, NO se esta usando ahorita porque no esta en corrutinas
    public void MoveEnemies()
    {
        foreach(BaseUnit Enemy1 in Enemies)
        {
            var randomMove = Random.Range(1, 5);
            if (randomMove == 1 && Enemy1.OccupiedTile.y < GridManager.Instance._height - 1)
            {
                var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y + 1));
                newTile.SetUnit(Enemy1);
            }
            if (randomMove == 2 && Enemy1.OccupiedTile.x > GridManager.Instance._width / 2)
            {
                var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x - 1, Enemy1.OccupiedTile.y));
                newTile.SetUnit(Enemy1);
            }
            if (randomMove == 3 && Enemy1.OccupiedTile.y > 0)
            {
                var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x, Enemy1.OccupiedTile.y - 1));
                newTile.SetUnit(Enemy1);
            }
            if (randomMove == 4 && Enemy1.OccupiedTile.x < GridManager.Instance._width - 1)
            {
                var newTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x + 1, Enemy1.OccupiedTile.y));
                newTile.SetUnit(Enemy1);
            }

            var randomMove2 = Random.Range(0, 6);

            if (randomMove2 < 3)
            {
                Debug.Log("disparo");
                //Debug.Log(_attack);
                //Debug.Log(_attack.AttackPrefab);
                var randomPrefab = _attackE.AttackPrefab;
                var attackSpawned = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
                var randomSpawnTile = GridManager.Instance.GetTileAtPosition(new Vector2(Enemy1.OccupiedTile.x - 1, Enemy1.OccupiedTile.y));

                randomSpawnTile.SetAttack(attackSpawned);
                Attacks.Add(attackSpawned);
            }
        }
        
    }


    //version de mover heores pero con con restriccion

    IEnumerator MoverHeroeSlow(BaseUnit hero1, int player)
    {
        if(player == 0)
        {
            var newTile = hero1.OccupiedTile;
            var Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(false);



            if (Input.GetKey(KeyCode.W) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
            {
                newTile = hero1.OccupiedTile.UpTile();
                //Debug.Log("entra arriba?");
            }
            if (Input.GetKey(KeyCode.A) && hero1.OccupiedTile.x > 0)
            {
                newTile = hero1.OccupiedTile.LeftTile();
            }
            if (Input.GetKey(KeyCode.S) && hero1.OccupiedTile.y > 0)
            {
                newTile = hero1.OccupiedTile.DownTile();
            }
            if (Input.GetKey(KeyCode.D) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
            {
                newTile = hero1.OccupiedTile.RightTile();
            }

            newTile.SetUnit(hero1);
            Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(true);


            //Aqui esta el ataque de heroe, separarlo (ya se separó)
            //AttackHero(hero1);
        }else if (player == 1)
        {
            var newTile = hero1.OccupiedTile;
            var Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(false);



            if (Input.GetKey(KeyCode.UpArrow) && hero1.OccupiedTile.y < GridManager.Instance._height - 1)
            {
                newTile = hero1.OccupiedTile.UpTile();
                //Debug.Log("entra arriba?");
            }
            if (Input.GetKey(KeyCode.LeftArrow) && hero1.OccupiedTile.x > 0)
            {
                newTile = hero1.OccupiedTile.LeftTile();
            }
            if (Input.GetKey(KeyCode.DownArrow) && hero1.OccupiedTile.y > 0)
            {
                newTile = hero1.OccupiedTile.DownTile();
            }
            if (Input.GetKey(KeyCode.RightArrow) && hero1.OccupiedTile.x < GridManager.Instance._width / 2 - 1)
            {
                newTile = hero1.OccupiedTile.RightTile();
            }

            newTile.SetUnit(hero1);
            Highlight = hero1.GetHighlightHero();
            Highlight._highlight.SetActive(true);


            //Aqui esta el ataque de heroe, separarlo (ya se separó)
            //AttackHero(hero1);
        }

        yield return new WaitForSeconds(2f);
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
                    Enemy1.OccupiedTile.LeftTile();
                }                
            }
            else if (randomMove == 3 && Enemy1.OccupiedTile.y > 0)
            {
                if (Enemy1.OccupiedTile.DownTile().OccupiedUnit == null)
                {
                    Enemy1.OccupiedTile.DownTile();
                }
                
            }
            else if (randomMove == 4 && Enemy1.OccupiedTile.x < GridManager.Instance._width - 1)
            {
                if(Enemy1.OccupiedTile.RightTile().OccupiedUnit == null)
                {
                    Enemy1.OccupiedTile.RightTile();
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
                var attackSpawned = Instantiate(prefab, Vector3.zero, Quaternion.identity);
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
            TakeDamage();
            //MoveHeroes();

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
                StartCoroutine(MoverHeroeSlow(Heroes[0],0));
                AttackHero(Heroes[0],0);
                if (SecondPlayer)
                {
                    StartCoroutine(MoverHeroeSlow(Heroes[1], 1));
                    AttackHero(Heroes[1],1);
                }
                tiempoUltimaEjecucion3 = Time.time;
            }
        }
        
    }

    //codigo para tirar un poder especial

    public void SpecialAttack(ScriptableAttack att, Tile tile)
    {
        if (att != null && tile != null)
        {
            var area = new List<Tile>();
            var atts = new List<BaseAttack>();
            area.Add(tile);
            if(att.AttackPrefab.AreaOfEffect == 2)
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
                    var attackSpawned = Instantiate(att.AttackPrefab, Vector3.zero, Quaternion.identity);
                    t.SetAttack(attackSpawned);
                    Destruir(attackSpawned);
                }
                
            }
            
            //Destruir(attackSpawned);
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
                unit.Destroy();
                Enemies.Remove(unit);
                if(Enemies.Count == 0)
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
}
