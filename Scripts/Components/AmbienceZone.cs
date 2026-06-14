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
using UnityEngine.Audio;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Palexen.Audio.Atmos
{
    [AddComponentMenu("Palexen/Atmos/Ambience Zone")]
    public class AmbienceZone : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Behaviour")]
        public AmbienceZoneBehaviour _behaviour;

        [MyHeader("Activation Mode")]
        [Tooltip("This is the activation mode, select it according to your preference")]
        public TargetAllowedVia _targetAllowedVia = TargetAllowedVia.tag;

        [MyHeader("Select Tag")]
        [Tooltip("The name of the tag that you will use to activate the trigger")]
        [TagField] public string _tagName = "Player";

        [MyHeader("Select Layer")]
        [Tooltip("Activation mode by trigger, tag or layer, subsequently configure the required parameter according to the activation mode")]
        public LayerMask _layerMask = 1;

        [MyHeader("Transitions behaviours")]
        public AudioTransitionState transitionState = AudioTransitionState.fadeOut;
        public AffectGeneralAmbience affectToGeneralAmbience;

        [MyHeader("Audio Configuration")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)] public AudioSource ambienceZoneSource;
        [VectorSlider(0, 1)] public Vector2 minMaxVolume = new(0, 1);
        public float updateSpeed = 1f;

        [MyHeader("Audio Mixer Settings")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)] public AudioMixer _master;
        public float _timeToReach;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] public AudioMixerSnapshot[] _snapshots;
        [Range(0, 1)] public float[] _weightsOnEnter;
        [Range(0, 1)] public float[] _weightsOnExit;

        public AudioSnapshot[] _snapshotsSetup;

        List<AudioMixerSnapshot> snapshots;
        List<float> weightsOnEnter;
        List<float> weightsOnExit;

        [MyHeader("Events")]
        public bool addEvents;
        public UnityEvent _onTriggerEnter;
        public UnityEvent _onTriggerExit;

        #endregion

        #region METHODS

        /// <summary>
        /// Get and Set state
        /// </summary>
        public AmbienceZoneBehaviour Behaviour { get { return _behaviour; } set { _behaviour = value; } }

        public bool AddEventsCapability { get { return addEvents; } set { addEvents = value; } }

        private void Start()
        {
            if (Behaviour == AmbienceZoneBehaviour.snapshots)
            {
                snapshots = new List<AudioMixerSnapshot>();
                weightsOnEnter = new List<float>();
                weightsOnExit = new List<float>();

                foreach (var s in _snapshotsSetup)
                {
                    snapshots.Add(s._snapshot);
                }

                foreach(var a in _snapshotsSetup)
                {
                   weightsOnEnter.Add(a._weightEnter);
                }

                foreach (var b in _snapshotsSetup)
                {
                    weightsOnExit.Add(b._weightExit);
                }

                _snapshots = snapshots.ToArray();
                _weightsOnEnter = weightsOnEnter.ToArray();
                _weightsOnExit = weightsOnExit.ToArray();
            }
        }

        private void Update()
        {
            UpdateAudio();
        }

        /// <summary>
        /// Updates the audio volume based on the current transition state, applying fade-in or fade-out effects as
        /// needed.
        /// </summary>
        /// <remarks>This method should be called regularly, such as once per frame, to ensure smooth
        /// audio transitions. The volume is adjusted incrementally towards the target value depending on whether a
        /// fade-in or fade-out is in progress.</remarks>
        void UpdateAudio()
        {
            if (Behaviour == AmbienceZoneBehaviour.ambience)
            {
                if (transitionState == AudioTransitionState.fadeIn)
                {
                    ambienceZoneSource.volume = Mathf.MoveTowards(ambienceZoneSource.volume, minMaxVolume.y, Time.deltaTime * updateSpeed);
                }

                if (transitionState == AudioTransitionState.fadeOut)
                {
                    ambienceZoneSource.volume = Mathf.MoveTowards(ambienceZoneSource.volume, minMaxVolume.x, Time.deltaTime * updateSpeed);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (_targetAllowedVia)
            {
                case TargetAllowedVia.tag:

                    if (other.CompareTag(_tagName))
                    {
                        if (Behaviour == AmbienceZoneBehaviour.ambience)
                        {
                            transitionState = AudioTransitionState.fadeIn;

                            if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                            {
                                GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                                ga.TransitionTo = AudioTransitionState.fadeOut;
                            }
                        }

                        if(Behaviour == AmbienceZoneBehaviour.snapshots)
                        {
                            _master.TransitionToSnapshots(_snapshots, _weightsOnEnter, _timeToReach);
                        }

                        if (AddEventsCapability)
                        {
                            _onTriggerEnter.Invoke();
                        }
                    }

                    break;

                case TargetAllowedVia.layer:

                    if (((1 << other.gameObject.layer) & _layerMask) != 0)
                    {
                        if (Behaviour == AmbienceZoneBehaviour.ambience)
                        {
                            transitionState = AudioTransitionState.fadeIn;

                            if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                            {
                                GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                                ga.TransitionTo = AudioTransitionState.fadeOut;
                            }
                        }

                        if(Behaviour == AmbienceZoneBehaviour.snapshots)
                        {
                            _master.TransitionToSnapshots(_snapshots, _weightsOnEnter, _timeToReach);
                        }

                        if (AddEventsCapability)
                        {
                            _onTriggerEnter.Invoke();
                        }
                    }

                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (_targetAllowedVia)
            {
                case TargetAllowedVia.tag:

                    if (other.CompareTag(_tagName))
                    {
                        if (Behaviour == AmbienceZoneBehaviour.ambience)
                        {
                            transitionState = AudioTransitionState.fadeOut;

                            if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                            {
                                GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                                ga.TransitionTo = AudioTransitionState.fadeIn;
                            }
                        }

                        if (Behaviour == AmbienceZoneBehaviour.snapshots)
                        {
                            _master.TransitionToSnapshots(_snapshots, _weightsOnExit, _timeToReach);
                        }

                        if (AddEventsCapability)
                        {
                            _onTriggerExit.Invoke();
                        }
                    }

                    break;

                case TargetAllowedVia.layer:

                    if (((1 << other.gameObject.layer) & _layerMask) != 0)
                    {
                        if (Behaviour == AmbienceZoneBehaviour.ambience)
                        {
                            transitionState = AudioTransitionState.fadeOut;

                            if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                            {
                                GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                                ga.TransitionTo = AudioTransitionState.fadeIn;
                            }
                        }

                        if (Behaviour == AmbienceZoneBehaviour.snapshots)
                        {
                            _master.TransitionToSnapshots(_snapshots, _weightsOnExit, _timeToReach);
                        }

                        if (AddEventsCapability)
                        {
                            _onTriggerExit.Invoke();
                        }
                    }

                    break;
            }
        }

        #endregion
    }
}
