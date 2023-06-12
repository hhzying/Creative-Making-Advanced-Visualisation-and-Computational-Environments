using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    //检测玩家的位置
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;
    Animator m_Animator; // 对Animator组件的引用

     void Start()
    {
        m_Animator = GetComponent<Animator>(); // 获取Animator组件的引用
    }

    //当触发器与其他游戏对象的碰撞器接触时，Unity会调用OnTriggerEnter和OnTriggerExit方法，并将接触的碰撞器传递给这些方法作为参数。
    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

//检查敌人的视线是否清晰
    void Update ()
    {
        //检查玩家是否在视线范围内
        if (m_IsPlayerInRange)
        {
            //敌人与玩家间的向量差与方向
            Vector3 direction = player.position - transform.position + Vector3.up;
            //创建射线
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    // 使用Animator组件设置bool参数
                    if (m_Animator != null)
                    {
                        m_Animator.SetBool("PlayerInRange", true);
                    }
                   gameEnding.CaughtPlayer ();
                }
            }
        }
    }
}