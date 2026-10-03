using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    Vector2 velocity;
    float gravity = -10f;

    PlayerController controller;
    void Start()
    {
        controller = GetComponent<PlayerController> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity.y += gravity * Time.fixedDeltaTime;
        Vector2 displacement = velocity * Time.fixedDeltaTime;
        controller.Move(displacement);
    }
}
