using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpCoin : MonoBehaviour
{
    [Header("Coin Data")]
    [SerializeField] private List<GameObject> noteObjects;
    [SerializeField] private List<Material> materialsList;

    [Header("Hover settings")] 
    [SerializeField] private float hoverVelocity = 5f;
    [SerializeField] private float hoverDistance = 0.2f;
    
    private Vector3 _center;
    private GameObject _instantiatedNote;
    private int _actualDirection = 1;
    
    
    public void Start()
    {
        GameObject noteObject = noteObjects[Random.Range(0, noteObjects.Count)];
        Material material = materialsList[Random.Range(0, materialsList.Count)];

        _instantiatedNote = Instantiate(noteObject, gameObject.transform);
        
        SkinnedMeshRenderer renderer = _instantiatedNote.GetComponentInChildren<SkinnedMeshRenderer>();
        
        renderer.material = material;
        renderer.rootBone = gameObject.transform;

        _center = _instantiatedNote.transform.position;
    }

    public void Update()
    {
        _actualDirection = (_instantiatedNote.transform.position - _center).magnitude > hoverDistance ? -_actualDirection : _actualDirection;

        _instantiatedNote.transform.position += new Vector3(0,  _actualDirection * hoverVelocity * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddJump();
            Destroy(gameObject);
        }
    }
}
