using System.Collections.Generic;
using Detection;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    // Example class to show off detection system (not to be used).
    [RequireComponent(typeof(Detector2D), typeof(LineRenderer))]
    public sealed class Example : MonoBehaviour
    {
        [SerializeField]
        private Slider _sliderRotation = null;
        [SerializeField]
        private Slider _sliderAngle = null;
        [SerializeField]
        private Slider _sliderRadius = null;
        [SerializeField]
        private Text _textStatus = null;

        // Number of arc segments to draw for detector vision.
        [SerializeField, Min(1)]
        private int _arcSegments = 64;
        public int arcSegments
        {
            get
            {
                return _arcSegments;
            }
            set
            {
                _arcSegments = value >= 1 ? value : 1;
            }
        }

        private Detector2D _detector = null;
        private LineRenderer _lineRenderer = null;

        private void Awake()
        {
            _detector = GetComponent<Detector2D>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 360.0f * (_sliderRotation.value - 0.5f)));
            _detector.visionAngle = 360.0f * _sliderAngle.value;
            _detector.visionRadius = 8.0f * _sliderRadius.value;

            if (_detector.GetDetectables().Length > 0 )
            {
                _textStatus.text = "Detected!";
            }
            else
            {
                _textStatus.text = "Nothing detected!";
            }
        }

        private void Update()
        {
            // Haven't tested this much; might be broken in some cases.
            // Draw arc segments.
            List<Vector3> linePoints = new List<Vector3>();
            linePoints.Add(Vector3.zero);
            float arcAngle = Vector2.SignedAngle(Vector2.right, _detector.visionDirection) - (_detector.visionAngle / 2.0f);
            float arcLength = _detector.visionAngle;
            float arcSegmentAngle = arcLength / _arcSegments;
            float arcRadius = _detector.visionRadius;
            // Iterate through arc segments with theta arcAngle and make points with radius.
            for (int segment = 0; segment < _arcSegments; ++segment)
            {
                Vector3 point = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * arcAngle) * arcRadius,
                    Mathf.Sin(Mathf.Deg2Rad * arcAngle) * arcRadius
                    );
                arcAngle += arcSegmentAngle;
                linePoints.Add(point);
            }
            linePoints.Add(Vector3.zero);
            _lineRenderer.positionCount = linePoints.Count;
            _lineRenderer.SetPositions(linePoints.ToArray());
        }
    }
}
