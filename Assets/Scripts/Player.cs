using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    Vector2 velocity;
    float moveSpeed = 10f;

    float gravity;
    float jumpVelocity;

    [SerializeField] float jumpHeight = 4f;
    [SerializeField] float timeToJumpApex = 0.5f;

    PlayerController controller;

    public float MovementDirection { get; set; }
    public bool IsJumpPressed { get; set; }

    void Start()
    {
        controller = GetComponent<PlayerController> ();
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void FixedUpdate()
    {
        bool isGrounded = controller.CollisionData.below;
        float targetVelocityX = moveSpeed * MovementDirection;

        if (isGrounded || controller.CollisionData.above)
        {
            velocity.y = 0f;
        }

        if (isGrounded && IsJumpPressed)
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = targetVelocityX;
        velocity.y += gravity * Time.fixedDeltaTime;

        Vector2 displacement = velocity * Time.fixedDeltaTime;
        controller.Move(displacement);
    }
}
