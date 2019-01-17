using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour{

    public Text speechText;
    public Button[] choiceButtons;
    public GameObject buttonPanel;
    public Canvas speechCanvas;

    public GameState gs;

    private Stage currentStage;

    void Start(){
        speechCanvas.enabled = false;
    }

    public void beginSpeech(int characterID) {
        currentStage = GetStage(characterID, 0, 0); //0,0 at the end to indicate the first stage of speech
            speechText.text = currentStage.speech;
            for (int i = 0; i < choiceButtons.Length && i < currentStage.options.Count; i++){
                if (currentStage.options[i].enabled){
                    choiceButtons[i].interactable = true;
                    choiceButtons[i].GetComponentInChildren<Text>().text = currentStage.options[i].text;
                }
                else{
                    choiceButtons[i].GetComponentInChildren<Text>().text = "";
                    choiceButtons[i].interactable = false;
                }
            }
            for (int i = currentStage.options.Count; i < choiceButtons.Length; i++) {
                choiceButtons[i].GetComponentInChildren<Text>().text = "";
                choiceButtons[i].interactable = false;
            }
            speechCanvas.enabled = true;
    }

    void progressSpeech(int choice) {
        currentStage = GetStage(currentStage.characterID, currentStage.stageIndex+1, choice);
        speechText.text = currentStage.speech;
        if (currentStage.speech == "EXIT") {
            ExitSpeech();
        }
        else if (currentStage.speech == "BACK") {
            beginSpeech(currentStage.characterID);
        }
        else {
            for (int i = 0; i < choiceButtons.Length && i < currentStage.options.Count; i++)
            {
                if (currentStage.options[i].enabled)
                {
                    choiceButtons[i].interactable = true;
                    choiceButtons[i].GetComponentInChildren<Text>().text = currentStage.options[i].text;
                }
                else
                {
                    choiceButtons[i].GetComponentInChildren<Text>().text = "";
                    choiceButtons[i].interactable = false;
                }
            }
            for (int i = currentStage.options.Count; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].GetComponentInChildren<Text>().text = "";
                choiceButtons[i].interactable = false;
            }
        }
    }

    public void HandleChoice1(){
        progressSpeech(1);
    }
    public void HandleChoice2(){
        progressSpeech(2);
    }
    public void HandleChoice3(){
        progressSpeech(3);
    }
    public void HandleChoice4(){
        progressSpeech(4);
    }
    public void HandleChoice5(){
        progressSpeech(5);
    }
    public void HandleChoice6(){
        progressSpeech(6);
    }
    public void HandleChoice7(){
        progressSpeech(7);
    }

    //Gets a stage object which containts speech and choices for replies
    private Stage GetStage(int characterID, int stageIndex, int choice) {
        Stage s = new Stage();
        switch (characterID) {
            case 17:
                s = GetConciergeSpeech(stageIndex, choice);
                break;
        }
        s.stageIndex = stageIndex;
        s.characterID = characterID;
        return s;
    }

    //First item in list is alway the speech of the character and the following items are options for the player
    private Stage GetConciergeSpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.spokenToConcierge && !gs.fixedAllProblems) {
                    s.speech = ("Hello sir, I am afraid we are not accepting bookings at the moment and regrettably must direct you away from our custom.");
                    
                }
                else if (gs.spokenToConcierge && !gs.fixedAllProblems) {
                    s.speech = ("Hello again sir. How may I help you?");
                    
                }
                else if (gs.fixedAllProblems) {
                    s.speech = ("Thank you for helping us sir but I am afraid there is still something wrong...something I cannot explain.");
                }
                s.options.Add(new Option ("Why are you not accepting bookings?", !gs.spokenToConcierge));
                s.options.Add(new Option ("Why does that other man look so worried?", true));
                s.options.Add(new Option ("The lift operator said that your hat was of fantastic shape, style and finesse!", !gs.spokenToLiftOperator && gs.foundOutAboutFeud));
                s.options.Add(new Option ("The lift operator has given up on the lift, deciding that it's too much effort. If I fix the lift in his place will that be okay?", gs.spokenToLiftOperator && gs.foundOutAboutFeud));
                s.options.Add(new Option ("I apologise for what I said about your hat before. I actually thing it is beautiful. I was merely jealous of you, especially since its colour works so well with your complexion.", gs.insultedHat));
                s.options.Add(new Option ("Goodbye", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                        s.speech = ("The lift is currently broken, we're incredibly understaffed and frankly the whole place is just too much of a shambles!");
                        break;
                    case 2:
                        s.speech = ("Our incapable lobby boy cannot put suitcases on a trolley unfortunately.");
                        break;
                    case 3:
                        s.speech = ("As if! Don't you lie to me sir!");
                        break;
                    case 4:
                        s.speech = ("Hmm...what do you think of my hat?");
                        break;
                    case 5:
                        s.speech = ("I like to think of myself as a forgiving man so I will accept your apology. Here, you can have the cog and fix the lift. It's probably all for the best this way.");
                        //TODO GIVE COG
                        break;
                    case 6:
                        Debug.Log("Speech Exit attempt");
                        s.speech = ("EXIT");
                        break;

                }
                s.options.Add(new Option("Why is this place in such a mess?", (choice == 1)));
                s.options.Add(new Option("Is there any way I can help?", choice == 1));
                s.options.Add(new Option("I think it is fantastic! May I enquire as to where I could attain one for myself?", choice == 4));
                s.options.Add(new Option("I fear it a very unattractive garment. I hope you only wear it because it is required in the uniform.", choice == 4));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));//ALWATS HAVE THESE LAST TWO AT THE END OF ANY STAGE PAST ZERO
                s.options.Add(new Option("Goodbye", true));
                break;
            case 2:
                switch (choice) {
                    case 1:
                        s.speech = ("I...I can't divulge that I am afraid sir.");
                        break;
                    case 2:
                        s.speech = ("It's a bit odd for you to be so eager but honestly I can't really turn down help at the moment can I? If you were to help I would appreciate it greatly. Thank you sir.");
                        break;
                    case 3:
                        s.speech = ("Ooooh a good fashionista never reveals his secrets I'm afraid. But for your good taste I will certainly give you the cog so you can fix the lift.");
                        gs.gotCog = true;
                        //TODO Give cog
                        break;
                    case 4:
                        s.speech = ("Uniform!? Get out of here!");
                        gs.insultedHat = true;
                        break;
                    case 5:
                        s.speech = ("BACK");
                        break;
                    case 6:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("So what things do I need to help out with?", choice == 2));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 3:
                switch (choice) {
                    case 1:
                        s.speech = ("If you could help out our lobby boy with those suitcases it would be very much appreciated. Regrettably two customers, Mrs. Ludwig and Mr. Jaques, are staying at the hotel even though we're not accepting any more vacancies because they were here before things got too out of hand; checking up on them would be appreciated. Unfortunately Mrs. Ludwig is on the first floor, there are no stairs in this hotel and the lift is broken but I do not want to allow the lift operator the satisfaction of fixing it so I am keeping the cog hidden. Doing these things would be very appreciated but unfortunately I fear this hotel is cursed and we may be near the end.");
                        break;
                    case 2:
                        s.speech = ("BACK");
                        break;
                    case 3:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("Why won't you let the lift operator fix the lift?", choice == 1));
                s.options.Add(new Option("What makes you think the hotel is cursed?", choice == 1));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));//ALWATS HAVE THESE LAST TWO AT THE END OF ANY STAGE PAST ZERO
                s.options.Add(new Option("Goodbye", true));
                break;
            case 4:
                switch (choice) {
                    case 1:
                        s.speech = ("He mocked my hat.");
                        break;
                    case 2:
                        s.speech = ("I can't explain. It's just a feeling I have I am afraid");
                        break;
                    case 3:
                        s.speech = ("BACK");
                        break;
                    case 4:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("That's it?", choice == 1));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));//ALWATS HAVE THESE LAST TWO AT THE END OF ANY STAGE PAST ZERO
                s.options.Add(new Option("Goodbye", true));
                break;
            case 5:
                switch (choice) {
                    case 1:
                        s.speech = ("Yes. What more do you need?");
                        break;
                    case 2:
                        s.speech = ("BACK");
                        break;
                    case 3:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("Please could you give me the cog to fix your lift? Surely it doesn't make sense to prevent it from being fixed? What about Mrs. Ludwig?", choice == 1));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));//ALWATS HAVE THESE LAST TWO AT THE END OF ANY STAGE PAST ZERO
                s.options.Add(new Option("Goodbye", true));
                break;
            case 6:
                switch (choice) {
                    case 1:
                        s.speech = ("I say that you haven't the right to question me sir! Or are you on his side? If you are then I'll have you know I will not give the cog up until the man admits that my hat is of a fantastic shape, style and finesse! ");
                        gs.foundOutAboutFeud = true;
                        gs.spokenToConcierge = true;
                        break;
                    case 2:
                        s.speech = ("BACK");
                        break;
                    case 3:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));//ALWATS HAVE THESE LAST TWO AT THE END OF ANY STAGE PAST ZERO
                s.options.Add(new Option("Goodbye", true));
                break;
            case 7:
                switch (choice) {
                    case 1:
                        s.speech = ("BACK");
                        break;
                    case 2:
                        s.speech = ("EXIT");
                        break;
                }
                break;
        }
        return s;
    }

    private void ExitSpeech() {
        speechCanvas.enabled = false;
    }

    private class Stage {
        public int stageIndex;
        public string speech;
        public List<Option> options;
        public int characterID;

        public Stage() {
            stageIndex = -1;
            speech = "";
            options = new List<Option>();
            characterID = -1;
        }
    }

    private class Option {
        public string text;
        public bool enabled;

        public Option(string text, bool enabled) {
            this.text = text;
            this.enabled = enabled;
        }
    }
}
