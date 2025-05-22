using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;


[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FollowRowStayInColumnActionFollowRowStayInColumnActionFollowRowStayInColumnActionFollowRowStayInColumn", story: "[Enemy] follow [Hero] row while staying in fixed [column]", category: "Action", id: "b086a0917b346b5f6e7c8fe21e81da08")]
public partial class EnemyFollowsHero : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;
    [SerializeReference] public BlackboardVariable<int> Column;

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
        if (Enemy.Value.OccupiedTile == null || Hero.Value.OccupiedTile == null)
            return Status.Failure;

        int enemyX = Enemy.Value.OccupiedTile.PositionXTile();
        int enemyY = Enemy.Value.OccupiedTile.PositionYTile();
        int heroY = Hero.Value.OccupiedTile.PositionYTile();

        // Mantener la columna fija
        if (enemyX < Column)
        {
            Enemy.Value.MoveRight();
            return Status.Running;
        }
        else if (enemyX > Column)
        {
            Enemy.Value.MoveLeft();
            return Status.Running;
        }

        // Ya está en la columna correcta, entonces seguir la fila del héroe
        if (enemyY < heroY)
        {
            Enemy.Value.MoveUp();
            return Status.Running;
        }
        else if (enemyY > heroY)
        {
            Enemy.Value.MoveDown();
            return Status.Running;
        }

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

