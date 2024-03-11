using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using System.Collections;

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
        [SerializeField] private List<Sprite> crewmateImages;

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
        [SerializeField] private Image crewmateHeadshotImage;
        [SerializeField] private TMP_Text textCrewmateName;
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





        private void Awake()
        {
            crewmateImages = new List<Sprite>(Resources.LoadAll<Sprite>("UI/Sprites"));
            crewmateNames = new CrewmateNames();
            crewmates = new List<Crewmate>();
            playerCrewmates = new List<Crewmate>();
            CreateRandomCrewmates();
            crewmateUI.SetActive(false);
            finishedGO.SetActive(false);
            particleCannon.SetActive(false);
        }

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


        #region - Start Selection Round -
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
        private void AddCrewmateToPlayerCollection(Crewmate curCrewmate)
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

        private Crewmate GetRandomCrewmate()
        {
            Crewmate curCrewmate = crewmates[Random.Range(0, crewmates.Count)];
            crewmateHeadshotImage.sprite = curCrewmate.crewmateImage;
            textCrewmateName.text = (curCrewmate.crewmateFirstName + " " + curCrewmate.crewmateLastName);
            textCrewmateHobby.text = curCrewmate.hobby.ToString();
            return curCrewmate;
        }
        #endregion


        #region - Create Pool Of Crewmates -
        private void CreateRandomCrewmates()
        {
            crewmates = new List<Crewmate>(); //Instantiate the list

            //Add number of crewmates based on numOfCrewmates
            for (int i = 0; i < numOfCrewmates; i++)
            {
                crewmates.Add(CreateCrewmate()); 
            }

        }

        private Crewmate CreateCrewmate()
        {
            Crewmate curCrewmate = new Crewmate();
            curCrewmate.crewmateFirstName = crewmateNames.firstNames[Random.Range(1, crewmateNames.firstNames.Length)];
            curCrewmate.crewmateLastName = crewmateNames.lastNames[Random.Range(1, crewmateNames.lastNames.Length)];
            curCrewmate.crewmateImage = crewmateImages[Random.Range(0, crewmateImages.Count)];
            curCrewmate.hobby = (Hobbies)Random.Range(0, 5);
            curCrewmate.isParasite = (Random.Range(0f, 1f) <= parasiteChance) ? true : false;
            return curCrewmate;
        }
        #endregion

        #region - Identifiers -
        public RoundState GetRoundState()
        {
            return roundState;
        }

        #endregion
    }


}
