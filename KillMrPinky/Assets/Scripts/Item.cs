using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Describes an item present in the world
public class Item : MonoBehaviour{

    public int ID;
    public int takeID; //-1 if can't take
    public string[] reactionText;
    public int linkedItem; //-1 if none
}
