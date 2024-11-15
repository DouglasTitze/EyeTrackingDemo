using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasButton : MonoBehaviour
{
    [SerializeField] bool isBackBtn = false;

    [SerializeField] private CanvasTextManager manager;

     public void click()
    {
        int x = 1;
        if (isBackBtn) { x = -1; }

        manager.changeSection(manager.curSection + x);
    }
}
