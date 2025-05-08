using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Showprecast", story: "[Enemy] show precast", category: "Action", id: "9cac88c92ef22f0854f4a9b74c0febfc")]
public partial class ShowprecastAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    BaseAttack attack;
    UnitManager manager;
    protected override Status OnStart()
    {
        attack = Enemy.Value.Attacks[0];
        manager = UnitManager.Instance;
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

