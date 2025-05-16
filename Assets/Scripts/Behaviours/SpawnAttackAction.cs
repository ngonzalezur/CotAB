using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnAttack", story: "[Self] spawn attack", category: "Action", id: "db3ae30ca0b50fe4f3141db0de82226c")]
public partial class SpawnAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Self;
    BaseAttack attack;
    UnitManager manager;
    protected override Status OnStart()
    {
        attack = Self.Value.Attacks[0];
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
            target = att(Self.Value, attack);
        }
        else
        {
            target = null;
        }
        manager.SetAttacksInTiles(target,attack);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

