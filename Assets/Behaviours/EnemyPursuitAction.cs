using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyPursuit", story: "[Enemy] moves to [Target]", category: "Action", id: "af47a70f26f9a17700b6313063417b10")]
public partial class EnemyPursuitAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<BaseUnit> Target;

    protected override Status OnStart()
    {
        Target.Value = GameObject.FindFirstObjectByType<BaseHero>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Target.Value == null) return Status.Success;
        if(Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() < Target.Value.OccupiedTile.PositionYTile())
        {
            Enemy.Value.MoveUp();
        }
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() > Target.Value.OccupiedTile.PositionYTile())
        {
            Enemy.Value.MoveDown();
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

