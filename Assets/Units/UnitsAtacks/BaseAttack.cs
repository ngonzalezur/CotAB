using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.UI.CanvasScaler;
//using TMPro;

public class BaseAttack : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public int Damage;
    public Faction Faction;
    public int AreaOfEffect;
    public float CoolDown;
    public float LastCast1;
    public float LastCast2;
    //public bool activeInHierarchy;
    //public TMP_InputField inputDmg;
    //public TMP_InputField inputCool;

    public int DoVeneno = 0;
    public int attackType; //0 para lazar en casilla highlight, 1 para ataque en linea, 2 skillshot , 3 ataque meelee, por ahora va asi, se modifica en el unit manager

    public void Destroy()
    {
        //if(this != null) Destroy(gameObject);
        UnitManager.Instance.Attacks.Remove(this);
        this.gameObject.SetActive(false);
        this.OccupiedTile.OccupiedAttack = null;
    }

    public virtual void DoDamage(BaseUnit unit)
    {
        if (unit == null) return;
        if (unit.Faction != Faction)
        {
            if (DoVeneno > 0)
            {
                unit.veneno += DoVeneno;
            }
            //Debug.Log("soy normal");
            unit.Health -= Math.Abs(Damage);
            unit.ActualHeath.text = Math.Max(unit.Health,0) + " / " + unit.MaxHealth;
            var audio = unit?.GetComponent<AudioSource>();
            audio?.Play();
            
        }
    }

    //public void Update()
    //{

    //    if (int.TryParse(inputDmg.text, out var dmg) && inputDmg != null && inputDmg.text != "")
    //    {
    //        Damage = dmg;
    //    }
    //    if (float.TryParse(inputCool.text, out var cool) && inputCool != null && inputCool.text != "")
    //    {
    //        CoolDown = cool;
    //    }            
    //}

    
}
