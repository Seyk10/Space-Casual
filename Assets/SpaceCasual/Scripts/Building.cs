using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float Health;
    public float Resource;

    public Material Full_Health;
    public Material Half_Health;
    public Material No_Health;

    Renderer renderer;
    float Judge_Health;

    public bool CanTakeDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        Judge_Health = Health/2;
    }
    public void DamageModel(float Damage)
    {
        if (CanTakeDamage) {
            StartCoroutine(TakeDamage(Damage));
        }
    }
    IEnumerator TakeDamage(float Damage)
    {
        CanTakeDamage = false;
        Health -= Damage;

        if (Health > Judge_Health)
        {
            renderer.material = Full_Health;
        }
        if (Health <= Judge_Health)
        {
            renderer.material = Half_Health;
            Debug.Log("Building Entered Damaged State");
        }
        if(Health <= 0)
        {
            renderer.material = No_Health;
            Destroy(gameObject, 0.75f);
            Debug.Log("Building has been Destroyed");
        }
        yield return new WaitForSecondsRealtime(0.5f);
        CanTakeDamage = true;
    }

}
