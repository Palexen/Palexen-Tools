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
using System;
using UnityEngine;
using Palexen.Gameplay;
using Palexen.Sequences;
using UnityEngine.Events;
using Palexen.Scriptables;
using Palexen.Audio.Atmos;
using Palexen.Gameplay.UI;
using Palexen.CustomPhysics;
using Palexen.Gameplay.Input;
using Palexen.Gameplay.Player;
using System.Collections.Generic;

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

    public enum FootstepsSurface { concrete, grass, water, glass, gravel, rock, sand, wood, dirt, snow, mud, metal }
    public enum SurfaceType { mesh, terrain }

    public enum HealthCondition { parent, single, byChilds }
    public enum HealthImportance { notImportant, important }

    public enum Language { english, spanish, french, german, japanese, chinese, korean, russian }
    public enum DialogAudioFeature { useAudio, noAudio }
    public enum Initializer { auto, manual }

    #endregion

    #region TERRAIN SURFACE
    [Serializable]
    public class TerrainSurface
    {
        public string textureName;
        public FootstepsSurface surfaceType;
    }
    #endregion

    #region OBJECTS MANAGER
    [Serializable]
    public class ObjectManager
    {
        [Tooltip("The name of the object collection")] public string _regionName = "Collection Name";
        [Tooltip("Object behavior")] public ObjectManagerInteractionMode objectsBehaviour;
        [Tooltip("Collection of objects in list format")] [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)] public GameObject[] _objects;

        /// <summary>
        /// call to activate, deactivate, or destroy objects within the Objects Manager array
        /// </summary>
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

        /// <summary>
        /// Call this method to invoke events that are handled within this component
        /// </summary>
        public void ApplyBehaviour()
        {
            _behaviour.Invoke();
        }
    }

    #endregion

    #region DIALOG SYSTEM

    [Serializable]
    public class DialogScript
    {
        public string scriptID = "Part 0";
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] public DialogContainer _dialogContainer;
    }

    [Serializable]
    public class DialogSequencer
    {
        public string _langName;
        public List<DialogScript> _sequence;
    }

    #endregion

    #region LANG TEXT

    [Serializable]
    public class LangText
    {
        public string _langName;
        [Space]
        [TextArea(3, 20)] public string _text;
    }

    #endregion

    #region CUSTOM INSPECTORS

#if UNITY_EDITOR

    #region INPUT SCHEMA

    [CustomEditor(typeof(GameInputSchema))]
    [CanEditMultipleObjects]
    public class GameInputSchemaEditor : Editor
    {
        GameInputSchema gis;
        SerializedProperty schema;
        SerializedProperty _actionSchema;
        private void OnEnable()
        {
            gis = (GameInputSchema)target;
            schema = serializedObject.FindProperty("schema");
            _actionSchema = serializedObject.FindProperty("_actionSchema");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Game Input Schema</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("It globally establishes an action control scheme to change the control scheme or separate " +
                "the platform.\r\n\r\nYou can also modify the scheme by activating a different one when calling the " +
                "instance and setting a new input with the type in the <color=green>SetSchema(InputSchema newSchema);</color> method.\r\n\r\n" +
                "This is essential when your users prefer their own input control and its respective scheme.", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 200));

            serializedObject.Update();
            EditorGUILayout.PropertyField(schema);
            EditorGUILayout.PropertyField(_actionSchema);
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region INTERACTABLE OBJECT
    [CustomEditor(typeof(InteractableComponent))]
    [CanEditMultipleObjects]
    public class InteractableObjectEditor : Editor
    {
        InteractableComponent io;
        SerializedProperty interactButton;
        SerializedProperty playMethods;
        SerializedProperty externalBehaviours;
        SerializedProperty objectManager;
        SerializedProperty motorVelocity;
        SerializedProperty vibrationTimer;
        SerializedProperty baseIcon;
        SerializedProperty baseButton;
        SerializedProperty inputSchemaBehaviour;
        SerializedProperty schema;
        SerializedProperty _PCButton;
        SerializedProperty _nintentdoSwitchButton;
        SerializedProperty _xBOXButton;
        SerializedProperty _PlayStationButton;
        SerializedProperty _touchScreenButton;

        private void OnEnable()
        {
            io = (InteractableComponent)target;
            interactButton = serializedObject.FindProperty("interactButton");
            playMethods =  serializedObject.FindProperty("playMethods");
            externalBehaviours = serializedObject.FindProperty("externalBehaviours");
            objectManager = serializedObject.FindProperty("objectManager");
            motorVelocity = serializedObject.FindProperty("motorVelocity");
            vibrationTimer = serializedObject.FindProperty("vibrationTimer");
            baseIcon = serializedObject.FindProperty("baseIcon");
            baseButton = serializedObject.FindProperty("baseButton");
            inputSchemaBehaviour = serializedObject.FindProperty("inputSchemaBehaviour");
            schema = serializedObject.FindProperty("schema");
            _PCButton = serializedObject.FindProperty("_PCButton");
            _nintentdoSwitchButton = serializedObject.FindProperty("_nintentdoSwitchButton");
            _xBOXButton = serializedObject.FindProperty("_xBOXButton");
            _PlayStationButton = serializedObject.FindProperty("_PlayStationButton");
            _touchScreenButton = serializedObject.FindProperty("_touchScreenButton");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Interactable Object</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("You can interact with (player needs <color=green>Player interaction Script</color>), mark this object as " +
                "<color=cyan>interactable layer</color>", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();
            EditorGUILayout.PropertyField(interactButton);
            EditorGUILayout.PropertyField(playMethods);
            EditorGUILayout.PropertyField(externalBehaviours);
            EditorGUILayout.PropertyField(objectManager);
            EditorGUILayout.PropertyField(motorVelocity);
            EditorGUILayout.PropertyField(vibrationTimer);
            EditorGUILayout.PropertyField(baseIcon);
            EditorGUILayout.PropertyField(baseButton);
            EditorGUILayout.PropertyField(inputSchemaBehaviour);
            EditorGUILayout.PropertyField(schema);
            EditorGUILayout.PropertyField(_PCButton);
            EditorGUILayout.PropertyField(_nintentdoSwitchButton);
            EditorGUILayout.PropertyField(_xBOXButton);
            EditorGUILayout.PropertyField(_PlayStationButton);
            EditorGUILayout.PropertyField(_touchScreenButton);
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region PLAYER INTERACTION
    [CustomEditor(typeof(PlayerInteraction))]
    [CanEditMultipleObjects]
    public class PlayerInteractionEditor : Editor
    {
        PlayerInteraction pi;
        SerializedProperty button;
        SerializedProperty interactableLayerMask;
        SerializedProperty interactionMethod;
        SerializedProperty maxDistance;

        private void OnEnable()
        {
            pi = (PlayerInteraction)target;
            button = serializedObject.FindProperty("button");
            interactableLayerMask = serializedObject.FindProperty("interactableLayerMask");
            interactionMethod = serializedObject.FindProperty("interactionMethod");
            maxDistance = serializedObject.FindProperty("maxDistance");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Player Interaction</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Setup the player interaction system, you can set the layer for interactable objects, " +
                "the method to detect them and the max distance to interact", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 90));

            serializedObject.Update();
            EditorGUILayout.PropertyField(button);
            EditorGUILayout.PropertyField(interactableLayerMask);
            EditorGUILayout.PropertyField(interactionMethod);
            EditorGUILayout.PropertyField(maxDistance);
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region INTERACTABLE HUD

    [CustomEditor(typeof(InteractableHUD))]
    [CanEditMultipleObjects]
    public class InteractableHUDEditor : Editor
    {
        InteractableHUD ih;
        SerializedProperty m_animator;
        SerializedProperty baseImage;
        SerializedProperty baseImageButton;

        private void OnEnable()
        {
            ih = (InteractableHUD)target;
            m_animator = serializedObject.FindProperty("m_animator");
            baseImage = serializedObject.FindProperty("baseImage");
            baseImageButton = serializedObject.FindProperty("baseImageButton");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Interactable HUD</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("A Representation on screen when you can interact with many objects in yout game", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_animator);
            EditorGUILayout.PropertyField(baseImage);
            EditorGUILayout.PropertyField(baseImageButton);
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region WORDL ICON

    [CustomEditor(typeof(WorldIcon))]
    [CanEditMultipleObjects]
    public class WorldIconEditor : Editor
    {
        WorldIcon wi;
        SerializedProperty m_3DIconMethod;
        SerializedProperty sizeControl;
        SerializedProperty maxDistance;
        SerializedProperty m_UIFadeMethod;
        SerializedProperty canvasGroup;
        SerializedProperty icon;
        SerializedProperty opacity;
        SerializedProperty minDistance;
        SerializedProperty fadeSpeed;

        private void OnEnable()
        {
            wi = (WorldIcon)target;
            m_3DIconMethod = serializedObject.FindProperty("m_3DIconMethod");
            sizeControl = serializedObject.FindProperty("sizeControl");
            maxDistance = serializedObject.FindProperty("maxDistance");
            m_UIFadeMethod = serializedObject.FindProperty("m_UIFadeMethod");
            canvasGroup = serializedObject.FindProperty("canvasGroup");
            icon = serializedObject.FindProperty("icon");
            opacity = serializedObject.FindProperty("opacity");
            minDistance = serializedObject.FindProperty("minDistance");
            fadeSpeed = serializedObject.FindProperty("fadeSpeed");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>World Icon</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Create world icon in this object, you can set the method to show it and the distance to show it", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_3DIconMethod);

            if (wi.m_3DIconMethod == Icon3DMethod.distance)
            {
                EditorGUILayout.PropertyField(sizeControl);
                EditorGUILayout.PropertyField(maxDistance);
            }
            else
            {
                EditorGUILayout.PropertyField(m_UIFadeMethod);

                if (wi.m_UIFadeMethod == Icon3DUIUsage.canvasGroup)
                {
                    GUILayout.Box("The icon will fade using the canvas group component, you need to assign it in the field below",
                        PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                    EditorGUILayout.PropertyField(canvasGroup);
                }
                else
                {
                    GUILayout.Box("The icon will fade using the image component, you need to assign it in the field below",
                        PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
                    EditorGUILayout.PropertyField(icon);
                }
                EditorGUILayout.PropertyField(opacity);
                EditorGUILayout.PropertyField(minDistance);
                EditorGUILayout.PropertyField(fadeSpeed);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

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

    #region FOOTSTEPS SYSTEM

    [CustomEditor(typeof(FootstepsSystem))]
    public class FootstepsSystemEditor : Editor
    {
        FootstepsSystem fs;
        SerializedProperty surfaceBehaviour;
        SerializedProperty meshLayerMask;
        SerializedProperty terrainLayerMask;
        SerializedProperty currentSurface;
        SerializedProperty foots;
        SerializedProperty concrete;
        SerializedProperty grass;
        SerializedProperty water;
        SerializedProperty glass;
        SerializedProperty wood;
        SerializedProperty gravel;
        SerializedProperty rock;
        SerializedProperty sand;
        SerializedProperty dirt;
        SerializedProperty snow;
        SerializedProperty mud;
        SerializedProperty metal;
        SerializedProperty terrainTextureIndex;
        SerializedProperty terrainSurfaceSettings;
        SerializedProperty voice;
        SerializedProperty climb;

        private void OnEnable()
        {
            fs = (FootstepsSystem)target;
            surfaceBehaviour = serializedObject.FindProperty("surfaceBehaviour");
            meshLayerMask = serializedObject.FindProperty("meshLayerMask");
            terrainLayerMask = serializedObject.FindProperty("terrainLayerMask");
            currentSurface = serializedObject.FindProperty("currentSurface");
            foots = serializedObject.FindProperty("foots");
            concrete = serializedObject.FindProperty("concrete");
            grass = serializedObject.FindProperty("grass");
            water = serializedObject.FindProperty("water");
            glass = serializedObject.FindProperty("glass");
            wood = serializedObject.FindProperty("wood");
            gravel = serializedObject.FindProperty("gravel");
            rock = serializedObject.FindProperty("rock");
            sand = serializedObject.FindProperty("sand");
            dirt = serializedObject.FindProperty("dirt");
            snow = serializedObject.FindProperty("snow");
            mud = serializedObject.FindProperty("mud");
            metal = serializedObject.FindProperty("metal");
            terrainTextureIndex = serializedObject.FindProperty("terrainTextureIndex");
            terrainSurfaceSettings = serializedObject.FindProperty("terrainSurfaceSettings");
            voice = serializedObject.FindProperty("voice");
            climb = serializedObject.FindProperty("climb");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);
            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Footsteps System</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Create footsteps system in this level, you can set different audio clips for each terrain texture", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();
            EditorGUILayout.PropertyField(surfaceBehaviour);
            EditorGUILayout.PropertyField(meshLayerMask);
            EditorGUILayout.PropertyField(terrainLayerMask);
            EditorGUILayout.PropertyField(currentSurface);
            EditorGUILayout.PropertyField(foots);
            EditorGUILayout.PropertyField(concrete);
            EditorGUILayout.PropertyField(grass);
            EditorGUILayout.PropertyField(water);
            EditorGUILayout.PropertyField(glass);
            EditorGUILayout.PropertyField(wood);
            EditorGUILayout.PropertyField(gravel);
            EditorGUILayout.PropertyField(rock);
            EditorGUILayout.PropertyField(sand);
            EditorGUILayout.PropertyField(dirt);
            EditorGUILayout.PropertyField(snow);
            EditorGUILayout.PropertyField(mud);
            EditorGUILayout.PropertyField(metal);
            EditorGUILayout.PropertyField(terrainTextureIndex);
            EditorGUILayout.PropertyField(terrainSurfaceSettings);
            EditorGUILayout.PropertyField(voice);
            EditorGUILayout.PropertyField(climb);
            serializedObject.ApplyModifiedProperties();
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

    #region HEALTH SYSTEM

    [CustomEditor(typeof(HealthSystem))]
    public class HealthGOEditor : Editor
    {
        HealthSystem hg;
        SerializedProperty _behaviour;
        SerializedProperty _healthRange;
        SerializedProperty _exceededThreshold;
        SerializedProperty _afterKillObject;
        SerializedProperty _afterExceeded;
        SerializedProperty _animator;
        SerializedProperty dieTriggerNames;
        SerializedProperty onFinishDieAnimations;
        SerializedProperty _rigidbodies;

        private void OnEnable()
        {
            hg = (HealthSystem)target;
            _behaviour = serializedObject.FindProperty("_behaviour");
            _healthRange = serializedObject.FindProperty("_healthRange");
            _exceededThreshold = serializedObject.FindProperty("_exceededThreshold");
            _afterKillObject = serializedObject.FindProperty("_afterKillObject");
            _afterExceeded = serializedObject.FindProperty("_afterExceeded");
            _animator = serializedObject.FindProperty("_animator");
            dieTriggerNames = serializedObject.FindProperty("dieTriggerNames");
            onFinishDieAnimations = serializedObject.FindProperty("onFinishDieAnimations");
            _rigidbodies = serializedObject.FindProperty("_rigidbodies");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);
            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Health System</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("Manage the HP of this object; after it reaches 0, the object will handle after-kill events, implemented in an event", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 60));

            Color c = setting.contextSeparatorColor;

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_behaviour);

            if (hg._behaviour == HealthCondition.byChilds)
            {
                EditorGUILayout.HelpBox("Configure the other health component scripts to add HP to this object, " +
                    "making sure to initialize the event with AddHealthAtParent and setting this object " +
                    "as the parent of the component.", MessageType.Info);
            }

            if (hg._behaviour == HealthCondition.single)
            {
                EditorGUILayout.PropertyField(_healthRange);
            }

            if (hg._behaviour == HealthCondition.parent)
            {
                EditorGUILayout.PropertyField(_healthRange);
                EditorGUILayout.HelpBox("Parental usage is not available for this script, so marking it as a " +
                    "parent will result in it being used as a single.", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(_exceededThreshold);

            GUILayout.Space(10);

            string currentHeader1 = hg.showEventsGroup ? "Hide Events" : "Show and Setup Events";

            hg.showEventsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(hg.showEventsGroup, currentHeader1);
            EditorGUI.indentLevel++;
            if (hg.showEventsGroup)
            {
                EditorGUILayout.PropertyField(_afterKillObject);
                EditorGUILayout.PropertyField(_afterExceeded);
                EditorGUILayout.PropertyField(onFinishDieAnimations);
                EditorGUILayout.HelpBox("You can add components and run them externally or in animation events.", MessageType.Info);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            if (!hg.useAnimationFeatures)
            {
                if(GUILayout.Button("Enable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hg.useAnimationFeatures = true;
                }
            }


            if (hg.useAnimationFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animation Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

                EditorGUILayout.PropertyField(_animator);
                EditorGUILayout.PropertyField(dieTriggerNames);

                if (GUILayout.Button("Disable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hg.useAnimationFeatures = false;
                }
            }

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);


            if (!hg.usePhysicsFeatures)
            {
                if (GUILayout.Button("Enable Physics Features", PalexenEditorStyles.BigButton))
                {
                    hg.usePhysicsFeatures = true;
                }
            }

            if (hg.usePhysicsFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Ragdoll or Physics Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

                EditorGUILayout.PropertyField(_rigidbodies);
                EditorGUILayout.Space(10);

                if (GUILayout.Button("Fetch Rigidbodies"))
                {
                    hg.FetchRigidbodies();
                }

                if(hg._rigidbodies.Length >= 1)
                {
                    if (GUILayout.Button("Draw Gizmos on physics"))
                    {
                        foreach (Rigidbody rb in hg._rigidbodies)
                        {
                            if (rb != null || rb.gameObject.GetComponent<ShapeVisualizer>() == null)
                            {
                                if (rb.gameObject.GetComponent<ShapeVisualizer>() == null)
                                {
                                    rb.gameObject.AddComponent<ShapeVisualizer>();
                                }
                            }
                        }
                    }

                    if (GUILayout.Button("Add Velocity Limiter"))
                    {
                        foreach (Rigidbody rb in hg._rigidbodies)
                        {
                            if (rb != null)
                            {
                                if (rb.gameObject.GetComponent<RigidbodyVelocityLimitation>() == null)
                                {
                                    rb.gameObject.AddComponent<RigidbodyVelocityLimitation>();
                                }
                            }
                        }
                    }

                    RigidbodyVelocityLimitation[] rl;
                    rl = hg.gameObject.GetComponentsInChildren<RigidbodyVelocityLimitation>();

                    hg.showVelocityLimiters = EditorGUILayout.BeginFoldoutHeaderGroup(hg.showVelocityLimiters, "Rigidbody Velocity Limitation Settings");
                    EditorGUI.indentLevel++;

                    if (hg.showVelocityLimiters)
                    {
                        if (rl.Length >= 1)
                        {
                            for (int i = 0; i < rl.Length; i++)
                            {
                                if (rl[i] != null)
                                {
                                    rl[i].maxVelocity = EditorGUILayout.FloatField($"{rl[i].gameObject.name}", rl[i].maxVelocity);
                                }
                            }
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndFoldoutHeaderGroup();

                    PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

                    hg.showShapeSettings = EditorGUILayout.BeginFoldoutHeaderGroup(hg.showShapeSettings, "Shape Visualizer Settings");

                    EditorGUI.indentLevel++;

                    if (hg.showShapeSettings)
                    {
                        foreach (ShapeVisualizer sv in hg.gameObject.GetComponentsInChildren<ShapeVisualizer>())
                        {
                            if (sv != null)
                            {
                                sv.shapeColor = EditorGUILayout.ColorField($"{sv.gameObject.name}", sv.shapeColor);
                            }
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndFoldoutHeaderGroup();

                    PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

                    if (GUILayout.Button("Mark as Kinematic Ragdoll or physics"))
                    {
                        hg.KinematicRagdoll();
                    }
                }

                PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

                if (GUILayout.Button("Disable Physics Features", PalexenEditorStyles.BigButton))
                {
                    hg.usePhysicsFeatures = false;
                }
            }

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region HEALTH COMPONENT

    [CustomEditor(typeof(HealthComponent))]
    public class HealthComponentEditor : Editor
    {
        HealthComponent hc;
        SerializedProperty _health;
        SerializedProperty _exceededThreshold;
        SerializedProperty _affectsOn;
        SerializedProperty _importanceLevel;
        SerializedProperty _atStart;
        SerializedProperty _onTakeDamage;
        SerializedProperty _atDie;
        SerializedProperty _atExceeded;
        SerializedProperty _healthParent;
        SerializedProperty _animator;
        SerializedProperty triggerNames;

        private void OnEnable()
        {
            hc = (HealthComponent)target;
            _health = serializedObject.FindProperty("_health");
            _exceededThreshold = serializedObject.FindProperty("_exceededThreshold");
            _affectsOn = serializedObject.FindProperty("_affectsOn");
            _importanceLevel = serializedObject.FindProperty("_importanceLevel");
            _atStart = serializedObject.FindProperty("_atStart");
            _onTakeDamage = serializedObject.FindProperty("_onTakeDamage");
            _atDie = serializedObject.FindProperty("_atDie");
            _atExceeded = serializedObject.FindProperty("_atExceeded");
            _healthParent = serializedObject.FindProperty("_healthParent");
            _animator = serializedObject.FindProperty("_animator");
            triggerNames = serializedObject.FindProperty("triggerNames");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);
            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Health Component</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("It manages the HP of this object and handles events that occur when it is affected", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            Color c = setting.contextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_affectsOn);

            if(hc._affectsOn == HealthCondition.parent)
            {
                EditorGUILayout.HelpBox("This component will affect the parent health, so make sure to set a parent with " +
                    "a HealthGO or Health System script and set it as the health parent", MessageType.Info);
                EditorGUILayout.PropertyField(_healthParent);
            }

            if (hc._affectsOn == HealthCondition.single)
            {
                EditorGUILayout.HelpBox("This component will affect only itself, and a parent script is not necessary", MessageType.Info);
            }

            if (hc._affectsOn == HealthCondition.byChilds)
            {
                EditorGUILayout.HelpBox("The `by child` option is not available because this script functions as a component, " +
                    "so marking it as `by child` will make it use the single component.", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(_health);
            EditorGUILayout.PropertyField(_exceededThreshold);

            EditorGUILayout.PropertyField(_importanceLevel);

            if(hc._importanceLevel == HealthImportance.notImportant)
            {
                EditorGUILayout.HelpBox("When marked as Not Important, the object can die independently without affecting the parent.", MessageType.Info);
            }

            if(hc._importanceLevel == HealthImportance.important)
            {
                EditorGUILayout.HelpBox("When marked as Important, the death of the object will cause the parent to lose all its HP" +
                    " and die instantly, good for headshots or too critical damages!.", MessageType.Info);
            }

            string headerText = hc.showEvents ? "Hide Events" : "Show and Setup Events";

            hc.showEvents = EditorGUILayout.BeginFoldoutHeaderGroup(hc.showEvents, headerText);

            EditorGUI.indentLevel++;

            if (hc.showEvents)
            {
                EditorGUILayout.PropertyField(_atStart);
                EditorGUILayout.PropertyField(_onTakeDamage);
                EditorGUILayout.PropertyField(_atDie);
                EditorGUILayout.PropertyField(_atExceeded);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;

            if(!hc.animationFeatures)
            {
                if (GUILayout.Button("Enable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hc.animationFeatures = true;
                }
            }

            if (hc.animationFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animation Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
                EditorGUILayout.PropertyField(_animator);
                EditorGUILayout.PropertyField(triggerNames);

                if (GUILayout.Button("Disable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hc.animationFeatures = false;
                }
            }
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            string headerText2 = hc.showPresets ? "Hide Presets" : "Show and Setup Presets";

            hc.showPresets = EditorGUILayout.BeginFoldoutHeaderGroup(hc.showPresets, headerText2);
            EditorGUI.indentLevel++;

            if (hc.showPresets)
            {
                if(GUILayout.Button("Set Preset: Head", PalexenEditorStyles.BigButton))
                {
                    hc.HeadExample();
                }

                if (GUILayout.Button("Set Preset: Chest", PalexenEditorStyles.BigButton))
                {
                    hc.ChestExample();
                }

                if (GUILayout.Button("Set Preset: Common Part", PalexenEditorStyles.BigButton))
                {
                    hc.BodyPartExample();
                }
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region DIALOG SYSTEM

    [CustomEditor(typeof(DialogSystem))]
    public class DialogSystemEditor : Editor
    {
        DialogSystem _dialog;
        SerializedProperty _lang;
        SerializedProperty _catchLang;
        SerializedProperty _dialogAudioFeature; 
        SerializedProperty _afterComplete;
        SerializedProperty _langAudioSource;
        SerializedProperty _subtitles;
        SerializedProperty _dialogSequencer;

        SerializedProperty isPlaying;
        SerializedProperty playback;
        SerializedProperty currentSequence;
        SerializedProperty dialogComplete;
        SerializedProperty playbackTimer;
        SerializedProperty nextToPlay;

        private void OnEnable()
        {
            _dialog = (DialogSystem)target;
            _lang = serializedObject.FindProperty("_lang");
            _catchLang = serializedObject.FindProperty("_catchLang");
            _dialogAudioFeature = serializedObject.FindProperty("_dialogAudioFeature");
            _afterComplete = serializedObject.FindProperty("_afterComplete");
            _langAudioSource = serializedObject.FindProperty("_langAudioSource");
            _subtitles = serializedObject.FindProperty("_subtitles");
            _dialogSequencer = serializedObject.FindProperty("_dialogSequencer");

            isPlaying = serializedObject.FindProperty("isPlaying");
            playback = serializedObject.FindProperty("playback");
            currentSequence = serializedObject.FindProperty("currentSequence");
            dialogComplete = serializedObject.FindProperty("dialogComplete");
            playbackTimer = serializedObject.FindProperty("playbackTimer");
            nextToPlay = serializedObject.FindProperty("nextToPlay");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Dialog System</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("Dialogue system for all your contexts, whether for narration or NPCs. It supports " +
                "multiple languages, and you can also use audio for the dialogue system.\r\n\r\nTip: Make sure to manage your " +
                "project well when configuring all your dialogues, whether they are text, " +
                "voice, or both!", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 120));

            Color c = setting.contextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_lang);
            EditorGUILayout.PropertyField(_catchLang);
            EditorGUILayout.PropertyField(_dialogAudioFeature);

            if (_dialog._dialogAudioFeature == DialogAudioFeature.useAudio)
            {
                EditorGUILayout.PropertyField(_langAudioSource);
            }

            EditorGUILayout.PropertyField(_afterComplete);
            EditorGUILayout.PropertyField(_subtitles);

            EditorGUILayout.Space();

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Sequences & Languages Setup</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize, TextAnchor.MiddleLeft));
            EditorGUILayout.PropertyField(_dialogSequencer);

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (!_dialog.debugMode)
            {
                if (GUILayout.Button("Enter Debug Mode"))
                {
                    _dialog.debugMode = true;

                }
            }
            else
            {
                if (GUILayout.Button("Exit Debug Mode"))
                {
                    _dialog.debugMode = false;
                }
            }

            if (_dialog.debugMode)
            {
                EditorGUILayout.PropertyField(isPlaying);
                EditorGUILayout.PropertyField(playback);
                EditorGUILayout.PropertyField(currentSequence);
                EditorGUILayout.PropertyField(dialogComplete);
                EditorGUILayout.PropertyField(playbackTimer);
                EditorGUILayout.PropertyField(nextToPlay);
            }
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;

            if (GUILayout.Button("Play", PalexenEditorStyles.BigButton))
            {
                _dialog.PlayDialog();
            }
            if (GUILayout.Button("Replay", PalexenEditorStyles.BigButton))
            {
                _dialog.RePlay();
            }
            if (GUILayout.Button("Break", PalexenEditorStyles.BigButton))
            {
                _dialog.BreakIntoDialogue();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region LANG

    [CustomEditor(typeof(LangManager))]
    public class LangManagerEditor : Editor
    {
        LangManager lm;
        SerializedProperty _lang;

        private void OnEnable()
        {
            lm = (LangManager)target;
            _lang = serializedObject.FindProperty("_lang");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Lang Manager</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("This handles the game's language; you can update this singleton using the <color=red>SetLang();</color> method." +
                "\r\n\r\n<color=green>Note:</color> Other scripts that natively support this system contain a method to update the language, " +
                "but if you've already created other systems that use this singleton, you'll need to update it manually.", 
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 150));

            Color c = setting.contextSeparatorColor;

            serializedObject.Update();

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.PropertyField(_lang);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            if (EditorApplication.isPlaying)
            {
                GUI.color = c;
                EditorGUILayout.HelpBox("Debug Purposes", MessageType.None);
                GUI.color = Color.white;

                if (GUILayout.Button("Set English", PalexenEditorStyles.BigButton))
                {
                    lm.SetEnglish();
                }
                if (GUILayout.Button("Set Spanish", PalexenEditorStyles.BigButton))
                {
                    lm.SetSpanish();
                }
                if (GUILayout.Button("Set French", PalexenEditorStyles.BigButton))
                {
                    lm.SetFrench();
                }
                if (GUILayout.Button("Set German", PalexenEditorStyles.BigButton))
                {
                    lm.SetGerman();
                }
                if (GUILayout.Button("Set Japanese", PalexenEditorStyles.BigButton))
                {
                    lm.SetJapanese();
                }
                if (GUILayout.Button("Set Chinese", PalexenEditorStyles.BigButton))
                {
                    lm.SetChinese();
                }
                if (GUILayout.Button("Set Korean", PalexenEditorStyles.BigButton))
                {
                    lm.SetKorean();
                }
                if (GUILayout.Button("Set Russian", PalexenEditorStyles.BigButton))
                {
                    lm.SetRussian();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region TEXT TRANSLATOR

    [CustomEditor(typeof(LangTextConversion))]
    public class LangTextConversionEditor : Editor
    {
        LangTextConversion _ltc;
        SerializedProperty _lang;
        SerializedProperty _catchLang;
        SerializedProperty _text;
        SerializedProperty _conversions;

        private void OnEnable()
        {
            _ltc = (LangTextConversion)target;
            _lang = serializedObject.FindProperty("_lang");
            _catchLang = serializedObject.FindProperty("_catchLang");
            _text = serializedObject.FindProperty("_text");
            _conversions = serializedObject.FindProperty("_conversions");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Text Translator</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("Translate the text into the selected language, previously configured in a subtitles component.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 60));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_lang);
            EditorGUILayout.PropertyField(_catchLang);
            EditorGUILayout.PropertyField(_text);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.PropertyField(_conversions);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region RIGIDBODY LIMITER

    [CustomEditor(typeof(RigidbodyVelocityLimitation))]
    public class RigidbodyVelocityLimitationEditor : Editor
    {
        RigidbodyVelocityLimitation rb;
        SerializedProperty maxVelocity;

        private void OnEnable()
        {
            rb = (RigidbodyVelocityLimitation)target;
            maxVelocity = serializedObject.FindProperty("maxVelocity");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);
            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Rigidbody Velocity Limiter</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            GUILayout.Box("Limit the velocity of this Rigidbody", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();
            EditorGUILayout.PropertyField(maxVelocity);
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
