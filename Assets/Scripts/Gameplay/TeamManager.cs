using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] List<Team> teamList = new List<Team>();

    private void Awake()
    {
        Instance = this;

        SetupTeams();
    }

    void SetupTeams()
    {
        for (int i = 0; i < teamList.Count; i++)
        {
            teamList[i].Setup(i);
        }
    }

    public Color GetTeamColor(int teamId)
    {
        if(teamId < 0 || teamId >= teamList.Count)
        {
            Debug.LogError("Team ID out of range");
            return Color.white;
        }
        return teamList[teamId].Color;
    }

    public void JoinTeam(int teamId, GameObject obj)
    {
        if (teamId < 0 || teamId >= teamList.Count)
        {
            Debug.LogError("Team ID out of range");
            return;
        }

        obj.transform.position = teamList[teamId].SpawnPoint.position;
        obj.transform.rotation = teamList[teamId].SpawnPoint.rotation;
    }

    [System.Serializable]
    public class Team
    {
        public int Id { get; private set; }
        [SerializeField] string name; public string Name { get => name; }
        [SerializeField] Color color; public Color Color { get=>color; }
        [SerializeField] Transform spawnPoint; public Transform SpawnPoint { get => spawnPoint; }
        public int Score { get; private set; }

        public void Setup(int id) { Id = id; }

        public void AddScore(int amount = 1)  { Score += amount; }
        public void RemoveScore(int amount = 1) { Score -= amount;}
        public void ResetScore() { Score = 0; }
    }

    public static TeamManager Instance { get; private set; }
}
