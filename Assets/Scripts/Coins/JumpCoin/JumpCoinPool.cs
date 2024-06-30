using System.Collections;
using System.Collections.Generic;
using Coins.JumpCoin;
using Manager;
using ScriptableObjects.Scripts;
using UnityEngine;

public class JumpCoinPool : MonoBehaviour
{
    [SerializeField] private int amountToPool = 20;
    [SerializeField] private JumpCoinConfig jumpCoinConfig;
    
    [Header("Events")]
    [SerializeField] private string levelPassed = "levelPassed";
    
    public static JumpCoinPool Instance;
    
    private List<GameObject> _pooledJumpCoins;
    private JumpCoinFactory _jumpCoinFactory;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        _jumpCoinFactory = new JumpCoinFactory(jumpCoinConfig);
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        _pooledJumpCoins = new List<GameObject>();
        for(int i = 0; i < amountToPool; i++)
        {
            CreateNote();
        }
        
        EventManager.Instance.SubscribeTo(levelPassed, ClearPooledNotes);
    }
    
    public GameObject GetPooledNote()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!_pooledJumpCoins[i].activeInHierarchy)
            {
                return _pooledJumpCoins[i];
            }
        }
        
        return CreateNote();
    }

    public void ClearPooledNotes(Dictionary<string, object> message)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            _pooledJumpCoins[i].SetActive(false);
        }
    }
    
    private GameObject CreateNote()
    {
        GameObject note = _jumpCoinFactory.CreateJumpCoin();
            
        note.SetActive(false);
            
        _pooledJumpCoins.Add(note);

        return note;
    }

    public void ReturnToPool(GameObject instantiatedNote)
    {
        instantiatedNote.transform.SetParent(gameObject.transform);
        instantiatedNote.SetActive(false);
    }
}
