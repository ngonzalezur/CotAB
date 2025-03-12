using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.UI.CanvasScaler;

public class DuplicadorVeneno : BaseAttack
{
    public override void DoDamage(BaseUnit unit)
    {
        if (unit.Faction != Faction)
        {
            if (DoVeneno > 0)
            {
                unit.veneno += DoVeneno;
            }
            //Debug.Log("duplicado");
            unit.veneno *= 2;
            unit.Health -= Math.Abs(Damage);
            unit.ActualHeath.text = Math.Max(unit.Health, 0) + " / " + unit.MaxHealth;

        }
    }
}
