/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using Palexen.Gameplay.Input;
using Palexen.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Palexen.Scriptables
{
    [ScriptDescription("Input Schema Container", "Scriptable")]
    [CreateAssetMenu(fileName = "New Input Schema", menuName = "Palexen/UI/Input Schema")]
    public class InputSchemaContainer : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Custom UI Input Schema")]
        public string _schemaName;
        [Header("Setup your custom UI Schema controller")]
        public ActionVariables[] buttonSchemas;
        ActionVariables _uiVariableResult;

        #endregion

        #region API

        public ActionVariables GetSchema(int SchemaID)
        {
            _uiVariableResult = buttonSchemas[SchemaID];

            return _uiVariableResult;
        }

        #endregion
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR
    [CustomEditor(typeof(InputSchemaContainer))]
    [CanEditMultipleObjects]
    public class InputSchemaContainerEditor : Editor
    {
        private SerializedProperty _schemaName;
        private SerializedProperty buttonSchemas;

        private void OnEnable()
        {
            _schemaName = serializedObject.FindProperty("_schemaName");
            buttonSchemas = serializedObject.FindProperty("buttonSchemas");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Input Schema</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Configure a button scheme for your main actions, then you can invoke the result by creating a connection " +
                "between this scheme and the <color=green>GetSchema(int);</color> method to get an on-screen " +
                "indicator of the key or button to press. You'll need advanced programming skills to extend its use; " +
                "\n\r\nOtherwise, you can still use it with the default configuration and the base player interaction components in " +
                "<color=magenta>Game Logic</color> or <color=magenta>Gameplay</color>.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 170));

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_schemaName);
            EditorGUILayout.PropertyField(buttonSchemas);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}