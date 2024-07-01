using System;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Credits
{
    public class CreditsLayoutHandler : MonoBehaviour
    {
        [SerializeField] private CreditsDataConfig creditsConfig;
        [SerializeField] private GameObject creditTitle;
        [SerializeField] private GameObject creditText;

        [Header("Credits options")] 
        [SerializeField] private float spacing = 80;

        [SerializeField] private float creditsVelocity = 100;

        private float _initialYPosition;
        
        void Start()
        {
            _initialYPosition = gameObject.transform.position.y;
            
            foreach (var creditsConfigCredit in creditsConfig.credits)
            {
                GameObject credit = Instantiate(new GameObject(), gameObject.transform);
                credit.AddComponent<VerticalLayoutGroup>();
                credit.GetComponent<VerticalLayoutGroup>().spacing = spacing;

                GameObject creditTitleObj = Instantiate(creditTitle, credit.transform);
                creditTitleObj.GetComponent<TextMeshProUGUI>().text = creditsConfigCredit.title;
                
                foreach (var member in creditsConfigCredit.members)
                {
                    GameObject creditTextObj = Instantiate(creditText, credit.transform);
                    creditTextObj.GetComponent<TextMeshProUGUI>().text = member;
                }
            }
        }

        private void OnDisable()
        {
            var vector3 = gameObject.transform.position;
            vector3.y = _initialYPosition;
            gameObject.transform.position = vector3;
        }

        private void Update()
        {
            var vector3 = gameObject.transform.position;
           
            vector3.y += creditsVelocity * Time.deltaTime;

            gameObject.transform.position = vector3;
        }
    }
}
