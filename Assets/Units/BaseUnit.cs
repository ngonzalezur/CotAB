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

    public GameObject efecto;

    public BaseAttack[] Attacks = new BaseAttack[4];

    public int veneno = 0;

    public float MaxStamina = 3;
    public float MoveCooldown = 0;
    public float CastMana = 0;

    public float lastCastBA = 0;

    public Animator animator;

    public void Awake()
    {
        Health = MaxHealth;
        this.ActualHeath.text = this.Health + " / " + this.MaxHealth;
        veneno = 0;
        lastCastBA = 0;
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
            lastCastBA = Time.time;
        }
    }
}
