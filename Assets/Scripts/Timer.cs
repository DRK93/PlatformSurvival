using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerCount;
    [field: SerializeField] private float timer;
    [field: SerializeField] private float tickTime;
    [field: SerializeField] private float oneSecTextTick;
    private float m_TickTimeToSpawn;
    public Action countdown;
    private void Start()
    {
        timer = 0f;
        m_TickTimeToSpawn = tickTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        oneSecTextTick += Time.deltaTime;
        if (oneSecTextTick > 1f)
        {
            oneSecTextTick = 0f;
            if(m_TickTimeToSpawn!= 0f)
                OneSecondTick();
        }

        if (timer > tickTime)
        {
            countdown?.Invoke();
            m_TickTimeToSpawn = tickTime;
            timer = 0f;

        }
            
    }

    private void OneSecondTick()
    {
        m_TickTimeToSpawn--;
        timerCount.text = "Magic cube in: " + m_TickTimeToSpawn;
    }
}
