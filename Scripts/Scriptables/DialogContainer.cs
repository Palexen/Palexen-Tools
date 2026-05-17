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
        public string _actorName;
        public Color _actorColor = Color.white;

        [TextArea(3, 20)]
        public string _dialogText;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public AudioClip _langClip;

        [Header("In case of not using sound")]
        public float _onScreenTimeDialog = 2;
    }

#if UNITY_EDITOR

    #region MAIN CUSTOM EDITOR

    [CustomEditor(typeof(DialogContainer))]
    [CanEditMultipleObjects]
    public class DialogContainerEditor : Editor
    {
        DialogContainer _container;
        SerializedProperty _actorName;
        SerializedProperty _actorColor;
        SerializedProperty _dialogText;
        SerializedProperty _langClip;
        SerializedProperty _onScreenTimeDialog;

        private void OnEnable()
        {
            _container = (DialogContainer)target;
            _actorName = serializedObject.FindProperty("_actorName");
            _actorColor = serializedObject.FindProperty("_actorColor");
            _dialogText = serializedObject.FindProperty("_dialogText");
            _langClip = serializedObject.FindProperty("_langClip");
            _onScreenTimeDialog = serializedObject.FindProperty("_onScreenTimeDialog");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            Color c = new (.392f, .584f, .929f, 1);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Dialog Container</color> \n " +
                $"(Closed Caption <color={"#" + c.ConvertToHex()}>CC</color>)",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize, TextAnchor.MiddleCenter, FontStyle.Bold, 60));

            serializedObject.Update();

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_actorName);
            EditorGUILayout.PropertyField(_actorColor);
            EditorGUILayout.PropertyField(_dialogText);
            EditorGUILayout.PropertyField(_langClip);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_onScreenTimeDialog);

            serializedObject.ApplyModifiedProperties();

        }
    }

    public class CreateDialogContainer
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Sequences/Create Dialog Container")]
#else
        [MenuItem("Palexen/Create Dialog Container", false, 5)]
#endif
        static void CreateAsset()
        {
            DialogContainer asset = ScriptableObject.CreateInstance<DialogContainer>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.scriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Dialog Container");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Dialog Container.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Dialog Container created at: </color><color=cyan>{assetPath}</color>");
        }
    }

    #endregion

#endif
}
