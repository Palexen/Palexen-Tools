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

            PrefabLoaderEvents.OnPrefabCategoryChanged += OnCategoryChanged;

            RegisterCallback<DetachFromPanelEvent>(_ =>
            {
                PrefabLoaderEvents.OnPrefabCategoryChanged -= OnCategoryChanged;
            });
        }

        void OnCategoryChanged()
        {
            LoadPrefabsFromProject();
            RefreshButtons();
        }


        public void RefreshButtons()
        {
            string path = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(path);

            Clear();

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

            int currentIndex = setting._entities.entities.FindIndex(p => p != null && p._label == setting.prefabIndex);
            if (currentIndex == -1) currentIndex = 0;

            string iconStr = setting._entities.entities.Count > currentIndex && setting._entities.entities[currentIndex] != null
                ? setting._entities.entities[currentIndex]._icon
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

            if (setting._entities != null)
            {
                int currentIndex = setting._entities.entities.FindIndex(p => p != null && p._label == setting.prefabIndex);
                if (currentIndex == -1) currentIndex = 0;

                if (currentIndex >= 0 && setting._entities.entities.Count > currentIndex && setting._entities.entities[currentIndex] != null)
                {
                    string[] guids = AssetDatabase.FindAssets($"l:{setting._entities.entities[currentIndex]._label}", new[] { "Assets" });
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
        PrefabLoaderOverlay() : base(PrefabButtonContainer.id)
        {
            displayName = "Quick Prefabs";
            this.collapsedIcon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon_quick.png") :
                AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon_quick_2.png");
        }
    }



    [EditorToolbarElement(id, typeof(SceneView))]
    public class EntityLoaderToolbar : EditorToolbarDropdownToggle, IAccessContainerWindow
    {
        public const string id = "PrefabLoaderToolbar/DropdownToggle";

        public EditorWindow containerWindow { get; set; }

        EntityLoaderToolbar()
        {
            text = "Entities";
            icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon.png") :
                AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon_2.png");

            tooltip = "Load Entities in this Scene";

            dropdownClicked += ShowOptions;
        }

        void ShowOptions()
        {
           var menu = new GenericMenu();

            string pathT = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(pathT);

            if (setting._entities != null)
            {
                foreach (var entity in setting._entities.entities)
                {
                    menu.AddItem(
                        new GUIContent(entity._label),
                        setting.CurrentPrefab == entity._label,
                        () => SetEntity(entity._label)
                    );
                }
            }

            menu.ShowAsContext();
        }

        void SetEntity(string value)
        {
            string pathT = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(pathT);

            setting.CurrentPrefab = value;

            EditorUtility.SetDirty(setting);

            PrefabLoaderEvents.OnPrefabCategoryChanged?.Invoke();
        }
    }

    public static class PrefabLoaderEvents
    {
        public static System.Action OnPrefabCategoryChanged;
    }

    [Overlay(typeof(SceneView), "Entity Loader")]
    public class EntityLoaderOverlay : ToolbarOverlay
    {
        EntityLoaderOverlay() : base(EntityLoaderToolbar.id)
        {
            displayName = "Entity Loader";
            this.collapsedIcon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture2D>
                ("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon.png") :
                AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.palexen.tools/Editor Default Resources/Prefab_Icon_2.png");
        }
    }
}

#endif
#endif
