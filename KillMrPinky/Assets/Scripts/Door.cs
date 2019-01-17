using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Describes a portal through which new scenes can be accessed
public class Door : MonoBehaviour{

    public int ID; //-1 if can't take
    public int linkedItem;//-1 if none
    public bool canEnter;
    public string[] reactionText;
    public int linkedScene; //-1 if none
}
