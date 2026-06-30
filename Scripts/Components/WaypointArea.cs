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

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Gameplay
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Waypoint Area", "Waypoint Area for your NPC's")]
#endif
    [AddComponentMenu("Palexen/Gameplay/Waypoint Area")]
    public class WaypointArea : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Prefix")]
        [SerializeField] private string _prefix = "Waypoint: ";

        [SerializeField] private LayerMask _targetLayer;

        [MyHeader("Waypoints")]
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)][SerializeField] private Waypoint[] _waypoints;

        [MyHeader("Editor Visual")]
        [SerializeField] private Color _gizmoColor = Color.yellow;
        [SerializeField] private float _catchDistance = .25f;

        bool canPaint;

        #endregion

        #region PROPERTIES

        public LayerMask TargetLayer { get { return _targetLayer; } }
        public Waypoint[] Waypoints { get { return _waypoints; } set { _waypoints = value; } }
        public Color GizmoColor { get { return _gizmoColor; } }
        public float CatchDistance { get { return _catchDistance; } }

        #endregion

        #region UNITY METHODS

        private void OnValidate()
        {
            RenameChilds();
        }

        private void OnDrawGizmos()
        {
            if (CanPaint)
            {
                Gizmos.color = _gizmoColor;

                for (int i = 0; i < _waypoints.Length; i++)
                {
                    if (_waypoints.Length != 0)
                    {
                        Transform currentChild = _waypoints[i].transform;
                        Transform nextChild;

                        if (i == _waypoints.Length - 1)
                        {
                            nextChild = _waypoints[0].transform;
                        }
                        else
                        {
                            nextChild = _waypoints[i + 1].transform;
                        }

                        currentChild.LookAt(nextChild);

                        Gizmos.DrawLine(currentChild.position, nextChild.position);
                    }
                }
            }
        }

        #endregion

        #region MECHANICS


        #endregion

        #region API

        public void FetchAndBuild()
        {
            _waypoints = GetComponentsInChildren<Waypoint>();
        }

        public void RenameChilds()
        {
            try
            {
                foreach (var target in _waypoints)
                {
                    if (target != null)
                    {
                        int index = target.transform.GetSiblingIndex() + 1;

                        target.gameObject.name = _prefix + index;
                    }
                }
            }
            catch
            {

            }
        }

        public bool CanPaint { get { return canPaint; } set { canPaint = value; } }

        #endregion
    }
}
