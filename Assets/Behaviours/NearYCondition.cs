using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "NearYCondition", story: "[Enemy] is [Y] tiles away from [Hero]", category: "Conditions", id: "c003f9bcf069c70efa7f4054f6838063")]
public partial class NearYCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> Y;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;

    public override bool IsTrue()
    {
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionYTile() - Hero.Value.OccupiedTile.PositionYTile() != Y )
        {
            return false;
        }
        return true;
    }
    public override void OnStart()
    {
        BaseUnit persistentHero = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);
        if (persistentHero == null)
        {
            Hero.Value = GameObject.FindFirstObjectByType<BaseHero>();
        }
        else
        {
            Hero.Value = persistentHero;
        }
    }

    public override void OnEnd()
    {
    }
}
