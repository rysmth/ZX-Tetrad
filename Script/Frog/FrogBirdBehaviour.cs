using UnityEngine;

// Ryan Joshua Smith Frog Bird Behaviour script for Frog micro-game 2026

public class FrogBirdBehaviour : FrogBaseAnimal // Inheritance implementation
{
    private void Awake() // Encapsulation implementation
    {
        movementSpeed = 3;
    }

    private void Update()
    {
        MoveForward(); // Inherited function example from base animal script
    }

    public override void MoveForward() // Polymorphism implementation 
    {
        // This would be where implementation of differences would occur but it's still just using the function of base animal with a message to demonstrate

        //Debug.Log("A Bird is Flying");

        base.MoveForward();
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