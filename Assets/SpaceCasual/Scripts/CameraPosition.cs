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
        Vector3 PointA;
        Vector3 PointB;
        Vector3 PointC;

        float Y_Offset = Distance * Mathf.Tan(Angle);

        PointA = Spaceship.position;
        PointB = Spaceship.position - (Spaceship.forward * Distance);
        PointC = PointB + (Spaceship.up * Y_Offset);

        TargetPosition = PointC;

        Debug.DrawLine(PointA, PointB, Color.green);
        Debug.DrawLine(PointB, PointC, Color.blue);
        Debug.DrawLine(PointC, PointA, Color.red);

        transform.rotation = Quaternion.LookRotation(Spaceship.position - transform.position, Spaceship.transform.up);

        if(Mathf.Abs((transform.position - TargetPosition).magnitude) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Delay * Time.deltaTime);
        }
    }
}
