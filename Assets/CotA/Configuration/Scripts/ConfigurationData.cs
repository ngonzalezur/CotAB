using System;

namespace CotA.Configuration
{
    [Serializable]
    public class ConfigurationData
    {
        public UnitData robot;
        public UnitData nina;

        [Serializable]
        public class UnitData
        {
            public float movementCoolDown = 1f;
        }
    }
}