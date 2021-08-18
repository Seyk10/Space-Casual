using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Transform Transform;
    [SerializeField] float Radius;
    [SerializeField] float MaxAltitude;
    [SerializeField] float MinAltitude;

    SphereCollider[] sphereColliders; 
    // Start is called before the first frame update
    void Start()
    {
        Transform = GetComponent<Transform>();

        sphereColliders = transform.gameObject.GetComponents<SphereCollider>();
        CalculateBounding();
    }
    void Update()
    {
        Debug.DrawLine(transform.position, transform.up.normalized * Radius, Color.green);
        Debug.DrawLine(transform.position, (transform.up + transform.forward).normalized * MinAltitude, Color.yellow);
        Debug.DrawLine(transform.position, transform.forward.normalized * MaxAltitude, Color.red);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public float GetMaxAltitude()
    {
        CalculateBounding();
        return MaxAltitude;
    }
    public float GetMinAltitude()
    {
        CalculateBounding();
        return MinAltitude;
    }
    public float GetRadius()
    {
        return Radius;
    }
    void CalculateBounding()
    {
        List<float> extents = new List<float>();

        foreach (SphereCollider collider in sphereColliders)
        {
            float curr = collider.bounds.extents.x;
            extents.Add(curr);
        }
        for (int i = 0; i < extents.Count; i++)
        {
            for (int j = i + 1; j < extents.Count; j++)
            {
                if (extents[i] > extents[j])
                {
                    float temp = extents[i];
                    extents[i] = extents[j];
                    extents[j] = temp;
                }
            }
        }
        Radius = extents[0];
        MinAltitude = extents[1];
        MaxAltitude = extents[2];
    }
}
