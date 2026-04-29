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

namespace Palexen.Tools
{
    [AddComponentMenu("Palexen/Tools/Shape Visualizer")]
    [ScriptDescription("Shape Visualizer", "Allows draw gizmos to the Unity Editor, you can draw many shapes forms as you need ")]
    public class ShapeVisualizer : MonoBehaviour
    {
        #region VARIABLES

        public Color shapeColor = Color.cyan;
        Collider tempCollider;

        #endregion

        #region METHODS

        private void OnDrawGizmos()
        {
            Collider[] tempColliders = gameObject.GetComponents<Collider>();

            Gizmos.color = shapeColor;
            Gizmos.matrix = transform.localToWorldMatrix;

            if (tempColliders != null)
            {
                foreach (var tempCollider in tempColliders)
                {
                    if (tempCollider is BoxCollider bx)
                    {
                        Gizmos.DrawWireCube(bx.center, bx.size);
                    }

                    if (tempCollider is SphereCollider sp)
                    {
                        Gizmos.DrawWireSphere(sp.center, sp.radius);
                    }

                    if (tempCollider is MeshCollider mc)
                    {
                        Gizmos.DrawWireMesh(mc.sharedMesh);
                    }

                    if (tempCollider is CapsuleCollider cc)
                    {
                        float r = cc.radius;
                        float h = cc.height;
                        float halfH = Mathf.Max(0, (h * 0.5f) - r);
                        Vector3 c = cc.center;

                        Vector3 top = c, bot = c;
                        if (cc.direction == 0) { top.x += halfH; bot.x -= halfH; }
                        else if (cc.direction == 1) { top.y += halfH; bot.y -= halfH; }
                        else { top.z += halfH; bot.z -= halfH; }

                        Gizmos.DrawWireSphere(top, r);
                        Gizmos.DrawWireSphere(bot, r);

                        if (cc.direction == 1)
                        {
                            Gizmos.DrawLine(top + Vector3.right * r, bot + Vector3.right * r);
                            Gizmos.DrawLine(top + Vector3.left * r, bot + Vector3.left * r);
                            Gizmos.DrawLine(top + Vector3.forward * r, bot + Vector3.forward * r);
                            Gizmos.DrawLine(top + Vector3.back * r, bot + Vector3.back * r);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Collider[] tempColliders = gameObject.GetComponents<Collider>();

            Gizmos.color = shapeColor;
            Gizmos.matrix = transform.localToWorldMatrix;

            foreach (var tempCollider in tempColliders)
            {
                if (tempCollider is BoxCollider bx)
                {
                    Gizmos.DrawCube(bx.center, bx.size);
                }

                if (tempCollider is SphereCollider sp)
                {
                    Gizmos.DrawSphere(sp.center, sp.radius);
                }

                if (tempCollider is MeshCollider mc)
                {
                    Gizmos.DrawMesh(mc.sharedMesh);
                }

                if (tempCollider is CapsuleCollider cc)
                {
                    Matrix4x4 oldMatrix = Gizmos.matrix;

                    float offsetValue = Mathf.Max(0, (cc.height * 0.5f) - cc.radius);
                    Vector3 offset = Vector3.zero;

                    offset[cc.direction] = offsetValue;

                    Vector3 point1 = cc.center + offset;
                    Vector3 point2 = cc.center - offset;

                    DrawCapsule(point1, point2, cc.radius);

                    Gizmos.matrix = oldMatrix;
                }
            }
        }

        private void DrawCapsule(Vector3 p1, Vector3 p2, float radius)
        {
            Gizmos.DrawSphere(p1, radius);
            Gizmos.DrawSphere(p2, radius);
            Gizmos.DrawLine(p1 + -Vector3.up * radius, p2 + Vector3.up * radius);
        }

        #endregion
    }
}
