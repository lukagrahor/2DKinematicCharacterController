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
    const float skinWidth = 0.015f;

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
        collisions.Reset();
        UpdatePlayerCorners();
        // explain why the order is important
        HorizontalCollisions(ref displacement);
        VerticalCollisions(ref displacement);
        rigidBody.MovePosition(rigidBody.position + displacement);
    }

    void CalculateNumberOfRays()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.RoundToInt(bounds.size.y / desiredRaySpacing);
        verticalRayCount = Mathf.RoundToInt(bounds.size.x / desiredRaySpacing);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        //Debug.Log($"sizeY: {bounds.size.y}, rayDistance: {desiredRaySpacing}, horizontalRayCount: {horizontalRayCount}, horizontalRaySpacing: {horizontalRaySpacing}");
    }

    void UpdatePlayerCorners()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        playerCorners.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        playerCorners.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        playerCorners.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        playerCorners.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void VerticalCollisions(ref Vector2 displacement)
    {
        float directionY = Mathf.Sign(displacement.y);
        float rayLength = Mathf.Abs(displacement.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? playerCorners.bottomLeft : playerCorners.topLeft;
            rayOrigin += (verticalRaySpacing * i + displacement.x) * Vector2.right; // this is the reason why Horizontal collisions must run first
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, directionY * rayLength * Vector2.up, hit ? Color.green : Color.red);

            if (hit)
            {
                displacement.y = (hit.distance - skinWidth) * directionY; // explain why must the skinWidth be added back
                rayLength = hit.distance; // explain why does the rayLenght need to be set to the hit distance

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void HorizontalCollisions(ref Vector2 displacement)
    {
        float directionX = Mathf.Sign(displacement.x);
        float rayLength = Mathf.Abs(displacement.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? playerCorners.topLeft : playerCorners.topRight;
            rayOrigin += (horizontalRaySpacing * i) * Vector2.down;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionX * Vector2.right, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, directionX * rayLength * Vector2.right, hit ? Color.green : Color.red);

            if (hit)
            {
                displacement.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }
}
