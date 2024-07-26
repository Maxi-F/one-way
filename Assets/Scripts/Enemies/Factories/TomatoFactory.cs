using ScriptableObjects.Scripts;
using UnityEngine;
using Utils;

namespace Enemies.Factories
{
    public class TomatoFactory : IFactory<TomatoConfig>
    {
        private TomatoConfig _config;
    
        public void SetConfig(TomatoConfig config)
        {
            _config = config;
        }

        public GameObject CreateObject()
        {
            GameObject tomatoObject = _config.tomatoObject;

            GameObject instantiatedObject = GameObject.Instantiate(tomatoObject);

            return instantiatedObject;
        }
    }
}
