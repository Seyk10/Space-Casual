using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [Header("Angle at which the camera looks at the player")]
    [SerializeField] float Angle;
    [Header("How far behind the player the camera will follow")]
    [SerializeField] float Distance;
    [Header("Delay between Player and Camera movement")]
    [Range(1, 10)]
    [SerializeField] float FollowSpeed;
    [Space(20)]
    [Header("Assign Follow Target Here")]
    [SerializeField] Transform Spaceship;
    
    Vector3 TargetPosition;

    void Update()
    {
        Vector3 PointA;
        Vector3 PointB;
        Vector3 PointC;

        float Y_Offset = Distance * Mathf.Tan(Angle);

        PointA = Spaceship.position;                                   //Ship's position
        PointB = Spaceship.position - (Spaceship.forward * Distance);  //Position behind the player
        PointC = PointB + (Spaceship.up * Y_Offset);                   //Vertical offset from player

        TargetPosition = PointC;    //Set the camera's target position

        Debug.DrawLine(PointA, PointB, Color.green);                   //For Drawing lines in the Editor
        Debug.DrawLine(PointB, PointC, Color.blue);                    //For Drawing lines in the Editor
        Debug.DrawLine(PointC, PointA, Color.red);                     //For Drawing lines in the Editor
        
        //Keeps the camera centered on the player, movement is on a delay
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Spaceship.position - transform.position, Spaceship.transform.up), FollowSpeed * Time.deltaTime); 

        //Smoothly move the camera to it's target position, movement is on a delay
        Vector3 Refrence = Vector3.zero;
        transform.position = Vector3.Lerp(transform.position, TargetPosition, FollowSpeed * Time.deltaTime);
    }
}
