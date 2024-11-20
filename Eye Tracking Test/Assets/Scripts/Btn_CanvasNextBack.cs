using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_CanvasNextBack : MonoBehaviour
{
    [SerializeField] bool isBackBtn = false;

    [SerializeField] private CanvasManager manager;

     public void click()
    {
        int x = 1;
        if (isBackBtn) { x = -1; }

        manager.changeSection(manager.curSection + x);
    }
}
