using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlinker : MonoBehaviour
{
	[SerializeField] private MeshRenderer indicatorLight;
	[SerializeField] private Material matOn, matOff;
	[SerializeField] private GameObject light;
	[SerializeField] private float blinkSpeed;

    void Start()
    {
		StartCoroutine(Blink());
    }

	private IEnumerator Blink()
	{
		indicatorLight.material = matOn;
		if (light != null) light.SetActive(true);

		yield return new WaitForSeconds(blinkSpeed);

		indicatorLight.material = matOff;
		if (light != null) light.SetActive(false);

		yield return new WaitForSeconds(blinkSpeed);

		StartCoroutine(Blink());
	}
}
