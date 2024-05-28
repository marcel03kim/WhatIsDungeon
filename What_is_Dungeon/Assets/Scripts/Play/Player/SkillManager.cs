using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    public void FireSkill(int level)
    {
        switch (level)
        {
            case 2:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 1:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 0:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
        }
    }

    public void IceSkill(int level)
    {
        switch (level)
        {
            case 2:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 1:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 0:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
        }
    }
    public void WindSkill(int level)
    {
        switch (level)
        {
            case 2:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 1:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 0:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
        }
    }
    public void LightningSkill(int level)
    {
        switch (level)
        {
            case 2:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 1:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case 0:
                GameObject.FindGameObjectsWithTag("Enemy");
                break;
        }
    }
}