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

namespace Palexen.Audio.Atmos
{
    [AddComponentMenu("Palexen/Atmos/Global Ambience", 1)]
    public class GeneralAmbience : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Audio Setup")]
        public AudioTransitionState transitionState = AudioTransitionState.fadeIn;
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)] public AudioSource ambienceSource;
        [VectorSlider(0, 1)] public Vector2 minMaxAudio = new(0, 1);
        public float updateSpeed = 1f;

        #endregion

        #region METHODS

        private void Update()
        {
            UpdateAudio();
        }

        void UpdateAudio()
        {
            if(transitionState == AudioTransitionState.fadeIn)
            {
                ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, minMaxAudio.y, Time.deltaTime * updateSpeed);
            }

            if (transitionState == AudioTransitionState.fadeOut)
            {
                ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, minMaxAudio.x, Time.deltaTime * updateSpeed);
            }
        }

        /// <summary>
        /// This is called when you need to perform a manual transmission; this method is also used with the Ambience Zone component.
        /// </summary>
        /// <param name="newState"></param>
        public void AtmosFadeIn(AudioTransitionState newState)
        {
            transitionState = newState;
        }

        /// <summary>
        /// This is called when you need to perform a manual transmission; this method is also used with the Ambience Zone component.
        /// </summary>
        /// <param name="newState"></param>
        public void AtmosFadeOut(AudioTransitionState newState)
        {
            transitionState = newState;
        }

        #endregion

        #region API

        /// <summary>
        /// Call this method to change the ambience audio clip, it will automatically fade out the current sound, 
        /// change the clip, and then fade in the new sound.
        /// </summary>
        /// <param name="newClip"></param>
        public void SetAmbience(AudioClip newClip)
        {
            if (newClip.name != ambienceSource.clip.name)
            {
                ambienceSource.clip = newClip;
                UpdatingSound();
            }
        }

        void UpdatingSound()
        {
            AtmosFadeOut(AudioTransitionState.fadeOut);
            Invoke(nameof(Changing), 2);
        }

        void Changing()
        {
            ambienceSource.Stop();
            ambienceSource.Play();
            UpdateComplete();
        }

        void UpdateComplete()
        {
            AtmosFadeIn(AudioTransitionState.fadeIn);
        }

        #endregion
    }
}
