using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour{

    public bool spokenToConcierge;
    public bool fixedAllProblems;
    public bool helpedLobbyBoy;
    public bool spokenToLiftOperator;
    public bool foundOutAboutFeud;
    public bool insultedHat;
    public bool gotCog;
    public bool gotWine;
    public bool fixedLift;
    public bool gotJournal;
    public bool got002Pin;
    public bool knowsAboutWine;
    public bool deliveredWine;

    public bool patientAwake;

    // Start is called before the first frame update
    void Start(){
        spokenToConcierge = false;
        spokenToLiftOperator = false;
        foundOutAboutFeud = false;
        fixedAllProblems = false;
        helpedLobbyBoy = false;
        insultedHat = false;
        gotCog = false;
        gotJournal = false;
        got002Pin = false;
        gotWine = false;
        patientAwake = false;
        fixedLift = false;
        knowsAboutWine = false;
        deliveredWine = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
}
