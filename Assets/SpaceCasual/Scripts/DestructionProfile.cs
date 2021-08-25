using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DestructionProfile", order = 1)]
public class DestructionProfile : ScriptableObject
{
    public GameObject DestructionModel;
    public float FuelValue;
    public float ScoreValue;

}
