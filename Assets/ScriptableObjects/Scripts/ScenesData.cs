using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;

#endif

[Serializable]
public class SerializableScene
{
    public string path;
    public string name;
    public int index;

    public Action OnSceneAdded;
    public Action OnSceneRemoved;
    
    public SerializableScene(string path, int index)
    {
        this.path = path;
        this.index = index;
    }
}

[CreateAssetMenu(menuName = "Create ScenesData", fileName = "ScenesData", order = 0)]
public class ScenesData : ScriptableObject
{
    public List<SerializableScene> scenes;
    
    /// <summary>
    /// Gets a Serializable Scene by scene name.
    /// </summary>
    public SerializableScene GetSceneByName(string aName)
    {
        return scenes.Find(scene => scene.name == aName);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        CheckScenes();
    }
    
    /// <summary>
    /// Remakes the List of serializable scenes, depending on build settings.
    /// </summary>
    private void CheckScenes()
    {
        foreach (var editorBuildSettingsScene in EditorBuildSettings.scenes)
        {
            if (editorBuildSettingsScene.enabled)
            {
                int index = SceneUtility.GetBuildIndexByScenePath(editorBuildSettingsScene.path);

                if (scenes[index] != null)
                {
                    scenes[index].index = index;
                    scenes[index].path = editorBuildSettingsScene.path;
                }
                else
                {
                    scenes.Add(new SerializableScene(editorBuildSettingsScene.path, index));
                }
            }
        }
    }
    
#endif
}
