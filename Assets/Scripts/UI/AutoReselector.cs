using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class AutoReselector : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
    
        public bool IsCanvasOpen { get; set; }

        private GameObject _lastSelectedObject;

        void Start()
        {
            IsCanvasOpen = false;
        }
    
        void Update()
        {
            if (IsCanvasOpen)
            {
                Debug.Log(eventSystem.currentSelectedGameObject);
                if (eventSystem.currentSelectedGameObject == null)
                    eventSystem.SetSelectedGameObject(_lastSelectedObject);
                else
                    _lastSelectedObject = eventSystem.currentSelectedGameObject;
            }
        }
    }
}
