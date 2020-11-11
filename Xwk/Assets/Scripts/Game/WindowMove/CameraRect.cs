using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机绘制辅助类
/// </summary>
public class CameraRect : MonoBehaviour {

    Camera theCamera;
    float upperDistance;
    float lowerDistance;
    Transform tx;
    void OnDrawGizmos()
    {
        if (!theCamera)
        {
            theCamera = Camera.main;
        }
        tx = theCamera.transform;
        upperDistance = theCamera.farClipPlane;
        lowerDistance = theCamera.nearClipPlane;
        FindLower2UpperCorners();
    }
    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];

        float halfFOV = (theCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = theCamera.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        // UpperLeft
        corners[0] = tx.position - (tx.right * width);
        corners[0] += tx.up * height;
        corners[0] += tx.forward * distance;

        // UpperRight
        corners[1] = tx.position + (tx.right * width);
        corners[1] += tx.up * height;
        corners[1] += tx.forward * distance;

        // LowerLeft
        corners[2] = tx.position - (tx.right * width);
        corners[2] -= tx.up * height;
        corners[2] += tx.forward * distance;

        // LowerRight
        corners[3] = tx.position + (tx.right * width);
        corners[3] -= tx.up * height;
        corners[3] += tx.forward * distance;

        return corners;
    }

    void FindLower2UpperCorners()
    {
        Vector3[] corners_upper = GetCorners(upperDistance);
        Vector3[] corners_lower = GetCorners(lowerDistance);

        Debug.DrawLine(corners_lower[0], corners_upper[0], Color.green);
        Debug.DrawLine(corners_lower[1], corners_upper[1], Color.green);
        Debug.DrawLine(corners_lower[2], corners_upper[2], Color.green);
        Debug.DrawLine(corners_lower[3], corners_upper[3], Color.green);


        Debug.DrawLine(corners_upper[0], corners_upper[1], Color.green);
        Debug.DrawLine(corners_upper[1], corners_upper[3], Color.green);
        Debug.DrawLine(corners_upper[2], corners_upper[0], Color.green);
        Debug.DrawLine(corners_upper[3], corners_upper[2], Color.green);
    }

  
}
