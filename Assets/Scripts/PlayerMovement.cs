using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Voxell.Util;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SphereCollider m_SphereCollider;
    [SerializeField] private float m_CollisionRadius = 0.5f;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_Damping = 0.98f;
    [SerializeField] private float m_DashVelocity;

    private float3
        m_MovementDirection,
        m_ForwardDirection;
    private quaternion m_TargetRotation;

    // for PBD physics
    [SerializeField, InspectOnly] private float3
        m_Position,
        m_PrevPosition,
        m_Velocity;

    private Dictionary<int, float3> m_CollisionPoints;

    private void Start()
    {
        this.m_MovementDirection = 0.0f;
        this.m_Position = this.transform.position;
        this.m_PrevPosition = this.m_Position;
        this.m_Velocity = 0.0f;

        // this.m_ContactPoints = new List<float3>();
        this.m_CollisionPoints = new Dictionary<int, float3>();
    }

    public void SetTransform(Transform trans)
    {
        this.m_Position = trans.position;
        this.m_PrevPosition = this.m_Position;
        this.m_TargetRotation = trans.rotation;

        this.transform.SetPositionAndRotation(trans.position, trans.rotation);
    }

    public void SetMoveDirection(float2 direction)
    {
        this.m_MovementDirection.x = direction.x;
        this.m_MovementDirection.z = direction.y;

        if (math.length(this.m_MovementDirection) > math.EPSILON)
        {
            this.m_TargetRotation = quaternion.LookRotation(
                math.normalize(this.m_MovementDirection),
                new float3(0.0f, 1.0f, 0.0f)
            );
        } else
        {
            this.m_TargetRotation = this.transform.rotation;
        }
    }

    public void Dash()
    {
        this.m_Velocity += this.m_ForwardDirection * this.m_DashVelocity;
    }

    private void Update()
    {
        this.m_ForwardDirection = this.transform.forward;
        // cannot perform any calculations anyway
        if (Time.deltaTime < math.EPSILON) return;

        // normalize so that it remains a unit vector
        if (math.all(this.m_MovementDirection != 0.0f))
        {
            this.m_MovementDirection = math.normalize(this.m_MovementDirection);
        }

        // apply movement
        this.m_Position += this.m_MovementDirection * this.m_Speed * Time.deltaTime;
        // apply rotation
        this.transform.rotation = Quaternion.RotateTowards(
            this.transform.rotation, this.m_TargetRotation,
            this.m_RotationSpeed * Time.deltaTime
        );

        // apply velocity
        this.m_Position += this.m_Velocity * Time.deltaTime;

        // solve all contact points
        foreach (float3 point in this.m_CollisionPoints.Values)
        {
            float3 n = this.m_Position - point;
            float d = math.length(n);
            float c = 0.5f - d;

            if (d == 0.0f || c < 0.0f) continue;
            // normalize n to a unit vector
            n /= d;
            this.m_Position += n * c;
        }

        // position based dynamics style update
        this.m_Velocity = (this.m_Position - this.m_PrevPosition) / Time.deltaTime;
        this.m_Velocity *= this.m_Damping;
        this.m_PrevPosition = this.m_Position;

        this.transform.position = this.m_Position;
    }

    private void OnCollisionStay(Collision collision)
    {
        try
        {
            this.m_CollisionPoints[collision.collider.GetInstanceID()] = collision.contacts[0].point;
        } catch {}
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.m_CollisionPoints.Add(collision.collider.GetInstanceID(), collision.contacts[0].point);
    }

    private void OnCollisionExit(Collision collision)
    {
        this.m_CollisionPoints.Remove(collision.collider.GetInstanceID());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, this.m_CollisionRadius);
    }
}
