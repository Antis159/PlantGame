using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    [Serializable]
    public class KeyBinds
    {
        public KeyCode PlayerMoveForwards;
        public KeyCode PlayerMoveBackwards;
        public KeyCode PlayerMoveLeft;
        public KeyCode PlayerMoveRight;
    }
    public static GlobalData instance;
    [SerializeField] private KeyBinds _KeyBindsClass;
    public KeyBinds KeyBindsClass { get => _KeyBindsClass; }
    public PlayerController playerController {get; private set;}
    [SerializeField] SaveLoadManager saveLoadManager;
    [SerializeField] bool loadGameData;
    [SerializeField] bool SkipSaveOnQuit = false;
    public bool isLoaded = false;
    public bool isPaused = false;
    [Header("StartMenu / StartGame_Play")]
    public bool skipStartMenu = false;
    [SerializeField] GameObject startMenuControllerObj;
    [SerializeField] List<GameObject> manualEnable_OnGameStart_List;
    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        GetComponentsOnAwake();
        startMenuControllerObj.SetActive(true);
        GameStart_ManualEnable(false);
        StartWithDefaultKeyBinds();
    }
    void Start()
    {
        StartCoroutine(DelayStart());
    }
    IEnumerator DelayStart()
    {
        // skip 1 frame to make sure other objs run Start();
        yield return null;
        if (skipStartMenu == true)
            StartGame_Play();
    }
    private void GetComponentsOnAwake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public void StartGame_Play()
    {
        GameStart_ManualEnable(true);
        if (loadGameData == true)
            saveLoadManager.LoadGameData();
        isLoaded = true;
        startMenuControllerObj.SetActive(false);
        StartCoroutine(AutoSave_Coroutine());
    }
    private void GameStart_ManualEnable(bool value)
    {
        foreach (var item in manualEnable_OnGameStart_List)
        {
            if (item.activeSelf != value)
                item.SetActive(value);
        }
    }
    public bool IsRunning()
    {
        if (isPaused == true || isLoaded == false)
            return false;

        return true;
    }
    void OnApplicationQuit()
    {
        if (SkipSaveOnQuit == false)
            saveLoadManager.SaveGameData();
    }
    public void SetPause(bool value)
    {
        isPaused = value;
    }
    IEnumerator AutoSave_Coroutine()
    {
        yield return new WaitForSeconds(360f);
        saveLoadManager.SaveGameData();
        StartCoroutine(AutoSave_Coroutine());
    }
    private void StartWithDefaultKeyBinds()
    {
        _KeyBindsClass.PlayerMoveForwards = KeyCode.W;
        _KeyBindsClass.PlayerMoveBackwards = KeyCode.S;
        _KeyBindsClass.PlayerMoveLeft = KeyCode.A;
        _KeyBindsClass.PlayerMoveRight = KeyCode.D;
    }
}