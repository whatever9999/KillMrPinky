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
        patientAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
}
