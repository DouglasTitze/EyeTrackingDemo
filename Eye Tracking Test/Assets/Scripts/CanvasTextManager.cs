using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable] public class Sections
{
    public string Header;

    [TextArea(5, 10)]
    public string Text;
}

public class CanvasTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Button btn_back;
    [SerializeField] private Button btn_next;
    [SerializeField] private GameObject cubes;
    [SerializeField] private CanvasEyeInteractable eyeInteractableScript;
    [Space]
    [SerializeField] private List<Sections> TextSections = new List<Sections>();
    
    [HideInInspector] public int curSection = 0;
    private int numberOfSections;
    private void Start()
    {
        numberOfSections = TextSections.Count;
        changeSection(curSection);
    }

    public void changeSection(int sectionNum)
    {
        if (sectionNum < numberOfSections)
        {
            // Update canvas text and curSection
            curSection = sectionNum;
            Header.text = TextSections[curSection].Header;
            Text.text = TextSections[curSection].Text;

            // Update buttons to avoid going out of bounds
            updateButtons();

            // Check if the cubes section is being displayed
            toggleCubes();

            // Check if the canvas section is being displayed
            toggleCanvasTimerSection();
        }
    }

    private void updateButtons()
    {
        if (curSection == numberOfSections - 1)
        {
            btn_next.interactable = false;
            btn_back.interactable = true;
        }
        else if (curSection == 0)
        {
            btn_back.interactable = false;
            btn_next.interactable = true;
        }
        else
        {
            btn_back.interactable = true;
            btn_next.interactable = true;
        }
    }

    private void toggleCubes()
    {
        if (curSection == 2)
        {
            cubes.SetActive(true);
        }
        else
        {
            cubes.SetActive(false);
        }
    }

    private void toggleCanvasTimerSection()
    {
        if (curSection == 3) { eyeInteractableScript.isSectionEnabled = true; }
        else { eyeInteractableScript.isSectionEnabled = false; }
        
    }
}
