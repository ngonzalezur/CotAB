using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckIfInPosXandY", story: "[Enemy] in [X] file and [Y] column (-1 for any)", category: "Conditions", id: "56f358c070a868becb81c21beec11b4f")]
public partial class CheckIfInPosXandYCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<int> Y;

    public override bool IsTrue()
    {
        if((X == Enemy.Value.OccupiedTile.x || X == -1) && (Y == Enemy.Value.OccupiedTile.y || Y == -1))
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
