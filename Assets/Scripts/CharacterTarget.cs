using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTarget : MonoBehaviour
{
    private Transform playerCamera;
    private CharacterMovement characterMovement;
	private SubmarineController submarineController;
    [SerializeField] private int playerReach;
    [SerializeField] private LayerMask layerMask;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		characterMovement = GetComponent<CharacterMovement>();
		submarineController = GetComponentInParent<SubmarineController>();
	}

	private void Update()
	{
		RaycastHit hit;

		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach, layerMask))
		{
			Debug.Log("Hit: " + hit.collider.name + " on layer " + hit.collider.gameObject.layer);
			InteractionObject interact = hit.collider.transform.GetComponent<InteractionObject>();
			if (Input.GetKey(KeyCode.E))
			{
				interact._interactionEvent.Invoke();
			}
			else
			{
				submarineController.StopAllMovement();
			}
		}
	}
}
