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
using UnityEngine;
using Palexen.Tools;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.Scriptables
{
    [CreateAssetMenu(fileName = "New Dialog Container", menuName = "Palexen/Dialog/Dialog Container")]
    public class DialogContainer : ScriptableObject
    {
        public string _dialogName;

        [TextArea(3, 20)]
        public string _dialogText;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] public AudioClip _langClip;

        [Header("In case of not using sound")]
        public float _onScreenTimeDialog = 2;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(DialogContainer))]
    [CanEditMultipleObjects]
    public class DialogContainerEditor : Editor
    {
        DialogContainer _container;
        SerializedProperty _dialogName;
        SerializedProperty _dialogText;
        SerializedProperty _langClip;
        SerializedProperty _onScreenTimeDialog;

        private void OnEnable()
        {
            _container = (DialogContainer)target;
            _dialogName = serializedObject.FindProperty("_dialogName");
            _dialogText = serializedObject.FindProperty("_dialogText");
            _langClip = serializedObject.FindProperty("_langClip");
            _onScreenTimeDialog = serializedObject.FindProperty("_onScreenTimeDialog");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Dialog Container</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_dialogName);
            EditorGUILayout.PropertyField(_dialogText);
            EditorGUILayout.PropertyField(_langClip);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_onScreenTimeDialog);

            serializedObject.ApplyModifiedProperties();

        }
    }

#endif
}
