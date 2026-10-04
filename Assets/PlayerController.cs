using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpQueued;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // Keyboard inputs for WASD / Arrows
        float h = 0f;
        float v = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;

            if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpQueued = true;
            }
        }

        moveInput = new Vector2(h, v);
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (c.GetContact(0).normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision c)
    {
        isGrounded = false;
    }
}
    