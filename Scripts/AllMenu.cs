using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject rules;
    [SerializeField] GameObject[] rulesBackground;

    private int rulesBacgroundNuvmer = 0;
    public void OnRestartButton()
    {
        // Восстанавливаем время
        Time.timeScale = 1f;

        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void OnNextLevelButton()
    {
        Time.timeScale = 1f;
        // Загружаем следующий уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFirstLevelButton()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnSecondLevelButton()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void OnThirdLevelButton()
    {
        SceneManager.LoadScene("Level_3");
    }

    public void OnRulesButton()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            rulesBacgroundNuvmer = 0;
            rulesBackground[0].SetActive(true);
            rulesBackground[1].SetActive(false);
            rulesBackground[2].SetActive(false);
            rules.SetActive(true);

        }
        else
        {
            menu.SetActive(true);
            rules.SetActive(false);
        }
    }

    public void OnLeftButton()
    {
        rulesBackground[rulesBacgroundNuvmer].SetActive(false);
        rulesBackground[--rulesBacgroundNuvmer].SetActive(true);
    }

    public void OnRightButton()
    {
        rulesBackground[rulesBacgroundNuvmer].SetActive(false);
        rulesBackground[++rulesBacgroundNuvmer].SetActive(true);
    }

    public void OnSelectLevelButton()
    {
        if (menu) {
            menu.SetActive(false);
            levelSelect.SetActive(true);

        }
    }

    public void OnBack()
    {
        if (menu) {
            menu.SetActive(true);
            levelSelect.SetActive(false);
        }
    }

    public void OnContinueButton()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>().SetControl(true);
        GameObject.FindGameObjectWithTag("Background").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Background").transform.GetChild(3).gameObject.SetActive(false);
    }


    public void OnExitButton()
    {
        Debug.Log("пїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅ...");

        // В редакторе останавливаем проигрывание
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // В собранной версии закрываем приложение
        Application.Quit();
        #endif
    }
}
