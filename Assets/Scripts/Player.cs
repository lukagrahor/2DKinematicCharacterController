using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    Vector2 velocity;
    float moveSpeed = 10f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    [SerializeField] float maxJumpHeight = 4f;
    [SerializeField] float minJumpHeight = 0.5f;
    [SerializeField] float timeToJumpApex = 0.5f;

    PlayerController controller;

    public float MovementDirection { get; set; }
    public bool IsJumpPressed { get; set; }

    [SerializeField] float accelerationTimeAirborne = 0.2f;
    [SerializeField] float acceleratonTimeGrounded = 0.1f;
    float velocityXSmoothing;

    [SerializeField] float maxJumpBufferTime = 0.1f;
    float jumpBufferCounter;

    [SerializeField] float coyoteTime = 0.2f;
    float coyoteCounter;

    public void SetJumpBuffer()
    {
        jumpBufferCounter = maxJumpBufferTime;
    }

    void Start()
    {
        controller = GetComponent<PlayerController> ();
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void FixedUpdate()
    {
        bool isGrounded = controller.CollisionData.below;
        float targetVelocityX = moveSpeed * MovementDirection;

        jumpBufferCounter = Mathf.Max(0, jumpBufferCounter - Time.fixedDeltaTime);

        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter = Mathf.Max(0, coyoteCounter - Time.fixedDeltaTime);

        if (isGrounded || controller.CollisionData.above)
        {
            velocity.y = 0f;
        }

        if (coyoteCounter > 0f && jumpBufferCounter > 0f)
        {
            velocity.y = maxJumpVelocity;
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        if (!IsJumpPressed && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, isGrounded ? acceleratonTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;

        Vector2 displacement = velocity * Time.fixedDeltaTime;
        controller.Move(displacement);
    }
}
