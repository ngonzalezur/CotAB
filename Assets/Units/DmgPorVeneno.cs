using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.UI.CanvasScaler;
public class DmgPorVeneno : BaseAttack
{
    public override void DoDamage(BaseUnit unit)
    {
        if (unit.Faction != Faction)
        {
            if (DoVeneno > 0)
            {
                unit.veneno += DoVeneno;
            }
            //Debug.Log("puuuuuuuuuum");
            unit.Health -= Math.Abs(Damage + unit.veneno);
            unit.veneno = 0;
            unit.ActualHeath.text = Math.Max(unit.Health, 0) + " / " + unit.MaxHealth;

        }
    }
}
