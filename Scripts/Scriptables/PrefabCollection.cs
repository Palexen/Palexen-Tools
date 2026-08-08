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

#if UNITY_EDITOR
using UnityEditor;
#endif

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Scriptables
{
    #if PALEXEN_TOOLS
    [ScriptDescription("PrefabCollection", "Improved Monobehavior")]
#endif
    [CreateAssetMenu(fileName = "New Prefab Collection", menuName = "Palexen/PrefabCollection")]
    public class PrefabCollection : ScriptableObject
    {
        #region VARIABLES

        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] [SerializeField] private GameObject[] _prefabs;

        #endregion

        #region PROPERTIES

        public GameObject[] Prefabs { get { return _prefabs; }  set { _prefabs = value; } }
        
        #endregion

        #region UNITY METHODS



        #endregion

        #region MECHANICS



        #endregion

        #region API

        public GameObject GetRandomPrefab()
        {
            int randomIndex = Random.Range(0, _prefabs.Length);

            return _prefabs[randomIndex];
        }

        #endregion
    }

    #region CUSTOM EDITOR
#if UNITY_EDITOR

    [CustomEditor(typeof(PrefabCollection))]
    [CanEditMultipleObjects]
    public class PrefabCollectionEditor : Editor
    {
        SerializedProperty _prefabs;

        private void OnEnable()
        {
            _prefabs = serializedObject.FindProperty("_prefabs");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Prefab Collection</color>",
                PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));

            GUILayout.Box("Add your prefabs here!",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            GUILayout.Space(10);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_prefabs, new GUIContent("Prefabs"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreatePrefabCollectionAsset
    {
        public static PrefabCollection tempAsset;

#if PALEXEN_UP_TOOLBAR
        [MenuItem("Prefabs/Create New Prefab Collection")]
#else
        [MenuItem("Palexen/Create New Prefab Collection", false, 1)]
#endif
        public static void CreateAsset()
        {
            PrefabCollection asset = ScriptableObject.CreateInstance<PrefabCollection>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.ScriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Prefabs");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Prefab Collection.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
            tempAsset = asset;

            Debug.Log($"<color=green>Prefab Collection created at: </color><color=cyan>{assetPath}</color>");
        }

        public void CreateNewPrefabCollectionsAsset()
        {
            CreateAsset();
        }

        public PrefabCollection GetCurrent()
        {
            return tempAsset;
        }
    }

#endif
    #endregion
}
