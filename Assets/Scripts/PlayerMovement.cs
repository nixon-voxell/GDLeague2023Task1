using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Voxell.Util;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_Damping = 0.98f;
    [SerializeField] private float m_DashVelocity;
    [SerializeField] private AnimationCurve m_DashVelocityCurve;

    private float3
        m_MovementDirection,
        m_ForwardDirection;
    private quaternion m_TargetRotation;

    // for PBD physics
    [SerializeField, InspectOnly] private float3 m_Velocity;

    private void Start()
    {
        this.m_MovementDirection = 0.0f;
        this.m_Velocity = 0.0f;
    }

    public void SetTransform(Transform trans)
    {
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

        // apply rotation
        this.transform.rotation = Quaternion.RotateTowards(
            this.transform.rotation, this.m_TargetRotation,
            this.m_RotationSpeed * Time.deltaTime
        );

        this.m_Velocity *= this.m_Damping;

        this.m_Controller.Move(this.m_MovementDirection * this.m_Speed * Time.deltaTime);
        this.m_Controller.Move(this.m_Velocity * Time.deltaTime);
    }
}
