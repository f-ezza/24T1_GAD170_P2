using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrewmateInterview
{
    public enum RoundState { start, mid, end};
    
    public class GameManager : MonoBehaviour
    {
        //Crewmate Generation
        [Header("Crewmate Generation")]
        [SerializeField][RangeAttribute(0f, 1f)] private float parasiteChance;
        [SerializeField] private int numOfCrewmates;
        [SerializeField] private List<Crewmate> crewmates;
        [SerializeField] private CrewmateNames crewmateNames;

        //Object Reference
        [Header("Object References")]
        [SerializeField] private TMP_Text textRepOfCrewmates;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject folderPrefab;
        [SerializeField] private GameObject curFolder;
        [SerializeField] private KeyCode curKey;
        [SerializeField] private GameObject particleCannon;

        //UI References
        [Header("UI Object References")]
        [SerializeField] private GameObject crewmateUI;
        [SerializeField] private TMP_Text textCrewmateLastName;
        [SerializeField] private TMP_Text textCrewmateFirstName;
        [SerializeField] private TMP_Text textCrewmateHobby;
        [SerializeField] private GameObject finishedGO;

        //Player Fields
        [Header("Player Fields")]
        [SerializeField] private Crewmate curCrewmate;
        [SerializeField] private List<Crewmate> playerCrewmates;
        [SerializeField] private int amtOfPlayerCrewmates;

        //Game State
        [Header("Game State")]
        [SerializeField] private bool isRoundInProgress;
        [SerializeField] private bool haveCurCrewmate;
        [SerializeField] private bool gameEnded;

        //Identifiers
        [SerializeField] private RoundState roundState;



        #region - Initalisation -
        /*
        []---Awake Method---[]
        []The Awake Method does the following: 
        []It assigns a new Object Class that contains all the crew mate names
        []It initalises a new instance of a crewmates list to hold information on the soon to be generated crewmates
        []It initalises a new instance of a Crewmates list to hold the information of the players accquired crewmates
        []It hides the ui
        []It hides to game end text
        []It hides the particle emitter
        []---Conclusion of Awake Method---[]
        */
        private void Awake()
        {
            crewmateNames = new CrewmateNames();
            crewmates = new List<Crewmate>();
            playerCrewmates = new List<Crewmate>();

            crewmateUI.SetActive(false);
            finishedGO.SetActive(false);
            particleCannon.SetActive(false);

            CreateRandomCrewmates();
        }
        #endregion

        #region - Update Method - 
        /*
        []---Update Method---[]

        []---First Block---[]
        []Checks if the game has ended, if it has, it hides the UI of the current crewmate and returns to silence the update method
        []Checks if the list of global crewmates is null (not populated), it then fills the list up via calling CreateRandomCrewmates() and returning
        []Checks if the Players accquired crewmate list is = or greater than 10, if so it will call the EndGame() method, set the current crewmate ui display to false and sets gameEnded to false
        []---End First Block---[]

        []---Second Block---[]
        []Checks if a round is in progress (round refering to the player selecting if they wish to hire the crewmate or not), if it isnt it moves on to check if the curFolder which is a gameobject from a prefab is in existance
        []If the curFolder is not existing or null it then instantiates a new one from the provided folder prefab, accquires a new animator component gotten from the curfolder, sets the roundState to RoundState.start nad then sets isRoundInProgress to true
        []---End Second Block---[]

        []---Third Block---[]
        []Checks if a round is in progress and that we dont have a current crewmate
        []If the check is true we then set that we have a crewmate and then assign a curCrewmate (a gameobject variable) based on the return provided by the GetRandomCrewmate() method, set the round state to mid and activate the uiof the crewmates information
        []---End Third Block---[]

        []---Fourth Block---[]
        []We check if there is a round in progress and if the player has pressed keys e or q
        []If they have, we change round state to end, disable the ui of the current crewmate and then call the endRound function passing in the key that was pressed
        []We then check if the roundstate set to end and if so, we call endround again
        []---End Forth Block---[]
        */
        private void Update()
        {
            if(gameEnded) { crewmateUI.SetActive(false);  return; }
            if (crewmates == null) { CreateRandomCrewmates(); return; }
            if (playerCrewmates.Count >= 10)
            {
                EndGame();
                crewmateUI.SetActive(false);
                gameEnded = true;
            }

            if (!isRoundInProgress)
            {
                if (curFolder == null)
                {
                    curFolder = Instantiate(folderPrefab);
                    animator = curFolder.GetComponent<Animator>();
                    roundState = RoundState.start;
                    isRoundInProgress = true;
                }
            }

            if (isRoundInProgress && !haveCurCrewmate)
            {
                haveCurCrewmate = true;
                curCrewmate = GetRandomCrewmate();
                roundState = RoundState.mid;
                crewmateUI.SetActive(true);
            }

            if (isRoundInProgress && (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q)))
            {
                roundState = RoundState.end;
                crewmateUI.SetActive(false);
                EndRound(Input.GetKeyUp(KeyCode.E) ? KeyCode.E : KeyCode.Q);
                
            }
            if(roundState == RoundState.end)
            {
                EndRound(curKey);
            }
        }
        #endregion


        #region - Start Selection Round -
        /*
        []---End Round Method---[]

        []---First Block---[]
        []First we assign curKey to the key that was passed through in the call method
        []we then play the animation "Exit" which is retrieved by the previously called animator object assigned in 2nd block in update
        []We then check if the animation has finished playing and if so we then check if the key pressed was q or e
        []If it was Q we remove the crewmate from the active pool of potential crewmates to display and add another one through the method CreateCrewmate() and is added to the global crewmates list
        []We then set that we no longer have a valid current crewmate by assigning that to false and that the round has ended
        []If it was E we AddCrewmateToPlayerCollection using that method by passing in the curCrewmate, we then set that we dont have a current crewmate and that the round has ended
        []We then remove current folder (this will set curFolder to null)
        []---End First Block---[]
        */
        private void EndRound(KeyCode key)
        {
            curKey = key;
            animator.Play("Exit");
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                if (key == KeyCode.Q)
                {
                    crewmates.Remove(curCrewmate);
                    crewmates.Add(CreateCrewmate());
                    haveCurCrewmate = false;
                    isRoundInProgress = false;
                }
                else if (key == KeyCode.E)
                {
                    AddCrewmateToPlayerCollection(curCrewmate);
                    haveCurCrewmate = false;
                    isRoundInProgress = false;
                }
                Destroy(curFolder);
            }

        }

        /*
        []---AddCrewmateToPlayerCollection Method---[]
        []---First Block---[]
        []We first check if the passed in crewmate is a parasite, if the crewmate is a parasite we assign a new variable of type integer to keep track 
        []We then check if the player list of crewmates total numbers is greater or equal to 1 and that it is a valid list
        []We then loop over the amount of entries and check if the current crewmate in the loop has the same hobby as the crewmate that is a parasite
        []If they are the same then we delete the crewmember from the players crew and then add add a new crewmember to the global list
        []We then add the amount of crew lost by 1
        []After the loop ends, we deduct the visual display of the number of crewmates the player has
        []Update the text
        []Debug.Log out that the crewmate we selected was a parasite and then how many we lost
        []---End First Block---[]

        []---Second Block---[]
        []If the crewmate isnt a parasite we do this
        []Log out that we added the crewmember tot he players list of crewmembers
        []Add the crewmember to the list of players crew
        []Remove the current crewmate from the global list of crewmembers
        []Generate a new crewmember and add them to the global list of crewmates
        []Add by 1 the amount of crewmembers the player has
        []Update the text of player crewmembers
        []---End Second Block---[]
         */

        private void AddCrewmateToPlayerCollection(Crewmate curCrewmate)
        {
            if(curCrewmate != null) 
            {
                if (curCrewmate.isParasite)
                {
                    int amtOfCrewLost = 0;
                    if (playerCrewmates.Count >= 1 && playerCrewmates != null)
                    {
                        for (int i = 0; i < playerCrewmates.Count; i++)
                        {
                            if (playerCrewmates[i].hobby == curCrewmate.hobby)
                            {
                                playerCrewmates.RemoveAt(i);
                                crewmates.Add(CreateCrewmate());
                                amtOfCrewLost++;
                            }
                        }
                    }
                    amtOfPlayerCrewmates -= amtOfCrewLost;
                    textRepOfCrewmates.text = amtOfPlayerCrewmates.ToString();
                    Debug.Log("Oh no, " + curCrewmate.crewmateFirstName + " was an alien parasite!, you lost " + amtOfCrewLost + " crew members");
                }
                else
                {
                    Debug.Log("Added " + curCrewmate.crewmateFirstName + " to your crew.");
                    playerCrewmates.Add(curCrewmate);
                    crewmates.Remove(curCrewmate);
                    crewmates.Add(CreateCrewmate());
                    amtOfPlayerCrewmates++;
                    textRepOfCrewmates.text = amtOfPlayerCrewmates.ToString();
                }
            }
            else
            {
                Debug.Log("Crewmate is not valid!"); return;
            }
        }
        /*
        []---EndGame Method---[]
        []Log out that the player won
        []Log out a header
        []Log out the players crewmembers by looping over it and printing the current crewmembers valus
        []Turn the particle emitter on
        []Set the finished text on
        []---End EndGame Method---[]
         */
        private void EndGame()
        {
            Debug.Log("You won!");
            Debug.Log("This is your crew:");
            for (int i = 0; i < playerCrewmates.Count; ++i)
            {
                Debug.Log("Name: " + playerCrewmates[i].crewmateFirstName + " " + playerCrewmates[i].crewmateLastName + ", Hobby: " + playerCrewmates[i].hobby.ToString());
            }
            particleCannon.SetActive(true);
            finishedGO.SetActive(true);
        }


        /*
        []---GetRandomCrewmate Method---[]
        []Create a new object of type Crewmate and set it equal to a random crewmate in the global crewmate list
        []Assign the visual texts to the value of the randomly gathered crewmember
        []Return the crewmember to whatever called the method
        []---End GetRandomCrewmate Method---[]
         */
        private Crewmate GetRandomCrewmate()
        {
            Crewmate curCrewmate = crewmates[Random.Range(0, crewmates.Count)];
            textCrewmateFirstName.text = curCrewmate.crewmateFirstName;
            textCrewmateLastName.text = curCrewmate.crewmateLastName;
            textCrewmateHobby.text = curCrewmate.hobby.ToString();
            return curCrewmate;
        }
        #endregion


        #region - Create Pool Of Crewmates -
        /*
        []---CreateRandomCrewmates Method---[]
        []Instantiate a new list of crewmates
        []Loop over the number of wanted crew members of the game, in default its 10
        []Add the current crewmate generated using the CreateCrewmate Method
        []---End CreateRandomCrewmates---[]
         */
        private void CreateRandomCrewmates()
        {
            crewmates = new List<Crewmate>(); //Instantiate the list

            //Add number of crewmates based on numOfCrewmates
            for (int i = 0; i < numOfCrewmates; i++)
            {
                crewmates.Add(CreateCrewmate()); 
            }

        }

        /*
        []---CreateCrewmate Method---[]
        []Create a new local crewmate by copying the template from the crewmate class
        []Assign the variables of firstname, lastname, hobby and if they are a parasite
        []Return the newly created crewmate
        []---End CreateCrewmate Method---[]
         */
        private Crewmate CreateCrewmate()
        {
            Crewmate curCrewmate = new Crewmate();
            curCrewmate.crewmateFirstName = crewmateNames.firstNames[Random.Range(1, crewmateNames.firstNames.Length)];
            curCrewmate.crewmateLastName = crewmateNames.lastNames[Random.Range(1, crewmateNames.lastNames.Length)];
            curCrewmate.hobby = (Hobbies)Random.Range(0, 5);
            curCrewmate.isParasite = (Random.Range(0f, 1f) <= parasiteChance) ? true : false;
            return curCrewmate;
        }
        #endregion

        #region - Identifiers -
        /*
        []---GetRoundState---[]
        []Check what the state of the game is in
        []---End GetRoundState---[]
        */
        public RoundState GetRoundState
        {
            get 
            {
                return roundState;
            }
        }

        #endregion
    }


}
