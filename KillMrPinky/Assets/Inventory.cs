using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] positions;
    private int[] items;
    public GameObject[] ItemPrefabs;
    private GameObject[] displayItems;

    private void Start(){
        items = new int[15];
        for (int i = 0; i < 15; i++) {
            items[i] = -1;
        }
        displayItems = new GameObject[8];
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
        displayItems[location] = Instantiate(go, new Vector2(-8.25f + (location), 4.35f), Quaternion.identity); //-8.9,5  is the top left
        //TODO Investigate layers
    }
    GameObject getDisplayItem(int ID) {
        for (int i = 0; i < ItemPrefabs.Length; i++) {
            if (ItemPrefabs[i].GetComponent<Item>().ID == ID) return ItemPrefabs[i];
        }
        return null;
    }
}
