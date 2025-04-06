using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
	[SerializeField] private GameObject winPanel, loosePanel;
	[SerializeField] private CharacterTarget characterTarget;
	bool win, loose;

	public void Win()
	{
		if (loose) return;

		win = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		StopEngineNoise();

		winPanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void GameOver()
	{
		if (win) return;

		loose = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		StopEngineNoise();

		loosePanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void MainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void Retry()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	private void StopEngineNoise()
	{
		characterTarget.submarineEngineNoise.pitch = .3f;
	}
}
