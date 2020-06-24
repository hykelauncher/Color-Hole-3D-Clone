using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("UI Elements")] 
    public GameObject gameUi;
    public GameObject winUi; 
    public GameObject failUi;
    
    [Header("Buttons")] 
    public Button winButton; 
    public Button failButton;
    
    public List<GameObject> firstStageCubes;
    public List<GameObject> secondStageCubes;

    public static LevelController Instance;
    public bool didFirstStageEnded, didLevelEnded, canTranslate, didLevelStarted, isGameOver;

    public Slider firstStageBar, secondStageBar;
    
    private void Awake()
    {
        //In order to do an error check, I didnt use TryGetComponent and only used GetComponent
        firstStageCubes = GameObject.FindGameObjectsWithTag(Tags.FsCatchableCubeTag).ToList();
        secondStageCubes = GameObject.FindGameObjectsWithTag(Tags.SsCatchableCubeTag).ToList();
        if (Instance == null) Instance = this;
        gameUi = GameObject.FindGameObjectWithTag(Tags.GameUiTag);
        winUi = gameUi.transform.GetChild(1).gameObject;
        failUi = gameUi.transform.GetChild(2).gameObject;
        winButton = winUi.GetComponent<Button>();
        failButton = failUi.GetComponent<Button>();
        winButton.onClick.AddListener(LoadNextScene);
        failButton.onClick.AddListener(ReloadTheScene);
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.GetInt("Level", 0);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
        firstStageBar = gameUi.transform.GetChild(4).GetComponent<Slider>();
        firstStageBar.maxValue = firstStageCubes.Count;
        secondStageBar = gameUi.transform.GetChild(5).GetComponent<Slider>();
        secondStageBar.maxValue = secondStageCubes.Count;
    }

    private void Update()
    {
        if (!didFirstStageEnded && firstStageCubes.Count == 0)
        {
            canTranslate = true;
            didFirstStageEnded = true;
        }

        if (!didLevelEnded && secondStageCubes.Count == 0)
        {
            winUi.SetActive(true);
            didLevelEnded = true;
            canTranslate = true;
            secondStageBar.gameObject.SetActive(false);
            firstStageBar.gameObject.SetActive(false);
        }

        if (isGameOver && !failUi.activeInHierarchy)
        {
            failUi.SetActive(true);
            firstStageBar.gameObject.SetActive(false);
            secondStageBar.gameObject.SetActive(false);
        }
    }

    private void LoadNextScene()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void ReloadTheScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
