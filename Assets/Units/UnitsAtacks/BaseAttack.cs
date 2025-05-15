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
    public int Heal;
    public int Stamina;
    public Faction Faction;
    public int AreaOfEffect;
    public float CoolDown;
    public float LastCast1;
    public float LastCast2; //era para el segundo jugador peto ya no sera necesario, lo dejo por ahor apara no generar errores

    public float ManaCost = 0.5f;

    public BaseUnit invocacion;
    //public bool activeInHierarchy;
    //public TMP_InputField inputDmg;
    //public TMP_InputField inputCool;

    public int DoVeneno = 0;
    public int DoBurn = 0;
    public bool stun = false;
    public int attackType;
    public AttType type;
    public BaseAttack ExtraAttack;
    public int numRandomTiles = 0;

    SpriteRenderer sprite;
    ParticleSystem particle;
    public enum AttType
    {
        proyectil = 0,
        cast = 1,
        area = 2,
        muro = 3,
        fila = 4,
        allEnemies = 5,
        randomCast = 6,
        melee = 7,
        invocacion = 8,
        dashMelee = 9,
        parry = 10,
        cambiarFaction = 11,
        allHeros = 12,
        areamelee2x3 = 13,
        fila3x1 = 14,
        gancho = 15,
        areaLast2Columns = 16,
        areadelay = 17,
        areamelee1x3 = 18,
        areaglobal = 19,
        atractor = 20,
        barridofilainverso = 21,
        barridocolumnainverso = 22
    }

    public void ContadorKPIAciertoAttaque()
    {
        //me va tocar revisar los nombres de los ataques NICO NO CAMBIE NOMBRES O PONGA ESTOS
        if(UnitName != null)
        {
            Debug.Log(UnitName);
            if (UnitName == "ad1")
            {
                GameManager.HitAttDruid1++;
            }
            if (UnitName == "ad2")
            {
                GameManager.HitAttDruid2++;
            }
            if (UnitName == "ad3")
            {
                GameManager.HitAttDruid3++;
                //Debug.Log(GameManager.HitAttDruid4);
            }
            if (UnitName == "at4")
            {
                GameManager.HitAttDruid4++;
                //Debug.Log(GameManager.HitAttDruid4);
            }

            if (UnitName == "ar1")
            {
                GameManager.HitAttRobot1++;
            }
            if (UnitName == "ar2")
            {
                GameManager.HitAttRobot2++;
            }
            if (UnitName == "ar3")
            {
                GameManager.HitAttRobot3++;
            }
            if (UnitName == "ar4")
            {
                GameManager.HitAttRobot4++;
            }
            if (UnitName == "amd")
            {
                GameManager.HitMeleeDruid++;
            }
            if (UnitName == "amr")
            {
                GameManager.HitMeleeRobot++;
            }
            
        }
    }


    public void Destroy()
    {
        if (this == null || this.OccupiedTile == null) return;
        UnitManager.Instance.AttacksinPlay.Remove(this);
        //MauriManager.Instance.AttacksinPlay.Remove(this);
        //this.gameObject.SetActive(false);
        //this.OccupiedTile.OccupiedAttack = null;
        StartCoroutine(DeSpawnAttack());
    }

     public IEnumerator DeSpawnAttack()
    {
        QuitAttackFromTile();
        yield return new WaitForSeconds(1f);
        QuitSprite();
        QuitAttackPrefab();
    }

    public void QuitAttackFromTile()
    {
        OccupiedTile.OccupiedAttack = null;
        OccupiedTile = null;
    }

    public void QuitSprite()
    {
        if (TryGetComponent<SpriteRenderer>(out sprite))
        {
            sprite.gameObject.SetActive(false);
        }
        if (TryGetComponent<ParticleSystem>(out particle))
        {
            particle.gameObject.SetActive(false);
        }
    }

    public void QuitAttackPrefab()
    {
        if(sprite != null)
        {
            sprite.gameObject.SetActive(true);
        }
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }

    public virtual void DoDamage(BaseUnit unit)
    {
        //Debug.Log(UnitName);
        var sound = UnitManager.Instance.SoundManager;
        if (unit == null) return;
        if (unit.Faction != Faction)
        {
            if (stun)
            {
                unit.stun = stun;
            }
            if (DoVeneno > 0)
            {
                unit.veneno += DoVeneno;
            }
            if (DoBurn > 0)
            {
                unit.burn += DoBurn;
            }
            //Debug.Log("soy normal");
            
            if (!unit.parry)
            {
                ContadorKPIAciertoAttaque();
                unit.Health -= Math.Abs(Damage);
                unit.ActualHeath.text = Math.Max(unit.Health, 0) + " / " + unit.MaxHealth;
        
                //var audio = unit?.GetComponent<AudioSource>();
                //audio?.Play();


                if (unit.Faction == Faction.Hero && sound != null && !UnitManager.Instance.Invocaciones.Contains(unit)) //despues meter label de que tipo de heroe es para llama una funcion u otra
                {
                    //sound.PlayHeroDamage();
                    if (unit.UnitName == "Druid")
                    {
                        sound.PlayDruidDamage();
                    }
                    if (unit.UnitName == "Robot")
                    {
                        sound.PlayRobotDamage();
                    }
                }                
                if (unit.animator != null)
                {
                    unit.animator?.SetTrigger("TakeDamage");
                }
            }else if (unit.parry)
            {
                UnitManager.Instance.CastAttack(unit, this);
            }
            
            
            
        }
    }

    public virtual void DoHeal(BaseUnit unit)
    {
        ContadorKPIAciertoAttaque();
        if (unit == null) return;
        if (unit.Faction == Faction)
        {
            unit.Health += Math.Abs(Heal);
            unit.ActualHeath.text = Math.Min(unit.Health, unit.MaxHealth) + " / " + unit.MaxHealth;
            if(unit.Health > unit.MaxHealth)
            {
                unit.Health = unit.MaxHealth;
            }
            //var audio = unit?.GetComponent<AudioSource>();
            //audio?.Play();
            //if (unit.animator != null)
            //{
            //    unit.animator?.SetTrigger("TakeDamage");
            //}
        }
    }

    public virtual void DoStamina(BaseUnit unit)//No voy a reombrar esto porque me da miedo, peor ya no regenera estamina. Regenera mana
    {
        if (unit == null) return;
        if (unit.Faction == Faction)
        {
            unit.CastMana += Math.Abs(Stamina);
            //unit.ActualHeath.text = Math.Min(unit.Health, unit.MaxHealth) + " / " + unit.MaxHealth;
            //if (unit.Health > unit.MaxHealth)
            //{
            //    unit.Health = unit.MaxHealth;
            //}
            //var audio = unit?.GetComponent<AudioSource>();
            //audio?.Play();
            //if (unit.animator != null)
            //{
            //    unit.animator?.SetTrigger("TakeDamage");
            //}
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
