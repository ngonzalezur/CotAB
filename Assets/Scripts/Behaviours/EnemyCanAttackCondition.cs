using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EnemyCanAttack", story: "[Self] can Attack", category: "Conditions", id: "134eca958e85bb70ba7ddc911523b3b3")]
public partial class EnemyCanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Self;

    public override bool IsTrue()
    {
        
        return Time.time - Self.Value.lastCastBA > Self.Value.Attacks[0].CoolDown;
    }

    public override void OnStart()
    {

    }

    public override void OnEnd()
    {
    }
}
