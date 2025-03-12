using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CotA.Configuration
{
    [CreateAssetMenu(fileName = "DefaultConfigurationScriptableObject", menuName = "Scriptable Objects/DefaultConfigurationScriptableObject")]
    public class DefaultConfigurationScriptableObject : ScriptableObject
    {
        [SerializeField] private ConfigurationData _data;

        public ConfigurationData Data => _data;

        public void SaveToFile ()
        {
            FileConfigurationLoader.SaveToFile(Data);
        }
    }
}

