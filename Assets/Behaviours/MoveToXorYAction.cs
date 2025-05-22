using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToXorY", story: "[Enemy] moves to [X] column or [Y] file (excluisve -1 to ignore)", category: "Action", id: "9f8daaaec59f17854a1a0f94f6ad1b01")]
public partial class MoveToXorYAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<int> Y;
    int posX;
    int posY;   

    protected override Status OnStart()
    {
        if(Enemy.Value.OccupiedTile != null)
        {
            posX = Enemy.Value.OccupiedTile.x;
            posY = Enemy.Value.OccupiedTile.y;
        }        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Y != -1 && posY != Y)
        {
            if(posY < Y)
            {
                Enemy.Value.MoveUp();
            }
            else
            {
                Enemy.Value.MoveDown();
            }
        }else if(X != -1)
        {
            if (posX < X)
            {
                Enemy.Value.MoveRight();
            }
            else
            {
                Enemy.Value.MoveLeft();
            }
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

