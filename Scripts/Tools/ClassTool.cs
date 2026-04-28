/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © 2023 Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using System;
using UnityEngine;
using Palexen.Gameplay;
using UnityEngine.Events;
using Palexen.Scriptables;
using Palexen.Audio.Atmos;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.Tools
{
    #region ENUM
    public enum WorldActionMode { single, multiple }
    public enum GizmoColorUsage { self, context }
    public enum Icon3DMethod { distance, fadeDistance }
    public enum Icon3DUIUsage { canvasGroup, image }

    public enum ObjectManagerInteractionMode { activate, deactivate, destroy }
    public enum TargetAllowedVia { tag, layer }
    public enum AudioTransitionState { fadeIn, fadeOut }
    public enum AffectGeneralAmbience { yes, no }
    public enum InteractionButton { action, jump, change, crouch }
    public enum GetInputSchemaBehaviour { fromGameInputManager, fromInteractableScheme }
    public enum InputSchema { PC, nintendoSwitch, XBOX, playStation, touchScreen }
    #endregion

    #region OBJECTS MANAGER
    [Serializable]
    public class ObjectManager
    {
        [Tooltip("The name of the object collection")] public string _regionName = "Collection Name";
        [Tooltip("Object behavior")] public ObjectManagerInteractionMode objectsBehaviour;
        [Tooltip("Collection of objects in list format")] public GameObject[] _objects;

        public void ApplyChanges()
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                switch (objectsBehaviour)
                {
                    case ObjectManagerInteractionMode.activate:
                        _objects[i].SetActive(true);
                        break;

                    case ObjectManagerInteractionMode.deactivate:
                        _objects[i].SetActive(false);
                        break;

                    case ObjectManagerInteractionMode.destroy:
                        UnityEngine.Object.Destroy(_objects[i]);
                        break;
                }
            }
        }
    }
    #endregion

    #region MONOBEHAVIOUR TARGET

    [Serializable]
    public class BehaviourSet
    {
        public string _behaviourName = "New Script Behaviour";
        [Header("Set Behaviour of this script")]
        public UnityEvent _behaviour;

        public void ApplyBehaviour()
        {
            _behaviour.Invoke();
        }
    }

    #endregion

    #region CUSTOM INSPECTORS

#if UNITY_EDITOR

    #region TRIGGER OBJECT MANAGER
    [CustomEditor(typeof(TriggerObjectsManager))]
    public class TriggerObjectsManagerEditor : Editor
    {

        SerializedProperty _via;
        SerializedProperty _tag;
        SerializedProperty _layer;
        SerializedProperty _object;

        TriggerObjectsManager tgom;

        private void OnEnable()
        {
            tgom = (TriggerObjectsManager)target;
            _via = serializedObject.FindProperty("_targetAllowedVia");
            _tag = serializedObject.FindProperty("_tagName");
            _layer = serializedObject.FindProperty("_layerMask");
            _object = serializedObject.FindProperty("objects");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Trigger Object Manager</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("When something gets into the collider, you can enable, disable, or destroy that objects", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_via);

            EditorGUILayout.Space(5);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (tgom._targetAllowedVia == TargetAllowedVia.tag)
            {
                GUILayout.Box("The object will be affected if the <color=green>collider</color> has the tag specified in the <color=magenta>Tag</color> Name field", 
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_tag);
            }
            else
            {
                GUILayout.Box("The object will be affected if the <color=green>collider</color> is in the layer specified in the <color=magenta>Layer Mask</color> field",
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_layer);
            }
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(_object);

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();



            serializedObject.ApplyModifiedProperties();

            if (tgom.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Box Collider", PalexenEditorStyles.BigButton))
                {
                    if (tgom.gameObject.GetComponent<Collider>() == null)
                    {
                        tgom.gameObject.AddComponent<BoxCollider>();
                        tgom.gameObject.GetComponent<BoxCollider>().isTrigger = true;

                        tgom.gameObject.AddComponent<ShapeVisualizer>();

                    }
                }
            }

            if (tgom.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Sphere Collider", PalexenEditorStyles.BigButton))
                {
                    if (tgom.gameObject.GetComponent<Collider>() == null)
                    {
                        tgom.gameObject.AddComponent<SphereCollider>();
                        tgom.gameObject.GetComponent<SphereCollider>().isTrigger = true;

                        tgom.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (tgom.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Mesh Collider", PalexenEditorStyles.BigButton))
                {
                    if (tgom.gameObject.GetComponent<Collider>() == null)
                    {
                        tgom.gameObject.AddComponent<MeshCollider>();
                        tgom.gameObject.GetComponent<MeshCollider>().convex = true;
                        tgom.gameObject.GetComponent<MeshCollider>().isTrigger = true;

                        tgom.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (tgom.gameObject.GetComponent<Collider>() != null)
            {
                if (GUILayout.Button("Remove Collider", PalexenEditorStyles.BigButton))
                {
                    if (tgom.gameObject.GetComponent<Collider>() != null)
                    {
                        DestroyImmediate(tgom.gameObject.GetComponent<Collider>());
                    }

                    if (tgom.gameObject.GetComponent<ShapeVisualizer>() != null)
                    {
                        DestroyImmediate(tgom.gameObject.GetComponent<ShapeVisualizer>());
                    }
                }
            }
        }
    }
    #endregion

    #region GENERAL AMBIENCE

    [CustomEditor(typeof(GeneralAmbience))]
    public class GeneralAmbienceEditor : Editor
    {
        SerializedProperty _transition;
        SerializedProperty _source;
        SerializedProperty _minMax;
        SerializedProperty _speed;

        GeneralAmbience ga;

        private void OnEnable()
        {
            ga = (GeneralAmbience)target;

            _transition = serializedObject.FindProperty("transitionState");
            _source = serializedObject.FindProperty("ambienceSource");
            _minMax = serializedObject.FindProperty ("minMaxAudio");
            _speed = serializedObject.FindProperty("updateSpeed");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Ambience</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Create global ambience in this level", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_transition);
            EditorGUILayout.PropertyField(_source);
            EditorGUILayout.PropertyField(_minMax);
            EditorGUILayout.PropertyField(_speed);

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();

            if (ga.ambienceSource == null)
            {
                if (GUILayout.Button("Create Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() == null)
                    {
                        ga.gameObject.AddComponent<AudioSource>();
                        ga.gameObject.GetComponent<AudioSource>().loop = true;
                        ga.gameObject.GetComponent<GeneralAmbience>().ambienceSource = ga.gameObject.GetComponent<AudioSource>();
                    }
                }
            }

            if (ga.ambienceSource != null)
            {
                if (GUILayout.Button("Remove Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() != null)
                    {
                        ga.gameObject.GetComponent<GeneralAmbience>().ambienceSource = null;
                        DestroyImmediate(ga.gameObject.GetComponent<AudioSource>());
                    }
                }
            }
        }
    }

    #endregion

    #region AMBIENCE ZONE
    [CustomEditor(typeof(AmbienceZone))]
    public class AmbienceZoneEditor : Editor
    {
        AmbienceZone ga;
        SerializedProperty _via;
        SerializedProperty _tagName;
        SerializedProperty _layer;
        SerializedProperty _state;
        SerializedProperty _affect;
        SerializedProperty _source;
        SerializedProperty _minMax;
        SerializedProperty _speed;

        private void OnEnable()
        {
            ga = (AmbienceZone)target;
            _via = serializedObject.FindProperty("_targetAllowedVia");
            _tagName = serializedObject.FindProperty("_tagName");
            _layer = serializedObject.FindProperty("_layerMask");
            _state = serializedObject.FindProperty("transitionState");
            _affect = serializedObject.FindProperty("affectToGeneralAmbience");
            _source = serializedObject.FindProperty("ambienceZoneSource");
            _minMax = serializedObject.FindProperty("minMaxVolume");
            _speed = serializedObject.FindProperty("updateSpeed");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Ambience Zone</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Create Ambience Zone in this place", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_via);

            EditorGUILayout.Space(5);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (ga._targetAllowedVia == TargetAllowedVia.tag)
            {
                GUILayout.Box("The object will be affected if the <color=green>collider</color> has the tag specified in the <color=magenta>Tag</color> Name field",
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_tagName);
            }
            else
            {
                GUILayout.Box("The object will be affected if the <color=green>collider</color> is in the layer specified in the <color=magenta>Layer Mask</color> field",
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_layer);
            }
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(_state);
            EditorGUILayout.PropertyField(_affect);
            EditorGUILayout.PropertyField(_source);
            EditorGUILayout.PropertyField(_minMax);
            EditorGUILayout.PropertyField(_speed);

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();

            if (ga.ambienceZoneSource == null)
            {
                if (GUILayout.Button("Create Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() == null)
                    {
                        ga.gameObject.AddComponent<AudioSource>();

                        ga.gameObject.GetComponent<AudioSource>().loop = true;
                        ga.gameObject.GetComponent<AmbienceZone>().ambienceZoneSource = ga.gameObject.GetComponent<AudioSource>();
                    }
                }
            }

            if (ga.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Box Collider", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<Collider>() == null)
                    {
                        ga.gameObject.AddComponent<BoxCollider>();
                        ga.gameObject.GetComponent<BoxCollider>().isTrigger = true;

                        ga.gameObject.AddComponent<ShapeVisualizer>();

                    }
                }
            }

            if (ga.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Sphere Collider", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<Collider>() == null)
                    {
                        ga.gameObject.AddComponent<SphereCollider>();
                        ga.gameObject.GetComponent<SphereCollider>().isTrigger = true;

                        ga.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (ga.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Mesh Collider", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<Collider>() == null)
                    {
                        ga.gameObject.AddComponent<MeshCollider>();
                        ga.gameObject.GetComponent<MeshCollider>().convex = true;
                        ga.gameObject.GetComponent<MeshCollider>().isTrigger = true;

                        ga.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (GUILayout.Button("Reset Collider", PalexenEditorStyles.BigButton))
            {
                if (ga.gameObject.GetComponent<Collider>() != null)
                {
                    DestroyImmediate(ga.gameObject.GetComponent<Collider>());
                }

                if (ga.gameObject.GetComponent<ShapeVisualizer>() != null)
                {
                    DestroyImmediate(ga.gameObject.GetComponent<ShapeVisualizer>());
                }
            }
        }
    }
    #endregion

    #region TRIGGER EVENT

    [CustomEditor(typeof(TriggerEvent))]
    public class TriggerEventEditor : Editor
    {
        TriggerEvent te;
        SerializedProperty _via;
        SerializedProperty _tag;
        SerializedProperty _layer;
        SerializedProperty _event;
        private void OnEnable()
        {
            te = (TriggerEvent)target;
            _via = serializedObject.FindProperty("_targetAllowedVia");
            _tag = serializedObject.FindProperty("_tag");
            _layer = serializedObject.FindProperty("_layer");
            _event = serializedObject.FindProperty("_event");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);
            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Trigger Event</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("You can activate events by entering this collider", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
            serializedObject.Update();

            EditorGUILayout.PropertyField(_via);

            EditorGUILayout.Space(5);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (te._targetAllowedVia == TargetAllowedVia.tag)
            {
                GUILayout.Box("The event will be triggered if the <color=green>collider</color> has the tag specified in the <color=magenta>Tag</color> field",
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_tag);
            }
            else
            {
                GUILayout.Box("The event will be triggered if the <color=green>collider</color> is in the layer specified in the <color=magenta>Layer Mask</color> field",
                    PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                EditorGUILayout.PropertyField(_layer);
            }
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_event);

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();

            if (te.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Box Collider", PalexenEditorStyles.BigButton))
                {
                    if (te.gameObject.GetComponent<Collider>() == null)
                    {
                        te.gameObject.AddComponent<BoxCollider>();
                        te.gameObject.GetComponent<BoxCollider>().isTrigger = true;

                        te.gameObject.AddComponent<ShapeVisualizer>();

                    }
                }
            }

            if (te.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Sphere Collider", PalexenEditorStyles.BigButton))
                {
                    if (te.gameObject.GetComponent<Collider>() == null)
                    {
                        te.gameObject.AddComponent<SphereCollider>();
                        te.gameObject.GetComponent<SphereCollider>().isTrigger = true;

                        te.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (te.gameObject.GetComponent<Collider>() == null)
            {
                if (GUILayout.Button("Create Mesh Collider", PalexenEditorStyles.BigButton))
                {
                    if (te.gameObject.GetComponent<Collider>() == null)
                    {
                        te.gameObject.AddComponent<MeshCollider>();
                        te.gameObject.GetComponent<MeshCollider>().convex = true;
                        te.gameObject.GetComponent<MeshCollider>().isTrigger = true;
                        te.gameObject.AddComponent<ShapeVisualizer>();
                    }
                }
            }

            if (GUILayout.Button("Reset Collider", PalexenEditorStyles.BigButton))
            {
                if (te.gameObject.GetComponent<Collider>() != null)
                {
                    DestroyImmediate(te.gameObject.GetComponent<Collider>());
                }

                if (te.gameObject.GetComponent<ShapeVisualizer>() != null)
                {
                    DestroyImmediate(te.gameObject.GetComponent<ShapeVisualizer>());
                }
            }
        }
    }

    #endregion

    #region ASYNC LOADER

    [CustomEditor(typeof(AsyncResourcesLoader))]
    public class AsyncLoaderEditor : Editor
    {
        AsyncResourcesLoader async;
        SerializedProperty _res;
        SerializedProperty _timer;

        private void OnEnable()
        {
            async = (AsyncResourcesLoader)target;
            _res = serializedObject.FindProperty("gameplayResources");
            _timer = serializedObject.FindProperty("activationInterval");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Async Loader</color>", 
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Manage the spawn resources on a scene", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_res);
            EditorGUILayout.PropertyField(_timer);

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region SHAPE VISUALIZER

    [CustomEditor(typeof(ShapeVisualizer))]
    public class ShapeVisualizerEditor : Editor
    {
        ShapeVisualizer sp;
        SerializedProperty _color;

        private void OnEnable()
        {
            sp = (ShapeVisualizer)target;
            _color = serializedObject.FindProperty("shapeColor");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Shape Visualizer</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Allows draw gizmos to the Unity Editor, you can draw many shapes forms as you need", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_color);

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

#endif

    #endregion
}
