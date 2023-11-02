using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IPausable
{
    public float speed;
    public Camera followCamera;
    public UnityEvent OnPlayerLost;
    private Rigidbody m_Rb;
    private GameObject m_Elevator;
    private float m_ElevatorOffsetY;
    private Vector3 m_CameraPos;
    private float m_SpeedModifier;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_ElevatorOffsetY = 0;
        m_SpeedModifier = 1;
        m_CameraPos = followCamera.transform.position - m_Rb.position;
        enabled = false;
    }

    void FixedUpdate()
    {
        if (transform.position.y <= -10.0f)
        {
            OnPlayerLost.Invoke();
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 playerPos = m_Rb.position;
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        //Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        Quaternion targetRotation = new Quaternion();

        if (movement!=Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(movement);
        }
        //---------------------------------------------------------
        // First method of player rotation

        /*Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, movement);
        
        if (m_Elevator !=null)
        {
            playerPos.y = m_Elevator.transform.position.y + m_ElevatorOffsetY;
        }
        // Checking if going backward
        if(Mathf.Approximately(Vector3.Dot(movement, Vector3.forward), -1.0f))
        {
            // LookRotation - rotation on Y axis so target will be rotate into direction
            targetRotation = Quaternion.LookRotation(-Vector3.forward);
        }
         */

        //------------------------------------
        //Second method of player rotation

        //Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);

        if (m_Elevator != null)
        {
            playerPos.y = m_Elevator.transform.position.y + m_ElevatorOffsetY;
        }

        targetRotation = Quaternion.RotateTowards(
            transform.rotation, 
            targetRotation, 
            360*Time.fixedDeltaTime);

        m_Rb.MovePosition(playerPos + movement * m_SpeedModifier * speed * Time.fixedDeltaTime );
        m_Rb.MoveRotation(targetRotation);
    }
    private void LateUpdate()
    {
        followCamera.transform.position = m_Rb.position + m_CameraPos;
    }

    public void OnGameStart ()
    {
        enabled = true;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            m_SpeedModifier = 2;
            StartCoroutine(nameof(BonusSpeedCountdown));
        }

        if (collision.gameObject.CompareTag("Enemy") && m_SpeedModifier > 1)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * 30, ForceMode.Impulse);
        }
    }

    private IEnumerator BonusSpeedCountdown()
    {
        yield return new WaitForSeconds(5.0f);
        m_SpeedModifier = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            m_Elevator = other.gameObject;
            m_ElevatorOffsetY = transform.position.y - m_Elevator.transform.position.y;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Elevator"))
        {
            m_Elevator = null;
            m_ElevatorOffsetY = 0;
        }
    }
}
