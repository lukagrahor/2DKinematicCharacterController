using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D> ();
    }

    public void Move(Vector2 displacement)
    {
        rigidBody.MovePosition(rigidBody.position + displacement);
    }
}
