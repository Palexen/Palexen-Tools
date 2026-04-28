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

namespace Palexen.Scriptables
{
    [ScriptDescription("Animator Container", "Scriptable")]
    [CreateAssetMenu(fileName = "New Animator Container", menuName = "Palexen/Animation/Animator Library")]
    public class AnimatorLibrary : ScriptableObject
    {
        #region VARIABLES

        [MyHeader("Animator Library")]
        public string _genderOrType = "Gender, Type, Class";
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public RuntimeAnimatorController[] _animators;

        #endregion

        #region API

        public RuntimeAnimatorController GetRandomAnimator()
        {
            int i = Random.Range(0, _animators.Length);
            return _animators[i];
        }

        #endregion
    }
}
