using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonChanges : Selectable
{
    GameObject Normal;
    GameObject Highlighted;
    GameObject Pressed;
    BaseEventData m_BaseEvent;

    void Update() {
        if(IsHighlighted(m_BaseEvent))
        {
            Normal.SetActive(false);
            Highlighted.SetActive(true);
            Pressed.SetActive(false);
        } else if(IsPressed(m_BaseEvent))
        {
            Normal.SetActive(false);
            Highlighted.SetActive(false);
            Pressed.SetActive(true);
        } else
        {
            Normal.SetActive(true);
            Highlighted.SetActive(false);
            Pressed.SetActive(false);
        }
    }
}
