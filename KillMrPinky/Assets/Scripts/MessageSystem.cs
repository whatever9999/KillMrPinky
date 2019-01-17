using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystem : MonoBehaviour{

    public Canvas messageCanvas;
    public Text text;

    private void Awake(){
        DontDestroyOnLoad(messageCanvas);
    }

    private void Start(){
        messageCanvas.enabled = false;
    }

    public void showMessage(string message) {
        text.text = message;
        messageCanvas.enabled = true;
    }

    public void closeMessage() {
        messageCanvas.enabled = false;
    }
}
