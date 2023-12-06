using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp 
{
    public int year;
    public enum Season
    {
        Spring,
        Summer, 
        Fall,
        Winter
    }
    public Season season;

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

    public GameTimestamp(int year, Season season, int day, int hour, int minute )
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    //Creating a new instance of a GameTimestamp from another pre-existing one
    public GameTimestamp(GameTimestamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;
    }

    //Time increase by 1 minute
    public void UpdateClock()
    {
        minute++;

        //60 mins/hour
        if (minute >= 60)
        {
            //Reset
            minute = 0;
            hour++;
        }

        //24 hours/day
        if (hour >= 24)
        {
            //Reset
            hour = 0;
            day++;
        }

        if (day >= 30)
        {
            //Reset
            day = 1;
            if(season == Season.Winter)
            {
                season = Season.Spring;
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
        //Total days passed
        int daysPassed = YearsToDays( year ) + SeasonsToDays(season) + day;

        int dayIndex = daysPassed % 7;
        return (DayOfTheWeek)dayIndex;
    }

    //Convert
    public static int HoursToMinutes(int hour)
    {
        return hour * 60;
    }

    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int year)
    {
        return year * 4 * 30;
    }

    public static int DaysToHours(int day)
    {
        return day * 24;
    }

    public static int CompareTimestamp(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        //Convert to hours
        int timestamps1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamps2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;

        int difference = timestamps2Hours - timestamps1Hours;
        return Mathf.Abs(difference);
    }
}
