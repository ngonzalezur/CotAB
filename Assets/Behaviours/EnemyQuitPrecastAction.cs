using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyQuitPrecast", story: "[Enemy] disapper precast", category: "Action", id: "d086f25b270c7bb87b8587f568f2b555")]
public partial class EnemyQuitPrecastAction : Action
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
        Dictionary<int, Ataques> Ataque = manager.Ataque;
        manager.SetAttackDictionary();
        var target = new List<Tile>();
        if (Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            target = att(Enemy.Value, attack);
        }
        else
        {
            target = null;
        }
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(false);
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

