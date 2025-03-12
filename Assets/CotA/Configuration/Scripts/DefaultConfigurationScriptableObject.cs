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

        public void SaveToFile()
        {
            FileConfigurationLoader.SaveToFile(Data);
        }
    }
    [CustomEditor(typeof(DefaultConfigurationScriptableObject))]
    public class DefaultConfigurationScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Renderizar el Inspector predeterminado
            DrawDefaultInspector();

            // Obtener el script objetivo
            DefaultConfigurationScriptableObject myScript = (DefaultConfigurationScriptableObject)target;

            // Crear un botón en el inspector
            if (GUILayout.Button("Save To File"))
            {
                // Llamar a la función SaveToFile
                myScript.SaveToFile();
            }
        }
    }
}
