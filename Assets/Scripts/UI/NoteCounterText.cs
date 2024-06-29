using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;

public class NoteCounterText : MonoBehaviour
{
    [SerializeField] private string noteModifiedEvent = "noteModified";
    [SerializeField] private string initNotesEvent = "initNotes";

    private TextMeshProUGUI _textMesh;

    private int notes = 0;
    void OnEnable()
    {
        _textMesh ??= GetComponent<TextMeshProUGUI>();
        
        EventManager.Instance.SubscribeTo(noteModifiedEvent, ModifyNotes);
        EventManager.Instance.SubscribeTo(initNotesEvent, InitNotes);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(noteModifiedEvent, ModifyNotes);
        EventManager.Instance.UnsubscribeTo(initNotesEvent, InitNotes);
    }

    void InitNotes(Dictionary<string, object> message)
    {
        notes = (int)message["value"];
        
        _textMesh.text = notes.ToString();
    }

    
    void ModifyNotes(Dictionary<string, object> message)
    {
        notes += (int)message["value"];

        _textMesh.text = notes.ToString();
    }
}
