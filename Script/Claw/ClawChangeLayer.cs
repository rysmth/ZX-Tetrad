using UnityEngine;

// Ryan Joshua Smith Claw Once Item Is Grabbed Script 2022

public class ClawChangeLayer : MonoBehaviour
{
    private GameObject grabbedObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        else
        {
            ChangeLayer(collision.gameObject);
        }
    }

    private void ChangeLayer(GameObject obj)
    {
        grabbedObject = obj;

        int carriedLayer = LayerMask.NameToLayer("Grabbed Object"); // Simply changes layer of object collided with to a layer that does not interact with platform

        obj.layer = carriedLayer;
    }
}