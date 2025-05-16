using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "QuitStun", story: "[Enemy] quit stun", category: "Action", id: "c95038358f4e6c41dfc4e5781ff2ebdc")]
public partial class QuitStunAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Enemy == null || Enemy.Value == null) return Status.Success;
        Enemy.Value.stun = false;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

