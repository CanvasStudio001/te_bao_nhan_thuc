using UnityEngine;
using Cinemachine;

public class CameraCollisionAvoidance : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public LayerMask obstacleLayer;
    public float minimumDistance = 1.0f;
    public float damping = 1.0f;

    void Start()
    {
        if (virtualCamera != null)
        {
            var collider = virtualCamera.GetComponent<CinemachineCollider>();
            if (collider == null)
            {
                collider = virtualCamera.gameObject.AddComponent<CinemachineCollider>();
            }

            // Cấu hình CinemachineCollider
            collider.m_AvoidObstacles = true;
            collider.m_CollideAgainst = obstacleLayer;
            collider.m_MinimumDistanceFromTarget = minimumDistance;
            collider.m_Damping = damping;
        }
        else
        {
            Debug.LogError("Virtual camera is not assigned.");
        }
    }
}
