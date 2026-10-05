using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    Vector2 velocity;
    float gravity = -10f;
    float moveSpeed = 10f;

    PlayerController controller;

    public float MovementDirection { get; set; }

    void Start()
    {
        controller = GetComponent<PlayerController> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isGrounded = controller.CollisionData.below;
        float targetVelocityX = moveSpeed * MovementDirection;

        if (isGrounded || controller.CollisionData.above)
        {
            velocity.y = 0f;
        }

        velocity.x = targetVelocityX;
        velocity.y += gravity * Time.fixedDeltaTime;

        Vector2 displacement = velocity * Time.fixedDeltaTime;
        controller.Move(displacement);
    }
}
