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

namespace Palexen.Audio
{
    [CreateAssetMenu(fileName = "New Audio Library", menuName = "Palexen/Audio/Audio Library")]
    [ScriptDescription("Audio Library", "A sound collection in one place!")]
    public class AudioLibrary : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Sounds")]
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] public AudioClip[] sounds;

        #endregion

        #region API

        public AudioClip GetRandomClip()
        {
            if (sounds == null || sounds.Length == 0)
            {
                Debug.LogError("<color=yellow>Audio Library</color> is empty! Please add some audio clips.");
                return null;
            }
            int randomIndex = Random.Range(0, sounds.Length);
            return sounds[randomIndex];
        }

        #endregion
    }
}
