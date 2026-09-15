using UnityEngine;

// Ryan Joshua Smith Frog Insect Behaviour script for Frog micro-game 2026

public class FrogInsectBehaviour : FrogBaseAnimal // Inheritance
{
    private void Awake() // Encapsulation
    {
        movementSpeed = 2;
    }

    private void Update()
    {
        MoveForward(); // Inherited function example from base animal script
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //  Prevents duplicates from slowing performance due to amount in scene, by removing them
        if (collision.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}