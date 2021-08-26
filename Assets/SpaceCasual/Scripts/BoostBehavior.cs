using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostBehavior : MonoBehaviour
{
    [SerializeField] float SpeedIncrease;
    [SerializeField] private UnityEvent OnTrigger;
    InputReader _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<InputReader>();                        //Access Input data, Processed by a InputReader class
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
