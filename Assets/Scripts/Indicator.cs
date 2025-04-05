using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
	[SerializeField] private Detector detector;
	[SerializeField] private Image indicatorLight;
	[SerializeField] private Color colorOff, colorOn;
	[SerializeField] private int blinkSpeed;

	bool isBlinking = false;

	private void Update()
	{
		if(detector.objectDetected > 0 && !isBlinking)
		{
			isBlinking = true;
			StartCoroutine(Blink(blinkSpeed));
		}
	}

	private IEnumerator Blink(int blinkSpeed)
	{
		indicatorLight.color = colorOn;

		yield return new WaitForSeconds(blinkSpeed / 10);

		indicatorLight.color = colorOff;

		yield return new WaitForSeconds(blinkSpeed / 10);

		if (detector.objectDetected <= 0)
			isBlinking = false;
		else
			StartCoroutine(Blink(blinkSpeed));
	}
}
