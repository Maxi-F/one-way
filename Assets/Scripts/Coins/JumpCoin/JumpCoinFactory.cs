using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts;
using UnityEngine;

public class JumpCoinFactory
{
    private JumpCoinConfig _creationConfig;

    public JumpCoinFactory(JumpCoinConfig config)
    {
        _creationConfig = config;
    }

    /// <summary>
    /// Creates a Jump coin using the jump coin config provided
    /// </summary>
    /// <param name="parent">parent object</param>
    /// <returns></returns>
    public GameObject CreateJumpCoin(GameObject parent)
    {
        GameObject noteObject = _creationConfig.noteObjects[Random.Range(0, _creationConfig.noteObjects.Count)];
        Material material = _creationConfig.materialsList[Random.Range(0, _creationConfig.materialsList.Count)];

        GameObject instantiatedNote = GameObject.Instantiate(noteObject, parent.transform);

        SkinnedMeshRenderer renderer = instantiatedNote.GetComponentInChildren<SkinnedMeshRenderer>();

        instantiatedNote.AddComponent<HoveringCoin>();
        instantiatedNote.GetComponent<HoveringCoin>().SetHoverSettings(_creationConfig.hoverVelocity, _creationConfig.hoverDistance);

        renderer.material = material;
        renderer.rootBone = parent.transform;
        
        return instantiatedNote;
    }
}
