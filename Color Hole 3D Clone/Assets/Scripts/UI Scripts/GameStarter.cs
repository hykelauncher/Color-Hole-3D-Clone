using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStarter : MonoBehaviour, IPointerDownHandler
{
    private TextMeshProUGUI _levelText;

    private void Start()
    {
        _levelText = LevelController.Instance.gameUi.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _levelText.text = "LEVEL " + PlayerPrefs.GetInt("Level", PlayerPrefs.GetInt("Level" + 1));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LevelController.Instance.didLevelStarted = true;
        gameObject.SetActive(false);
        _levelText.gameObject.SetActive(false);
        LevelController.Instance.firstStageBar.gameObject.SetActive(true);
        LevelController.Instance.secondStageBar.gameObject.SetActive(true);
    }
}
