using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
	//public int objectDetected;
	public HashSet<Collider> colliderDetected = new HashSet<Collider>();

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Submarine"))
			colliderDetected.Add(other);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Submarine"))
			colliderDetected.Remove(other);
	}
}
