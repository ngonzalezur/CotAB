using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyAttack", story: "[Enemy] attack", category: "Action", id: "ee98dc62225d6ccb61da30c763db471a")]
public partial class EnemyAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Enemy.Value.Attack();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

