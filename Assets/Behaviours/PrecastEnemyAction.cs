using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;
using System.Collections.Generic;
using static UnitManager;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PrecastEnemy", story: "[Enemy] precast attack [X] in [target]", category: "Action", id: "28e4ef42a6499a02f2fbe8be64f7b5d4")]
public partial class PrecastEnemyAction : Action
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
        ShowPrecast(target);
        return Status.Success;
    }

    public void ShowPrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(true);
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

