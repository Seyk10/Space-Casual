using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] Vector2 _Movement;
    [Range(0,2)]
    [SerializeField] float X_Sensitivity;
    [Range(0, 2)]
    [SerializeField] float Y_Sensitivity;

    [SerializeField] bool Boost;

    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2 GetMovement()
    {
        Vector2 ProcessedMovement = Vector3.zero;
        ProcessedMovement.x = _Movement.x * X_Sensitivity;
        ProcessedMovement.y = _Movement.y * Y_Sensitivity;

        return ProcessedMovement;
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        _Movement = context.ReadValue<Vector2>();
    }
}
