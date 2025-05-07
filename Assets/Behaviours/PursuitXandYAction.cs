using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PursuitXandY", story: "[Enemy] pursuit to [X] horizontal an [Y] vertical to [Hero]", category: "Action", id: "36295f154d0aace03565055a7e597c5a")]
public partial class PursuitXandYAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<int> Y;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;

    protected override Status OnStart()
    {
        BaseUnit persistentHero = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);
        if (persistentHero == null)
        {
            Hero.Value = GameObject.FindFirstObjectByType<BaseHero>();
        }
        else
        {
            Hero.Value = persistentHero;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() - Hero.Value.OccupiedTile.PositionYTile() > Y)
        {
            Enemy.Value.MoveUp();
            return Status.Success;
        }
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() - Hero.Value.OccupiedTile.PositionYTile() < Y)
        {
            Enemy.Value.MoveDown();
            return Status.Success;
        }
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionXTile() - Hero.Value.OccupiedTile.PositionXTile() > X)
        {
            Enemy.Value.MoveRight();
            return Status.Success;
        }
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionXTile() - Hero.Value.OccupiedTile.PositionXTile() < X)
        {
            Enemy.Value.MoveLeft();
            return Status.Success;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

