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
	[SerializeField] private Transform warningBlinkerLight;
	[SerializeField] private Material blinkerOn, blinkerOff;
	[SerializeField] private LayerMask collisionLayer;
	[SerializeField] private GameUI gameUI;
	[SerializeField] private AudioSource collisionNoise;
	[SerializeField] private AudioSource warningNoise;
	[SerializeField] private AudioSource explosionNoise;
	[SerializeField] private Material[] damagedGlassMat;
	[SerializeField] private MeshRenderer glass;
	[SerializeField] private CameraController cameraController;
	private float blinkSpeed;
    private int currentLife;

	private void Awake()
	{
		currentLife = subMaxLife;
	}

	public void TakeDamage(int amount)
	{
		if (currentLife <= 0) return;

		cameraController.ShakeCamera(.1f, .05f);

		currentLife -= amount;
		if(currentLife <= 0)
		{
			currentLife = 0;
			explosionNoise.Play();
			gameUI.GameOver();
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
		{
			TakeDamage(1);
			collisionNoise.pitch = Random.Range(0.7f, 1.1f);
			collisionNoise.Play();
		}
	}

	private void UpdateLifeBar()
	{
		float ratio = (float)currentLife / (float)subMaxLife;
		int newRatio = Mathf.CeilToInt(lifeBars.Length * ratio);

		if(newRatio <= 4 & newRatio > 0)
			glass.material = damagedGlassMat[newRatio - 1];

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
		warningNoise.Stop();
		warningNoise.Play();
		warningBlinker.GetComponent<MeshRenderer>().material = blinkerOn;
		warningBlinkerLight.gameObject.SetActive(true);

		yield return new WaitForSeconds(blinkSpeed);

		warningBlinker.GetComponent<MeshRenderer>().material = blinkerOff;
		warningBlinkerLight.gameObject.SetActive(false);

		yield return new WaitForSeconds(blinkSpeed);

		StartCoroutine(Blinker());
	}
}
