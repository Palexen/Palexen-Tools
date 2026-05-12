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

namespace Palexen.CustomPhysics
{
    [AddComponentMenu("Palexen/Custom Physics/Rigidbody Velocity Limiter")]
    [RequireComponent(typeof(Rigidbody))]
    [ScriptDescription("Rigidbody Velocity Limiter", "Limit Rigidbody Velocity")]
    [ExecuteInEditMode]
    public class RigidbodyVelocityLimitation : MonoBehaviour
    {
        #region VARIABLES
        public float maxVelocity = 25f;
        Rigidbody _rigidbody;
        #endregion

        #region METHODS

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
#if UNITY_6000_0_OR_NEWER
            if (_rigidbody.linearVelocity.magnitude > maxVelocity)
                _rigidbody.linearVelocity = Vector3.ClampMagnitude(_rigidbody.linearVelocity, maxVelocity);
#else
            if(_rigidbody.velocity.magnitude > maxVelocity)
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxVelocity);
#endif
        }

        #endregion
    }
}
