using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructionBehavior : MonoBehaviour
{
    public DestructionProfile profile;  //Destruction Profile will be a scriptableObject, holding damage models, particles, and data.
    float FuelAdd;
    float ScoreAdd;
    bool Destroyed;

    ParticleSystem ContactEffect;
    ParticleSystem RubbleEffect;

    [SerializeField] private UnityEvent OnBreakDown;

    void Start()
    {
        FuelAdd = profile.FuelValue;
        ScoreAdd = profile.ScoreValue;
    }
    public void ApplyContact(Vector3 contactPoint)
    {
        ParticleSystem contact = Instantiate(ContactEffect, contactPoint, Quaternion.identity);
        contact.Play();
    }
    public void Crumble()
    {
        Instantiate(profile.DestructionModel, transform.position, transform.rotation);
        Instantiate(RubbleEffect, transform.position, transform.rotation);
        OnBreakDown.Invoke();
    }
}

