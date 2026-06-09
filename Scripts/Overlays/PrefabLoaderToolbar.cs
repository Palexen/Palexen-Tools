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
#if UNITY_EDITOR
#if UNITY_2021_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using Palexen.Scriptables;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Palexen.Overlays
{
    [EditorToolbarElement(id, typeof(SceneView))]
    class PrefabButtonContainer : VisualElement, ISupportsOverlays
    {
        public const string id = "PrefabLoaderToolbar/ButtonContainer";
        private List<GameObject> prefabList = new();

        private Orientation m_Orientation;
        public Orientation Orientation
        {
            get => m_Orientation;
            set
            {
                if (m_Orientation != value)
                {
                    m_Orientation = value;
                    UpdateLayoutOrientation();
                }
            }
        }

        public PrefabButtonContainer()
        {
            LoadPrefabsFromProject();

            RefreshButtons();
        }

        public void RefreshButtons()
        {
            string path = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(path);

            Clear();

            // Protección si el archivo de configuración no existe en Resources
            if (setting == null)
            {
                Label errorLabel = new Label(" [Palexen] Config asset not found in Resources. ");
                errorLabel.style.color = Color.red;
                Add(errorLabel);
                return;
            }

            EditorToolbarButton updateBtn = new EditorToolbarButton();
            updateBtn.text = $"📥 Update";
            updateBtn.tooltip = $"Update if not updated yet!";
            updateBtn.clicked += () =>
            {
                LoadPrefabsFromProject();
                RefreshButtons();
            };

            updateBtn.style.marginBottom = 2;
            updateBtn.style.marginRight = 2;
            Add(updateBtn);

            if (prefabList.Count == 0)
            {
                Label noPrefabsLabel = new Label(" No Prefabs here ");
                noPrefabsLabel.style.color = Color.gray;
                noPrefabsLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                noPrefabsLabel.style.marginLeft = 5;
                noPrefabsLabel.style.marginTop = 5;
                Add(noPrefabsLabel);

                UpdateLayoutOrientation();
                return;
            }

            int currentIndex = setting.quickPrefabs.FindIndex(p => p != null && p._label == setting.prefabIndex);
            if (currentIndex == -1) currentIndex = 0;

            string iconStr = setting.quickPrefabs.Count > currentIndex && setting.quickPrefabs[currentIndex] != null
                ? setting.quickPrefabs[currentIndex]._icon
                : "📦";

            foreach (var prefab in prefabList)
            {
                EditorToolbarButton characterButton = new();

                characterButton.text = $"{iconStr} {prefab.name}";
                characterButton.clicked += () => InstantiatePrefabInScene(prefab);
                characterButton.tooltip = $"Click to Instantiate: {prefab.name}.";

                characterButton.style.marginBottom = 2;
                characterButton.style.marginRight = 2;

                Add(characterButton);
            }

            UpdateLayoutOrientation();
        }

        void LoadPrefabsFromProject()
        {
            string pathT = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(pathT);

            prefabList.Clear();
            if (setting == null) return;

            int currentIndex = setting.quickPrefabs.FindIndex(p => p != null && p._label == setting.prefabIndex);
            if (currentIndex == -1) currentIndex = 0;

            if (currentIndex >= 0 && setting.quickPrefabs.Count > currentIndex && setting.quickPrefabs[currentIndex] != null)
            {
                string[] guids = AssetDatabase.FindAssets($"l:{setting.quickPrefabs[currentIndex]._label}", new[] { "Assets" });
                HashSet<string> processedPaths = new();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (processedPaths.Contains(path)) continue;
                    processedPaths.Add(path);

                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (prefab != null)
                    {
                        var assetType = PrefabUtility.GetPrefabAssetType(prefab);
                        if (assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant)
                        {
                            prefabList.Add(prefab);
                        }
                    }
                }
            }
        }

        private void UpdateLayoutOrientation()
        {
            if (Orientation == Orientation.Vertical)
            {
                style.flexDirection = FlexDirection.Column;
                style.alignItems = Align.Stretch;
            }
            else
            {
                style.flexDirection = FlexDirection.Row;
                style.alignItems = Align.Center;
            }
        }


        void InstantiatePrefabInScene(GameObject prefab)
        {
            GameObject spawnedObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (SceneView.lastActiveSceneView != null)
            {
                spawnedObj.transform.position = SceneView.lastActiveSceneView.pivot;
            }

            Undo.RegisterCreatedObjectUndo(spawnedObj, $"Instantiate {prefab.name}");
            Selection.activeGameObject = spawnedObj;
        }
    }

    [Overlay(typeof(SceneView), "Quick Prefabs")]
    public class PrefabLoaderOverlay : ToolbarOverlay
    {
        PrefabLoaderOverlay() : base(PrefabButtonContainer.id) { }
    }
}
#endif
#endif
