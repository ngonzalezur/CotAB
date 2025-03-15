
using UnityEngine;
using UnityEditor;

namespace CotA.Configuration
{
    [CustomEditor(typeof(DefaultConfigurationScriptableObject))]
    public class DefaultConfigurationScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Renderizar el Inspector predeterminado
            DrawDefaultInspector();

            // Obtener el script objetivo
            DefaultConfigurationScriptableObject myScript = (DefaultConfigurationScriptableObject)target;

            // Crear un bot�n en el inspector
            if (GUILayout.Button("Save To File"))
            {
                // Llamar a la funci�n SaveToFile
                myScript.SaveToFile();
            }


        }
    }
}


#if UNITY_EDITOR

#if UNITY_ANDROID

#elif UNITY_IOS

#endif

#endif