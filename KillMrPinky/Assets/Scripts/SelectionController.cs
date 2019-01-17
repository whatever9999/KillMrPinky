using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour{

    private Vector2 clickLocation;
    private Vector2 itemGrabbedPosition;
    private Vector2 releaseLocation;
    private Vector2 itemDroppedPosition;
    private bool didMouseMove;
    private bool didClickItem;

    private Transform clickTarget;
    private Transform releaseTarget;

    private enum PropType {Item, Door, Character}

    // Start is called before the first frame update
    void Start(){
        didClickItem = false;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(0)) {
            clickTarget = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, -1f).transform;
            if (clickTarget != null) { 
            clickLocation = new Vector2((Input.mousePosition.x), (Input.mousePosition.y));
            itemGrabbedPosition = clickTarget.position;
            if (clickTarget.tag == "InventoryItem") didClickItem = true;
            }
        }
        else if (Input.GetMouseButton(0)) {
            if (didClickItem && CheckMouseMoved(clickLocation, Input.mousePosition)) {
                Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                camPos.z = clickTarget.position.z;
                clickTarget.position = camPos;
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            releaseLocation = new Vector2((Input.mousePosition.x),(Input.mousePosition.y));
            didMouseMove = CheckMouseMoved(clickLocation, releaseLocation);

            if (!didMouseMove)
            {
                switch (clickTarget.tag)
                {
                    case "InventoryItem":
                    case "WorldItem":
                        HandleItem(clickTarget.GetComponent<Item>());
                        break;
                    case "Door":
                        HandleDoor(clickTarget.GetComponent<Door>());
                        break;
                    case "Character":
                        HandleCharacter(clickTarget.GetComponent<Character>());
                        break;
                }
            }
            else if (didMouseMove && didClickItem)
            {
                itemDroppedPosition = clickTarget.position;
                clickTarget.position = new Vector2(1000, 1000);
                releaseTarget = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f).transform;
                if (releaseTarget != null){
                    clickTarget.position = itemDroppedPosition;
                    if (GetPropID(clickTarget) == GetPropLinkedItem(releaseTarget)) HandleCombo(clickTarget, releaseTarget);
                    else clickTarget.position = itemGrabbedPosition;
                }
                else clickTarget.position = itemGrabbedPosition;
            }
            else if(didClickItem){
                clickTarget.position = itemGrabbedPosition;
            }
            didClickItem = false;
        }
    }

    bool CheckMouseMoved(Vector2 cL, Vector2 rL) {
        if (rL.x - cL.x > 10 || rL.x - cL.x < -10|| rL.y - cL.y > 10 || rL.y - cL.y < -10)  return true; 
        else return false;
    }

    int GetPropID(Transform T) {
        switch (T.tag)
        {
            case "WorldItem":
            case "InventoryItem":
                return clickTarget.GetComponent<Item>().ID;
            case "Door":
                return clickTarget.GetComponent<Door>().ID;
            case "Character":
                return clickTarget.GetComponent<Character>().ID;
        }
        return -1;
    }

    int GetPropLinkedItem(Transform T) {
        if (T != null) {
            switch (T.tag)
            {
                case "Item":
                case "InventoryItem":
                    return T.GetComponent<Item>().linkedItem;
                case "Door":
                    return T.GetComponent<Door>().linkedItem;
                case "Character":
                    return T.GetComponent<Character>().linkedItem;
            }
        }
        return -1;
    }

    void HandleCombo(Transform T1, Transform T2) {
        //TODO
    }

    void HandleItem(Item i) {
        Debug.Log("handling Item");
        if (i.takeID != -1) GetComponentInParent<Inventory>().Add(i.takeID);
        //TODO HANDLE COMMENTS HERE
    }

    void HandleDoor(Door d) {
        //TODO
    }

    void HandleCharacter(Character c) {
        GetComponentInParent<SpeechController>().beginSpeech(c.ID);
        //TODO
    }
}
