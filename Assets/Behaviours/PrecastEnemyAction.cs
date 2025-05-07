using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PrecastEnemy", story: "[Enemy] precast", category: "Action", id: "28e4ef42a6499a02f2fbe8be64f7b5d4")]
public partial class PrecastEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

