using UnityEngine;

public static class Extensions
{
    /**
     * Rotates a Vector2 by the given angle in degrees.
     */
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        angle *= Mathf.Deg2Rad;// Convert angle to radians.
        return new Vector2(
            (vector.x * Mathf.Cos(angle)) - (vector.y * Mathf.Sin(angle)),
            (vector.x * Mathf.Sin(angle)) + (vector.y * Mathf.Cos(angle))
        );
    }
}
