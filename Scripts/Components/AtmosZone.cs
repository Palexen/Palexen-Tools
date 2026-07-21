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
    [AddComponentMenu("Palexen/Atmos/Atmos Zone")]
    public class AtmosZone : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Behaviour")]
        [SerializeField] private AmbienceZoneBehaviour _behaviour;

        [MyHeader("Activation Mode")]
        [Tooltip("This is the activation mode, select it according to your preference")]
        [SerializeField] private TargetAllowedVia _targetAllowedVia = TargetAllowedVia.tag;

        [MyHeader("Select Tag")]
        [Tooltip("The name of the tag that you will use to activate the trigger")]
        [TagField][SerializeField] private string _tagName = "Player";

        [MyHeader("Select Layer")]
        [Tooltip("Activation mode by trigger, tag or layer, subsequently configure the required parameter according to the activation mode")]
        [SerializeField] private LayerMask _layerMask = 1;

        [MyHeader("Transitions behaviours")]
        [SerializeField] private AudioTransitionState transitionState = AudioTransitionState.fadeOut;
        [SerializeField] private AffectGeneralAmbience affectToGeneralAmbience;

        [MyHeader("Audio Configuration")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)][SerializeField] private AudioSource ambienceZoneSource;
        [VectorSlider(0, 1)][SerializeField] private Vector2 minMaxVolume = new(0, 1);
        [SerializeField] private float updateSpeed = 1f;

        [MyHeader("Audio Mixer Settings")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)][SerializeField] private AudioMixer _master;
        [SerializeField] private float _timeToReach = 1;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)][SerializeField] private AudioMixerSnapshot[] _snapshots;
        [Range(0, 1)][SerializeField] private float[] _weightsOnEnter;
        [Range(0, 1)][SerializeField] private float[] _weightsOnExit;

        [SerializeField] private AudioSnapshot[] _snapshotsSetup;

        List<AudioMixerSnapshot> snapshots;
        List<float> weightsOnEnter;
        List<float> weightsOnExit;

        [MyHeader("Events")]
        [SerializeField] private bool addEvents;
        [SerializeField] private UnityEvent _onTriggerEnter;
        [SerializeField] private UnityEvent _onTriggerExit;

        bool isIn;
        AudioClip tempClip;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Get and Set state
        /// </summary>
        public AmbienceZoneBehaviour Behaviour { get { return _behaviour; } set { _behaviour = value; } }
        public TargetAllowedVia TargetType { get { return _targetAllowedVia; } }
        public AudioSource AmbienceZoneSource { get { return ambienceZoneSource; } set { ambienceZoneSource = value; } }
        public bool AddEventsCapability { get { return addEvents; } set { addEvents = value; } }
        public bool IsIn { get { return isIn; } }

        #endregion

        #region METHODS

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
                                Atmos ga = FindFirstObjectByType<Atmos>();
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

                        isIn = true;
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
                                Atmos ga = FindFirstObjectByType<Atmos>();
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

                        isIn = true;
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
                                Atmos ga = FindFirstObjectByType<Atmos>();
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

                        isIn = false;
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
                                Atmos ga = FindFirstObjectByType<Atmos>();
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

                        isIn = false;
                    }

                    break;
            }
        }

        #endregion

        #region API

        public void SetAmbience(AudioClip newClip, float transitionSpeed = .2f)
        {
            if (isIn)
            {
                if (newClip.name != ambienceZoneSource.clip.name)
                {
                    tempClip = newClip;
                    updateSpeed = transitionSpeed;
                    UpdatingSound();
                }
            }
            else
            {
                if (newClip.name != ambienceZoneSource.clip.name)
                {
                    tempClip = newClip;
                    ambienceZoneSource.clip = tempClip;
                }
            }
        }

        void UpdatingSound()
        {
            transitionState = AudioTransitionState.fadeOut;
            Invoke(nameof(Changing), 1 / updateSpeed);
        }

        void Changing()
        {
            ambienceZoneSource.Stop();
            ambienceZoneSource.clip = tempClip;
            ambienceZoneSource.Play();
            UpdateComplete();
        }

        void UpdateComplete()
        {
            transitionState = AudioTransitionState.fadeIn;
        }

        #endregion
    }
}
