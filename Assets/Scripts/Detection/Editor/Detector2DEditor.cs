/*************************************************/
/* Detection/Detector2DEditor.cs                 */
/* Project: Game Mechanic - Detection (2D)       */
/* Author: James Brusewitz (valedict0)           */
/* Date Completed: 5/19/2024                     */
/*************************************************/

using UnityEditor;
using UnityEngine;

namespace Detection.Editor
{
    [CustomEditor(typeof(Detector2D))]
    public class Detector2DEditor : UnityEditor.Editor
    {
        private readonly Color visionBorder = new Color(1.0f, 1.0f, 1.0f, 0.500f);
        private readonly Color visionDetect = new Color(1.0f, 0.0f, 0.0f, 1.000f);

        private void OnSceneGUI()
        {
            Detector2D detector = (Detector2D)target;

            // Get vision properties from detectable.
            float visionAngle = detector.visionAngle;
            float visionRadius = detector.visionRadius;
            float visionRotationZ = detector.transform.rotation.eulerAngles.z;
            float visionRotation = Vector2.Angle(Vector2.down, detector.visionDirection) + visionRotationZ;
            Vector3 visionOrigin = detector.transform.position;
            Vector3 visionBoundL = detector.visionDirection.Rotate((visionAngle / 2.0f) + visionRotation).normalized;
            Vector3 visionBoundR = detector.visionDirection.Rotate(-(visionAngle / 2.0f) + visionRotation).normalized;

            // Draw borders of vision.
            Handles.color = visionBorder;
            Handles.DrawWireArc(visionOrigin, Vector3.forward, visionBoundR, visionAngle, visionRadius);
            Handles.DrawLine(visionOrigin, visionOrigin + (visionBoundL * visionRadius), 1.0f);
            Handles.DrawLine(visionOrigin, visionOrigin + (visionBoundR * visionRadius), 1.0f);

            // Draw vision lines to detectables.
            Handles.color = visionDetect;
            foreach (Detectable2D detectable in detector.GetDetectables())
            {
                Handles.DrawDottedLine(visionOrigin, detectable.transform.position, 1.0f);
            }
        }
    }
}
