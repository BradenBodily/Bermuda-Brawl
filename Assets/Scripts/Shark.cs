using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour {
    
    [SerializeField] World m_world = null;
    [SerializeField] [Range(0.0f, 20.0f)] float m_speed = 10.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_response = 10.0f;
    [SerializeField] [Range(0.0f, 5.0f)] public float m_power = 1.0f;
    [SerializeField] [Range(0.0f, 20.0f)] public float m_radius = 1.0f;

    Vector3 m_velocity = Vector3.zero;
    
    void Update()
    {
        Vector3 newVelocity = Vector3.zero;
        newVelocity = newVelocity + Wander();
        newVelocity = newVelocity.normalized * m_speed;
        newVelocity.y = 0;
        m_velocity = Vector3.Lerp(m_velocity, newVelocity, m_response * Time.deltaTime);

        Vector3 position = transform.position + m_velocity * Time.deltaTime;
        position = m_world.WrapPosition(position);

        transform.position = position;
        if (m_velocity.magnitude != 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(m_velocity);
        }
    }

    Vector3 Wander()
    {
        Vector3 randomPoint = Vector3.zero;
        Vector2 v2 = Random.insideUnitCircle.normalized * m_radius;
        randomPoint.x = v2.x;
        randomPoint.z = v2.y;

        Vector3 forward = transform.forward * 2.0f;
        Vector3 direction = forward + randomPoint;
        Vector3 velocity = direction.normalized * m_power;

        Debug.DrawLine(transform.position, transform.position + direction, Color.white);

        return velocity;
    }
}
