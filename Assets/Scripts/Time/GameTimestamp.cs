using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public int year;
    public enum Seasons
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public Seasons season;

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    public int day;
    public int hour;
    public int minute;

    public GameTimestamp(int year, Seasons season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    //Creatiing a new instance of a GameTimestamp from another pre-existing one
    public GameTimestamp(GameTimestamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;
    }

    // Increments the time by 1 min
    public void UpdateClock()
    {
        minute++;

        //60 min/h
        if (minute >= 60)
        {
            //reset minutes
            minute = 0;
            hour++;
        }

        //24h/day
        if (hour >= 24)
        {
            //reset hour
            hour = 0;
            day++;
        }

        //30 days/season
        if (day >= 30)
        {
            //reset day
            day = 1;
            if (season == Seasons.Winter)
            {
                season = Seasons.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        //Total time to days
        int daysPassed = YearsToDays(year) + SeasonsToDays(season) + day;

        int dayIndex = daysPassed % 7;

        return (DayOfTheWeek)dayIndex;
    }

    public static int HoursToMinutes(int hours)
    {
        return hours * 60;
    }

    public static int DayToHour(int days)
    {
        return days * 24;
    }

    public static int SeasonsToDays(Seasons season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 30 * 4;
    }

    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    //Calculate the difference between 2 timestamps in hours
    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        //Convert to hours
        int timestamp1InHours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.day;
        int timestamp2InHours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.day;

        int difference = timestamp2InHours - timestamp1InHours;

        return Mathf.Abs(difference);
    }
}
