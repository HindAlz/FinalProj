using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 moveDirection;
    private CameraMovement cameraScript;

    private int leftTouchId = -1;
    private Vector2 leftTouchStart;
    private Vector2 leftTouchCurrent;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject cameraObj = GameObject.Find("Main Camera");
        if (cameraObj != null)
        {
            cameraScript = cameraObj.GetComponent<CameraMovement>();
        }
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width * 0.5f && leftTouchId == -1)
                {
                    leftTouchId = touch.fingerId;
                    leftTouchStart = touch.position;
                    leftTouchCurrent = touch.position;
                }
                else if (touch.fingerId == leftTouchId && touch.phase == TouchPhase.Moved)
                {
                    leftTouchCurrent = touch.position;
                }
                else if (touch.fingerId == leftTouchId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    leftTouchId = -1;
                }
            }
        }
    }

    private void MovePlayer()
    {
        if (leftTouchId != -1)
        {
            Vector2 touchDelta = (leftTouchCurrent - leftTouchStart).normalized;
            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

            moveDirection = (right * touchDelta.x + forward * touchDelta.y).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }
}
