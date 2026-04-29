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
using Palexen.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Palexen.Gameplay.UI
{
    [AddComponentMenu("Palexen/UI/Interaction HUD")]
    [ScriptDescription("Interaction HUD", "A Representation on screen when you can interact with many objects in yout game")]
    public class InteractableHUD : MonoBehaviour
    {
        #region VARIABLES

        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public Animator m_animator;

        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage)] public Image baseImage;
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage)] public Image baseImageButton;

        Sprite baseIcon;
        Sprite baseButton;

        #endregion

        #region METHODS

        /// <summary>
        /// Reference to the interactable script, once get it, the animation will be play!
        /// </summary>
        /// <param name="HUDState"></param>
        public void GetHUD(bool HUDState)
        {
            m_animator.SetBool("IsInteractable", HUDState);
        }

        /// <summary>
        /// Allow change the shceme button icon in the input
        /// and the base icon allow change the base background icon, like hand, to sign the action!
        /// </summary>
        /// <param name="newBaseIcon"></param>
        /// <param name="newBaseButton"></param>
        public void SetSprites(Sprite newBaseIcon, Sprite newBaseButton)
        {
            baseIcon = newBaseIcon;
            baseButton = newBaseButton;
            baseImage.sprite = baseIcon;
            baseImageButton.sprite = baseButton;
        }

        #endregion
    }
}
