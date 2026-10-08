using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    PlayerController controller;

    Vector2 velocity;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float maxJumpHeight = 4f;
    [SerializeField] float minJumpHeight = 0.5f;
    [SerializeField] float timeToJumpApex = 0.5f;

    [SerializeField] float accelerationTimeAirborne = 0.2f;
    [SerializeField] float acceleratonTimeGrounded = 0.1f;
    float velocityXSmoothing;

    [SerializeField] float maxJumpBufferTime = 0.1f;
    float jumpBufferCounter;

    [SerializeField] float coyoteTime = 0.2f;
    float coyoteCounter;

    public float MovementDirection { get; set; }
    public bool IsJumpPressed { get; set; }

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

            // reset to avoid firing more than once
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        // starts slowing down the speed of the jump if space is released earlier
        if (!IsJumpPressed && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }

        if (controller.CollisionData.left || controller.CollisionData.right)
        {
            velocity.x = 0f;
            velocityXSmoothing = 0f;
        }

        // first update the velocity, then the position (displacement) - semi-implicit Euler integration
        float targetVelocityX = moveSpeed * MovementDirection;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, isGrounded ? acceleratonTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;

        Vector2 displacement = velocity * Time.fixedDeltaTime;
        controller.Move(displacement);
    }
}
