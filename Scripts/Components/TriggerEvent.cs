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
using UnityEngine.Events;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Gameplay
{
    [AddComponentMenu("Palexen/Gameplay/Trigger Event")]
    #if PALEXEN_TOOLS
    [ScriptDescription("Trigger Event", "You can activate events by entering this collider")]
    #endif
    public class TriggerEvent : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Activation Via")]
        public TargetAllowedVia _targetAllowedVia;

        [MyHeader("Select Tag")]
        [TagField] public string _tag = "Player";

        [MyHeader("Select Layer")]
        public LayerMask _layer;

        [MyHeader("Event to Trigger")]
        public UnityEvent _event;

        #endregion

        #region UNITY METHODS

        private void OnTriggerEnter(Collider other)
        {
            switch (_targetAllowedVia)
            {
                case TargetAllowedVia.tag:
                    if (other.CompareTag(_tag))
                        OnPlayEvent();
                    break;
                case TargetAllowedVia.layer:
                    if (((1 << other.gameObject.layer) & _layer) != 0)
                        OnPlayEvent();
                    break;
            }
        }

        #endregion

        #region MECHANICS

        void OnPlayEvent()
        {
            _event.Invoke();
        }

        #endregion

        #region API

    

        #endregion
    }
}
