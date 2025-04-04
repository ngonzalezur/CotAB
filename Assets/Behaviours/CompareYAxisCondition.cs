using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CompareYAxis", story: "[SelfY] is equal to [TargetY]", category: "Conditions", id: "16942a50df909032fca9e045ead44267")]
public partial class CompareYAxisCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> SelfY;
    [SerializeReference] public BlackboardVariable<BaseUnit> TargetY;

    public override bool IsTrue()
    {
        if (SelfY.Value.OccupiedTile != null && TargetY.Value.OccupiedTile != null)
        {
            return SelfY.Value.OccupiedTile.PositionYTile() == TargetY.Value.OccupiedTile.PositionYTile();
        }
        return false;
    }

    public override void OnStart()
    {
        TargetY.Value = GameObject.FindFirstObjectByType<BaseHero>();
    }

    public override void OnEnd()
    {
    }
}
