using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetTarget", story: "Get [Target] from [Enemy] Attack [X]", category: "Action", id: "f9964a48787873c1760d5e73f2bc0e4a")]

public partial class GetTargetAction : Action
{

    [SerializeReference] public BlackboardVariable<List<Tile>> Target;
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    UnitManager uManager;
    protected override Status OnStart()
    {
        uManager = UnitManager.Instance;
        Target.Value = new List<Tile>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Target.Value = SetTarget(Enemy.Value, Enemy.Value.Attacks[X]);
        return Status.Success;
    }

    List<Tile> SetTarget(BaseUnit unit, BaseAttack attack)
    {
        var target = new List<Tile>();
        if (uManager.Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            target = att(unit, attack);
        }
        return target;
    }

    protected override void OnEnd()
    {
    }
}

