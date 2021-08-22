using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Dog : Animal
{
    public Dog()
    {
        this._name = "Dog";
        this.speed = 1f;
    }

    public void Sit()
    {
        animator.SetBool("Sit_b", true);
    }

    public void Stand()
    {
        animator.SetBool("Sit_b", false);
    }
}

