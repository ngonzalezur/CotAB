using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HeroPosition", story: "[Hero] is position [x]", category: "Conditions", id: "e2a96736ab397dc2ef4fefe7bae665ec")]
public partial class HeroPositionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;
    [SerializeReference] public BlackboardVariable<int> X;

    public override bool IsTrue()
    {
        if (Hero?.Value == null || Hero.Value.OccupiedTile == null)
            return false;

        int heroX = Hero.Value.OccupiedTile.PositionXTile();

        return heroX == 0 || heroX == 1 || heroX == X.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
