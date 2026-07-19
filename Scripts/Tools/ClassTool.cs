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
using Palexen.Levels;
using Palexen.Gameplay;
using UnityEngine.Audio;
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
    public enum DialogOrder { sequenced, random }
    public enum SubtitlesUsage {yes, no}
    public enum Initializer { auto, manual }
    public enum LevelLoadMode { catchAndLoad, loadOnly }
    public enum LoadingBarMode { none, slider, fill }

    public enum AmbienceZoneBehaviour { ambience, snapshots }

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

    #region QUICK PREFABS

    [Serializable]
    public class QuickPrefab
    {
        public string _label = "Prop";
        public string _icon = "📦";
    }

    #endregion

    #region AUDIO SNAPSHOT

    [Serializable]
    public class AudioSnapshot
    {
        public string _SnapshotName;
        [FieldColor(FieldPropertyColor.yellow)] public AudioMixerSnapshot _snapshot;
        [Range(0, 1)] public float _weightEnter;
        [Range(0, 1)] public float _weightExit;
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

            if (wi.IconMethod == Icon3DMethod.distance)
            {
                EditorGUILayout.PropertyField(sizeControl);
                EditorGUILayout.PropertyField(maxDistance);
            }
            else
            {
                EditorGUILayout.PropertyField(m_UIFadeMethod);

                if (wi.FadeMethod == Icon3DUIUsage.canvasGroup)
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
    [CanEditMultipleObjects]
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
            if (tgom.TargetAllowedVia == TargetAllowedVia.tag)
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

    #region ATMOS

    [CustomEditor(typeof(Atmos))]
    [CanEditMultipleObjects]
    public class GeneralAmbienceEditor : Editor
    {
        SerializedProperty _transition;
        SerializedProperty _source;
        SerializedProperty _minMax;
        SerializedProperty _speed;

        Atmos ga;

        private void OnEnable()
        {
            ga = (Atmos)target;

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

            if (ga.AmbienceSource == null)
            {
                if (GUILayout.Button("Create Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() == null)
                    {
                        ga.gameObject.AddComponent<AudioSource>();
                        ga.gameObject.GetComponent<AudioSource>().loop = true;
                        ga.gameObject.GetComponent<Atmos>().AmbienceSource = ga.gameObject.GetComponent<AudioSource>();
                    }
                }
            }

            if (ga.AmbienceSource != null)
            {
                if (GUILayout.Button("Remove Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() != null)
                    {
                        ga.gameObject.GetComponent<Atmos>().AmbienceSource = null;
                        DestroyImmediate(ga.gameObject.GetComponent<AudioSource>());
                    }
                }
            }
        }
    }

    #endregion

    #region ATMOS ZONE
    [CustomEditor(typeof(AtmosZone))]
    [CanEditMultipleObjects]
    public class AmbienceZoneEditor : Editor
    {
        AtmosZone ga;
        SerializedProperty _behaviour;
        SerializedProperty _via;
        SerializedProperty _tagName;
        SerializedProperty _layer;
        SerializedProperty _state;
        SerializedProperty _affect;
        SerializedProperty _source;
        SerializedProperty _minMax;
        SerializedProperty _speed;
        SerializedProperty _master;
        SerializedProperty _timeToReach;
        SerializedProperty _snapshots;
        SerializedProperty _weightsOnEnter;
        SerializedProperty _weightsOnExit;
        SerializedProperty _onTriggerEnter;
        SerializedProperty _onTriggerExit;
        SerializedProperty _snapshotsSetup;

        private void OnEnable()
        {
            ga = (AtmosZone)target;
            _behaviour = serializedObject.FindProperty("_behaviour");
            _via = serializedObject.FindProperty("_targetAllowedVia");
            _tagName = serializedObject.FindProperty("_tagName");
            _layer = serializedObject.FindProperty("_layerMask");
            _state = serializedObject.FindProperty("transitionState");
            _affect = serializedObject.FindProperty("affectToGeneralAmbience");
            _source = serializedObject.FindProperty("ambienceZoneSource");
            _minMax = serializedObject.FindProperty("minMaxVolume");
            _speed = serializedObject.FindProperty("updateSpeed");
            _master = serializedObject.FindProperty("_master");
            _timeToReach = serializedObject.FindProperty("_timeToReach");
            _snapshots = serializedObject.FindProperty("_snapshots");
            _weightsOnEnter = serializedObject.FindProperty("_weightsOnEnter");
            _weightsOnExit = serializedObject.FindProperty("_weightsOnExit");
            _onTriggerEnter = serializedObject.FindProperty("_onTriggerEnter");
            _onTriggerExit = serializedObject.FindProperty("_onTriggerExit");
            _snapshotsSetup = serializedObject.FindProperty("_snapshotsSetup");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Ambience Zone</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Create Ambience Zone in this place", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_behaviour);
            EditorGUILayout.PropertyField(_via);

            EditorGUILayout.Space(5);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (ga.TargetType == TargetAllowedVia.tag)
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

            if (ga.Behaviour == AmbienceZoneBehaviour.ambience)
            {
                EditorGUILayout.PropertyField(_state);
                EditorGUILayout.PropertyField(_affect);
                EditorGUILayout.PropertyField(_source);
                EditorGUILayout.PropertyField(_minMax);
                EditorGUILayout.PropertyField(_speed);
            }

            if (ga.Behaviour == AmbienceZoneBehaviour.snapshots)
            {
                EditorGUILayout.PropertyField(_master);
                EditorGUILayout.PropertyField(_timeToReach);
                /*EditorGUILayout.PropertyField(_snapshots);
                EditorGUILayout.PropertyField(_weightsOnEnter);
                EditorGUILayout.PropertyField(_weightsOnExit);*/

                EditorGUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Snapshot");
                GUILayout.Label("On Enter");
                GUILayout.Label("On Exit");

                EditorGUILayout.EndHorizontal();

                PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

                EditorGUILayout.PropertyField(_snapshotsSetup, true);
            }

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();

            if (!ga.AddEventsCapability)
            {
                if (GUILayout.Button("Add Events Capability", PalexenEditorStyles.BigButton))
                {
                    ga.AddEventsCapability = true;
                }
            }

            if (ga.AddEventsCapability)
            {
                EditorGUILayout.PropertyField(_onTriggerEnter);
                EditorGUILayout.PropertyField(_onTriggerExit);

                if (GUILayout.Button("Remove Events Capability", PalexenEditorStyles.BigButton))
                {
                    ga.AddEventsCapability = false;
                }
            }

            EditorGUILayout.Separator();
            GUI.color = setting.contextSeparatorColor;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.Separator();

            if (ga.AmbienceZoneSource == null)
            {
                if (GUILayout.Button("Create Audio Source", PalexenEditorStyles.BigButton))
                {
                    if (ga.gameObject.GetComponent<AudioSource>() == null)
                    {
                        ga.gameObject.AddComponent<AudioSource>();

                        ga.gameObject.GetComponent<AudioSource>().loop = true;
                        ga.gameObject.GetComponent<AtmosZone>().AmbienceZoneSource = ga.gameObject.GetComponent<AudioSource>();
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

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endregion

    #region AUDIO SNAPSHOT DRAWER

    [CustomPropertyDrawer(typeof(AudioSnapshot))]
    public class AudioSnapshotEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty snapshot = property.FindPropertyRelative("_snapshot");
            SerializedProperty weightEnter = property.FindPropertyRelative("_weightEnter");
            SerializedProperty weightExit = property.FindPropertyRelative("_weightExit");

            EditorGUI.BeginProperty(position, label, property);

            Rect contentPos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float halfWidth = position.width / 3f;

            Rect snapshotRect = new Rect(position.x, position.y, halfWidth - 5, contentPos.height);
            Rect weightRectEnter = new Rect(position.x + halfWidth, contentPos.y, halfWidth - 5, contentPos.height);
            Rect weightRectExit = new Rect(position.x + (halfWidth * 2), contentPos.y, halfWidth - 5, contentPos.height);

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(snapshotRect, snapshot, GUIContent.none);
            EditorGUI.PropertyField(weightRectEnter, weightEnter, GUIContent.none);
            EditorGUI.PropertyField(weightRectExit, weightExit, GUIContent.none);

            EditorGUI.indentLevel = oldIndent;

            /*weightEnter.floatValue = EditorGUI.Slider(weightRectEnter, weightEnter.floatValue, 0f, 1f);
            weightExit.floatValue = EditorGUI.Slider(weightRectExit, weightExit.floatValue, 0f, 1f);*/

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    #endregion

    #region TRIGGER EVENT

    [CustomEditor(typeof(TriggerEvent))]
    [CanEditMultipleObjects]
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
            if (te.TargetAllowedVia == TargetAllowedVia.tag)
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
    [CanEditMultipleObjects]
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
    [CanEditMultipleObjects]
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
    [CanEditMultipleObjects]
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

            if (Application.isPlaying)
            {
                float current = hg.CurrentHealth;
                float maxValue = hg.HealthRange.y;

                float progress = Mathf.Clamp01(current / maxValue);

                Rect bar = EditorGUILayout.GetControlRect(false, 22);

                if (current > 0)
                {
                    EditorGUI.ProgressBar(bar, progress, "Current Health: " + current + "Hp");
                } 
                else
                {
                    EditorGUI.ProgressBar(bar, 0, "Your Game Object is Dead!");
                }

                //-----------------------------

                float Ecurrent = hg.CurrentHealth;
                float EmaxValue = hg.ExceededGoal;

                float Eprogress = Ecurrent < 0 ? Mathf.Clamp01(Ecurrent / EmaxValue) : 0f;

                Rect Ebar = EditorGUILayout.GetControlRect(false, 22);

                string b;

                if (Ecurrent <= EmaxValue)
                {
                    b = "Exceeded!";
                }
                else
                {
                    b = "Excess Goal: ";
                }

                if (Ecurrent >= EmaxValue)
                {
                    if(hg.CurrentHealth>0)
                    {
                        EditorGUI.ProgressBar(Ebar, 0, "Your Game Object Still Alive");
                    }
                    else
                    {
                        EditorGUI.ProgressBar(Ebar, Eprogress, b + hg.ExceededGoal + "Hp " + "| Current Hp: " + Ecurrent);
                    }
                }
                else
                {
                    EditorGUI.ProgressBar(Ebar, 1, b);
                }
            }

            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_behaviour);

            if (hg.Behaviour == HealthCondition.byChilds)
            {
                EditorGUILayout.HelpBox("Configure the other health component scripts to add HP to this object, " +
                    "making sure to initialize the event with AddHealthAtParent and setting this object " +
                    "as the parent of the component.", MessageType.Info);
            }

            if (hg.Behaviour == HealthCondition.single)
            {
                EditorGUILayout.PropertyField(_healthRange);
            }

            if (hg.Behaviour == HealthCondition.parent)
            {
                EditorGUILayout.PropertyField(_healthRange);
                EditorGUILayout.HelpBox("Parental usage is not available for this script, so marking it as a " +
                    "parent will result in it being used as a single.", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(_exceededThreshold);

            GUILayout.Space(10);

            string currentHeader1 = hg.EventsGroup ? "Hide Events" : "Show and Setup Events";

            hg.EventsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(hg.EventsGroup, currentHeader1);
            EditorGUI.indentLevel++;
            if (hg.EventsGroup)
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

            if (!hg.AnimationFeatures)
            {
                if(GUILayout.Button("Enable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hg.AnimationFeatures = true;
                }
            }


            if (hg.AnimationFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animation Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

                EditorGUILayout.PropertyField(_animator);
                EditorGUILayout.PropertyField(dieTriggerNames);

                if (GUILayout.Button("Disable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hg.AnimationFeatures = false;
                }
            }

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);


            if (!hg.PhysicsFeatures)
            {
                if (GUILayout.Button("Enable Physics Features", PalexenEditorStyles.BigButton))
                {
                    hg.PhysicsFeatures = true;
                }
            }

            if (hg.PhysicsFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Ragdoll or Physics Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

                EditorGUILayout.PropertyField(_rigidbodies);
                EditorGUILayout.Space(10);

                if (GUILayout.Button("Fetch Rigidbodies"))
                {
                    hg.FetchRigidbodies();
                }

                if(hg.Rigidbodies.Length > 1)
                {
                    if (GUILayout.Button("Draw Gizmos on physics"))
                    {
                        foreach (Rigidbody rb in hg.Rigidbodies)
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
                        foreach (Rigidbody rb in hg.Rigidbodies)
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

                    hg.VelocityFeatures = EditorGUILayout.BeginFoldoutHeaderGroup(hg.VelocityFeatures, "Rigidbody Velocity Limitation Settings");
                    EditorGUI.indentLevel++;

                    if (hg.VelocityFeatures)
                    {
                        if (rl.Length >= 1)
                        {
                            for (int i = 0; i < rl.Length; i++)
                            {
                                if (rl[i] != null)
                                {
                                    rl[i].MaxVelocity = EditorGUILayout.FloatField($"{rl[i].gameObject.name}", rl[i].MaxVelocity);
                                }
                            }
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndFoldoutHeaderGroup();

                    PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

                    hg.ShapeFeatures = EditorGUILayout.BeginFoldoutHeaderGroup(hg.ShapeFeatures, "Shape Visualizer Settings");

                    EditorGUI.indentLevel++;

                    if (hg.ShapeFeatures)
                    {
                        foreach (ShapeVisualizer sv in hg.gameObject.GetComponentsInChildren<ShapeVisualizer>())
                        {
                            if (sv != null)
                            {
                                sv.ShapeColor = EditorGUILayout.ColorField($"{sv.gameObject.name}", sv.ShapeColor);
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
                    hg.PhysicsFeatures = false;
                }
            }

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Test Damage", PalexenEditorStyles.BigButton))
                {
                    hg.SetTestDamage();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region HEALTH COMPONENT

    [CustomEditor(typeof(HealthComponent))]
    [CanEditMultipleObjects]
    public class HealthComponentEditor : Editor
    {
        HealthComponent hc;
        SerializedProperty _health;
        SerializedProperty _exceededThreshold;
        SerializedProperty _affectsOn;
        SerializedProperty _importanceLevel;
        SerializedProperty _atStart;
        SerializedProperty _onTakeDamage;
        SerializedProperty _onMelee;
        SerializedProperty _atDie;
        SerializedProperty _atExceeded;
        SerializedProperty _onAddHealth;
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
            _onMelee = serializedObject.FindProperty("_onMelee");
            _atDie = serializedObject.FindProperty("_atDie");
            _atExceeded = serializedObject.FindProperty("_atExceeded");
            _onAddHealth = serializedObject.FindProperty("_onAddHealth");
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
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 60));

            Color c = setting.contextSeparatorColor;

            if (Application.isPlaying)
            {
                float current = hc.CurrentHealth;
                float maxValue = hc.HealthRange.y;

                float progress = Mathf.Clamp01(current / maxValue);

                Rect bar = EditorGUILayout.GetControlRect(false, 22);

                if (current > 0)
                {
                    EditorGUI.ProgressBar(bar, progress, "Current Health: " + current + "Hp");
                }
                else
                {
                    EditorGUI.ProgressBar(bar, 0, "Your Game Object is Dead!");
                }

                //-----------------------------

                float Ecurrent = hc.CurrentHealth;
                float EmaxValue = hc.ExceededGoal;

                float Eprogress = Ecurrent < 0 ? Mathf.Clamp01(Ecurrent / EmaxValue) : 0f;

                Rect Ebar = EditorGUILayout.GetControlRect(false, 22);

                string b;

                if (Ecurrent <= EmaxValue)
                {
                    b = "Exceeded!";
                }
                else
                {
                    b = "Excess Goal: ";
                }

                if (Ecurrent >= EmaxValue)
                {
                    if (hc.CurrentHealth > 0)
                    {
                        EditorGUI.ProgressBar(Ebar, 0, "Your Game Object Still Alive");
                    }
                    else
                    {
                        EditorGUI.ProgressBar(Ebar, Eprogress, b + hc.ExceededGoal + "Hp " + "| Current Hp: " + Ecurrent);
                    }
                }
                else
                {
                    EditorGUI.ProgressBar(Ebar, 1, b);
                }
            }

            GUILayout.Space(10);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_affectsOn);

            if(hc.AffectsOn == HealthCondition.parent)
            {
                EditorGUILayout.HelpBox("This component will affect the parent health, so make sure to set a parent with " +
                    "a HealthGO or Health System script and set it as the health parent", MessageType.Info);
                EditorGUILayout.PropertyField(_healthParent);
            }

            if (hc.AffectsOn == HealthCondition.single)
            {
                EditorGUILayout.HelpBox("This component will affect only itself, and a parent script is not necessary", MessageType.Info);
            }

            if (hc.AffectsOn == HealthCondition.byChilds)
            {
                EditorGUILayout.HelpBox("The `by child` option is not available because this script functions as a component, " +
                    "so marking it as `by child` will make it use the single component.", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(_health);
            EditorGUILayout.PropertyField(_exceededThreshold);

            EditorGUILayout.PropertyField(_importanceLevel);

            if(hc.ImportanceLevel == HealthImportance.notImportant)
            {
                EditorGUILayout.HelpBox("When marked as Not Important, the object can die independently without affecting the parent.", MessageType.Info);
            }

            if(hc.ImportanceLevel == HealthImportance.important)
            {
                EditorGUILayout.HelpBox("When marked as Important, the death of the object will cause the parent to lose all its HP" +
                    " and die instantly, good for headshots or too critical damages!.", MessageType.Info);
            }

            string headerText = hc.ShowEvents ? "Hide Events" : "Show and Setup Events";

            hc.ShowEvents = EditorGUILayout.BeginFoldoutHeaderGroup(hc.ShowEvents, headerText);

            EditorGUI.indentLevel++;

            if (hc.ShowEvents)
            {
                EditorGUILayout.PropertyField(_atStart);
                EditorGUILayout.PropertyField(_onTakeDamage);
                EditorGUILayout.PropertyField(_onMelee);
                EditorGUILayout.PropertyField(_atDie);
                EditorGUILayout.PropertyField(_atExceeded);
                EditorGUILayout.PropertyField(_onAddHealth);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;

            if(!hc.AnimationFeatures)
            {
                if (GUILayout.Button("Enable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hc.AnimationFeatures = true;
                }
            }

            if (hc.AnimationFeatures)
            {
                GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animation Features</color>",
                    PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
                EditorGUILayout.PropertyField(_animator);
                EditorGUILayout.PropertyField(triggerNames);

                if (GUILayout.Button("Disable Animation Features", PalexenEditorStyles.BigButton))
                {
                    hc.AnimationFeatures = false;
                }
            }
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            string headerText2 = hc.ShowPresets ? "Hide Presets" : "Show and Setup Presets";

            hc.ShowPresets = EditorGUILayout.BeginFoldoutHeaderGroup(hc.ShowPresets, headerText2);
            EditorGUI.indentLevel++;

            if (hc.ShowPresets)
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

            GUILayout.Space(10);
            GUI.color = c;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Test Damage", PalexenEditorStyles.BigButton))
                {
                    hc.SetTestDamage();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region DIALOG SYSTEM

    [CustomEditor(typeof(DialogSystem))]
    [CanEditMultipleObjects]
    public class DialogSystemEditor : Editor
    {
        DialogSystem _dialog;
        SerializedProperty _lang;
        SerializedProperty _catchLang;
        SerializedProperty _dialogAudioFeature; 
        SerializedProperty _afterComplete;
        SerializedProperty _langAudioSource;
        SerializedProperty _subtitles;
        SerializedProperty _order;
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
            _order = serializedObject.FindProperty("_order");
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
                "voice, or both!", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 170));

            Color c = setting.contextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_lang);
            EditorGUILayout.PropertyField(_catchLang);
            EditorGUILayout.PropertyField(_dialogAudioFeature);

            if (_dialog.Feature == DialogAudioFeature.useAudio)
            {
                EditorGUILayout.PropertyField(_langAudioSource);
            }

            EditorGUILayout.PropertyField(_afterComplete);
            EditorGUILayout.PropertyField(_subtitles);

            EditorGUILayout.Space();

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Sequences & Languages Setup</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize, TextAnchor.MiddleLeft));
            EditorGUILayout.PropertyField(_order);
            EditorGUILayout.PropertyField(_dialogSequencer);

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            if (!_dialog.InDebug)
            {
                if (GUILayout.Button("Enter Debug Mode"))
                {
                    _dialog.InDebug = true;

                }
            }
            else
            {
                if (GUILayout.Button("Exit Debug Mode"))
                {
                    _dialog.InDebug = false;
                }
            }

            if (_dialog.InDebug)
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

            if (EditorApplication.isPlaying)
            {
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
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

    #region LANG

    [CustomEditor(typeof(LangManager))]
    [CanEditMultipleObjects]
    public class LangManagerEditor : Editor
    {
        LangManager lm;
        SerializedProperty _lang;
        SerializedProperty _subtitles;

        private void OnEnable()
        {
            lm = (LangManager)target;
            _lang = serializedObject.FindProperty("_lang");
            _subtitles = serializedObject.FindProperty("_subtitles");
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
            EditorGUILayout.PropertyField(_subtitles);
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
    [CanEditMultipleObjects]
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
    [CanEditMultipleObjects]
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
    [CanEditMultipleObjects]
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

    #region LEVELS

    [CustomEditor(typeof(LevelLoader))]
    [CanEditMultipleObjects]
    public class LevelLoaderEditor : Editor
    {
        LevelLoader ll;
        SerializedProperty _loadMode;
        SerializedProperty _loadSceneMode;
        SerializedProperty loadingSceneName;
        SerializedProperty _delayTimer;
        SerializedProperty _delayScreen;
        SerializedProperty _loadingBar;
        SerializedProperty _slider;
        SerializedProperty _imageToFill;
        SerializedProperty _fadeScreen;
        SerializedProperty _eventsAfterFinish;
        SerializedProperty _useRootActivation;

        void OnEnable()
        {
            ll = (LevelLoader)target;
            _loadMode = serializedObject.FindProperty("_loadMode");
            _loadSceneMode = serializedObject.FindProperty("_loadSceneMode");
            loadingSceneName = serializedObject.FindProperty("loadingSceneName");
            _delayTimer = serializedObject.FindProperty("_delayTimer");
            _delayScreen = serializedObject.FindProperty("_delayScreen");
            _loadingBar = serializedObject.FindProperty("_loadingBar");
            _slider = serializedObject.FindProperty("_slider");
            _imageToFill = serializedObject.FindProperty("_imageToFill");
            _fadeScreen = serializedObject.FindProperty("_fadeScreen");
            _eventsAfterFinish = serializedObject.FindProperty("_eventsAfterFinish");
            _useRootActivation = serializedObject.FindProperty("_useRootActivation");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Level Loader</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));
            
            GUILayout.Box("Load scenes via the loading scene, or from here",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_loadMode);
            EditorGUILayout.PropertyField(_loadSceneMode);

            if (ll.LoadMode != LevelLoadMode.catchAndLoad)
            {
                EditorGUILayout.PropertyField(loadingSceneName);
                GUILayout.Box("Enter the scene name you want to load.\r\n" +
                    "\n<b>Note:</b> It is recommended to only perform an additive load on the scene that is used exclusively to load other scenes.", 
                    PalexenEditorStyles.CoolBox(10, TextAnchor.MiddleLeft, FontStyle.Normal, 80));
            }
            EditorGUILayout.PropertyField(_delayTimer);
            EditorGUILayout.PropertyField(_delayScreen);

            EditorGUILayout.PropertyField(_loadingBar);
            if (ll._loadingBar != LoadingBarMode.none)
            {
                if (ll._loadingBar == LoadingBarMode.slider)
                {
                    EditorGUILayout.PropertyField(_slider);
                }

                if (ll._loadingBar == LoadingBarMode.fill)
                {
                    EditorGUILayout.PropertyField(_imageToFill);
                }
            }

            EditorGUILayout.PropertyField(_fadeScreen);
            if (ll.FadeScreen == null)
            {
                EditorGUILayout.HelpBox("This field is optional; however, to enhance the loading effect " +
                    "between scenes, consider adding a screen that creates a fade effect, or a screen that " +
                    "notifies the player that a loading cycle is about to begin.", MessageType.Warning);
            }
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.PropertyField(_eventsAfterFinish);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);
            EditorGUILayout.PropertyField(_useRootActivation);
            GUILayout.Box("In root activation, by setting it to true, you will need to put all your objects inside a " +
                "parent object and deactivate them completely. This helps manage loading times well, but you may need " +
                "to further optimize how things are processed, especially dynamic objects.",
                PalexenEditorStyles.CoolBox(10, TextAnchor.MiddleLeft, FontStyle.Normal, 90));

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        LevelManager lm;
        SerializedProperty sceneName;
        SerializedProperty _delayTimer;
        SerializedProperty _rootActivation;

        private void OnEnable()
        {
            lm = (LevelManager)target;
            sceneName = serializedObject.FindProperty("sceneName");
            _delayTimer = serializedObject.FindProperty("_delayTimer");
            _rootActivation = serializedObject.FindProperty("_rootActivation");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Level Manager</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Catch and load levels",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));
        }
    }

    #endregion

    #region WAYPOINT SYSTEM

    [CustomEditor(typeof(WaypointArea))]
    [CanEditMultipleObjects]
    public class WaypointAreaEditor : Editor
    {
        WaypointArea _wp;
        SerializedProperty _prefix;
        SerializedProperty _targetLayer;
        SerializedProperty _waypoints;
        SerializedProperty _gizmoColor;
        SerializedProperty _catchDistance;

        private void OnEnable()
        {
            _wp = (WaypointArea)target;
            _wp.FetchAndBuild();
            _prefix = serializedObject.FindProperty("_prefix");
            _targetLayer = serializedObject.FindProperty("_targetLayer");
            _waypoints = serializedObject.FindProperty("_waypoints");
            _gizmoColor = serializedObject.FindProperty("_gizmoColor");
            _catchDistance = serializedObject.FindProperty("_catchDistance");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Waypoint Area</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Waypoint Area for your NPC's \n" +
                "Hold <color=red>shift</color> + click to create <color=green>waypoint</color>",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_prefix);
            EditorGUILayout.PropertyField(_targetLayer);
            EditorGUILayout.PropertyField(_waypoints);
            EditorGUILayout.PropertyField(_gizmoColor);
            EditorGUILayout.PropertyField(_catchDistance);

            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2);

            if(GUILayout.Button("Fetch & Build", PalexenEditorStyles.BigButton))
            {
                _wp.FetchAndBuild();
                _wp.CanPaint = true;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            BuildPreview();
            Paint();
        }

        void Paint()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            Handles.color = setting.gizmosColor;

            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if (e.shift)
            {
                HandleUtility.AddDefaultControl(controlID);

                Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(r, out RaycastHit mh, Mathf.Infinity, _wp.TargetLayer))
                {
                    switch (setting.contextGizmoForm)
                    {
                        case GizmoForm.sphere:
                            Handles.SphereHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.cube:
                            Handles.CubeHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.cylinder:
                            Handles.CylinderHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.cone:
                            Handles.ConeHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.arrow:
                            Handles.ArrowHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.circle:
                            Handles.CircleHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.square:
                            Handles.RectangleHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                        case GizmoForm.dot:
                            Handles.DotHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), setting.gizmoSize, EventType.Repaint);
                            break;
                    }
                }

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Waypoint");

                    if (prefab == null)
                    {
                        Debug.LogError("Prefab Waypoint not found!");
                        return;
                    }

                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _wp.TargetLayer))
                    {
                        Undo.IncrementCurrentGroup();

                        GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        clone.transform.position = hit.point;
                        clone.transform.parent = _wp.transform;
                        _wp.FetchAndBuild();

                        Undo.RegisterCreatedObjectUndo(clone, "Waypoint Placed");
                        _wp.FetchAndBuild();
                        _wp.RenameChilds();

                        EditorUtility.SetDirty(_wp.gameObject);

                        e.Use();
                    }
                }
            }
        }

        public void BuildPreview()
        {
            _wp = (WaypointArea)target;

            if (_wp.Waypoints.Length > 1)
            {
                foreach (var p in _wp.Waypoints)
                {
                    if (p != null)
                    {
                        Handles.color = _wp.GizmoColor;
                        Handles.DrawWireDisc(p.transform.position, Vector3.up, _wp.CatchDistance);

                        Handles.color = Color.white;
                        Handles.ArrowHandleCap(0, p.transform.position, p.transform.rotation, _wp.CatchDistance, EventType.Repaint);
                    }
                }
            }
        }
    }

    [CustomEditor(typeof(Waypoint))]
    [CanEditMultipleObjects]
    public class WaypointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // HIDE INTERNAL
        }
    }

    #endregion

    #region PREFAB PAINTER

    [CustomEditor(typeof(PrefabPainter))]
    public class PrefabPainterEditor : Editor
    {
        PrefabPainter _pp;
        SerializedProperty _mouseBehaviour;
        SerializedProperty _targetLayer;
        SerializedProperty _density;
        SerializedProperty _radius;
        SerializedProperty _brushColor;
        SerializedProperty _prefabs;
        SerializedProperty _YRandomizer;
        SerializedProperty _sizeRandomizer;
        SerializedProperty _brush;
        float alpha;
        float next = 0f;

        bool _isPainting = false;
        Color ppc;

        GameObject tempBursh;

        private void OnEnable()
        {
            _pp = (PrefabPainter)target;
            _targetLayer = serializedObject.FindProperty("_targetLayer");
            _mouseBehaviour = serializedObject.FindProperty("_mouseBehaviour");
            _density = serializedObject.FindProperty("_density");
            _radius = serializedObject.FindProperty("_radius");
            _brushColor = serializedObject.FindProperty("_brushColor");
            _prefabs = serializedObject.FindProperty("_prefabs");
            _YRandomizer = serializedObject.FindProperty("_YRandomizer");
            _sizeRandomizer = serializedObject.FindProperty("_sizeRandomizer");
            _brush = serializedObject.FindProperty("_brush");
        }

        private void OnDisable()
        {
            DestroyImmediate(tempBursh);
            tempBursh = null;
        }

        private void OnDestroy()
        {
            DestroyImmediate(tempBursh);
            tempBursh = null;
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Prefab Painter</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Prefab Painter \n" +
                "Hold <color=red>shift</color> + click to <color=green>Paint your Prefabs</color>",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_targetLayer);
            EditorGUILayout.PropertyField(_mouseBehaviour);

            if(_pp.MouseBehaviour == EventType.MouseDrag)
            {
                EditorGUILayout.PropertyField(_density);
            }

            EditorGUILayout.PropertyField(_radius);
            EditorGUILayout.PropertyField(_brushColor);
            GUILayout.Label($"<color={"#" + setting.headerColorValue.ConvertToHex()}>Prefabs</color>",
                PalexenEditorStyles.CoolTitle(setting.headerSize, TextAnchor.MiddleLeft));
            EditorGUILayout.PropertyField(_prefabs);
            EditorGUILayout.PropertyField(_YRandomizer);
            EditorGUILayout.PropertyField(_sizeRandomizer);

            GUI.color = _pp.BrushColor;
            EditorGUILayout.PropertyField(_brush);
            GUI.color = Color.white;

            if (_pp.transform.childCount > 0)
            {
                GUILayout.Space(10);
                GUI.color = setting.contextSeparatorColor;
                EditorGUILayout.HelpBox("", MessageType.None);
                GUI.color = Color.white;
                GUILayout.Space(10);
                if (GUILayout.Button("Clear all content", PalexenEditorStyles.BigButton))
                {
                    foreach(Transform t in _pp.transform)
                    {
                        DestroyImmediate(t.gameObject);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            Paint();
        }

        void Paint()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            // BRUSH PULSE
            if (!_isPainting)
            {
                float colorA = _pp.BrushColor.a;
                float wave = (Mathf.Sin(Time.time * _pp.Density / 10) + 1f) / 2f;
                //alpha = Mathf.PingPong(Time.time, colorA);
                alpha = wave * colorA;


                var correctColor = new Color(_pp.BrushColor.r, _pp.BrushColor.g, _pp.BrushColor.b, alpha);
                ppc = correctColor;

                // BRUSH EDITOR HANDLES
                Handles.color = correctColor;
            }
            else
            {
                ppc = _pp.BrushColor;
                Handles.color = _pp.BrushColor;
            }

            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if (e.shift)
            {
                if (tempBursh == null && _pp.Brush != null)
                {
                    GameObject tmpBrush = (Instantiate(_pp.Brush));

                    tempBursh = tmpBrush;
                }

                HandleUtility.AddDefaultControl(controlID);

                Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(r, out RaycastHit mh, Mathf.Infinity, _pp.TargetLayer))
                {
                    float ra = _pp.Radius;

                    if (ra <= 0)
                    {
                        ra = setting.gizmoSize;
                    }

                    // BRUSH PREVIEW
                    if (_pp.Brush != null)
                    {
                        tempBursh.transform.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_MainColor", ppc);
                        tempBursh.transform.SetPositionAndRotation(mh.point, Quaternion.LookRotation(mh.normal));
                        tempBursh.transform.localScale = new Vector3(ra + 1f, ra + 1f, ra + 1f);
                        tempBursh.transform.SetParent(_pp.transform);
                    }

                    if (_pp.Brush == null)
                    {
                        switch (setting.contextGizmoForm)
                        {
                            case GizmoForm.sphere:
                                Handles.SphereHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.cube:
                                Handles.CubeHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.cylinder:
                                Handles.CylinderHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.cone:
                                Handles.ConeHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.arrow:
                                Handles.ArrowHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.circle:
                                Handles.CircleHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.square:
                                Handles.RectangleHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                            case GizmoForm.dot:
                                Handles.DotHandleCap(controlID, mh.point, Quaternion.LookRotation(mh.normal), ra, EventType.Repaint);
                                break;
                        }
                    }
                }

                if (e.type == _pp.MouseBehaviour && e.button == 0)
                {
                    _isPainting = true;
                    int i = UnityEngine.Random.Range(0, _pp.Prefabs.Length);

                    GameObject[] prefab = _pp.Prefabs;
                    GameObject _t = prefab[i];

                    float size = UnityEngine.Random.Range(_pp.SizeRandomizer.x, _pp.SizeRandomizer.y);
                    Vector3 newSize = new(size, size, size);

                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                    if(_pp.MouseBehaviour == EventType.MouseDown)
                    {
                        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _pp.TargetLayer))
                        {
                            Undo.IncrementCurrentGroup();
                            GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(_t);

                            float rad = UnityEngine.Random.Range(0, _pp.Radius);
                            clone.transform.position = hit.point + GetBrushOffset(hit, rad);

                            Quaternion alignToSurface = Quaternion.FromToRotation(Vector3.up, hit.normal);
                            Quaternion randomYaw = Quaternion.AngleAxis(UnityEngine.Random.Range(_pp.YRandomizer.x, _pp.YRandomizer.y), hit.normal);

                            clone.transform.rotation = randomYaw * alignToSurface;

                            clone.transform.parent = _pp.transform;

                            clone.transform.localScale = newSize;

                            Undo.RegisterCreatedObjectUndo(clone, "Prefabs Placed!");
                            // Other Actions
                            EditorUtility.SetDirty(_pp.gameObject);

                            e.Use();
                        }
                    }

                    if (_pp.MouseBehaviour == EventType.MouseDrag)
                    {
                        _isPainting = true;
                        if (Time.time >= next)
                        {
                            next = Time.time + 1 / _pp.Density * 2;

                            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _pp.TargetLayer))
                            {
                                Undo.IncrementCurrentGroup();
                                GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(_t);

                                float rad = UnityEngine.Random.Range(0, _pp.Radius);
                                clone.transform.position = hit.point + GetBrushOffset(hit, rad);

                                Quaternion alignToSurface = Quaternion.FromToRotation(Vector3.up, hit.normal);
                                Quaternion randomYaw = Quaternion.AngleAxis(UnityEngine.Random.Range(_pp.YRandomizer.x, _pp.YRandomizer.y), hit.normal);

                                clone.transform.rotation = randomYaw * alignToSurface;

                                clone.transform.parent = _pp.transform;

                                clone.transform.localScale = newSize;

                                Undo.RegisterCreatedObjectUndo(clone, "Prefabs Placed!");
                                // Other Actions
                                EditorUtility.SetDirty(_pp.gameObject);

                                e.Use();
                            }
                        }
                    }
                }
            }
            else
            {
                DestroyImmediate(tempBursh);
                tempBursh = null;
                _isPainting = false;
            }
        }

        Vector3 GetBrushOffset(RaycastHit hit, float radius)
        {
            Vector3 normal = hit.normal;

            Vector3 tangent = Vector3.Cross(normal, Vector3.up);

            if (tangent.sqrMagnitude < 0.001f)
                tangent = Vector3.Cross(normal, Vector3.right);

            tangent.Normalize();

            Vector3 bitangent = Vector3.Cross(normal, tangent);

            Vector2 circle = UnityEngine.Random.insideUnitCircle * radius;

            return tangent * circle.x + bitangent * circle.y;
        }
    }

    #endregion

    #region HIERARCHY COLOR

    [CustomEditor(typeof(HierarchyColor))]
    [CanEditMultipleObjects]
    public class HierarchyColorEditor : Editor
    {
        HierarchyColor _hc;
        SerializedProperty _indentation;
        SerializedProperty _myColor;
        SerializedProperty _fontStyle;
        SerializedProperty _fontColor;
        SerializedProperty _icons;

        private void OnEnable()
        {
            _hc = (HierarchyColor)target;
            _indentation = serializedObject.FindProperty("_indentation");
            _myColor = serializedObject.FindProperty("_myColor");
            _fontStyle = serializedObject.FindProperty("_fontStyle");
            _fontColor = serializedObject.FindProperty("_fontColor");
            _icons = serializedObject.FindProperty("_icons");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Custom Hierarchy</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Customize the view of this object in your hierarchy!",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_indentation);
            EditorGUILayout.PropertyField(_myColor);
            EditorGUILayout.PropertyField(_fontStyle);
            EditorGUILayout.PropertyField(_fontColor);
            EditorGUILayout.PropertyField(_icons);

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion

#endif

    #endregion
}
