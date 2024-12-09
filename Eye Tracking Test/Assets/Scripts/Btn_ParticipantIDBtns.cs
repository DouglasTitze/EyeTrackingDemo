using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_ParticipantIDBtns : MonoBehaviour
{
    [SerializeField] DataManager manager;
    public void clickNumber()
    {
        string name = gameObject.name;

        if (name == "del")
        {
            if (manager.participantID.Length != 0)
            {
                manager.participantID = manager.participantID.Substring(0, manager.participantID.Length - 1);
            }
        }
        else
        {
            manager.participantID += gameObject.name;
        }
    }
}