using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayManager : MonoBehaviour
{
    private string _activeLevelSceneName;
    private SceneryManager _sceneryManager;
    
    [SerializeField] private SensibilitySettings _playerSettings;
    [SerializeField] private SceneChangeData initLevelScene;
    [SerializeField] private SceneChangeData menuScene;
    [SerializeField] private SceneChangeData gameplayScene;
    
    void Awake()
    {
        _activeLevelSceneName = initLevelScene.sceneName;
        _sceneryManager ??= FindObjectOfType<SceneryManager>();
        _sceneryManager.LoadScene(_activeLevelSceneName);
        _sceneryManager.SubscribeEventToAddScene(menuScene.sceneName, UnloadGameplay);
    }

    public void LevelPassed(string nextLevel)
    {
        if (_activeLevelSceneName == nextLevel) return;
        _sceneryManager.UnloadScene(_activeLevelSceneName);
        _sceneryManager.LoadScene(nextLevel);
        _activeLevelSceneName = nextLevel;
    }

    public void HandleWin()
    {
        _sceneryManager.LoadScene(menuScene.sceneName);
    }

    private void UnloadGameplay()
    {//
        _sceneryManager.UnloadScene(_activeLevelSceneName);
        _sceneryManager.UnloadScene(gameplayScene.sceneName);
        _sceneryManager.UnsubscribeEventToAddScene(menuScene.sceneName, UnloadGameplay);
    }

    public void BackToMenu()
    {
        _sceneryManager.LoadScene(menuScene.sceneName);
    }

    public void SetSensibility(float newSensibility)
    {
        _playerSettings.sensibility = newSensibility;
        PlayerPrefs.SetFloat("Sensibility", _playerSettings.sensibility);
    }

    public float GetSensibility()
    {
        return _playerSettings.sensibility;
    }
}
