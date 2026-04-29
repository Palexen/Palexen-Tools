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
using Palexen.Gameplay.Input;
using Palexen.Gameplay.UI;
using Palexen.Tools;

namespace Palexen.Gameplay.Player
{
    [AddComponentMenu("Palexen/Gameplay/Player Interaction")]
    [ScriptDescription("Player Interaction", "You can interact with objects with Interactable script added")]
    public class PlayerInteraction : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Interactable Objects Setup")]
        public InteractionButton button;
        InteractionButton currentObjectButton;

        public LayerMask interactableLayerMask;
        public QueryTriggerInteraction interactionMethod = QueryTriggerInteraction.Collide;
        public float maxDistance = 1;

        Interactable_Input ii;
        bool isInteractable;
        InteractableHUD interactableHUD;

        #endregion

        #region METHODS

        /// <summary>
        /// Try connect with the current selected interaction button
        /// </summary>
        private void Awake()
        {
            ii = new();

            ii.Actions.InteractWithAction.performed += ctx_ => TryInteract(InteractionButton.action);
            ii.Actions.InteractWithJump.performed += ctx_ => TryInteract(InteractionButton.jump);
            ii.Actions.InteractWithChange.performed += ctx_ => TryInteract(InteractionButton.change);
            ii.Actions.InteractWithCrouch.performed += ctx_ => TryInteract(InteractionButton.crouch);
        }

        /// <summary>
        /// Search for interaction HUD
        /// </summary>
        private void Start()
        {
            interactableHUD = FindFirstObjectByType<InteractableHUD>();
        }

        /// <summary>
        /// Enable input for this component
        /// </summary>
        private void OnEnable()
        {
            ii.Enable();
        }

        /// <summary>
        /// Disable input for this component
        /// </summary>
        private void OnDisable()
        {
            ii.Disable();
        }

        /// <summary>
        /// Update the scaner raycast
        /// </summary>
        private void Update()
        {
            UpdateScaner();
        }

        /// <summary>
        /// Compare the raycast to the other layers, if is the mask selected in layer mask,
        /// The raycast search for that layer, once find it, it will initialize the functions!
        /// </summary>
        void UpdateScaner()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, interactableLayerMask, interactionMethod))
            {
                isInteractable = true;

                InteractableComponent ic = hit.transform.gameObject.GetComponent<InteractableComponent>();
                currentObjectButton = ic.GetInteractionButton();
                button = currentObjectButton;
                interactableHUD.SetSprites(ic.GetBaseIcon(), ic.GetBaseButton());
            }
            else
            {
                isInteractable = false;
            }

            interactableHUD.GetHUD(isInteractable);
        }

        /// <summary>
        /// Once find the correct layer, you can interact with!
        /// </summary>
        public void InteractWith()
        {
            if (isInteractable)
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, interactableLayerMask, interactionMethod))
                {
                    InteractableComponent ic = hit.transform.gameObject.GetComponent<InteractableComponent>();

                    ic.Interact();
                }
            }
        }

        /// <summary>
        /// It will try interact with the button selected from interactable component object, setup in the inspector!
        /// </summary>
        /// <param name="pressedButton"></param>
        void TryInteract(InteractionButton pressedButton)
        {
            if (!isInteractable) return;

            if (pressedButton == currentObjectButton)
            {
                InteractWith();
            }
        }

        #endregion
    }
}
