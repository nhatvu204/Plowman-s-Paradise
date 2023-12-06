using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set;}

    [Header("Internal clock")]
    [SerializeField]
    GameTimestamp timestamp;

    public float timeScale = 1.0f;

    [Header("Day/Night cycle")]
    //Transformation of Directional Light (Sun)
    public Transform sunTransform;

    //List of Objects to inform of changes to time
    List<ITimeTracker> listeners = new List<ITimeTracker>();

    private void Awake()
    {
        //If there is more than 1 instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the timestamp
        timestamp = new GameTimestamp(0, GameTimestamp.Seasons.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
       while (true)
        {
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }
    }

    //A tick of the in-game time
    public void Tick()
    {
        timestamp.UpdateClock();

        //Inform each listeners of the new time state
        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();
    }

    //Day/Night cycle 
    void UpdateSunMovement()
    {
        //Current time to minutes
        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //Get the timestamp
    public GameTimestamp GetGameTimestamp()
    {
        //Return a cloned instance
        return new GameTimestamp(timestamp);
    }


    //Handling listeners
    //Add the object to the list of listeners
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    //Remove the object from the list of listeners
    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
