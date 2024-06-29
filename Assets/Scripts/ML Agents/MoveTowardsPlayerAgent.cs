using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//mlagents-learn --run-id=idname

public class MoveTowardsPlayerAgent : Agent
{
  Rigidbody2D rBody;
 // [SerializeField] private Color winColor;
  //[SerializeField] private Color loseColor;
  //[SerializeField] private SpriteRenderer wall1;
  //[SerializeField] private SpriteRenderer wall2;
 // [SerializeField] private SpriteRenderer wall3;
  //[SerializeField] private SpriteRenderer wall4;
  

  void Start() {
        rBody = GetComponent<Rigidbody2D>();
    }

    public Transform Player;
    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(Random.Range(-1.5f,0.1f), Random.Range(-0.7f, 0.5f), 0 );
        Player.localPosition = new Vector3(Random.Range(-1.5f,0.1f), Random.Range(-0.7f, 0.5f), 0 );
    }

    public override void CollectObservations(VectorSensor sensor)
{
    // Target and Agent positions
    sensor.AddObservation(Player.localPosition);
    sensor.AddObservation(this.transform.localPosition);

    // Agent velocity
    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.y);
}
    public float forceMultiplier = 10f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        float moveX = actionBuffers.ContinuousActions[0];
        float moveY = actionBuffers.ContinuousActions[1];
        float moveSpeed = .5f;
        transform.localPosition += new Vector3(moveX, moveY,0f) * Time.deltaTime * moveSpeed;


        /*
        Debug.Log(actionBuffers.ContinuousActions[0]);
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToPlayer = Vector3.Distance(this.transform.localPosition, Player.localPosition);

        // Reached target
        if (distanceToPlayer < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        */

        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController playerController)){
           // wall1.color = winColor;
           // wall2.color = winColor;
           // wall3.color = winColor;
           // wall4.color = winColor;
            SetReward(+1f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)){
          //  wall1.color = loseColor;
           // wall2.color = loseColor;
           // wall3.color = loseColor;
           // wall4.color = loseColor;
            SetReward(-1f);
            EndEpisode();
        }
       
    }

    public override void Heuristic (in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    
    
}
