using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted { get; private set; }

    [SerializeField] private Transform cameraHolder, cameraMain;
    [SerializeField] private GameObject dialog1, dialog2, dialog3;
    [SerializeField] private GameObject mainCanvas;

    private void Awake()
    {
        isGameStarted = false;
        mainCanvas.SetActive(false);
        cameraMain.GetComponent<Camera>().fieldOfView = 30;
        StartCoroutine(ShowDialog());
    }

    private IEnumerator ShowDialog()
    {
        dialog1.SetActive(true);

        yield return new WaitForSeconds(1);

        dialog1.SetActive(false);
        dialog2.SetActive(true);

        yield return new WaitForSeconds(2);

        dialog2.SetActive(false);
        dialog3.SetActive(true);

        StartCoroutine(RotateCamera());
    }

    private IEnumerator RotateCamera()
    {
        Camera main = cameraMain.GetComponent<Camera>();
        while(main.fieldOfView < 60)
        {
            main.fieldOfView += Time.deltaTime * 100;
            yield return 0;
        }

        main.fieldOfView = 60;

        float duration = .5f;
        var startRotation1 = cameraHolder.rotation;
        var startRotation2 = cameraMain.rotation;

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            cameraHolder.rotation = Quaternion.Slerp(startRotation1, Quaternion.identity, timer / duration);
            cameraMain.rotation = Quaternion.Slerp(startRotation2, Quaternion.identity, timer / duration);
            yield return 0;
        }

        mainCanvas.SetActive(true);
        isGameStarted = true;
    }
}
