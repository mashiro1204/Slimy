﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject gameStartUI;
    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        SlimyEvents.dieEvent.AddListener(GameOver);
        SlimyEvents.gameStartEvent.AddListener(GameStart);
        SlimyEvents.levelStartEvent.AddListener(LevelStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    void GameStart()
    {
        gameStartUI.SetActive(false);
    }

    void LevelStart()
    {
        gameStartUI.SetActive(false);
        Debug.Log("fsdfdsfsdfsdfd");
    }
}
