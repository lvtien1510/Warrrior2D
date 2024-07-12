using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;

    [Header("Camera Limits")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float lookAhead;
    private Vector3 targetPosition;

    private void LateUpdate()
    {
        // Xác định vị trí mục tiêu
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y, transform.position.z);

        // Làm mượt chuyển động của camera
        targetPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);

        // Áp dụng giới hạn cho vị trí của camera
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Cập nhật vị trí camera
        transform.position = targetPosition;

        // Cập nhật lookAheadX và lookAheadY
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);
    }
}
