using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostBehavior : MonoBehaviour
{
    [SerializeField] float SpeedIncrease;
    [SerializeField] float FuelCost;
    [SerializeField] private UnityEvent OnTriggerBoost;
    InputReader _input;
    FuelSystem Fuel;
    OrbitalMovement Player;
    [HideInInspector] public float FuelLevel;
    [HideInInspector] public bool Boosting;

    [SerializeField] private UnityEvent BoostCollision;



    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<InputReader>();                        //Access Input data, Processed by a InputReader class
        Fuel = GameObject.FindGameObjectWithTag("Fuel System").GetComponent<FuelSystem>();
        Player = GetComponent<OrbitalMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        FuelLevel = Fuel.CurrentValue;
        if (_input.GetBoost() && FuelLevel >= FuelCost)
        {
            StartCoroutine(BoostCycle());
        }

        if(!Boosting)
        {
            float SpeedWas = Player.CurrentSpeed;
            Player.CurrentSpeed = Mathf.Lerp(SpeedWas, Player.MinimumSpeed, Player.TimeToDecelerate);
        }
    }
    IEnumerator BoostCycle()
    {
        Boosting = true;
        Player.OnAccelerate.Invoke();
        Player.CurrentSpeed += SpeedIncrease;
        Player.CurrentSpeed = Mathf.Clamp(Player.CurrentSpeed, Player.MinimumSpeed, Player.MaximumSpeed);

        Fuel.LoseFuel(FuelCost);
        yield return new WaitForSeconds(Player.TimeToDecelerate);
        Boosting = false;
    }
    public void BoostedCollision()
    {
        BoostCollision.Invoke();
    }
}
