using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    [SerializeField] LayerMask collisionMask;

    int horizontalRayCount;
    int verticalRayCount;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    float desiredRaySpacing = 0.1f;

    PlayerCorners playerCorners;

    CollisionInfo collisions;
    public CollisionInfo CollisionData => collisions;

    struct PlayerCorners
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            right = left = false;
        }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D> ();
        boxCollider = GetComponent<BoxCollider2D> ();
        CalculateNumberOfRays();
    }

    public void Move(Vector2 displacement)
    {
        UpdatePlayerCorners();
        VerticalCollisions(ref displacement);
        rigidBody.MovePosition(rigidBody.position + displacement);
    }

    void CalculateNumberOfRays()
    {
        Bounds bounds = boxCollider.bounds;

        horizontalRayCount = Mathf.RoundToInt(bounds.size.y / desiredRaySpacing);
        verticalRayCount = Mathf.RoundToInt(bounds.size.x / desiredRaySpacing);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        //Debug.Log($"sizeY: {bounds.size.y}, rayDistance: {desiredRaySpacing}, horizontalRayCount: {horizontalRayCount}, horizontalRaySpacing: {horizontalRaySpacing}");
    }

    void UpdatePlayerCorners()
    {
        Bounds bounds = boxCollider.bounds;

        playerCorners.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        playerCorners.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        playerCorners.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        playerCorners.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void VerticalCollisions(ref Vector2 displacement)
    {
        float directionY = Mathf.Sign(displacement.y);
        float rayLength = Mathf.Abs(displacement.y);

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? playerCorners.bottomLeft : playerCorners.topLeft;
            rayOrigin += (verticalRaySpacing * i + displacement.x) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, directionY * rayLength * Vector2.up, hit ? Color.green : Color.red);

            if (hit)
            {
                displacement.y = hit.distance * directionY;
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }
}
