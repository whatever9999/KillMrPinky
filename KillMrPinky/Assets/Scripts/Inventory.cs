using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int[] items;
    public GameObject[] ItemPrefabs;
    private GameObject[] displayItems;

    private void Start(){
        items = new int[15];
        for (int i = 0; i < 15; i++) {
            items[i] = -1;
        }
        displayItems = new GameObject[8];
        DontDestroyOnLoad(this);
    }

    public void Remove(Item item) {
        bool found = false;
        for (int i = 0; i < items.Length && !found; i++) {
            if (item.ID == items[i]) {
                found = true;
                items[i] = -1;
                Destroy(displayItems[i]);
            }
        }
    }

    public void Add(int ID) {
        bool added = false;
        int location = 0;
        for (int i = 0; !added; i++) {
            if (items[i] == -1){
                items[i] = ID;
                location = i;
                added = true;
            }
        }
        GameObject go = getDisplayItem(ID); 
        displayItems[location] = Instantiate(go, new Vector2(-8.75f + (location), 4.85f), Quaternion.identity); //-8.9,5  is the top left
        Vector3 position = displayItems[location].transform.position;
        position.z -= 1.1f;
        displayItems[location].transform.position = position;
        //TODO Investigate layers
    }

    public void resetPositions() {
        for (int i = 0; i < displayItems.Length; i++) {
            try {
                GameObject go = displayItems[i];
                Destroy(displayItems[i]);
                displayItems[i] = Instantiate(go, new Vector2(-8.75f + (i), 4.85f), Quaternion.identity); //-8.9,5  is the top left
                Vector3 position = displayItems[i].transform.position;
                position.z -= 1.1f;
                displayItems[i].transform.position = position;
            }
            catch (System.Exception e) {
                Debug.Log("ERROR AT " + i);
            }
        }
    }

    GameObject getDisplayItem(int ID) {
        for (int i = 0; i < ItemPrefabs.Length; i++) {
            if (ItemPrefabs[i].GetComponent<Item>().ID == ID) return ItemPrefabs[i];
        }
        return null;
    }
}
