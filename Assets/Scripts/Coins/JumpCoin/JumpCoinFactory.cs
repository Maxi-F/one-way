using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableObjects.Scripts;
using UnityEngine;

public class JumpCoinFactory
{
    private JumpCoinConfig _creationConfig;
    private Player _player;

    public JumpCoinFactory(JumpCoinConfig config, Player player)
    {
        _creationConfig = config;
        _player = player;
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
        instantiatedNote.AddComponent<CoinLookAt>();
        instantiatedNote.GetComponent<CoinLookAt>().SetTransform(_player.transform);

        renderer.material = material;
        renderer.rootBone = parent.transform;
        
        return instantiatedNote;
    }
}
