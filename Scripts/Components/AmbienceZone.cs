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
using UnityEngine;
using Palexen.Tools;

namespace Palexen.Audio.Atmos
{
    [AddComponentMenu("Palexen/Atmos/Ambience Zone")]
    public class AmbienceZone : MonoBehaviour
    {
        #region VARIABLES

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

        #endregion

        #region METHODS

        private void Update()
        {
            UpdateAudio();
        }

        void UpdateAudio()
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

        private void OnTriggerEnter(Collider other)
        {
            switch (_targetAllowedVia)
            {
                case TargetAllowedVia.tag:

                    if (other.CompareTag(_tagName))
                    {
                        transitionState = AudioTransitionState.fadeIn;

                        if(affectToGeneralAmbience == AffectGeneralAmbience.yes)
                        {
                            GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                            ga.AtmosFadeOut(AudioTransitionState.fadeOut);
                        }
                    }

                    break;

                case TargetAllowedVia.layer:

                    if (((1 << other.gameObject.layer) & _layerMask) != 0)
                    {
                        transitionState = AudioTransitionState.fadeIn;

                        if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                        {
                            GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                            ga.AtmosFadeOut(AudioTransitionState.fadeOut);
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
                        transitionState = AudioTransitionState.fadeOut;

                        if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                        {
                            GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                            ga.AtmosFadeOut(AudioTransitionState.fadeIn);
                        }
                    }

                    break;

                case TargetAllowedVia.layer:

                    if (((1 << other.gameObject.layer) & _layerMask) != 0)
                    {
                        transitionState = AudioTransitionState.fadeOut;

                        if (affectToGeneralAmbience == AffectGeneralAmbience.yes)
                        {
                            GeneralAmbience ga = FindFirstObjectByType<GeneralAmbience>();
                            ga.AtmosFadeOut(AudioTransitionState.fadeIn);
                        }
                    }

                    break;
            }
        }

        #endregion
    }
}
