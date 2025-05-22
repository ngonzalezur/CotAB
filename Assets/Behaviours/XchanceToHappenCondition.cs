using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "XchanceToHappen", story: "[X] chance to true", category: "Conditions", id: "448fde9eb0f90708084d831128084aba")]
public partial class XchanceToHappenCondition : Condition
{
    [SerializeReference] public BlackboardVariable<int> X;

    public override bool IsTrue()
    {
        var r = UnityEngine.Random.Range(0,100);
        if (r < 100-X)
        {
            return false;
        }
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
