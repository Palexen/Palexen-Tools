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
using UnityEngine.UI;

namespace Palexen.Gameplay.UI
{
    [AddComponentMenu("Palexen/UI/3D Icon")]
    [ScriptDescription("3D Icon", "Set 3D icons in your world (Mark with layer that can be rendererd with a depth camera)")]
    public class WorldIcon : MonoBehaviour
    {
        [MyHeader("World Setup")]
        public Icon3DMethod m_3DIconMethod;
        [VectorSlider(1, 50)] public Vector2Int sizeControl = new(1, 50);
        public float maxDistance = 20;
        Transform target;

        [MyHeader("Fade UI Setup")]
        public Icon3DUIUsage m_UIFadeMethod;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] public CanvasGroup canvasGroup;
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public Image icon;
        [VectorSlider(0, 1)] public Vector2 opacity = new(0, 1);
        public float minDistance = 15;
        public float fadeSpeed = 5;

        private void Start()
        {
            target = Camera.main.transform;
        }


        private void Update()
        {
            CatchCamera();
            CalculateSize();

            if (m_3DIconMethod == Icon3DMethod.fadeDistance)
            {
                UpdateUI();
            }
        }

        void CalculateSize()
        {
            float distance = Vector3.Distance(transform.position, target.position);

            float scaleFactor = Mathf.Clamp(sizeControl.x + (distance / maxDistance), sizeControl.x, sizeControl.y);

            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        void UpdateUI()
        {
            switch (m_UIFadeMethod)
            {
                case Icon3DUIUsage.canvasGroup:

                    if ((target.position - transform.position).magnitude <= minDistance)
                    {
                        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, opacity.y, Time.deltaTime * fadeSpeed);
                    }
                    else
                    {
                        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, opacity.x, Time.deltaTime * fadeSpeed);
                    }

                    break;
                default:


                case Icon3DUIUsage.image:

                    float alpha = icon.color.a;

                    if ((target.position - transform.position).magnitude <= minDistance)
                    {

                        alpha = Mathf.MoveTowards(icon.color.a, opacity.y, Time.deltaTime * fadeSpeed);

                        icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha);
                    }
                    else
                    {
                        alpha = Mathf.MoveTowards(icon.color.a, opacity.x, Time.deltaTime * fadeSpeed);

                        icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha);
                    }

                    break;
            }
        }

        void CatchCamera()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
