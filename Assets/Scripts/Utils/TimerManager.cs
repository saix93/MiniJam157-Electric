using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviourSingleton<TimerManager>
{
    private static List<Timer> timers = new List<Timer>();
    public List<Timer> TimersCopy;
    
    public static IReadOnlyList<Timer> Timers => timers.AsReadOnly();

    protected override void OnDestroy()
    {
        base.OnDestroy();

        ClearTimers();
    }

    void Update()
    {
        TimersCopy = new List<Timer>(timers);
        foreach (var timer in TimersCopy)
        {
            if (!timer.UpdateManually) timer.Tick(Time.deltaTime);
        }
    }

    public static void AddTimer(Timer timer)
    {
        timers.Add(timer);
    }

    public static void ClearTimers()
    {
        timers.Clear();
    }
}
