using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGoal : MonoBehaviour
{
    [SerializeField] int team;

    [SerializeField] GameObject[] visuals;

    private void Start()
    {
        SetupVisuals();
    }

    void SetupVisuals()
    {
        foreach (GameObject visual in visuals)
        {
            visual.GetComponent<MeshRenderer>().sharedMaterial = new Material(visual.GetComponent<MeshRenderer>().sharedMaterial);  
            visual.GetComponent<MeshRenderer>().sharedMaterial.color = TeamManager.Instance.GetTeamColor(team);
        }
    }
}
