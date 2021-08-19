using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


[System.Serializable]
public class FuelSystem : MonoBehaviour
{
    [SerializeField] public float InitialValue;
    [SerializeField] private float MaximumValue;
    [SerializeField] private UnityEvent OnReachingZero;

    bool TimerFinished;
    public float CurrentValue;
    GameObject Scaler;
    void Start()
    {
        Mathf.Clamp(MaximumValue, InitialValue, Mathf.Infinity);
        Mathf.Clamp(InitialValue, 0, MaximumValue);
        Scaler = GameObject.FindGameObjectWithTag("Fuel Scaler");
        CurrentValue = InitialValue;
        Scaler.transform.localScale = new Vector3(MaximumValue / CurrentValue, 1, 1);
    }

    void FixedUpdate()
    {
        if (CurrentValue == 0) return;         //If the timer reached 0, then it needs to stop counting down to avoid triggering the event again

        if (CurrentValue > 0)                                                               //Checking if the timer reached 0
        {
            CurrentValue -= Time.fixedDeltaTime;                                            //Subtracting from the timer
            Scaler.transform.localScale = new Vector3((CurrentValue/MaximumValue), 1, 1);   //Setting the progress bar's size
        }
        else
        {
            TimerFinished = true;       // If the timer reached 0, change the boolean in order to stop the timer.
            CurrentValue = 0;
            OnReachingZero.Invoke();    // Triggering the "timer finished" event.
            Debug.Log("Timer Finished, all out of fuel :(");
        }
    }
    public void AddFuel(float amount)
    {
        CurrentValue += amount;
    }
    public void LoseFuel(float amount)
    {
        CurrentValue -= amount;
    }
    public float GetFuel()
    {
        return CurrentValue;
    }
    public float GetInital()
    {
        return InitialValue;
    }
    public void SetInital(float value)
    {
        InitialValue = value;
    }
    public float GetMaximum()
    {
        return MaximumValue;
    }
    public void SetMaximum(float value)
    {
        MaximumValue = value;
    }

}
