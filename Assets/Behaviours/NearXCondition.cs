using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "NearX", story: "[Enemy] [X] tile away from [Hero]", category: "Conditions", id: "8219fde0503d128941df5d8a60c51ec6")]
public partial class NearXCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseUnit> Enemy;
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<BaseUnit> Hero;

    public override bool IsTrue()
    {
        if (Enemy.Value.OccupiedTile != null && Enemy.Value.OccupiedTile.PositionXTile() - Hero.Value.OccupiedTile.PositionXTile() != X)
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
