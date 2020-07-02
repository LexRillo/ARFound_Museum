using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MenuManagerScript : MonoBehaviour
{
    public Transform mainMenugui;
    public Transform settingsgui;
    public Transform visualizationgui;
    public Transform interactionsgui;

    // Update is called once per frame
    //void Update()
    //{
    //    if()
    //}
    public void openSettings()
    {
        mainMenugui.gameObject.SetActive(false);
        settingsgui.gameObject.SetActive(true);
    }

    public void settingsToMain()
    {
        settingsgui.gameObject.SetActive(false);
        mainMenugui.gameObject.SetActive(true);
    }

    public void openVisualization()
    {
        mainMenugui.gameObject.SetActive(false);
        visualizationgui.gameObject.SetActive(true);
    }

    public void visualizationToMain()
    {
        visualizationgui.gameObject.SetActive(false);
        mainMenugui.gameObject.SetActive(true);
    }

    public void openInteractions()
    {
        mainMenugui.gameObject.SetActive(false);
        interactionsgui.gameObject.SetActive(true);
    }

    public void interactionsToMain()
    {
        interactionsgui.gameObject.SetActive(false);
        mainMenugui.gameObject.SetActive(true);
    }
}
