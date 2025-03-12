using System;

namespace CotA.Configuration
{
    [Serializable]
    public class ConfigurationData
    {
        public UnitData robot;
        public UnitData nina;
        public UnitData artificer;
        public UnitData basicEnemy;
        public AttData[] basickAttack;
        public WorldData setup1;

        [Serializable]
        public class UnitData
        {
            public BaseUnit prefab;
            //public float movementCoolDown = 1f;
            public int MaxHealth = 10;
            public Faction Faction = 0;
            public BaseAttack[] Attacks = {null,null,null,null};
        }


        [Serializable]
        public class AttData
        {
            public BaseAttack prefab;
            public int Damage = 0;
            public int AreaOfEffect = 1;
            public float Cooldown =1f;
            public int DoVeneno = 0;
        }

        [Serializable]
        public class WorldData
        {
            public float TimeMoveEne = 2f;
            public float TimeAttEne = 0.5f;
            public float TimeMoveHero = 0.5f;
            public float TimeVeneno = 1f;
        }
    }
}