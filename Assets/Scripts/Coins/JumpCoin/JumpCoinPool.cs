using System;
using System.Collections.Generic;
using Manager;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coins.JumpCoin
{
    public class JumpCoinPool : MonoBehaviour
    {
        [SerializeField] private int amountToPool = 20;
        [SerializeField] private NoteConfig noteConfig;
        
        private static JumpCoinPool _instance;
        private int _amountOfNotes;
        public static JumpCoinPool Instance
        {
            // checks with null because if object is destroyed it returns true but object is not null.
            get { return _instance == null ? null : _instance; }
            private set { _instance = value; }
        }
    
        private List<GameObject> _pooledJumpCoins;
        private JumpCoinFactory _jumpCoinFactory;
    
        void Awake()
        {
            _amountOfNotes = amountToPool;
            
            if(Instance == null)
            {
                Instance = this;
            } else
            {
                Destroy(gameObject);
            }

            _jumpCoinFactory = new JumpCoinFactory(noteConfig);
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            _pooledJumpCoins = new List<GameObject>();
            for(int i = 0; i < _amountOfNotes; i++)
            {
                CreateNote();
            }
        }

        /// <summary>
        /// Obtains a pooled note from the notes pool.
        /// </summary>
        /// <returns>A Note gameobject.</returns>
        public GameObject GetPooledNote()
        {
            for(int i = 0; i < _pooledJumpCoins.Count; i++)
            {
                if(!_pooledJumpCoins[i].activeInHierarchy)
                {
                    return _pooledJumpCoins[i];
                }
            }
        
            return CreateNote();
        }
    
        /// <summary>
        /// Creates a note from the jump coin factory.
        /// </summary>
        /// <returns>A Note Gameobject.</returns>
        private GameObject CreateNote()
        {
            GameObject note = _jumpCoinFactory.CreateJumpCoin();
            note.transform.SetParent(gameObject.transform);
            
            note.SetActive(false);
            
            _pooledJumpCoins.Add(note);

            return note;
        }

        /// <summary>
        /// Returns a note to the pool, setting it as non active.
        /// </summary>
        /// <param name="instantiatedNote">Note to return to the pool.</param>
        public void ReturnToPool(GameObject instantiatedNote)
        {
            instantiatedNote.SetActive(false);
        }
    }
}
