using Puzventure.Omer;
using UnityEngine;
 

public class KeyItem : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Collider triggerCollider;
    private bool isPickedUp = false;
    private Spinner spinner;
    private Bobber bobber;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        triggerCollider = GetComponent<Collider>();
        spinner = GetComponent<Spinner>();
        bobber = GetComponent<Bobber>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.PickUpKey(this);
                isPickedUp = true;
                triggerCollider.enabled = false;

                if (spinner != null) spinner.enabled = false;
                if (bobber != null) bobber.enabled = false;

                transform.SetParent(player.transform);
                transform.localPosition = new Vector3(0f, 2f, 0f);
                transform.localRotation = Quaternion.identity; 
            }
        }
    }

    public void ResetToPad()
    {
        isPickedUp = false;
        transform.SetParent(null); 
        transform.position = startPosition; 
        transform.rotation = startRotation;
        triggerCollider.enabled = true; 

        if (spinner != null) spinner.enabled = true;
        if (bobber != null) bobber.enabled = true;
    }
}