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
    [AddComponentMenu("Palexen/Level Design/Prefab Painter")]
    public class PrefabPainter : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Setup")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private EventType _mouseBehaviour;
        [SerializeField] private float _density = 15;
        [SerializeField] private float _radius;
        [SerializeField][ColorUsage(true, true)] private Color _brushColor = new(0.6529321f, 0.6529321f, 1, 1f);

        [MyHeader("Prefabs")]
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)][SerializeField] private GameObject[] _prefabs;

        [MyHeader("Random Rotation")]
        [VectorSlider(0, 720)][SerializeField] private Vector2 _YRandomizer = new(0, 360);

        [MyHeader("Random Size")]
        [VectorSlider(.1f, 10)][SerializeField] private Vector2 _sizeRandomizer = new(.9f, 1);
        [SerializeField] private AlignToSurface _alingToSurface;
        [SerializeField] private float _yOffset;

        [MyHeader("Brush")]
        [SerializeField] private GameObject _brush;

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
        public AlignToSurface Alignment { get { return _alingToSurface; } }
        public float YOffset { get { return _yOffset; } }
        public float Density { get { return _density * 10; } }
        public float Radius { get { return _radius; } }
        public Color BrushColor { get { return _brushColor; } }
        public GameObject[] Prefabs { get { return _prefabs; } }
        public Vector2 YRandomizer {  get { return _YRandomizer; } }
        public Vector2 SizeRandomizer { get { return _sizeRandomizer; } }
        public GameObject Brush { get { return _brush; } }

        #endregion
    }
}
