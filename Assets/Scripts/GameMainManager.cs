using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameMainManager : MonoBehaviour
{
    public GameObject startScreen;
    public UnityEvent OnGameStart;
    private SpawnManager m_SpawnManager;

    private bool m_gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        ShowMenu();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowMenu();
        }
    }

    private void ShowMenu()
    {
        Time.timeScale = 0f;
        startScreen.SetActive(true);
    }

    private void HideMenu()
    {
        startScreen.SetActive(false);
    }

    public void ResumeGame()
    {
        if (m_gameStarted)
        {
            Time.timeScale = 1f;
            HideMenu();
        }
    }

    public void PlayGame()
    {
        if (!m_gameStarted)
        {
            Time.timeScale = 1f;
            StartGameRun();
        }
    }
    private void StartGameRun()
    {
        m_SpawnManager = FindObjectOfType<SpawnManager>();
        var elevators = FindObjectsOfType<Elevator>();
        for (int i=0; i < elevators.Length; i++ )
        {
            OnGameStart.AddListener(elevators[i].OnGameStart);
        }

        m_gameStarted = true;
        StartGame();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        OnGameStart.Invoke();
        m_SpawnManager.StartSpawning();
        startScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
