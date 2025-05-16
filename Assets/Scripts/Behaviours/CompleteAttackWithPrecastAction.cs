using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;
using System.Collections.Generic;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CompleteAttackWithPrecast", story: "[Enemy] cast attack [X] and show precast", category: "Action", id: "8add6ea2789196838eed8f2f99748c0a")]
public partial class CompleteAttackWithPrecastAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    UnitManager uManager;
    protected override Status OnStart()
    {
        uManager = UnitManager.Instance;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //StartCoroutine(Attack(Enemy.Value, Enemy.Value.Attacks[X], Target(Enemy.Value, Enemy.Value.Attacks[X])));
        return Status.Success;
    }
    void EnemyAttack(BaseUnit unit, BaseAttack attack)
    {
        if (unit != null && attack != null)
        {
            uManager.CastAttack(unit,attack);
        }
    }
    List<Tile> Target(BaseUnit unit, BaseAttack attack)
    {
        var target = new List<Tile>();
        if (uManager.Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            target = att(unit, attack);
        }
        return target;
     }

    void ShowPrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(true);
        }
    }

    void DeletePrecast(List<Tile> target)
    {
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(false);
        }
    }

    //IEnumerator Attack(BaseUnit unit, BaseAttack attack, List<Tile> target)
    //{
    //    if(unit != null && attack != null && target != null)
    //    {
    //        ShowPrecast(target);
    //        yield return new WaitForSeconds(1.5f);
    //        DeletePrecast(target);
    //        EnemyAttack(unit, attack);
    //    }
    //    yield return new WaitForSeconds(0f);
    //}

    protected override void OnEnd()
    {
    }
}

