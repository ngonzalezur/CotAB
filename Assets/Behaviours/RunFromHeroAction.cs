using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RunFromHero", story: "[Self] run from [Hero]", category: "Action", id: "eca0e3128dcc9192892fafd7f6e85e16")]
public partial class RunFromHeroAction : Action
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
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() == Target.Value.OccupiedTile.PositionYTile())
        {
            var r = UnityEngine.Random.Range(0, 100);
            if (r > 50)
            {
                Enemy.Value.MoveDown();
            }
            else
            {
                Enemy.Value.MoveUp();
            }
            
        }
        else if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() > Target.Value.OccupiedTile.PositionYTile())
        {
            Enemy.Value.MoveUp();
        }
        else if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() < Target.Value.OccupiedTile.PositionYTile())
        {
            Enemy.Value.MoveDown();
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

