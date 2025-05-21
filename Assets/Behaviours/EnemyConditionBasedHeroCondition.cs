using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EnemyConditionBasedHero", story: "[Enemy] meets [x] based on [hero]", category: "Conditions", id: "cb6fa3cb9975c2fbfa6563c2b9841c44")]
public partial class EnemyConditionBasedHeroCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;

    public override bool IsTrue()
    {
        int index = X.Value;

        switch (index)
        {
            case 0:
                return true;

            case 1:
                return true;

            case 2:
                return true;

            default:
                return false;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
