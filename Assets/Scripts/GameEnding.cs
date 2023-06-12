using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
   //Ui淡入淡出值
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    //指定玩家触发Ui
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    //如果抓住玩家，播放UI
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    //添加音频源引用
     public AudioSource exitAudio;
     public AudioSource caughtAudio;

  //给Canvas Group 创建一个布尔变量
    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    //计时器
    float m_Timer;
    //创建一个变量仅播一次
    bool m_HasAudioPlayed;

  //检测玩家控制的游戏对象
    void OnTriggerEnter (Collider other)
    //Player触及碰撞器出发游戏结束
    {
        if (other.gameObject == player)
        {
           //给Canvas Group 创建一个布尔变量
            m_IsPlayerAtExit = true;
        }
    }
        public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    void Update ()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    //1.如果玩家在关卡中逃脱，游戏仍然应该退出 2.Ui变化
     void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
            
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene (0);
            }
            else
            {
                Application.Quit ();
            }
        }
    }
}