using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    float idleTime = 0.4f;
    float timer = 0.0f;
    public float moveSpeed = 0.5f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    SerialPort stream = new SerialPort("COM3", 9600);  // 更改为正确的串口和波特率

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        stream.Open();  // 打开串口连接
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 从Arduino读取声音传感器的数值
        int soundValue = int.Parse(stream.ReadLine());

        // 将声音传感器的数值映射到移动速度
        float mappedSpeed = map(soundValue, 0, 1023, 0f, moveSpeed);

        // 根据Arduino发送的数据和声音传感器的数值来控制移动
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        m_Movement = m_Movement * mappedSpeed * Time.deltaTime;

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        if (!isWalking)
        {
            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                m_Animator.SetBool("IsWalking", false);
                timer = 0f;
            }
        }
        else
        {
            m_Animator.SetBool("IsWalking", true);
            timer = 0f;
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
        m_Movement = transform.TransformDirection(m_Movement);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    float map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
    }

    void OnDisable()
    {
        stream.Close();  // 关闭串口连接
    }
}