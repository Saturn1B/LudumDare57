using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectManager : MonoBehaviour
{
	private int totalCollectible;
	private int collected = 0;
	[SerializeField] private TMP_Text counterText;
	[SerializeField] private Indicator indicator;
	[SerializeField] private GameUI gameUI;
	[SerializeField] private AudioSource collectingNoise;

	private void Awake()
	{
		GetComponent<BoxCollider>().size = new Vector3(6, 6, 8) * indicator.detectionScale;

		GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
		totalCollectible = collectibles.Length;
		counterText.text = $"{collected} / {totalCollectible}";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Collectible"))
		{
			collectingNoise.pitch = Random.Range(0.85f, 1.1f);
			collectingNoise.Play();
			collected++;
			counterText.text = $"{collected} / {totalCollectible}";
			Destroy(other.gameObject);

			if(collected >= totalCollectible)
			{
				gameUI.Win();
			}
		}
	}
}
