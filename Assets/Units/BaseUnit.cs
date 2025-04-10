using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static UnityEngine.UI.CanvasScaler;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public int MaxHealth;
    public int Health;
    public TextMeshPro ActualHeath;
    public BaseEnemy EnemyType;
    public BaseHero HeroType;

    public bool stun = false;

    public GameObject efecto;

    public BaseAttack[] Attacks = new BaseAttack[4];

    public int veneno = 0;
    public int burn = 0;

    public float MaxStamina = 3;
    public float MoveCooldown = 0;
    public float regenStamina = 0;
    public float CastMana = 0;
    public float regenMana = 0;

    public float lastCastBA = 0;

    public Animator animator;

    public bool parry = false;
    public bool isPersistentHero = false;


    public void Awake()
    {
        if (isPersistentHero)
        {
            DontDestroyOnLoad(gameObject);
        }
        Health = MaxHealth;
        this.ActualHeath.text = this.Health + " / " + this.MaxHealth;
        veneno = 0;
        lastCastBA = 0;
        StartCoroutine(Veneno());
        StartCoroutine(Burn());
        StartCoroutine(GetBurn());
        StartCoroutine(CheckTileFaction());
    }

    public void Destroy()
    {
        if (this != null) Destroy(gameObject);
    }

    public Tile GetHighlightHero()
    {
        return this.OccupiedTile.RightTile().RightTile().RightTile().RightTile().RightTile().RightTile();
    }

    public Tile GetHighlightEnemy()
    {
        return this.OccupiedTile.LeftTile().LeftTile().LeftTile().LeftTile().LeftTile().LeftTile();
    }

    public void VenenoDamage()
    {
        if(this == null) return;
        if (veneno > 0)
        {
            Health -= 1;
            ActualHeath.text = Math.Max(Health, 0) + " / " + MaxHealth;
            veneno--;
            efecto?.SetActive(true);
        }
        else
        {
            efecto?.SetActive(false);
        }
    }

    IEnumerator CheckTileFaction()
    {
        while (true)
        {
            ReturnTFaction();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Veneno()
    {
        while (true)
        {
            
            if(veneno >0)
            {
                Health -= 1;
                ActualHeath.text = Math.Max(Health, 0) + " / " + MaxHealth;
                veneno--;
                efecto?.SetActive(true);
                if (veneno <= 0)
                {
                    efecto?.SetActive(false);
                }
            }            
            yield return new WaitForSeconds(2f);
        }        
    }
    IEnumerator Burn()
    {
        while (true)
        {
            if(burn > 0)
            {
                Health -= 1;
                ActualHeath.text = Math.Max(Health, 0) + " / " + MaxHealth;
                burn--;
                efecto?.SetActive(true);
                if (veneno <= 0)
                {
                    efecto?.SetActive(false);
                }
                ComboBurnPoison();
            }            
            yield return new WaitForSeconds(2f);
        }
    }

    public void MoveDown()
    {
        this.OccupiedTile.DownTile().SetUnit(this);
    }

    public void MoveUp()
    {
        this.OccupiedTile.UpTile().SetUnit(this);
    }

    public void Attack()
    {
        //basic attack
        if (Time.time - lastCastBA > Attacks[0].CoolDown)
        {
            UnitManager.Instance.AttackEnemy(this);
            //MauriManager.Instance.AttackEnemy(this);
            lastCastBA = Time.time;
        }
    }
    public void ComboBurnPoison()
    {
        if(burn > 0 && veneno > 0)
        {
            Health -= (int)Math.Floor(veneno * 1.5);
            GetNeighbor(veneno);
            burn = 0;
            veneno = 0;
        }                
    }

    public void GetNeighbor(int ven)
    {
        var target = new List<Tile>();
        UnitManager.Instance.AgregarSiNoNull(target, OccupiedTile.RightTile());
        UnitManager.Instance.AgregarSiNoNull(target, OccupiedTile.LeftTile());
        UnitManager.Instance.AgregarSiNoNull(target, OccupiedTile.UpTile());
        UnitManager.Instance.AgregarSiNoNull(target, OccupiedTile.DownTile());
        DoDamage(ven, target);
    }

    public void DoDamage(int damage, List<Tile> target)
    {
        foreach (var tile in target)
        {
            if(tile != null && tile.OccupiedUnit != null)
            {
                tile.OccupiedUnit.Health -= damage;
                tile.OccupiedUnit.ActualizarHealth();
            }
        }
    }

    public void ActualizarHealth()
    {
        ActualHeath.text = Math.Max(Health, 0) + " / " + MaxHealth;
    }

    IEnumerator GetBurn()
    {
        while (true)
        {
            if(this.OccupiedTile != null && this.OccupiedTile.Burning > 0)
            {
                burn = Math.Max(this.OccupiedTile.Burning, burn);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ReturnTFaction()
    {
        if(OccupiedTile != null)
        {
            if(OccupiedTile.Faction == Faction.Hero && Faction != Faction.Hero)
            {
                OccupiedTile.RightTile().SetUnit(this);
            }
            if (OccupiedTile.Faction == Faction.Enemy && Faction != Faction.Enemy)
            {
                OccupiedTile.LeftTile().SetUnit(this);
            }
        }
    }
}
