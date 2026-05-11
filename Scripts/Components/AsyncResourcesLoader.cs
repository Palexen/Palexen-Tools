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

namespace Palexen.Gameplay
{
    [AddComponentMenu("Palexen/Gameplay/Async Loader")]
    [ScriptDescription("Async Loader", "Manage the spawn resources on a scene")]
    public class AsyncResourcesLoader : MonoBehaviour
    {
        public GameObject[] gameplayResources;
        public float activationInterval = 1.0f;

        private int currentIndex = 0;

        private void Start()
        {
            foreach (GameObject res in gameplayResources)
            {
                res.SetActive(false);
            }

            InvokeRepeating(nameof(ActivateNextResource), 0f, activationInterval);
        }

        /// <summary>
        /// Activates the next resource in the gameplayResources collection, if available.
        /// </summary>
        /// <remarks>If all resources have already been activated, this method cancels further scheduled
        /// invocations of itself. This method is intended to be called repeatedly, typically as part of a timed
        /// sequence.</remarks>
        private void ActivateNextResource()
        {
            if (currentIndex < gameplayResources.Length)
            {
                gameplayResources[currentIndex].SetActive(true);
                currentIndex++;
            }
            else
            {
                CancelInvoke(nameof(ActivateNextResource));
            }
        }
    }
}
