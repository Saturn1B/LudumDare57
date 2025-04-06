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
	public AudioSource submarineEngineNoise;
	bool isPlayingNoise;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		characterMovement = GetComponent<CharacterMovement>();
		submarineController = GetComponentInParent<SubmarineController>();
	}

	InteractionObject currentObject;

	private void Update()
	{
		RaycastHit hit;

		Debug.DrawRay(playerCamera.position, playerCamera.forward * playerReach, Color.magenta, 1);
		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach, layerMask))
		{
			Debug.Log(hit.collider.transform.name);
			InteractionObject interact = hit.collider.transform.GetComponent<InteractionObject>();
			currentObject = interact;
			if (Input.GetKey(KeyCode.E))
			{
				interact._interactionEvent.Invoke();
				interact.transform.localPosition = new Vector3(0, -0.015f, 0);
				if (!isPlayingNoise)
				{
					StartCoroutine(FadeIn(submarineEngineNoise, .25f, 1f));
					isPlayingNoise = true;
				}
			}
			else
			{
				interact.transform.localPosition = new Vector3(0, 0, 0);
				submarineController.StopAllMovement();
				if (isPlayingNoise)
				{
					StartCoroutine(FadeIn(submarineEngineNoise, .25f, .3f));
					isPlayingNoise = false;
				}
			}
		}
		else
		{
			if (currentObject == null) return;

			currentObject.transform.localPosition = new Vector3(0, 0, 0);
			submarineController.StopAllMovement();
			if (isPlayingNoise)
			{
				StartCoroutine(FadeIn(submarineEngineNoise, .25f, .3f));
				isPlayingNoise = false;
			}
			currentObject = null;
		}
	}

	IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume = 1f)
	{
		float currentPitch = audioSource.pitch;
		float currentTime = 0f;
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.pitch = Mathf.Lerp(currentPitch, targetVolume, currentTime / duration);
			yield return null;
		}

		audioSource.pitch = targetVolume;
	}
}
