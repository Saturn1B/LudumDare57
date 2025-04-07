using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlinker : MonoBehaviour
{
	[SerializeField] private MeshRenderer indicatorLight;
	[SerializeField] private Material matOn, matOff;
	[SerializeField] private GameObject lightSource;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private float blinkSpeed;

    void Start()
    {
		StartCoroutine(Blink());
    }

	private IEnumerator Blink()
	{
		indicatorLight.material = matOn;
		if (lightSource != null) lightSource.SetActive(true);
		if (audioSource != null) audioSource.Play();

		yield return new WaitForSeconds(blinkSpeed);

		indicatorLight.material = matOff;
		if (lightSource != null) lightSource.SetActive(false);

		yield return new WaitForSeconds(blinkSpeed);

		StartCoroutine(Blink());
	}
}
