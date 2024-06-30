using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private string _activeLevelSceneName;
    private SceneryManager _sceneryManager;
    
    [SerializeField] private SensibilitySettings _playerSettings;
    [SerializeField] private string initLevelSceneName = "Level1";
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string gameplayUIMenuName = "gameplayUI";

    
    [Header("Events")]
    [SerializeField] private string menuActivatedEvent = "menuActivated";
    [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
    void Awake()
    {
        _activeLevelSceneName = initLevelSceneName;
        _sceneryManager ??= FindObjectOfType<SceneryManager>();
        _sceneryManager.LoadScene(_activeLevelSceneName);
        _sceneryManager.SubscribeEventToAddScene(menuSceneName, UnloadGameplay);
        
        EventManager.Instance.TriggerEvent(menuActivatedEvent, new Dictionary<string, object>() { { "name", gameplayUIMenuName } });
    }

    private void OnDisable()
    {
        EventManager.Instance.TriggerEvent(menuDeactivatedEvent, new Dictionary<string, object>() { { "name", gameplayUIMenuName } });
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        // TODO win scene
        _sceneryManager.LoadScene(menuSceneName);
    }

    private void UnloadGameplay()
    {
        _sceneryManager.UnloadScene(_activeLevelSceneName);
        _sceneryManager.UnloadScene(gameplaySceneName);
        _sceneryManager.UnsubscribeEventToAddScene(menuSceneName, UnloadGameplay);
    }

    public void BackToMenu()
    {
        _sceneryManager.LoadScene(menuSceneName);
    }

    public void SetSensibility(float newSensibility)
    {
        _playerSettings.sensibility = newSensibility;
        PlayerPrefs.SetFloat("Sensibility", _playerSettings.sensibility);
    }

    public void SetMusicVolume(float newMusicVolume)
    {
        PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume");
    }
    
    public void SetSoundVolume(float newMusicVolume)
    {
        PlayerPrefs.SetFloat("SoundVolume", newMusicVolume);
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat("SoundVolume");
    }

    public float GetSensibility()
    {
        return _playerSettings.sensibility;
    }

    public void HandleLose()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
     
        // TODO lose scene
        _sceneryManager.LoadScene(menuSceneName);
    }
}
