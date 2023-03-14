using UnityEngine;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody m_RigidBody;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Damping = 0.98f;
    [SerializeField] private float m_DashVelocity;

    private float3
        m_MovementDirection,
        m_ForwardDirection;

    // for PBD physics
    private float3
        m_Position,
        m_PrevPosition,
        m_Velocity;

    private void Start()
    {
        this.m_MovementDirection = 0.0f;
        this.m_Position = this.transform.position;
        this.m_PrevPosition = this.m_Position;
        this.m_Velocity = 0.0f;
    }

    private void Update()
    {
        this.m_ForwardDirection = this.transform.forward;
        // cannot perform any calculations anyway
        if (Time.deltaTime < math.EPSILON) return;

        // assign direction based on key input
        // TODO: change this for new input system in the future
        this.m_MovementDirection = 0.0f;
        this.m_MovementDirection.z += Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        this.m_MovementDirection.z += Input.GetKey(KeyCode.S) ? -1.0f : 0.0f;
        this.m_MovementDirection.x += Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
        this.m_MovementDirection.x += Input.GetKey(KeyCode.A) ? -1.0f : 0.0f;
        // normalize so that it remains a unit vector
        if (math.all(this.m_MovementDirection != 0.0f))
        {
            this.m_MovementDirection = math.normalize(this.m_MovementDirection);
        }

        // apply movement
        this.m_Position += this.m_MovementDirection * this.m_Speed * Time.deltaTime;
        // apply dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.m_Velocity += this.m_ForwardDirection * this.m_DashVelocity;
        }

        // position based dynamics style update
        this.m_Position += this.m_Velocity * Time.deltaTime;

        this.m_Velocity = (this.m_Position - this.m_PrevPosition) / Time.deltaTime;
        this.m_PrevPosition = this.m_Position;
        this.m_Velocity *= this.m_Damping;

        this.transform.position = this.m_Position;
    }
}
