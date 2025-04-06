using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLife : MonoBehaviour
{
    [SerializeField] private int subMaxLife;
    [SerializeField] private Transform[] lifeBars;
    [SerializeField] private Material matOn, matOff;
    [SerializeField] private float slowBlinkSpeed, fastBlinkSpeed;
	[SerializeField] private Transform warningBlinker;
	[SerializeField] private Material blinkerOn, blinkerOff;
	[SerializeField] private LayerMask collisionLayer;
	private float blinkSpeed;
    private int currentLife;

	private void Awake()
	{
		currentLife = subMaxLife;
	}

	public void TakeDamage(int amount)
	{
		currentLife -= amount;
		if(currentLife <= 0)
		{
			currentLife = 0;
			//TO DO GameOver
			//return;
		}
		else if(currentLife == 2)
		{
			blinkSpeed = slowBlinkSpeed;
			StartCoroutine(Blinker());
		}
		else if(currentLife == 1)
		{
			StopAllCoroutines();
			blinkSpeed = fastBlinkSpeed;
			StartCoroutine(Blinker());
		}

		UpdateLifeBar();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
			TakeDamage(1);
	}

	private void UpdateLifeBar()
	{
		float ratio = (float)currentLife / (float)subMaxLife;
		int newRatio = Mathf.CeilToInt(lifeBars.Length * ratio);

		for (int i = 0; i < lifeBars.Length; i++)
		{
			if (i < newRatio)
				lifeBars[i].GetComponent<MeshRenderer>().material = matOn;
			else
				lifeBars[i].GetComponent<MeshRenderer>().material = matOff;
		}
	}

	private IEnumerator Blinker()
	{
		warningBlinker.GetComponent<MeshRenderer>().material = blinkerOn;

		yield return new WaitForSeconds(blinkSpeed);

		warningBlinker.GetComponent<MeshRenderer>().material = blinkerOff;

		yield return new WaitForSeconds(blinkSpeed);

		StartCoroutine(Blinker());
	}
}
