using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] float Angle;
    [SerializeField] float Distance;
    [SerializeField] float Delay;
    [SerializeField] Transform Spaceship;
    Vector3 TargetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Spaceship = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        float Y_Offset = Distance * ((180 - (Angle + 90)) / 90);
        TargetPosition = (Spaceship.position - transform.position) + new Vector3(0, Y_Offset, -Distance);
        TargetPosition += Spaceship.position;

        Debug.DrawLine(transform.position, transform.position + (-transform.forward * Distance));
        Debug.DrawLine(transform.position, transform.position + (transform.up * Y_Offset));
        Debug.DrawLine(transform.position + (-transform.forward * Distance), transform.position + (transform.up * Y_Offset));
        transform.rotation = Quaternion.Euler(Angle,0,0);
        if(Mathf.Abs((transform.position - TargetPosition).magnitude) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Delay * Time.deltaTime);
        }
    }
}
