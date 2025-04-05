using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
	public int objectDetected;

	private void OnTriggerEnter(Collider other)
	{
		if(!other.CompareTag("Submarine"))
			objectDetected++;
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Submarine"))
			objectDetected--;
	}
}
