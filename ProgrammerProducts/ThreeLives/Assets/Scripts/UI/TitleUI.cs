using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    string _nextScene;

    [SerializeField]
    GameObject _waitPushGroup;
    [SerializeField]
    GameObject _mainMenuGroup;
    [SerializeField]
    GameObject _languageSelectGroup;

    enum MenuState { WaitPush, MainMenu, LanguageSelect }
    MenuState _menuSatate = MenuState.WaitPush;
    private void Start()
    {
        UpdateUI();
    }
    void Update()
    {
        switch (_menuSatate)
        {
            case MenuState.WaitPush:
                if (Input.anyKey)
                {
                    _menuSatate = MenuState.MainMenu;
                    UpdateUI();
                }
                break;
            case MenuState.MainMenu:
                break;
            case MenuState.LanguageSelect:
                break;
        }
    }
    public void UpdateUI()
    {
        _waitPushGroup.SetActive(false);
        _mainMenuGroup.SetActive(false);
        _languageSelectGroup.SetActive(false);
        switch (_menuSatate)
        {
            case MenuState.WaitPush:
                _waitPushGroup.SetActive(true);
                break;
            case MenuState.MainMenu:
                _mainMenuGroup.SetActive(true);
                _mainMenuGroup.GetComponentInChildren<Button>().Select();
                break;
            case MenuState.LanguageSelect:
                _languageSelectGroup.SetActive(true);
                _languageSelectGroup.GetComponentInChildren<Button>().Select();
                break;
        }
    }
    public void SetMenuStateByMenuObject(GameObject menuObject)
    {
        if (menuObject == _waitPushGroup)
        {
            _menuSatate = MenuState.WaitPush;
        }
        else if (menuObject == _mainMenuGroup)
        {
            _menuSatate = MenuState.MainMenu;
        }
        else if (menuObject == _languageSelectGroup)
        {
            _menuSatate = MenuState.LanguageSelect;
        }
        UpdateUI();
    }
    public void StartGame()
    {
        SceneTransition.Instance.LoadNextScene(_nextScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
