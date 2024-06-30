using ScriptableObjects.Scripts;
using UnityEngine;

namespace Coins.JumpCoin
{
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
        public GameObject CreateJumpCoin()
        {
            GameObject noteObject = _creationConfig.noteObjects[Random.Range(0, _creationConfig.noteObjects.Count)];
            Material material = _creationConfig.materialsList[Random.Range(0, _creationConfig.materialsList.Count)];

            GameObject instantiatedNote = GameObject.Instantiate(noteObject);
            instantiatedNote.transform.localScale = _creationConfig.noteScale;
            
            SkinnedMeshRenderer renderer = instantiatedNote.GetComponentInChildren<SkinnedMeshRenderer>();

            instantiatedNote.AddComponent<HoveringCoin>();
            instantiatedNote.AddComponent<CoinLookAt>();

            renderer.material = material;
        
            return instantiatedNote;
        }

        public void Activate(GameObject note, GameObject parent, Transform playerTransform)
        {
            SkinnedMeshRenderer renderer = note.GetComponentInChildren<SkinnedMeshRenderer>();

            note.transform.position = parent.transform.position;
            
            note.GetComponent<CoinLookAt>().SetTransform(playerTransform);
            note.GetComponent<HoveringCoin>().SetHoverSettings(_creationConfig.hoverVelocity, _creationConfig.hoverDistance);
            
            renderer.rootBone = parent.transform;
            
            note.SetActive(true);
        }
    }
}
