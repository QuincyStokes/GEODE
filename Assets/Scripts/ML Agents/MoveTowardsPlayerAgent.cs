using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//mlagents-learn --run-id=idname

public class MoveTowardsPlayerAgent : Agent
{
    private Rigidbody2D rBody;
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    /*
    [SerializeField] private SpriteRenderer wall1;
    [SerializeField] private SpriteRenderer wall2;
    [SerializeField] private SpriteRenderer wall3;
    [SerializeField] private SpriteRenderer wall4;
        */
    public override void Initialize() {
        rBody = GetComponent<Rigidbody2D>();
    }

    public GameObject Player;
    public override void OnEpisodeBegin() {
        //transform.localPosition = new Vector3(Random.Range(-1.5f,2f), Random.Range(-0.25f, 0.5f), 0 );
        //Player.transform.localPosition = new Vector3(Random.Range(-0.5f,1f), Random.Range(-0.55f, -1.5f), 0 );
    }

    public override void CollectObservations(VectorSensor sensor) {

        // Target and Agent positions
        sensor.AddObservation(Player.transform.localPosition);
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


        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToPlayer = Vector3.Distance(this.transform.localPosition, Player.transform.localPosition);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController playerController)){
            /*
            wall1.color = winColor;
            wall2.color = winColor;
            wall3.color = winColor;
            wall4.color = winColor;
            SetReward(+3f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)){
            wall1.color = loseColor;
            wall2.color = loseColor;
            wall3.color = loseColor;
            wall4.color = loseColor;
            SetReward(-1f);
            EndEpisode();
        }
        */
        }
       
    }

    public override void Heuristic (in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
        continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
    }
    
    
}
