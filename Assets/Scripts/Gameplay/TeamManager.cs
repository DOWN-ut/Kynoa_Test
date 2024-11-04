using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamManager : NetworkBehaviour
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

    [Rpc(SendTo.Everyone)]
    public void AddScoreRpc(int teamId, int amount = 1)
    {
        if (teamId < 0 || teamId >= teamList.Count)
        {
            Debug.LogError("Team ID out of range");
            return;
        }

        teamList[teamId].AddScore(amount);
    }

    public Transform GetBowlPoint(int teamId)
    {
        if (teamId < 0 || teamId >= teamList.Count)
        {
            Debug.LogError("Team ID out of range");
            return null;
        }
        return teamList[teamId].BowlPoint;
    }

    public int GetTeamScore(int teamId)
    {
        if (teamId < 0 || teamId >= teamList.Count)
        {
            Debug.LogError("Team ID out of range");
            return 0;
        }

        return teamList[teamId].Score;
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
        [SerializeField] Transform bowlPoint; public Transform BowlPoint { get => bowlPoint; }
        public int Score { get; private set; }

        public void Setup(int id) { Id = id; }

        public void AddScore(int amount = 1)  { Score += amount; }
        public void RemoveScore(int amount = 1) { Score -= amount;}
        public void ResetScore() { Score = 0; }
    }

    public static TeamManager Instance { get; private set; }
}
