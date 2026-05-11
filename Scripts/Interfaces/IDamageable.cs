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
    public interface IDamageable
    {
        #region METHODS
        /// <summary>
        /// Sets the current health value for the entity.
        /// </summary>
        /// <param name="health">The amount of health to add. Can be positive or negative.</param>
        void AddHealth(int health);

        /// <summary>
        /// Apply damage to the object.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        void TakeDamage(int damageAmount);

        /// <summary>
        /// Performs any actions required to terminate or deactivate the current instance.
        /// </summary>
        /// <remarks>Call this method to signal that the object should cease functioning or be removed from use.
        /// The specific effects depend on the implementation and may include resource cleanup or state changes.</remarks>
        void Die();

        /// <summary>
        /// Handles the event or condition when a predefined limit or threshold has been exceeded.
        /// </summary>
        void Exceeded();
        #endregion
    }
}
