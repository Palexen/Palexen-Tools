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
using Palexen.Gameplay.Input;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Palexen.Gameplay
{
    [AddComponentMenu("Palexen/Gameplay/Interactable Object")]
    [ScriptDescription("Interactable Object", "You can interact with (player needs Player interaction Script), mark this object as interactable layer")]
    public class InteractableComponent : MonoBehaviour
    {
        #region VARIABLES
        [MyHeader("Key Input")]
        public InteractionButton interactButton;

        [MyHeader("Interaction System")]
        public string[] playMethods = { "UpdateObjectManager", "PlayVibration", "ChangeLayer", "DestroyThisGameObject", "ApplyBehaviour" };

        [Header("Custom Scripts behaviours")]
        public List<BehaviourSet> externalBehaviours;

        [Header("Gameplay Objects Manager | Call as 'UpdateObjectManager' ")]
        public List<ObjectManager> objectManager = new();

        [MyHeader("Gamepad Setup")]
        public Vector2 motorVelocity = Vector2.one;
        public float vibrationTimer = 0.25f;

        [MyHeader("UI")]
        public Sprite baseIcon;
        public Sprite baseButton;

        [Header("INPUT SCHEME")]
        public GetInputSchemaBehaviour inputSchemaBehaviour;
        [Header("This Schema Only works From Interactable Schema (Below)")]
        public InputSchema schema;
        public Sprite _PCButton;
        public Sprite _nintentdoSwitchButton;
        public Sprite _xBOXButton;
        public Sprite _PlayStationButton;
        public Sprite _touchScreenButton;

        #endregion

        #region METHODS

        /// <summary>
        /// Disable Controller Haptics
        /// </summary>
        private void OnDisable()
        {
            Gamepad.current?.PauseHaptics();
        }

        /// <summary>
        /// Get Enable Input Schema Getter
        /// </summary>
        private void Start()
        {
            Invoke(nameof(EnableInputSchema), 1);
        }

        /// <summary>
        /// Switch the get input schema method
        /// </summary>
        void EnableInputSchema()
        {
            switch (inputSchemaBehaviour)
            {
                case GetInputSchemaBehaviour.fromGameInputManager:
                    GetInputSchemaFromGameInputManager();
                    break;

                case GetInputSchemaBehaviour.fromInteractableScheme:
                    GetInputSchemaFromComponent();
                    break;
            }
        }

        /// <summary>
        /// In case that the input method getter is Get from input manager schema
        /// this function will set the input schema for this interactable object from input manager schema
        /// </summary>
        void GetInputSchemaFromGameInputManager()
        {
            schema = GameInputSchema.Instance.GetInputSchema();

            switch (schema)
            {
                case InputSchema.PC:

                    if(interactButton == InteractionButton.action)
                    {
                        
                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[0].variants[0];
                    }

                    if (interactButton == InteractionButton.jump)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[1].variants[0];
                    }

                    if (interactButton == InteractionButton.change)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[2].variants[0];
                    }

                    if (interactButton == InteractionButton.crouch)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[3].variants[0];
                    }

                    break;

                case InputSchema.XBOX:

                    if (interactButton == InteractionButton.action)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[0].variants[1];
                    }

                    if (interactButton == InteractionButton.jump)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[1].variants[1];
                    }

                    if (interactButton == InteractionButton.change)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[2].variants[1];
                    }

                    if (interactButton == InteractionButton.crouch)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[3].variants[1];
                    }

                    break;

                case InputSchema.playStation:

                    if (interactButton == InteractionButton.action)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[0].variants[2];
                    }

                    if (interactButton == InteractionButton.jump)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[1].variants[2];
                    }

                    if (interactButton == InteractionButton.change)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[2].variants[2];
                    }

                    if (interactButton == InteractionButton.crouch)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[3].variants[2];
                    }

                    break;

                case InputSchema.nintendoSwitch:

                    if (interactButton == InteractionButton.action)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[0].variants[3];
                    }

                    if (interactButton == InteractionButton.jump)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[1].variants[3];
                    }

                    if (interactButton == InteractionButton.change)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[2].variants[3];
                    }

                    if (interactButton == InteractionButton.crouch)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[3].variants[3];
                    }

                    break;

                case InputSchema.touchScreen:

                    if (interactButton == InteractionButton.action)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[0].variants[4];
                    }

                    if (interactButton == InteractionButton.jump)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[1].variants[4];
                    }

                    if (interactButton == InteractionButton.change)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[2].variants[4];
                    }

                    if (interactButton == InteractionButton.crouch)
                    {

                        baseButton = GameInputSchema.Instance.GetInputSchemaContainer(0).buttonSchemas[3].variants[4];
                    }

                    break;

                default:
                    Debug.Log("Nothing to do!");
                    break;
            }
        }

        /// <summary>
        /// Ingone the global input shcema, and allow the custom setter if is the case
        /// </summary>
        void GetInputSchemaFromComponent()
        {
            baseButton = schema switch
            {
                InputSchema.nintendoSwitch => _nintentdoSwitchButton,
                InputSchema.XBOX => _xBOXButton,
                InputSchema.playStation => _PlayStationButton, 
                InputSchema.touchScreen => _touchScreenButton,
                _ => _PCButton,
            };
        }

        /// <summary>
        /// Interact with this game object!
        /// </summary>
        public void Interact()
        {
            for(int i = 0; i < playMethods.Length; i++)
            {
                Invoke(playMethods[i], 0);
            }
        }

        /// <summary>
        /// Applies some changes in your game logic when you need it, a clasic!
        /// </summary>
        void UpdateObjectManager()
        {
            for (int i = 0; i < objectManager.Count; i++)
            {
                objectManager[i].ApplyChanges();
            }
        }

        /// <summary>
        /// Change the layer of this interactable object if you still need it, of course, 
        /// you neen erase the delete method in the inspector
        /// </summary>
        void ChangeLayer()
        {
            gameObject.layer = 0;
        }

        /// <summary>
        /// Invoke External Evenets in your scripts, game objects and muche more!
        /// </summary>
        void ApplyBehaviour()
        {
            for(int i = 0; i < externalBehaviours.Count; i++)
            {
                externalBehaviours[i].ApplyBehaviour();
            }
        }

        /// <summary>
        /// Intersect the game button result and you need press it to interact
        /// </summary>
        /// <returns></returns>
        public InteractionButton GetInteractionButton()
        {
            return interactButton;
        }

        /// <summary>
        /// Get hand icon, connection with player interaction, at line 71
        /// </summary>
        /// <returns></returns>
        public Sprite GetBaseIcon()
        {
            return baseIcon;
        }

        /// <summary>
        /// Get hand icon, connection with player interaction, at line 71
        /// </summary>
        /// <returns></returns>
        public Sprite GetBaseButton()
        {
            return baseButton;
        }

        #region GAMEPAD FUNCTIONS

        void PlayVibration()
        {
            Gamepad.current?.SetMotorSpeeds(motorVelocity.x, motorVelocity.y);
            Invoke(nameof(StopVibration), vibrationTimer);
        }

        void StopVibration()
        {
            Gamepad.current?.PauseHaptics();
        }

        void DestroyThisGameObject()
        {
            Destroy(gameObject, vibrationTimer + 0.1f);
        }

        private void OnDestroy()
        {
            StopVibration();
        }

        #endregion

        #endregion
    }
}
