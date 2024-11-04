using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArenaGoal : NetworkBehaviour
{
    [SerializeField] int team;

    [SerializeField] float animateHeight;
    [SerializeField] float animateSpeed;

    [SerializeField] GameObject[] visuals;

    [SerializeField] ParticleSystem goalParticles;

    GameObject ball;

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

    private void Update()
    {
        if (IsServer && ball != null)
        {
            if (AnimateBall())
            {
                ball.transform.position = TeamManager.Instance.GetBowlPoint(team).position;
                ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                GameManager.Instance.RespawnBall();
                ball = null;
            }
        }
    }

    bool AnimateBall()
    {
        ball.transform.position = Vector3.MoveTowards(ball.transform.position, transform.position + (Vector3.up * animateHeight), animateSpeed * Time.deltaTime);
    
        return Vector3.Distance(ball.transform.position , transform.position + (Vector3.up * animateHeight)) < 0.1f;
    }

    void OnBallTrigger(GameObject b)
    {
        ball = b;
        PlayGoalEffectRpc();
    }

    [Rpc(SendTo.Everyone)]
    void PlayGoalEffectRpc()
    {
        goalParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer && ball == null)
        {
            if (other.CompareTag("Ball"))
            {
                OnBallTrigger(other.gameObject);
                TeamManager.Instance.AddScoreRpc(team);
            }
        }
    }
}
