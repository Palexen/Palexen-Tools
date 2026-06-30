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

        [MyHeader("Setup")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private EventType _mouseBehaviour;
        [SerializeField] private float _density = 15;
        [SerializeField] private float _radius;

        [MyHeader("Prefabs")]
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)][SerializeField] private GameObject[] _prefabs;

        [MyHeader("Random Rotation")]
        [VectorSlider(0, 720)][SerializeField] private Vector2 _YRandomizer = new(0, 360);

        [MyHeader("Random Size")]
        [VectorSlider(0, 2)][SerializeField] private Vector2 _sizeRandomizer = new(.9f, 1);

        #endregion

        #region UNITY METHODS

        #endregion

        #region MECHANICS



        #endregion

        #region API



        #endregion

        #region PROPERTIES

        public LayerMask TargetLayer { get { return _targetLayer; } }
        public EventType MouseBehaviour { get { return _mouseBehaviour; } }
        public float Density { get { return _density * 10; } }
        public float Radius { get { return _radius; } }
        public GameObject[] Prefabs { get { return _prefabs; } }
        public Vector2 YRandomizer {  get { return _YRandomizer; } }
        public Vector2 SizeRandomizer { get { return _sizeRandomizer; } }

        #endregion
    }
}
