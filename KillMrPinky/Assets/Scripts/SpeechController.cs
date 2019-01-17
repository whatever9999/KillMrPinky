using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour {

    public Text speechText;
    public Button[] choiceButtons;
    public GameObject buttonPanel;
    public Canvas speechCanvas;

    public GameState gs;

    private Stage currentStage;

    void Start() {
        speechCanvas.enabled = false;
    }

    private void Awake() {
        DontDestroyOnLoad(speechCanvas);
    }

    public void beginSpeech(int characterID) {
        currentStage = GetStage(characterID, 0, 0); //0,0 at the end to indicate the first stage of speech
        speechText.text = currentStage.speech;
        if (currentStage.speech == "EXIT") {
            ExitSpeech();
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
            speechCanvas.enabled = true;
        }
    }

    void progressSpeech(int choice) {
        currentStage = GetStage(currentStage.characterID, currentStage.stageIndex + 1, choice);
        speechText.text = currentStage.speech;
        if (currentStage.speech == "EXIT") {
            ExitSpeech();
            GetComponentInParent<SelectionController>().setToDefaultGameType();
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

    public void HandleChoice1() {
        Debug.Log("Button 1");
        progressSpeech(1);
    }
    public void HandleChoice2() {
        progressSpeech(2);
    }
    public void HandleChoice3() {
        progressSpeech(3);
    }
    public void HandleChoice4() {
        progressSpeech(4);
    }
    public void HandleChoice5() {
        progressSpeech(5);
    }
    public void HandleChoice6() {
        progressSpeech(6);
    }
    public void HandleChoice7() {
        progressSpeech(7);
    }

    //Gets a stage object which containts speech and choices for replies
    private Stage GetStage(int characterID, int stageIndex, int choice) {
        Stage s = new Stage();
        switch (characterID) {
            case 9:
                if (gs.patientAwake) s = GetPatientSpeech(stageIndex, choice);
                else s.speech = "EXIT";
                break;
            case 17:
                s = GetConciergeSpeech(stageIndex, choice);
                break;
            case 18:
                s = GetLobbyBoySpeech(stageIndex, choice);
                break;
            case 28:
                s = GetLiftOperatorSpeech(stageIndex, choice);
                break;
            case 55:
                s = GetMrJaquesSpeech(stageIndex, choice);
                break;
            case 48:
                s = GetMrsLudwigSpeech(stageIndex, choice);
                break;
            case 77:
                s = GetChefSpeech(stageIndex, choice);
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
                s.options.Add(new Option("Why are you not accepting bookings?", !gs.spokenToConcierge));
                s.options.Add(new Option("Why does that other man look so worried?", true));
                s.options.Add(new Option("The lift operator said that your hat was of fantastic shape, style and finesse!", !gs.spokenToLiftOperator && gs.foundOutAboutFeud));
                s.options.Add(new Option("The lift operator has given up on the lift, deciding that it's too much effort. If I fix the lift in his place will that be okay?", gs.spokenToLiftOperator && gs.foundOutAboutFeud));
                s.options.Add(new Option("I apologise for what I said about your hat before. I actually thing it is beautiful. I was merely jealous of you, especially since its colour works so well with your complexion.", gs.insultedHat));
                s.options.Add(new Option("Goodbye", true));
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

    private Stage GetLobbyBoySpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.spokenToConcierge) s.speech = ("Agh! Please don't scare me like that! I'm sorry sir but I cannot help you. Could you ask the man at the front desk instead please, thank you, sorry.");
                else if (gs.spokenToConcierge && !gs.helpedLobbyBoy) s.speech = ("Eep, sorry, why are you talking to me? Sorry? Thank you, sorry?");
                else if (gs.spokenToConcierge && gs.helpedLobbyBoy) s.speech = ("Ahhh, I feel so much better now you've helped me with those suitcases! Thank you so much sir!");
                s.options.Add(new Option("Sorry. I didn't mean to startle you.", !gs.spokenToConcierge));
                s.options.Add(new Option("was thinking that I could help you with those cases if you like?", gs.spokenToConcierge && !gs.helpedLobbyBoy));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                        s.speech = ("Hnng, that's okay. Sorry!");
                        break;
                    case 2:
                        s.speech = ("Oh yes please sir if you wouldn't mind! I'll throw them to you and you can put them in the trolly.");
                        break;
                    case 3:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option("Doesn't that overcomplicate things a bit?", choice == 2));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 2:
                switch (choice) {
                    case 1:
                        s.speech = ("We're in a rush sir! We must do this now! Thank you. Sorry.");
                        GetComponentInParent<SelectionController>().startLuggageMinigame();
                        break;
                    case 2:
                        s.speech = ("BACK");
                        break;
                    case 3:
                        s.speech = ("Exit");
                        break;
                }
                break;
        }
        return s;
    }

    private Stage GetLiftOperatorSpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.fixedLift) s.speech = ("Sorry sir, you shouldn't really be here. Please, leave me be.");
                else s.speech = ("(Fixed lift = true) Thanks for handling that lift. Man is insane; I need a new job.");
                s.options.Add(new Option("Okay.", !gs.spokenToConcierge));
                s.options.Add(new Option("I was hoping to fix the lift for you?", gs.spokenToConcierge));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                    case 3:
                        s.speech = ("EXIT");
                        break;
                    case 2:
                        s.speech = ("The nutcase has given you the cog?");
                        break;
                }
                s.options.Add(new Option("Uhh...no?", choice == 1 && !gs.gotCog));
                s.options.Add(new Option("Yep!Right here!", choice == 1 && gs.gotCog));
                s.options.Add(new Option("Can I ask some more questions? (Go back)", true));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 2:
                switch (choice) {
                    case 1:
                        s.speech = ("I'm sorry but if you expect to fix the lift with nothing then it's not the only thing with a missing part...");
                        break;
                    case 2:
                        s.speech = ("So you sucked up to him? I guess someone had to do it. Get on with it then. ");
                        break;
                    case 3:
                        s.speech = "BACK";
                        break;
                    case 4:
                        s.speech = "EXIT";
                        break;
                }
                break;
        }
        return s;
    }

    private Stage GetPatientSpeech(int stageIndex, int choice) {
        //TODO
        return null;
    }

    private Stage GetMrJaquesSpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.gotJournal && !gs.got002Pin) s.speech = ("Hello there...I assume you're an employee?");
                else if (gs.gotJournal) s.speech = ("Thank you for getting me my journal!");
                else if (!gs.gotJournal && gs.got002Pin) s.speech = ("Have you got my journal?");
                s.options.Add(new Option ("Why do you assume so?", !gs.gotJournal && !gs.got002Pin));
                s.options.Add(new Option ("I wanted to check up and see how you were doing sir?", gs.spokenToConcierge && !gs.got002Pin));
                s.options.Add(new Option (" Yes! Here it is...", gs.gotJournal));
                s.options.Add(new Option ("Oh no, sorry sir, I'll be right on that!", !gs.gotJournal));
                s.options.Add(new Option ("Goodbye.", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                        s.speech = ("You ARE in my hotel room? Are you not an employee?");
                        break;
                    case 2:
                        s.speech = ("Why, thank you, I am doing perfectly");
                        break;
                    case 3:
                        s.speech = ("Hmm...this isn't supposed to be here.");
                        gs.gotJournal = false;
                        break;
                    case 4:
                        s.speech = ("EXIT");
                        break;
                    case 5:
                        s.speech = ("EXIT");
                        break;
                }
                s.options.Add(new Option ("Oh, yes I am don't worry sir. ", choice == 1));
                s.options.Add(new Option ("Brilliant!",choice == 2));
                s.options.Add(new Option ("What's that?", choice == 3));
                s.options.Add(new Option ("Goodbye", true));
                break;
            case 2:
                switch (choice) {
                    case 1:
                        s.speech = "EXIT";
                        break;
                    case 2:
                        s.speech = "Though...if you wouldn't mind, I would like to write an entry in my journal about this time. I have had it stored in my safe. If you would go collect it I would be very appreciative.";
                        break;
                    case 3:
                        s.speech = "A slip of paper.";
                        break;
                    case 4:
                        s.speech = "EXIT";
                        break;
                }
                s.options.Add(new Option ("Ah, yes, alright. Not a problem sir! Right away.", choice == 2));
                s.options.Add(new Option ("Could I take that sir?", choice == 3));
                s.options.Add(new Option("Goodbye", true));
                break;
            case 3:
                switch (choice) {
                    case 1:
                        s.speech = ("Thank you! Here's the pin to get in that I was given.");
                        break;
                    case 2:
                        s.speech = ("Of course! It's certainly not mine.");
                        break;
                    case 3:
                        s.speech = "EXIT";
                        break;
                }
                s.options.Add(new Option("Goodbye!", choice == 1));
                s.options.Add(new Option("Goodbye!", choice == 2));
                break;
            case 4:
                switch (choice) {
                    case 1:
                        s.speech = "EXIT";
                        GetComponentInParent<Inventory>().Add(58);
                        gs.got002Pin = true;
                        break;
                    case 2:
                        s.speech = "EXIT";
                        break;
                }
                break;
        }
        return s;
    }

    private Stage GetChefSpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.knowsAboutWine) s.speech = "Who are you?";
                if (gs.knowsAboutWine && !gs.deliveredWine) s.speech = "Please help me think of what do do about Mrs. Ludwig!";
                if (gs.deliveredWine) s.speech = "I heard you delivered Mrs. Ludwig her wine! Thank you very much for helping me out. Now I can focus on clearing up this mess.";
                s.options.Add(new Option("I'm...a guest?", !gs.knowsAboutWine && !gs.spokenToConcierge));
                s.options.Add(new Option("I'm just an extra pair of hands for around the hotel. Is there anything I can help you with?", !gs.knowsAboutWine && gs.spokenToConcierge));
                s.options.Add(new Option("Goodbye!", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                        s.speech = "You shouldn't be here!";
                        break;
                    case 2:
                        s.speech = "Oh god yes! I've broken all the wine glasses and Mrs. Ludwig needs a glass of wine! I don't know what to do!";
                        break;
                    case 3:
                        s.speech = "EXIT";
                        break;
                }
                s.options.Add(new Option("Hmm...I'll think on it and sort something out don't worry.", choice == 2));
                s.options.Add(new Option("Goodbye!", true));
                break;
            case 2:
                switch (choice) {
                    case 1:
                        s.speech = "Thank you so much!";
                        break;
                    case 2:
                        s.speech = "EXIT";
                        break;
                }
                s.options.Add(new Option("Goodbye!", true));
                break;
            case 3:
                s.speech = "EXIT";
                break;
        }
        return s;
    }

    private Stage GetMrsLudwigSpeech(int stageIndex, int choice) {
        Stage s = new Stage();
        switch (stageIndex) {
            case 0:
                if (!gs.deliveredWine) s.speech = "Do you have my wine?";
                else s.speech = "Mmmm";
                s.options.Add(new Option("Sorry?", !gs.knowsAboutWine));
                s.options.Add(new Option("Ah, no but it's on its way don't worry. ", gs.knowsAboutWine && !gs.gotWine && !gs.deliveredWine));
                s.options.Add(new Option("Here we go!",gs.knowsAboutWine && gs.gotWine));
                s.options.Add(new Option("Goodbye.", true));
                break;
            case 1:
                switch (choice) {
                    case 1:
                        s.speech = "The service in this hotel is horrendous. My wine? From the kitchens? Goodness me.";
                        break;
                    case 2:
                        s.speech = "Hmm...";
                        break;
                    case 3:
                        s.speech = "It's about time!";
                        gs.deliveredWine = true;
                        break;
                    case 4:
                        s.speech = "Mmmm";
                        break;
                }
                s.options.Add(new Option("Goodbye.", true));
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
