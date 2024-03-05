using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrewmateInterview
{
    public class GameManager : MonoBehaviour
    {
        //Crewmate Generation
        [Header("Crewmate Generation")]
        [SerializeField][RangeAttribute(0f, 1f)] private float parasiteChance;
        [SerializeField] private int numOfCrewmates;
        [SerializeField] private List<Crewmate> crewmates;
        [SerializeField] private CrewmateNames crewmateNames;
        [SerializeField] private List<Sprite> crewmateImages;

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




        private void Awake()
        {
            crewmateImages = new List<Sprite>(Resources.LoadAll<Sprite>("UI/Sprites"));
            crewmateNames = new CrewmateNames();
            crewmates = new List<Crewmate>();
            playerCrewmates = new List<Crewmate>();
            CreateRandomCrewmates();
        }

        private void Update()
        {
            if(gameEnded) { return; }
            if (crewmates == null) { CreateRandomCrewmates(); return; }
            if (playerCrewmates.Count >= 10)
            {
                EndGame();
                gameEnded = true;
            }
            else if (!isRoundInProgress)
            {
                isRoundInProgress = true;
                Debug.Log("Started Round");
            }

            if (isRoundInProgress)
            {
                if (!haveCurCrewmate) 
                {
                    haveCurCrewmate = true;
                    curCrewmate = GetRandomCrewmate();    
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Pressed 'E'");
                    AddCrewmateToPlayerCollection(curCrewmate);
                    haveCurCrewmate = false;
                    isRoundInProgress = false;
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Pressed 'Q'");
                    crewmates.Remove(curCrewmate);
                    crewmates.Add(CreateCrewmate());
                    haveCurCrewmate = false;
                    isRoundInProgress = false;
                }
            }
        }

        #region - Start Selection Round -

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
                Debug.Log("Oh no, " + curCrewmate.crewmateFirstName + " was an alien parasite!, you lost " + amtOfCrewLost + " crew members");
            }
            else
            {
                Debug.Log("Added " + curCrewmate.crewmateFirstName + " to your crew.");
                playerCrewmates.Add(curCrewmate);
                crewmates.Remove(curCrewmate);
                crewmates.Add(CreateCrewmate());
                amtOfPlayerCrewmates++;
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
        }

        private Crewmate GetRandomCrewmate()
        {
            Debug.Log(crewmates.Count);
            Crewmate curCrewmate = crewmates[Random.Range(0, crewmates.Count)];
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
    }
}
