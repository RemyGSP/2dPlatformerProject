using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] private Transform[] groundCheckers;
    [SerializeField] private Transform[] leftCheckers;
    [SerializeField] private Transform[] rightCheckers;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float rayLength;
    private Movement movement;

    //Hay que hacer que de alguna manera los raycast de la derecha i la izquierda sigan apuntando correctamente incluso despues de girar al personaje
    private RaycastHit2D leftBorder;
    private RaycastHit2D middle;
    private RaycastHit2D rightBorder;
    private RaycastHit2D leftTopBorder;
    private RaycastHit2D leftMiddleBorder;
    private RaycastHit2D leftBottomBorder;
    private RaycastHit2D rightTopBorder;
    private RaycastHit2D rightMiddleBorder;
    private RaycastHit2D rightBottomBorder;

    private void Start()
    {

        movement = GetComponent<Movement>();

    }
    private void Awake()
    {

    }
    public bool CheckGrounded()
    {
        bool grounded = false;
        middle = Physics2D.Raycast(groundCheckers[0].position, Vector2.down, rayLength, ground);

        if (middle.collider != null )
        {
            grounded = true;
        }
        return grounded;
    }

    public bool CheckIfTraspassable()
    {
        bool traspassable = false;
        middle = Physics2D.Raycast(groundCheckers[0].position, Vector2.down, rayLength, ground);

        if ( middle.collider != null && middle.transform.gameObject.CompareTag("TraspassablePlatform"))
        {
            traspassable = true;
        }
        return traspassable;

    }

    public bool CheckSides()
    {
        bool contact = false;

        leftTopBorder = Physics2D.Raycast(leftCheckers[0].position, new Vector2(-1, 0), rayLength, ground);
        leftMiddleBorder = Physics2D.Raycast(leftCheckers[1].position, new Vector2(-1, 0), rayLength, ground);
        leftBottomBorder = Physics2D.Raycast(leftCheckers[2].position, new Vector2(-1, 0), rayLength, ground);
        rightTopBorder = Physics2D.Raycast(rightCheckers[0].position, new Vector2(1, 0), rayLength, ground);
        rightMiddleBorder = Physics2D.Raycast(rightCheckers[1].position, new Vector2(1, 0), rayLength, ground);
        rightBottomBorder = Physics2D.Raycast(rightCheckers[2].position, new Vector2(1, 0), rayLength, ground);

        if (leftTopBorder.collider != null || leftMiddleBorder.collider != null || leftBottomBorder.collider != null || rightTopBorder.collider != null ||rightMiddleBorder.collider != null || rightBottomBorder.collider != null)
        {
            contact = true;
        }
        return contact;
    }

    public bool CheckLeftSide()
    {
        bool contact = false;
        float horizontalDirection = Mathf.Sign(movement.lookingDirection);

        leftTopBorder = Physics2D.Raycast(leftCheckers[0].position, Vector2.left, rayLength, ground);
        leftMiddleBorder = Physics2D.Raycast(leftCheckers[1].position, Vector2.left, rayLength, ground);
        leftBottomBorder = Physics2D.Raycast(leftCheckers[2].position, Vector2.left, rayLength, ground);
        rightTopBorder = Physics2D.Raycast(rightCheckers[0].position, Vector2.left, rayLength, ground);
        rightMiddleBorder = Physics2D.Raycast(rightCheckers[1].position, Vector2.left, rayLength, ground);
        rightBottomBorder = Physics2D.Raycast(rightCheckers[2].position, Vector2.left, rayLength, ground);

        if (leftTopBorder.collider != null || leftMiddleBorder.collider != null || leftBottomBorder.collider != null || rightTopBorder.collider != null || rightMiddleBorder.collider != null || rightBottomBorder.collider != null)
        {
            contact = true;
        }
        return contact;
    }

    public Collider2D GetGroundCollider()
    {
        leftBorder = Physics2D.Raycast(groundCheckers[0].position, Vector2.down, rayLength, ground);
        middle = Physics2D.Raycast(groundCheckers[1].position, Vector2.down, rayLength, ground);
        rightBorder = Physics2D.Raycast(groundCheckers[2].position, Vector2.down, rayLength, ground);
        Collider2D groundCollider = null;
        if (leftBorder.collider != null) groundCollider = leftBorder.collider;
        else if (middle.collider != null) groundCollider = middle.collider;
        else if (rightBorder.collider != null) groundCollider = rightBorder.collider;



        return groundCollider;
    }
    public bool CheckRightSide()
    {
        bool contact = false;
        float horizontalDirection = Mathf.Sign(movement.lookingDirection);

        leftTopBorder = Physics2D.Raycast(leftCheckers[0].position, Vector2.right, rayLength, ground);
        leftMiddleBorder = Physics2D.Raycast(leftCheckers[1].position, Vector2.right, rayLength, ground);
        leftBottomBorder = Physics2D.Raycast(leftCheckers[2].position, Vector2.right, rayLength, ground);
        rightTopBorder = Physics2D.Raycast(rightCheckers[0].position, Vector2.right, rayLength, ground);
        rightMiddleBorder = Physics2D.Raycast(rightCheckers[1].position, Vector2.right, rayLength, ground);
        rightBottomBorder = Physics2D.Raycast(rightCheckers[2].position, Vector2.right, rayLength, ground);

        if (leftTopBorder.collider != null || leftMiddleBorder.collider != null || leftBottomBorder.collider != null || rightTopBorder.collider != null || rightMiddleBorder.collider != null || rightBottomBorder.collider != null)
        {
            contact = true;
        }
        return contact;
    }


    private void OnDrawGizmos()
    {
        DrawRaycastHitGizmo(leftBorder, Color.red);
        DrawRaycastHitGizmo(middle, Color.green);
        DrawRaycastHitGizmo(rightBorder, Color.blue);
        DrawRaycastHitGizmo(leftTopBorder, Color.yellow);
        DrawRaycastHitGizmo(leftMiddleBorder, Color.cyan);
        DrawRaycastHitGizmo(leftBottomBorder, Color.magenta);
        DrawRaycastHitGizmo(rightTopBorder, Color.white);
        DrawRaycastHitGizmo(rightMiddleBorder, Color.gray);
        DrawRaycastHitGizmo(rightBottomBorder, Color.black);
    }

    private void DrawRaycastHitGizmo(RaycastHit2D hit, Color color)
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = transform.right;

        if (hit.collider != null)
        {
            Debug.DrawRay(rayStart, rayDirection * rayLength, color);
            Gizmos.color = color;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Debug.DrawRay(rayStart, rayDirection * rayLength, color);
        }
    }

    public void DeactivateCollider(Collider2D collider, bool reactivable)
    {
        collider.enabled = false;
        if (reactivable) StartCoroutine(ReactivateCollider(collider,0.5f));
    }

    private IEnumerator ReactivateCollider(Collider2D collider, float timeToReactivate)
    {
        yield return new WaitForSeconds(timeToReactivate);
        collider.enabled = true;
    }
}


