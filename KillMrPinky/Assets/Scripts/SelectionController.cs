using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionController : MonoBehaviour{

    private Vector2 clickLocation;
    private Vector2 itemGrabbedPosition;
    private Vector2 releaseLocation;
    private bool didMouseMove;
    private bool didClickItem;

    public GameObject SleepingDoctor;
    public GameObject DDDOnPatient;
    public GameObject DDDInHand;

    public GameObject PanelClosed;
    public GameObject PanelOpen;
    public GameObject PanelFixed;
    public GameObject FirstButton;
    public GameObject SecondButton;
    public GameObject GroundButton;
    

    private Transform clickTarget;
    private Transform releaseTarget;

    private List<int> clickedItems;

    private enum GameType {Main, Message, Speech, Bags, Pool, paper}
    GameType gt;

    // Start is called before the first frame update
    void Start(){
        didClickItem = false;
        gt = GameType.Main;
        clickedItems = new List<int>();
        DontDestroyOnLoad(this.GetComponentInParent<Inventory>());
        SleepingDoctor.SetActive(false);
        DDDOnPatient.SetActive(false);
        DDDInHand.SetActive(true);
    }

    // Update is called once per frame
    void Update(){
        if (gt == GameType.Main) {
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
                releaseLocation = new Vector2((Input.mousePosition.x), (Input.mousePosition.y));
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
                    clickTarget.gameObject.SetActive(false);
                    releaseTarget = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f).transform;
                    clickTarget.gameObject.SetActive(true);
                    if (releaseTarget != null) {
                        if (GetPropID(clickTarget) == GetPropLinkedItem(releaseTarget)) HandleCombo(clickTarget.GetComponent<Item>(), releaseTarget);
                        else clickTarget.position = itemGrabbedPosition;
                    }
                    else clickTarget.position = itemGrabbedPosition;
                }
                else if (didClickItem) {
                    clickTarget.position = itemGrabbedPosition;
                }
                didClickItem = false;
            }
        }
        else if (gt == GameType.Message) {
            if (Input.GetMouseButtonDown(0)) {
                GetComponentInParent<MessageSystem>().closeMessage();
                gt = GameType.Main;
            }
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
                return T.GetComponent<Item>().ID;
            case "Door":
                return T.GetComponent<Door>().ID;
            case "Character":
                return T.GetComponent<Character>().ID;
        }
        return -1;
    }

    int GetPropLinkedItem(Transform T) {
        if (T != null) {
            switch (T.tag)
            {
                case "WorldItem":
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

    void HandleCombo(Item item, Transform T2) {
        if ((item.ID == 10) && (GetPropID(T2) == 6)) {
            GetComponentInParent<Inventory>().Remove(item);
            DDDInHand.SetActive(false);
            GetComponentInParent<Inventory>().Add(11);
        } else if (item.ID == 11 && GetPropID(T2) == 9) {
            StartCoroutine(ToSleep());
        } else if (item.ID == 29 && GetPropID(T2) == 90) {
            PanelClosed.SetActive(false);
            PanelOpen.SetActive(true);
            GetComponentInParent<Inventory>().Add(86);
        } else if (item.ID == 21 && GetPropID(T2) == 92) {
            PanelOpen.SetActive(false);
            PanelFixed.SetActive(true);
        } else if (item.ID == 82 && GetPropID(T2) == 91) {
            PanelFixed.SetActive(false);
            PanelClosed.SetActive(true);
            FirstButton.GetComponent<Door>().canEnter = true;
            SecondButton.GetComponent<Door>().canEnter = true;
        }
        Destroy(item.gameObject);
        //TODO
    }

    IEnumerator ToSleep() {
        DDDOnPatient.SetActive(true);
        yield return new WaitForSeconds(1);
        SleepingDoctor.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        yield return null;
    }

    void HandleItem(Item i) {
        if (!clickedItems.Contains(i.ID)){
            GetComponentInParent<MessageSystem>().showMessage(i.reactionText[0]);
            if (i.takeID != -1) GetComponentInParent<Inventory>().Add(i.takeID);
        }
        else if (clickedItems.Contains(i.ID) && i.reactionText.Length > 1){
            GetComponentInParent<MessageSystem>().showMessage(i.reactionText[1]);
        }
        else {
            GetComponentInParent<MessageSystem>().showMessage(i.reactionText[0]);
        }
        clickedItems.Add(i.ID);
        gt = GameType.Message;
    }

    void HandleDoor(Door d){
        if (d.canEnter){
            SceneManager.LoadScene(d.linkedScene);
            GetComponentInParent<Inventory>().resetPositions();
        }
        else {
            GetComponentInParent<MessageSystem>().showMessage(d.reactionText[0]);
            gt = GameType.Message;
        }
        //TODO
    }

    void HandleCharacter(Character c) {
        Debug.Log("TRYING TO TALK");
        gt = GameType.Speech;
        GetComponentInParent<SpeechController>().beginSpeech(c.ID);
        //TODO
    }

    public void setToDefaultGameType() {
        gt = GameType.Main;
    }

    public void startLuggageMinigame() {//TODO
    }
}
