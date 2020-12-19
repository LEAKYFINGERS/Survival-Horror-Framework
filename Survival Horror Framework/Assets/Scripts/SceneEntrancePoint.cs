////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        19.12.20
// Date last edited:    19.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script for a gameobject used to indicate one of the points the player character can 'enter' a scene using position and orientation.
public class SceneEntrancePoint : MonoBehaviour
{
    public Color GizmoDrawColor = new Color(1.0f, 0.0f, 0.0f, 1.0f); // The color used to draw a line gizmo to indicate the position and orientation of the scene entrance point.
    public float GizmoDrawRadius = 0.5f; // The width and length of the wire cube gizmo used to indicate the placement of the scene entrance point.
    public float GizmoDrawHeight = 2.0f; // The height of the wire cube gizmo used to indicate the placement of the scene entrance point.
    public float GizmoDrawOrientationLength = 1.0f; // The length of the line gizmo used to indicate the orientation of the scene entrance point.

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoDrawColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * GizmoDrawOrientationLength);
        Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(GizmoDrawRadius, GizmoDrawHeight, GizmoDrawRadius));
    }
}
