using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalActionMenu : MonoBehaviour
{
    public Animal animal;
    [SerializeField] Canvas menu;

    public void Awake()
    {
    }

    public void btnEat_Click()
    {
        animal.Eat();
    }

    public void btnWalk_Click()
    {
        animal.Walk();
    }

    public void btnSound_Click()
    {
        animal.Speak();
    }

    public bool hidden
    {
        get
        {
            return menu.enabled;
        }
        set
        {
            menu.enabled = value;
        }
    }
}
