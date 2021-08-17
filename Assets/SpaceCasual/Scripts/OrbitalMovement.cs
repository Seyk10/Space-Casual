using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMovement : MonoBehaviour
{
    [SerializeField] private Planet Planet;
    Vector3 PlanetPosition;
    float PlanetRadius;

    Transform PlayerOrigin;

    [Header("Altitude Controls")]
    [SerializeField] private float Altitude;
    [SerializeField] private float MinimumAltitude;
    [SerializeField] private float MaximumAltitude;

    [Header("Speed Controls")]
    [Tooltip("Speeds are representitive of surface speeds in meters per second")]
    [SerializeField] private float MinimumSpeed;
    [SerializeField] private float MaximumSpeed;
    [SerializeField] private float TurnSpeed;

    InputReader _input;

    bool PlanetAssigned;
    bool OnPlanet;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<InputReader>();
        PlayerOrigin = transform.parent.GetComponent<Transform>();
        PlanetRadius = Planet.GetRadius();
        PlanetPosition = Planet.GetPosition();

        transform.position = Vector3.zero + (-Vector3.forward * (PlanetRadius + MinimumAltitude));
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlanetStatus();
        GetAltitude();

        ApplyInput();

        ApplyBaseSpeed();
    }
    void ApplyBaseSpeed()
    {
        float Circumfrance = 2* Mathf.PI * (PlanetRadius + Altitude);
        float OneDegree = Circumfrance / 360;
        float Rotation = (MinimumSpeed * OneDegree);
        PlayerOrigin.Rotate(new Vector3(Rotation, 0, 0) * (10 * Time.deltaTime));
    }
    void CheckPlanetStatus()
    {
        if (Planet == null) PlanetAssigned = false;

        if (Planet != null)
        {
            PlanetAssigned = true;
            PlanetRadius = Planet.GetRadius();
            PlanetPosition = Planet.GetPosition();
            if (PlayerOrigin.position != PlanetPosition)
            {
                PlayerOrigin.position = PlanetPosition;
            }
        }
        else
        {
            OnPlanet = true;
        }
        
        if (!OnPlanet && PlanetAssigned)
        {
            transform.position = Vector3.Lerp(transform.position, (PlanetPosition + (transform.up * (PlanetRadius + MaximumAltitude-.5f))), 10f * Time.deltaTime);
        }
    }
    public float GetAltitude()
    {
        if (PlanetAssigned == false) return -1f;

        Physics.Raycast(transform.position, (PlanetPosition - transform.position).normalized, out RaycastHit AltitudeCheck);
        Altitude = AltitudeCheck.distance;
        Debug.DrawRay(transform.position, (PlanetPosition - transform.position), Color.green);

        if (Altitude >= MaximumAltitude)
        {
            OnPlanet = false;
        }
        else
        {
            OnPlanet = true;
        }

        return AltitudeCheck.distance;
    }
    public void ChangeAltitude(float delta)
    {
        delta *= -1;
        Vector3 AltitudeChange = new Vector3(0, delta, 0);

        if (Altitude < MinimumAltitude && delta > 0) delta = 0;
        else if (Altitude > MaximumAltitude && delta < 0) delta = 0;

        if(OnPlanet) transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, PlayerOrigin.position, delta), 10f * Time.deltaTime);
    }
    public void PlayerTurn(float delta)
    {
        PlayerOrigin.Rotate(new Vector3(0, 0, -delta) * TurnSpeed * (10 * Time.deltaTime));
    }
    public void ApplyInput()
    {
        if(_input.GetMovement() != Vector2.zero)
        {
            ChangeAltitude(-_input.GetMovement().y);

            PlayerTurn(_input.GetMovement().x);
        }

    }
}
