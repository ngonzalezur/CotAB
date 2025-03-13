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

    public void Awake()
    {
        Health = MaxHealth;
        this.ActualHeath.text = this.Health + " / " + this.MaxHealth;
        veneno = 0;
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
}
