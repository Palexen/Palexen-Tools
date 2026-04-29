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
using System.Collections.Generic;

namespace Palexen.Gameplay
{
    [AddComponentMenu("Palexen/Gameplay/Trigger Objects Manager")]
    public class TriggerObjectsManager : MonoBehaviour
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

        [Space(20)]
        [MyHeader("Objects Manager Setup")]
        [Tooltip("The collection of objects in list format, can be 1 or as many as you need")]
        public List<ObjectManager> objects = new();

        #endregion

        #region METHODS

        private void OnTriggerEnter(Collider other)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                switch (_targetAllowedVia)
                {
                    case TargetAllowedVia.tag:

                        if (other.CompareTag(_tagName))
                        {
                            objects[i].ApplyChanges();
                        }

                        break;

                    case TargetAllowedVia.layer:

                        if (((1 << other.gameObject.layer) & _layerMask) != 0)
                        {
                            objects[i].ApplyChanges();
                        }

                        break;
                }
            }
        }

        #endregion
    }
}
