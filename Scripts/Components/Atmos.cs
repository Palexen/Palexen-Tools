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
using Palexen.Tools;

namespace Palexen.Audio.Atmos
{
    [AddComponentMenu("Palexen/Atmos/Atmos", 1)]
    public class Atmos : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Audio Setup")]
        [SerializeField] private AudioTransitionState transitionState = AudioTransitionState.fadeIn;
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.errorMessage)][SerializeField] private AudioSource ambienceSource;
        [VectorSlider(0, 1)][SerializeField] private Vector2 minMaxAudio = new(0, 1);
        [SerializeField] private float updateSpeed = 1f;

        AudioClip tempClip;

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

        #endregion

        #region API

        /// <summary>
        /// This is called when you need to perform a manual transmission; this method is also used with the Ambience Zone component.
        /// </summary>
        /// <param name="newState"></param>
        [Obsolete("This method is obsolete. Use the transitionTo property instead.")]
        public void AtmosFadeIn(AudioTransitionState newState)
        {
            transitionState = newState;
        }

        /// <summary>
        /// This is called when you need to perform a manual transmission; this method is also used with the Ambience Zone component.
        /// </summary>
        /// <param name="newState"></param>
        [Obsolete("This method is obsolete. Use the transitionTo property instead.")]
        public void AtmosFadeOut(AudioTransitionState newState)
        {
            transitionState = newState;
        }

        /// <summary>
        /// Call this method to change the ambience audio clip, it will automatically fade out the current sound, 
        /// change the clip, and then fade in the new sound.
        /// </summary>
        /// <param name="newClip"></param>
        public void SetAmbience(AudioClip newClip, float transitionSpeed = .2f)
        {
            if (newClip.name != ambienceSource.clip.name)
            {
                tempClip = newClip;
                BlendSpeed = transitionSpeed;
                UpdatingSound();
            }
        }

        void UpdatingSound()
        {
            TransitionTo = AudioTransitionState.fadeOut;
            Invoke(nameof(Changing), 1 / BlendSpeed);
        }

        void Changing()
        {
            ambienceSource.Stop();
            ambienceSource.clip = tempClip;
            ambienceSource.Play();
            UpdateComplete();
        }

        void UpdateComplete()
        {
            TransitionTo = AudioTransitionState.fadeIn;
        }

        #endregion

        #region PROPERTIES

        public AudioSource AmbienceSource {  get { return ambienceSource; } set { ambienceSource = value; } }

        /// <summary>
        /// This property allows you to set the transition state for the ambience audio. You can choose between 
        /// fade-in and fade-out states, which will control how the audio transitions when changing clips or adjusting 
        /// volume. Setting this property will automatically trigger the appropriate audio transition behavior based on the selected state.
        /// </summary>
        public AudioTransitionState TransitionTo { get { return transitionState; } set { transitionState = value; } }

        /// <summary>
        /// Use this property to modify the blending speed between the old environment and the new environment you're aiming for.
        /// </summary>
        /// <remarks>Note: A lower number means the environment change will be slower but smoother, while a higher number 
        /// means it will be faster and smoother, but it depends on your scene design philosophy.</remarks>
        public float BlendSpeed { get { return updateSpeed; } set { updateSpeed = value; } }

        #endregion
    }
}
