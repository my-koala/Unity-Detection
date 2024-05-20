// Notes: Detector2D only detects other game objects with a Detectable2D component.
// Transform rotation z component rotates vision direction.

using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Detection
{
    public sealed class Detector2D : MonoBehaviour
    {
        // UnityEvents invoked when detectables enter and exit vision.
        public UnityEvent<Detectable2D> DetectableEntered = new UnityEvent<Detectable2D>();
        public UnityEvent<Detectable2D> DetectableExited = new UnityEvent<Detectable2D>();

        [Header("Vision")]

        // Distance from transform position to check for detectables.
        [SerializeField, Min(0.0f)]
        private float _visionRadius = 8.0f;
        public float visionRadius
        {
            get
            {
                return _visionRadius;
            }
            set
            {
                _visionRadius = value >= 0.0f ? value : 0.0f;
            }
        }

        // Maximum rotational angle in degrees from visionDirection to check for detectables.
        [SerializeField]
        private float _visionAngle = 120.0f;
        public float visionAngle
        {
            get
            {
                return _visionAngle;
            }
            set
            {
                _visionAngle = value;
            }
        }

        // Direction of vision to check for detectables.
        [SerializeField, Unity.Mathematics.PostNormalize]
        private Vector2 _visionDirection = Vector2.down;
        public Vector2 visionDirection
        {
            get
            {
                return _visionDirection;
            }
            set
            {
                _visionDirection = value.normalized;
            }
        }

        [Header("Physics")]

        // LayerMask to physics check against for detectables.
        [SerializeField]
        private LayerMask _layerMask = new LayerMask();
        public LayerMask layerMask
        {
            get
            {
                return _layerMask;
            }
            set
            {
                _layerMask = value;
            }
        }

        private HashSet<Detectable2D> _detectables = new HashSet<Detectable2D>();
        public Detectable2D[] GetDetectables()
        {
            Detectable2D[] detectables = new Detectable2D[_detectables.Count];
            _detectables.CopyTo(detectables);
            return detectables;
        }

        private void FixedUpdate()
        {
            // List of detectables detected this update.
            HashSet<Detectable2D> detectables = new HashSet<Detectable2D>();
            
            // Circle cast to find all colliders inside radius.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _visionRadius, _layerMask);
            foreach (Collider2D collider in colliders)
            {
                // Get detectable on collider.
                Detectable2D detectable = collider?.GetComponent<Detectable2D>();
                if (detectable == null)
                {
                    continue;// Skip colliders that are not detectables.
                }
                
                if (!detectable.isActiveAndEnabled)
                {
                    continue;// Skipping inactive/disabled detectables.
                }

                // Find collider direction and angle to detectable.
                Vector2 detectableDirection = detectable.transform.position - transform.position;
                float visionRotationZ = transform.rotation.eulerAngles.z;
                float detectableAngle = Vector2.Angle(_visionDirection, detectableDirection.Rotate(-visionRotationZ));

                if (detectableAngle > (_visionAngle / 2.0f))
                {
                    continue;// Skip detectables outside vision angle.
                }

                // Raycast to check for blocked vision to detectable.
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, detectableDirection, _visionRadius, _layerMask);
                if ((raycast.collider != collider) || (raycast.collider == null))
                {
                    continue;// Skip detectables with blocked vision.
                }

                detectables.Add(detectable);
            }

            // Note: Detectables entered and exited are cached before for accurate GetDetectables() during event invocation.
            HashSet<Detectable2D> entered = new HashSet<Detectable2D>();
            HashSet<Detectable2D> exited = new HashSet<Detectable2D>();

            foreach (Detectable2D detectable in detectables)
            {
                
                if (!_detectables.Contains(detectable))
                {
                    entered.Add(detectable);// If _detectables does not have detectable, it was just detected.
                }
                else
                {
                    _detectables.Remove(detectable);// Remove from _detectables to find exited.
                }
            }

            exited = _detectables;// Remaining _detectables were not detected this update, and have exited.

            // Update _detectables.
            _detectables = detectables;

            // Invoke entered and exited events.
            foreach (Detectable2D detectable in entered)
            {
                DetectableEntered.Invoke(detectable);
                detectable.DetectionStarted.Invoke(this);
            }
            foreach (Detectable2D detectable in exited)
            {
                DetectableExited.Invoke(detectable);
                detectable.DetectionStopped.Invoke(this);
            }
        }
    }
}
