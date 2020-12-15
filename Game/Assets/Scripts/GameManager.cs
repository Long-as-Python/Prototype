﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { private set; get; }

    public Movement _player { private set; get; }
    CameraManager _camera;
    UIController _ui;
    //AudioConroller
    OwnedItems ownedItems;
    IslesManager _islesManager;
    BlimpProperties _blimpProps;

    void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);

        _instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log(_instance);

        _player = TryToFindPlayer();

    }

    private void Start()
    {

    }


    void Update()
    {

    }


    private Movement TryToFindPlayer()
    {
        try
        {
            return FindObjectOfType<Movement>();
        }
        catch
        {
            Debug.LogError("Player not found! Please, add Player to scene for correct gameplay!");
            return null;
        }
    }

    public void StartGame()
    {

    }

    public void StopGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void LoadData()
    {

    }

    public void SaveData()
    {

    }
}
