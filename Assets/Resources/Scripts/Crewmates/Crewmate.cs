using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrewmateInterview
{
    public class CrewmateNames
    {
        public string[] firstNames = { "Ava", "Amelia", "Aiden", "Anthony", "Alex", "Avery", "Bella", "Brooke", "Blake", "Brandon", "Bailey", "Blake", "Chloe", "Claire", "Connor", "Carter", "Casey", "Cameron", "Charlotte", "Christopher", "Caleb", "Catherine", "Daniel", "David", "Ella", "Ethan", "Emma", "Emily", "Evan", "Grace", "Gabriel", "Hannah", "Henry", "Isabella", "Isaac", "Jack", "Jackson", "Jayden", "Liam", "Lily", "Lucas", "Madison", "Mia", "Mason", "Matthew", "Olivia", "Oliver", "Owen", "Sophia", "Samuel", "Sarah", "Tyler", "Victoria" };
        public string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Harris", "Jackson", "Young", "King", "White", "Wright", "Taylor", "Lee", "Lewis", "Hall", "Nguyen", "Kim", "Patel", "Wong", "Chen", "Taylor", "Clark", "Walker", "Hall", "Martin", "Zhang", "Johnson", "Lee", "Wang", "Garcia", "Davis", "Rodriguez", "Martinez", "Hernandez", "Smith"};
    }

    public enum Hobbies { Reading, Painting, Gaming, Cooking, Gardening, Traveling, Photography, Coding, Music, Sports, WatchingMovies, Fishing, Dancing, Hiking, Crafting, Yoga, Chess, Collecting, Writing, Meditation, Surfing }


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
