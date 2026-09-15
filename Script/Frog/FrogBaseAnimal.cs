using UnityEngine;

// Ryan Joshua Smith Frog Base animal script for Frog micro-game 2026

public class FrogBaseAnimal : MonoBehaviour
{
    [Header("Prefab Behaviour Variables")]
    protected int movementSpeed; // Encapsulation

    public virtual void MoveForward() // Movement function for all animals as an Abstraction example, avoids repetitive code
    {
        // Move's the prefab right each frame, depending on the rotation of the spawner so local not world 
        transform.position += transform.right * movementSpeed * Time.deltaTime;
    }
}