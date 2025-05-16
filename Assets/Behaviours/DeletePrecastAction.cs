using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;
using System.Collections;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DeletePrecast", story: "[Enemy] deletes precast attack [X] in [target]", category: "Action", id: "4b00a90cf844af129b90ad5bd86a36b0")]
public partial class DeletePrecastAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<List<Tile>> Target;
    UnitManager uManager;
    protected override Status OnStart()
    {
        uManager = UnitManager.Instance;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var target = Target.Value;
        DeletePrecast(target);
        return Status.Success;
    }

    public void DeletePrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(false);
        }
    }

    //List<Tile> Target(BaseUnit unit, BaseAttack attack)
    //{
    //    var target = new List<Tile>();
    //    if (uManager.Ataque.TryGetValue((int)attack.type, out Ataques att))
    //    {
    //        target = att(unit, attack);
    //    }
    //    return target;
    //}

    protected override void OnEnd()
    {
    }
}

