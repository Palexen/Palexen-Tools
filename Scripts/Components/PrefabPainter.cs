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

namespace Palexen.Tools
{
    #if PALEXEN_TOOLS
    [ScriptDescription("PrefabPainter", "Improved Monobehavior")]
#endif
    [AddComponentMenu("Palexen/Tools/Prefab Painter")]
    public class PrefabPainter : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Layer")]
        public LayerMask _targetLayer;

        [MyHeader("Prefabs")]
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] public GameObject[] _prefabs;

        [MyHeader("Random Rotation")]
        [VectorSlider(0, 720)] public Vector2 _YRandomizer = new(0, 360);

        #endregion

        #region UNITY METHODS

        #endregion

        #region MECHANICS

    

        #endregion

        #region API

    

        #endregion
    }
}
