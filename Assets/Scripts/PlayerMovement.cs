using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using Voxell.Util;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_Damping = 0.98f;
    [SerializeField] private float m_DashDuration = 1.0f;
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

        this.m_Controller.enabled = false;
        this.transform.SetPositionAndRotation(trans.position, trans.rotation);
        this.m_Controller.enabled = true;
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

    private void Update()
    {
        this.m_ForwardDirection = this.transform.forward;
        // cannot perform any calculations anyway
        if (Time.deltaTime < math.EPSILON) return;

        // normalize so that it remains a unit vector
        // if (math.all(this.m_MovementDirection != 0.0f))
        // {
        //     this.m_MovementDirection = math.normalize(this.m_MovementDirection);
        // }

        // apply rotation
        this.transform.rotation = Quaternion.RotateTowards(
            this.transform.rotation, this.m_TargetRotation,
            this.m_RotationSpeed * Time.deltaTime
        );

        this.m_Velocity *= this.m_Damping;

        this.m_Controller.Move(this.m_MovementDirection * this.m_Speed * Time.deltaTime);
        this.m_Controller.Move(this.m_Velocity * Time.deltaTime);
    }

    public IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + this.m_DashDuration)
        {
            float timePassed = Time.time - startTime;
            // multiply by 2 to get to the ping ponged value
            float percentage = timePassed / this.m_DashDuration * 2.0f;
            // Debug.Log(this.m_DashVelocityCurve.Evaluate(percentage));
            float magnitude = this.m_DashVelocityCurve.Evaluate(percentage) * this.m_DashVelocity;
            this.m_Velocity = this.m_ForwardDirection * magnitude;

            yield return null;
        }
    }
}
