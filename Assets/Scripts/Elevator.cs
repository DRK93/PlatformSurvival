using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IPausable
{
    private float m_TravelDistance = 0;
    private float m_MaxTravelDistance = 15;
    private float m_Speed = 5.0f;
    private Coroutine m_ReverseCorutine;
    private Rigidbody m_Rb;


    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        enabled = false;
    }

    void FixedUpdate()
    {
        if(m_TravelDistance >= m_MaxTravelDistance)
        {
            if (m_ReverseCorutine == null)
                m_ReverseCorutine = StartCoroutine(nameof(ElevatorStop));
        }
        else
        {
            float distanceStep = m_Speed * Time.fixedDeltaTime;
            m_TravelDistance += Mathf.Abs(distanceStep);

            Vector3 elevatorPos = m_Rb.position;
            elevatorPos.y += distanceStep;
            m_Rb.MovePosition(elevatorPos);
        }
    }

    public void OnGameStart()
    {
        StartCoroutine(StartElevator());
    }
    private IEnumerator StartElevator()
    {
        yield return new WaitForSeconds(3.0f);
        enabled = true;
    }
    private IEnumerator ElevatorStop()
    {
        yield return new WaitForSeconds(3.0f);
        m_TravelDistance = 0;
        m_Speed = -m_Speed;
        m_ReverseCorutine = null;
    }


}
