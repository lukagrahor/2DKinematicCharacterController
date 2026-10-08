using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    InputAction moveAction;
    InputAction jumpAction;
    void Start()
    {
        player = GetComponent<Player>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        float movementDirection = moveAction.ReadValue<Vector2>().x;
        player.MovementDirection = movementDirection;
        player.IsJumpPressed = jumpAction.IsPressed();
    }
}
