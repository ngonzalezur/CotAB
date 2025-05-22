using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveRandomX", story: "[Enemy] moves random in x", category: "Action", id: "3a35138f858957d68ae8438212d88c3e")]
public partial class MoveRandomXAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var r = UnityEngine.Random.Range(0, 2);
        if(r == 0)
        {
            Enemy.Value.MoveRight();
        }
        else
        {
            Enemy.Value.MoveLeft();
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

