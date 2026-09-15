using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ryan Joshua Smith Claw Out Of bounds Script 2022

public class ClawOutOfBounds : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}