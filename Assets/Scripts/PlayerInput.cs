using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    InputAction moveAction;
    void Start()
    {
        player = GetComponent<Player>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        float movementDirection = moveAction.ReadValue<Vector2>().x;
        player.MovementDirection = movementDirection;
    }
}
