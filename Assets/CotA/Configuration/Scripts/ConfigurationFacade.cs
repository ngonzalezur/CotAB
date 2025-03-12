using CotA.Configuration;
using UnityEngine;

namespace CotA.Configuration
{
    public class ConfigurationFacade: MonoBehaviour
    {
        [SerializeField] private DefaultConfigurationScriptableObject _defaultConfiguration;

        //private ConfigurationData _data;

        //public ConfigurationData GetData()
        //{
        //    return _data;
        //}

        //private void SetData(ConfigurationData data)
        //{
        //    _data = data;
        //}

        public ConfigurationData Data { get; private set; }

        private void Awake()
        {
            //Data = _loader.LoadFromFile();

            //if (Data == null)
            //    Data = _defaultConfiguration.Data;

            Data = FileConfigurationLoader.TryLoadFromFile() ?? _defaultConfiguration.Data;
        }
    }
}