# Unity-Detection
## Description
A simple 2D detection system for Unity.
## Implementation
Detector2D circle-casts for detectables, checking angles if the detectable is within vision, and raycasts to check for blocking obstacles. Detectables can be fetched with Detector2D.GetDetectables, and events are invoked on enter/exit.
## How to use
Attach a Detector2D to a game object and Detectable2D to another. Make sure detectable has a collider.
