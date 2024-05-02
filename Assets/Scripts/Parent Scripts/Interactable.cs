using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Parent Properties")]
    public bool pickupable = false;
    [SerializeField] private float interactionCooldown = 0;
    [SerializeField] private bool oneTimeInteraction = false;

    private float cooldownTimeStamp = 0;
    private bool hasBeenInteractedWith = false;

    [HideInInspector] public bool isPickedUp;

    private void Update()
    {
        Debug.Log(isPickedUp);      
    }

    public virtual void interaction()
    {
        
    }

    protected bool canInteract()
    {
        if (cooldownTimeStamp + interactionCooldown > Time.time || hasBeenInteractedWith == true)
            return false;

        if (oneTimeInteraction == true)
            hasBeenInteractedWith = true;

        cooldownTimeStamp = Time.time;

        return true;
    }
}
