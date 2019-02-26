using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Singleton<Wind> {

    [SerializeField] GameObject m_compassArrow;
    [SerializeField] float m_windSpeed;
    [SerializeField] float m_windTimer;
    Vector3 windVector = Vector3.zero;
    float currentTimer = 0.0f;
    bool isCalm = false;

    private void Start()
    {
        currentTimer = m_windTimer;
    }

    void Update () {

        currentTimer -= Time.deltaTime;
                
        if(currentTimer <= 0.0f)
        {
            if (isCalm)
            {
                SetWindVector();
                currentTimer = m_windTimer;
            }
            else
            {
                windVector = Vector3.zero;
                currentTimer = m_windTimer / 2;
            }

            isCalm = !isCalm;
        }
	}

    public Vector3 GetWindVector()
    {
        return windVector;
    }

    void SetWindVector()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Random.Range(-1.0f, 1.0f);
        direction.z = Random.Range(-1.0f, 1.0f);
        direction.Normalize();
        windVector = direction * m_windSpeed;
        SpinCompassArrow();
    }

    void SpinCompassArrow()
    {
        m_compassArrow.transform.rotation = Quaternion.identity;
        float angle = Vector3.Angle(m_compassArrow.transform.position, windVector);
        if(windVector.z > 0)
        {
            m_compassArrow.transform.rotation = m_compassArrow.transform.rotation * Quaternion.Euler(0.0f, 0.0f, angle);
        }
        else
        {
            m_compassArrow.transform.rotation = m_compassArrow.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -angle);
        }
        
        //float angle = Quaternion.Angle(m_compassArrow.transform.rotation, windVector);
    }
}
