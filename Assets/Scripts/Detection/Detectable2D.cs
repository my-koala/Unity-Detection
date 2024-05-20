/*************************************************/
/* Detectable2D.cs                               */
/* Project: Game Mechanic - Detection (2D)       */
/* Author: James Brusewitz (valedict0)           */
/* Date Completed: 5/19/2024                     */
/*************************************************/

using UnityEngine.Events;
using UnityEngine;

namespace Detection
{
    public sealed class Detectable2D : MonoBehaviour
    {
        public UnityEvent<Detector2D> DetectionStarted = new UnityEvent<Detector2D>();
        public UnityEvent<Detector2D> DetectionStopped = new UnityEvent<Detector2D>();
    }
}
