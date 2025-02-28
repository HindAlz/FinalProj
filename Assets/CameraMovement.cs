using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float touchSensitivity = 5f;

    private Vector2 touchCoords;
    private Transform player;
    private Vector3 eulers;

    private int rightTouchId = -1;

    private void Start()
    {
        player = transform.parent; // Camera follows the player
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void LateUpdate()
    {
        UpdateCameraRotation();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width * 0.5f && rightTouchId == -1)
                {
                    rightTouchId = touch.fingerId;
                    touchCoords = touch.position;
                }
                else if (touch.fingerId == rightTouchId && touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = (touch.position - touchCoords) * touchSensitivity * Time.deltaTime;
                    eulers.x -= delta.x;
                    eulers.y = Mathf.Clamp(eulers.y + delta.y, -89.99f, 89.99f);
                    touchCoords = touch.position;
                }
                else if (touch.fingerId == rightTouchId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    rightTouchId = -1;
                }
            }
        }
    }

    private void UpdateCameraRotation()
    {
        transform.rotation = Quaternion.Euler(-eulers.y, eulers.x + 180, 0);
        transform.position = player.position + Vector3.up * 2f;
    }
}
