using System.Collections.Generic;
using Coins.JumpCoin;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    public class ObjectPool<TConfig, TFactory> : MonoBehaviour where TFactory : IFactory<TConfig>, new()
    {
        [SerializeField] private int amountToPool = 20;
        [SerializeField] private TConfig objectConfig;
        
        private static ObjectPool<TConfig, TFactory> _instance;
        private int _amountOfNotes;
        public static ObjectPool<TConfig, TFactory> Instance
        {
            // checks with null because if object is destroyed it returns true but object is not null.
            get { return _instance == null ? null : _instance; }
            private set { _instance = value; }
        }
    
        private List<GameObject> _pooledObjects;
        private TFactory _objectFactory;
    
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

            _objectFactory = new TFactory();
            _objectFactory.SetConfig(objectConfig);
            
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            _pooledObjects = new List<GameObject>();
            for(int i = 0; i < _amountOfNotes; i++)
            {
                CreateObject();
            }
        }

        /// <summary>
        /// Obtains a pooled object from the objects pool.
        /// </summary>
        /// <returns>A Note gameobject.</returns>
        public GameObject GetPooledObject()
        {
            for(int i = 0; i < _pooledObjects.Count; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
        
            return CreateObject();
        }
    
        /// <summary>
        /// Creates an object from the setted factory.
        /// </summary>
        /// <returns>A Gameobject.</returns>
        private GameObject CreateObject()
        {
            GameObject gameObjectToPool = _objectFactory.CreateObject();
            gameObjectToPool.transform.SetParent(gameObject.transform);
            
            gameObjectToPool.SetActive(false);
            
            _pooledObjects.Add(gameObjectToPool);

            return gameObjectToPool;
        }

        /// <summary>
        /// Returns an object to the pool, setting it as non active.
        /// </summary>
        /// <param name="instantiatedObject">object to return to the pool.</param>
        public void ReturnToPool(GameObject instantiatedObject)
        {
            instantiatedObject.SetActive(false);
        }
    }
}
