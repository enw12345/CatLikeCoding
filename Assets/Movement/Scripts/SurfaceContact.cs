using UnityEngine;

public class SurfaceContact : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    private Rigidbody rb;
    private bool desiredJump;
    private Vector3 velocity, desiredVelocity;
    private int jumpPhase;
    private Material mat;

    private float minGroundDotProduct;
    private int groundContactCount;
    bool OnGround => groundContactCount > 0;
    private Vector3 contactNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        OnValidate();
    }

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        desiredJump |= Input.GetButtonDown("Jump");
        mat.SetColor("_Color", Color.white * (groundContactCount * 0.25f));
    }

    private void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }
        rb.velocity = velocity;

        ClearState();
    }

    private void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    private void UpdateState()
    {
        velocity = rb.velocity;
        if (OnGround)
        {
            jumpPhase = 0;
            if (groundContactCount > 1)
                contactNormal.Normalize();
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    private void Jump()
    {
        if (OnGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);

            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }

            velocity += contactNormal * jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            //Jump away from the ground surface, the direction of the ground's normal vector
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    private Vector3 ProjectOnPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    private void AdjustVelocity()
    {
        //get normalized vectors aligned with the ground
        Vector3 xAxis = ProjectOnPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnPlane(Vector3.forward).normalized;

        //get the current velocity to get the relative X and Z speed
        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        //calculate new speeds relative to the ground
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        //adjust the velocity by adding the differences between the new and old speeds along the relative axes
        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
