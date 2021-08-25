using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionBehavior : MonoBehaviour
{
    public GameObject profile;  //Destruction Profile will be a scriptableObject, holding damage models, particles, and data.
    float FuelAdd;
    float ScoreAdd;
    bool Destroyed;

    ParticleSystem ContactEffect;
    ParticleSystem RubbleEffect;

    void Start()
    {
        FuelAdd = profile.Fuel;
        ScoreAdd = profile.Score;
    }
    public void ApplyContact(Vector3 contactPoint)
    {
        ParticleSystem contact = Instantiate(ContactEffect, contactPoint, Quaternion.identity);
        contact.Play();
    }
    public void Crumble()
    {
        Instantiate(profile.Destroyed, transform.position);
        Instantiate(RubbleEffect, transform.position, Quaternion.identity);
    }
}

