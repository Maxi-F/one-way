using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NoteCounterText : MonoBehaviour
    {
        [SerializeField] private string noteModifiedEvent = "noteModified";
        [SerializeField] private string initNotesEvent = "initNotes";

        private TextMeshProUGUI _textMesh;

        private int _notes = 0;
        void OnEnable()
        {
            _textMesh ??= GetComponent<TextMeshProUGUI>();
        
            EventManager.Instance?.SubscribeTo(noteModifiedEvent, ModifyNotes);
            EventManager.Instance?.SubscribeTo(initNotesEvent, InitNotes);
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(noteModifiedEvent, ModifyNotes);
            EventManager.Instance?.UnsubscribeTo(initNotesEvent, InitNotes);
        }

        /// <summary>
        /// Inits notes counter.
        /// </summary>
        void InitNotes(Dictionary<string, object> message)
        {
            _notes = (int)message["value"];
        
            _textMesh.text = _notes.ToString();
        }
        
        /// <summary>
        /// Modifies notes counter.
        /// </summary>
        void ModifyNotes(Dictionary<string, object> message)
        {
            _notes += (int)message["value"];

            _textMesh.text = _notes.ToString();
        }
    }
}
