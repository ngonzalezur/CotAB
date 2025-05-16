using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsStun", story: "[Enemy] is stun", category: "Conditions", id: "1e9069dacec01f3ccb8d56e8763a8391")]
public partial class IsStunCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    public override bool IsTrue()
    {
        Debug.Log("estoy stunned");
        //if (Enemy == null || Enemy.Value == null) return false;
        //Debug.Log("estoy stunned");
        return Enemy.Value.stun;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
