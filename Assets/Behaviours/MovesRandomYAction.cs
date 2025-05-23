using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MovesRandomY", story: "[Enemy] moves random in y", category: "Action", id: "318c2e491bb6455c7896165b1b0c8e76")]
public partial class MovesRandomYAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var r = UnityEngine.Random.Range(0, 2);
        if (r == 0)
        {
            Enemy.Value.MoveUp();
        }
        else
        {
            Enemy.Value.MoveDown();
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

