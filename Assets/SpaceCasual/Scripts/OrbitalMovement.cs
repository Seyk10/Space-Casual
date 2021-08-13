using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMovement : MonoBehaviour
{
    [SerializeField] private Planet Planet;
    float PlanetRadius;
    Vector3 PlanetPosition;

    Transform PlayerOrigin;

    [Tooltip("Distance from the Player to the surface of the Planet")]
    [SerializeField] private float Altitude;
    [SerializeField] private float Minimum;
    [SerializeField] private float Maximum;

    bool PlanetAssigned;
    bool OnPlanet;
    // Start is called before the first frame update
    void Start()
    {
        PlayerOrigin = transform.parent.GetComponent<Transform>();
        PlanetRadius = Planet.GetRadius();
        PlanetPosition = Planet.GetPosition();

        transform.position = Vector3.zero + (-Vector3.forward * (PlanetRadius + Minimum));
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlanetStatus();
        if(!OnPlanet && PlanetAssigned)
        {
            transform.position = Vector3.Lerp(transform.position, (PlayerOrigin.position + transform.up * (PlanetRadius + Maximum)), 50f * Time.deltaTime);
        }
        GetAltitude();
    }
    void CheckPlanetStatus()
    {
        if (Planet == null) PlanetAssigned = false;

        if (Planet != null)
        {
            PlanetAssigned = true;
        }
    }
    public float GetAltitude()
    {
        Physics.Raycast(transform.position, (PlanetPosition - transform.position).normalized, out RaycastHit AltitudeCheck, PlanetRadius + Maximum);
        Altitude = AltitudeCheck.distance;
        Debug.Log(AltitudeCheck.distance);
        Debug.DrawRay(transform.position, (PlanetPosition - transform.position), Color.green);

        if (Altitude > Maximum)
        {
            OnPlanet = false;
        }

        return AltitudeCheck.distance;
    }
}
