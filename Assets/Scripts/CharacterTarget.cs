using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTarget : MonoBehaviour
{
    private Transform playerCamera;
    private CharacterMovement characterMovement;
    [SerializeField] private int playerReach;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		characterMovement = GetComponent<CharacterMovement>();
	}

	private void Update()
	{
		RaycastHit hit;

		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach))
		{
			if (hit.transform.GetComponent<InteractionObject>())
			{
				Debug.Log("HIT");
				if (Input.GetKey(KeyCode.E))
				{
					InteractionObject interact = hit.transform.GetComponent<InteractionObject>();
					interact._interactionEvent.Invoke();
				}
			}
		}
	}
}
