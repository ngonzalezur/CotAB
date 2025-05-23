using System;
using Unity.Behavior;
using UnityEngine;
using System.Linq;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Detectar_Heroe", story: "[Heroe] esta frente del [enemigo] en [x] casillas", category: "Conditions", id: "120dfca37c32d7237735486e5218acdb")]
public partial class DetectarHeroeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Heroe;
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemigo;
    [SerializeReference] public BlackboardVariable<int> X;

    public override bool IsTrue()
    {
        var posxE = Enemigo.Value?.OccupiedTile?.x;
        var posyE = Enemigo.Value?.OccupiedTile?.y;
        var posxH = Heroe.Value?.OccupiedTile?.x;
        var posyH = Heroe.Value?.OccupiedTile?.y;

        if (posyH == posyE)
        {
            if(posxH + X == posxE)
            {
                return true;
            }
        }
        return false;
    }

    public override void OnStart()
    {
        Heroe.Value = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);
    }

    public override void OnEnd()
    {
    }
}
