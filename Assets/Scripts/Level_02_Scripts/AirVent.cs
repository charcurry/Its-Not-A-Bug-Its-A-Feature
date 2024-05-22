using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    private Vector3 gustOrigin;
    private float playerCompensation;
    private bool isOn;
    private AudioSource audioSource;

    [Header("References")]
    [SerializeField] private ParticleSystem gustParticle;

    [Header("Properties")]
    [SerializeField] private float gustForce = 10;
    [SerializeField] private float airHeight = 10;
    [SerializeField] private bool isOnAtStart = true;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        isOn = isOnAtStart;

        if (isOn)
        {
            Debug.Log(audioSource);
            audioSource.Play();
            gustParticle.Play();
        }

        gameObject.GetComponent<CapsuleCollider>().height = airHeight;
        gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, (airHeight / 2) - 0.3f, 0);

        gustOrigin = transform.parent.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOn || other.gameObject.CompareTag("NailProjectile"))
            return;

        // Adds player compensation for the upword force equal to thier garvityMultplier
        if (other.gameObject.CompareTag("Player"))
            playerCompensation = other.GetComponent<PlayerController>().gravityMultiplier;
        else
            playerCompensation = 1;

        // Applies a upword force on any object in the trigger that gets weaker the further up the object is
        if (other.gameObject.CompareTag("Player") || other.GetComponent<Rigidbody>() && !other.GetComponent<Interactable>().isPickedUp)
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Clamp(playerCompensation * gustForce * (20 - Vector3.Distance(other.transform.position, gustOrigin)), Physics.gravity.y * playerCompensation, 20 * playerCompensation), ForceMode.Acceleration);
        }
    }

    public void TurnOn()
    {
        if (!isOnAtStart)
        {
            isOn = true;
            audioSource.Play();
            gustParticle.Play();
        }
    }

    public void TurnOff()
    {
        if (!isOnAtStart)
        {
            isOn = false;
            audioSource.Stop();
            gustParticle.Stop();
        }
    }
}
