using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public void Awake()
    {
        Health = MaxHealth;
        this.ActualHeath.text = this.Health + " / " + this.MaxHealth;
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

}
