using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour
{
    [Tooltip("The ShipMesh's Rigidbody")]
    public Rigidbody Rigidbody;
    [Tooltip("Player Input")]
    PlayerInput _PlayerInput;
    [Tooltip("Spaceship's Animator")]
    public Animator Animator;
    [Tooltip("Line Renderer, Ram indicator")]
    public LineRenderer Line;

    public float MoveSpeed;
    public float TurnSpeed;

    public float RamSpeed;
    public float RamForce;
    public float RamDuration;
    public float RamDamage;
    public float KnockBack;

    public float Health;
    float MaxHealth;

    public float Shield;
    float MaxShield;

    public float Resources;


    Vector2 Move_Input;
    Vector2 Speed_Input;

    public bool Ramming;

    void Update()
    {
        Ship_Yaw(Move_Input.x);   //Turns Ship Left/Right
        Ship_Pitch(Move_Input.y); //Tilts Ship Up/Down
        MoveShip_Z(Speed_Input.y); //Moves Ship Forward
        
        if (Ramming)
        {
            if(Physics.Raycast(transform.position, Rigidbody.transform.forward, out RaycastHit RammedInto, 1f))
            {
                if (RammedInto.collider.GetComponent<Building>())
                {
                    Debug.Log(RammedInto.collider.gameObject.name);
                    RammedInto.collider.GetComponent<Building>().DamageModel(RamDamage);
                }
            }
        }

    }
    
    float Pitch;
    public float PitchSpeed = 0.5f;
    public float MaxPitch = 60;      //You can tune pitch controls here if you want to.
    public float MinPitch = -45;
    public void Ship_Pitch(float input)
    {
        if (Mathf.Abs(input) > 0)
        {
            Pitch += input * PitchSpeed;
            Pitch = Mathf.Clamp(Pitch, MinPitch, MaxPitch);
            Rigidbody.transform.rotation = Quaternion.Euler(new Vector3(Pitch, Rigidbody.transform.rotation.eulerAngles.y, Rigidbody.transform.rotation.eulerAngles.z));
        }
        else
        {
            Pitch = Mathf.Lerp(Pitch, 0f, 1f * Time.deltaTime);      //Levels the ship if there is no pitch input
        }
    }
    public void Ship_Yaw(float input)
    {
        input *= TurnSpeed;
        Rigidbody.transform.Rotate(new Vector3(0f, input * TurnSpeed, 0f), Space.World);
    }
    public void MoveShip_XY(Vector2 input) 
    {
        Vector3 Movement = new Vector3(input.x, 0, input.y);
        Movement *= MoveSpeed;
        if(Rigidbody.velocity.magnitude < RamSpeed) Rigidbody.AddRelativeForce(Movement, ForceMode.Force);
    }

    public void MoveShip_Z(float input)
    {
        Rigidbody.AddRelativeForce(new Vector3(0,0,input), ForceMode.Force);
    }
    public void Ram() //Trigger Directly from input event.
    {
        StartCoroutine(RamCycle());
    }
    IEnumerator RamCycle()
    {
        Vector3 PreRamVelocity;
        PreRamVelocity = Rigidbody.velocity;
        
        //Line renderer, indicator stuff.
        Line.enabled = true;
        Line.SetPosition(0, Rigidbody.transform.position);
        Line.SetPosition(1, Rigidbody.transform.position + (Rigidbody.transform.transform.forward * RamSpeed));

        Rigidbody.AddRelativeForce(0, 0, RamSpeed, ForceMode.VelocityChange);   //Boost the ship forward
        
        Ramming = true;
        yield return new WaitForSecondsRealtime(RamDuration);   //Maintains the "Ramming" state for the duration of the Ram Cycle.
        Ramming = false;

        Line.enabled = false;
        Rigidbody.velocity = Rigidbody.velocity/4;  //This is my very Non-Scientific way of slowing the player down after Ramming

    }

    //Reading inputs from PlayerInput events
    public void Movement_Input(InputAction.CallbackContext input)
    {
        Move_Input = input.ReadValue<Vector2>();
    }
    public void SpeedControl_Input(InputAction.CallbackContext input)
    {
        Speed_Input = input.ReadValue<Vector2>();
    }

    void OnCollisionEnter(Collision collision)  //Check for destruction during collisions
    {
        if (Ramming)
        {
            if (collision.gameObject.CompareTag("Destructable"))
            {
                collision.gameObject.GetComponent<Building>().DamageModel(RamDamage);
                collision.rigidbody.AddExplosionForce(RamForce, collision.GetContact(0).point, 1f, 1f, ForceMode.Impulse);
                Rigidbody.AddForce(-Rigidbody.transform.forward * KnockBack, ForceMode.Impulse);
            }
        }
    }

}
