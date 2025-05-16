using System;
using Unity.Behavior;
using UnityEngine;
using System.Linq;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnitManager;
using static UnityEngine.UI.CanvasScaler;
using System.Collections.Generic;
using TMPro.Examples;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Showprecast", story: "[Enemy] show precast", category: "Action", id: "9cac88c92ef22f0854f4a9b74c0febfc")]
public partial class ShowprecastAction : Action
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
        //Dictionary<int, Ataques> Ataque = new Dictionary<int, Ataques>();
        Dictionary<int, Ataques> Ataque = manager.Ataque;
        manager.SetAttackDictionary();
        var target = new List<Tile>();
        if (Ataque.TryGetValue((int)attack.type, out Ataques att))
        {
            target = att(Enemy.Value, attack);
            Debug.Log("entre y hay target");
        }
        else
        {
            target = null;
            Debug.Log("no hay target");
        }
        foreach (Tile tile in target)
        {
            tile._precast.SetActive(true);
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

