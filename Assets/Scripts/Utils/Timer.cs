using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Timer
{
    public string Name { get; private set; }
    public float Duration { get; private set; }
    public float OriginalDuration { get; private set; }
    public float Current { get; private set; }
    public bool IsDone { get; private set; }
    public bool IsRunning { get; private set; }

    public bool RunIndefinitely { get; set; }
    public bool UpdateManually { get; set; }
    public Func<bool> TickCondition { get; set; }
    public Action<float> OnTick { get; set; }
    public Action OnComplete { get; set; }

    public bool IsReady => !IsRunning || IsDone;
    public float T => Current / Duration;
    public float Remaining => Duration - Current;

    public Timer(float newDuration = 0f, bool startInstantly = false, bool newRunIndefinitely = false, Action onComplete = null, Action<float> onTick = null, Func<bool> newTickCondition = null, string timerName = "")
    {
        SetDuration(newDuration, true);
        RunIndefinitely = newRunIndefinitely;
        TickCondition = newTickCondition;
        OnTick = onTick;
        OnComplete = onComplete;

        if (startInstantly)
        {
            Start();
        }
        else
        {
            IsDone = true;
        }
        
        var method = new StackFrame(1, false).GetMethod();
        var className = method.DeclaringType != null ? method.DeclaringType.Name : "";
        Name = $"{className} - {method.Name} ({timerName})";
        
        TryToCreateUtilEventsObject();
    }

    private void TryToCreateUtilEventsObject()
    {
        if (TimerManager.Instance == null)
        {
            var eventUpdaterGo = new GameObject($"Timer Manager");
            var timerManager = eventUpdaterGo.AddComponent<TimerManager>();
            timerManager.NewInstanceSustitutesOldOne = true;
        }
        
        TimerManager.AddTimer(this);
    }

    /// <summary>
    /// Empieza el timer, si ya ha empezado, lo reinicia
    /// </summary>
    public void Start(float newDuration = 0f, bool setAsOriginal = false)
    {
        if (newDuration > 0f) SetDuration(newDuration, setAsOriginal);

        Current = 0f;
        IsDone = false;
        IsRunning = true;
    }
    /// <summary>
    /// Para el timer
    /// </summary>
    public void Stop()
    {
        IsRunning = false;
    }
    /// <summary>
    /// Continua el timer, sin reiniciarlo
    /// </summary>
    public void Continue()
    {
        IsRunning = true;
    }
    /// <summary>
    /// Reinicia el timer y lo para
    /// </summary>
    public void Reset(bool resetDurationToOriginal = false)
    {
        if (resetDurationToOriginal) Duration = OriginalDuration;
        Current = 0f;
        IsDone = false;

        Stop();
    }
    /// <summary>
    /// Actualiza la duracion del timer
    /// </summary>
    public void SetDuration(float newDuration, bool setAsOriginal = false)
    {
        Duration = newDuration;
        if (setAsOriginal) OriginalDuration = Duration;
    }

    /// <summary>
    /// Tick del timer
    /// </summary>
    /// <param name="deltaTime">Tiempo entre ticks</param>
    public void Tick(float deltaTime)
    {
        if (!IsRunning) return;
        if (IsDone) return;
        if (TickCondition != null && !TickCondition.Invoke()) return;

        OnTick?.Invoke(deltaTime);

        Current += deltaTime;
        if (Current >= Duration)
        {
            OnComplete?.Invoke();
            IsDone = true;
            IsRunning = false;

            if (RunIndefinitely)
            {
                Reset();
                Start();
            }
        }
    }
}
