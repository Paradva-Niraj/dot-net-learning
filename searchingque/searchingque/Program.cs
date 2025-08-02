using System;
using System.Collections.Generic;

public abstract class Subject
{
    public string Name { get; set; }
    public double SpeedKmph { get; set; }
    public Subject(string name, double speedKmph)
    {
        Name = name;
        SpeedKmph = speedKmph;
    }
}

public class Car : Subject
{
    public Car(string name) : base(name, 80) { }
}

public class Human : Subject
{
    public Human(string name) : base(name, 5) { }
}

public class Animal : Subject
{
    public Animal(string name, double speedKmph) : base(name, speedKmph) { }
}

public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Location(double lat, double lng)
    {
        Latitude = lat;
        Longitude = lng;
    }
}

public static class SubjectSearch
{
    public static List<Location> PossibleLocations(Subject subject, Location lastSeen, DateTime lastSeenTime, DateTime currentTime)
    {
        double elapsedHours = (currentTime - lastSeenTime).TotalHours;
        double maxDistance = subject.SpeedKmph * elapsedHours;
        int directions = 8;
        double radiusInDegrees = maxDistance / 111.0;

        var locations = new List<Location>();
        for (int i = 0; i < directions; i++)
        {
            double angle = (2 * Math.PI / directions) * i;
            double newLat = lastSeen.Latitude + radiusInDegrees * Math.Cos(angle);
            double newLng = lastSeen.Longitude + radiusInDegrees * Math.Sin(angle) / Math.Cos(lastSeen.Latitude * Math.PI / 180);
            locations.Add(new Location(newLat, newLng));
        }
        return locations;
    }
}

class Program
{
    static void PrintLocations(string label, List<Location> locs)
    {
        Console.WriteLine(label);
        for (int i = 0; i < locs.Count; i++)
        {
            Console.WriteLine($"Direction {i + 1}: Lat {locs[i].Latitude:F5}, Lng {locs[i].Longitude:F5}");
        }
    }

    static void Main(string[] args)
    {
        var lastSeen = new Location(28.6139, 77.2090);
        DateTime now = DateTime.Now;
        DateTime lastSeenTime = now.AddHours(-2);

        Subject car = new Car("Red Hyundai");
        Subject human = new Human("John Doe");
        Subject animal = new Animal("Stray Dog", 12);

        var carLocs = SubjectSearch.PossibleLocations(car, lastSeen, lastSeenTime, now);
        var humanLocs = SubjectSearch.PossibleLocations(human, lastSeen, lastSeenTime, now);
        var animalLocs = SubjectSearch.PossibleLocations(animal, lastSeen, lastSeenTime, now);

        PrintLocations("Car Possible Locations:", carLocs);
        PrintLocations("Human Possible Locations:", humanLocs);
        PrintLocations("Animal Possible Locations:", animalLocs);
        Console.WriteLine("end");
    }
}
