using UnityEngine;

public class OrbitalMovement : MonoBehaviour
{
    enum MovementScheme { Orbital, FreeFlight };
    [SerializeField] MovementScheme MovementType;

    [SerializeField] private Planet Planet;                      //Planets hold data about their size and position
    Vector3 PlanetPosition;
    float PlanetRadius;

    Transform PlayerOrigin;

    [Header("Altitude Controls")]
    [Tooltip("Current distance between the player and the planet's surface")]
    [SerializeField] private float Altitude;
    [Tooltip("How close to the planet's surface can the player go?")]
    [SerializeField] private float MinimumAltitude;
    [Tooltip("How far from the planet's surface can the player go before leaving the planet?")]
    [SerializeField] private float MaximumAltitude;

    [Header("Speed Controls")]
    [SerializeField] private float MinimumSpeed;
    [SerializeField] private float MaximumSpeed;
    [Tooltip("How fast the player can turn (Left/Right)")]
    [SerializeField] private float TurnSpeed;

    InputReader _input;

    public bool PlanetAssigned;
    public bool InOrbit;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<InputReader>();                        //Access Input data, Processed by a InputReader class
        PlayerOrigin = transform.parent.GetComponent<Transform>();  //Get a refrence to the player's pivot point
        if (Planet != null)
        {
            MovementType = MovementScheme.Orbital;
            PlanetRadius = Planet.GetRadius();                          //Store the current planet's radius
            PlanetPosition = Planet.GetPosition();                      //Store the current planet's position
            MinimumAltitude = Planet.GetMinAltitude();
            MaximumAltitude = Planet.GetMaxAltitude();
            transform.position = Vector3.zero + (-transform.forward * (PlanetRadius + MinimumAltitude));  //If on a planet, move the player to it's surface
        }
        else
        {
            MovementType = MovementScheme.FreeFlight;
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (MovementType)    //Trigger certian functions based on the current movement scenerio
        {
            case MovementScheme.Orbital:
                GetAltitude();            //Tracks Altitude
                ApplyInput();             //Process player movement
                ApplyBaseSpeed();         //Apply Base movement speed
                break;
            case MovementScheme.FreeFlight:
                ApplyInput();             //Process player movement
                ApplyBaseSpeed();         //Apply Base movement speed
                break;
        }
    }
    void ApplyBaseSpeed()
    {
        float Circumfrance = 2 * Mathf.PI * (PlanetRadius);
        float OneDegree = Circumfrance / 360;
        float Rotation = (MinimumSpeed * OneDegree);
        if (MovementType == MovementScheme.Orbital)
        {
            PlayerOrigin.RotateAround(PlayerOrigin.position, transform.right, (Rotation * Time.deltaTime));
        }
        else
        {
            PlayerOrigin.transform.position += transform.forward * MinimumSpeed * (10 * Time.deltaTime);
        }
    }
    public float GetAltitude()
    {
        Physics.Raycast(transform.position, (PlanetPosition - transform.position).normalized, out RaycastHit AltitudeCheck);
        if (InOrbit)
        {
            Altitude = AltitudeCheck.distance;
        }
        else
        {
            Altitude = Mathf.Abs(Vector3.Distance(transform.position, PlanetPosition) - PlanetRadius);
        }
        Debug.DrawRay(transform.position, (PlanetPosition - transform.position), Color.green);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, AltitudeCheck.normal) * transform.rotation, Time.deltaTime);

        return AltitudeCheck.distance;
    }
    public void ChangeAltitude(float delta)
    {
        delta *= -1;
        Vector3 AltitudeChange = new Vector3(0, delta, 0);

        if (Altitude < MinimumAltitude && delta > 0) delta = 0;

        transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, PlayerOrigin.position, delta), 10f * Time.deltaTime);
    }
    public void PlayerTurn(float delta)
    {
        transform.Rotate(new Vector3(0, delta, 0) * TurnSpeed * (10 * Time.deltaTime));
    }
    void ChangePitch(float delta)
    {
        delta *= -1;
        if (Mathf.Abs(delta) > 0)
        {
            PlayerOrigin.transform.Rotate(delta, 0, 0);
            PlayerOrigin.Rotate(transform.right.normalized * delta, 0, 0);
        }
    }
    public void ApplyInput()
    {
        if (_input.GetMovement() != Vector2.zero)
        {
            switch (MovementType)
            {
                case MovementScheme.Orbital:
                    ChangeAltitude(-_input.GetMovement().y);
                    PlayerTurn(_input.GetMovement().x);
                    break;
                case MovementScheme.FreeFlight:
                    ChangePitch(-_input.GetMovement().y);
                    PlayerTurn(_input.GetMovement().x);
                    break;
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Planet>())
        {
            Planet = collision.GetComponent<Planet>();
            Vector3 Temporary = PlayerOrigin.position;
            PlayerOrigin.position = Planet.GetPosition();
            transform.position = Temporary;

            PlanetRadius = Planet.GetRadius();
            PlanetPosition = Planet.GetPosition();
            MinimumAltitude = Planet.GetMinAltitude() - PlanetRadius;
            MaximumAltitude = Planet.GetMaxAltitude() - PlanetRadius;
            MovementType = MovementScheme.Orbital;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<Planet>())
        {
            Planet = null;

            MovementType = MovementScheme.FreeFlight;
            PlayerOrigin.position = transform.position;
            transform.localPosition = Vector3.zero;
        }
    }
}
