using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private SceneNames _activeLevelScene;
    private SceneryManager _sceneryManager;
    
    void Awake()
    {
        _activeLevelScene = SceneNames.LevelOne;
        _sceneryManager ??= FindObjectOfType<SceneryManager>();
        _sceneryManager.AddScene(_activeLevelScene);
    }

    public void LevelPassed(SceneNames nextLevel)
    {
        _sceneryManager.UnloadScene(_activeLevelScene);
        _sceneryManager.AddScene(nextLevel);
        _activeLevelScene = nextLevel;
    }
}
