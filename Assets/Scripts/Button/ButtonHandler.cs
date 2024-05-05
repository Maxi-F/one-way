using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private SceneryManager _sceneryManager;
    [SerializeField] private SceneNames _scene;

    private void Start()
    {
        _sceneryManager = FindObjectOfType<SceneryManager>();
    }
    
    public void OnButtonClick()
    {
        if (_sceneryManager == null)
        {
            Debug.LogError($"{nameof(_sceneryManager)} is null!");
            return;
        }
        
        _sceneryManager.LoadScene(_scene);
    }
}
