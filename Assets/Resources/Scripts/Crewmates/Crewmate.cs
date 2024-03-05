using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrewmateInterview
{
    public class CrewmateNames
    {
        public string[] firstNames = {"Ava", "Amelia", "Aiden", "Anthony", "Alex", "Avery", "Bella", "Brooke", "Blake", "Brandon", "Bailey", "Blake", "Chloe", "Claire", "Connor", "Carter", "Casey", "Cameron"};
        public string[] lastNames = { "Smith", "Johnson", "Williams", "Brown","Jones", "Miller", "Garcia", "Jackson", "Moore", "Lopez", "Thompson"};
    }

    public enum Hobbies {Gaming, Sport, Fishing, Art, Reading, BirdWatching};

    public class Crewmate
    {

        //Name
        public string crewmateFirstName;
        public string crewmateLastName;

        //Hobby & Image
        public Hobbies hobby;
        public Sprite crewmateImage;

        //IsParasite
        public bool isParasite;

    }
}
