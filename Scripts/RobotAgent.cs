using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class RobotAgent : Agent
{
    public float FowardSpeed = 7f;
    public float turnSpeed = 180f;

    [Tooltip("Prefab of the killed bot")]
    public GameObject Rekilled;

    private PlayArea playArea;
    new private Rigidbody rigidbody;
    private GameObject destination;
    private bool isFull; // If true, penguin has a full stomach

    //Let us initialize the robot attacker
    public override void Initialize()
    {
        base.Initialize();
        playArea = GetComponentInParent<PlayArea>();
        destination = playArea.goal;
        rigidbody = GetComponent<Rigidbody>();
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        // actions for moving forward and turning left and right
        float forwardQuantity = actions.DiscreteActions[0];
        //float backwardQuantity = actions.DiscreteActions[2];
        float turnQuantity = 0f;
        if (actions.DiscreteActions[1] == 1f)
        {
            turnQuantity = -1f;
        }
        else if (actions.DiscreteActions[1] == 2f)
        {
            turnQuantity = 1f;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardQuantity * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnQuantity * turnSpeed * Time.fixedDeltaTime);

        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // For manual control
        int forwardAction = 0;
        int turnAction = 0;
       
        // To be modified

        // Put the actions into the array
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
       
    }

    public override void OnEpisodeBegin()
    {
       
        playArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(destination.transform.position, transform.position));
        sensor.AddObservation((destination.transform.position - transform.position).normalized);
        sensor.AddObservation(transform.forward); //Agent facing direction
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("bot"))
        {
            KillBot(collision.gameObject);
        }
        else if (collision.transform.CompareTag("goal"))
        {
            ExecuteAlways();
        }
    }

    private void KillBot(GameObject killObject)
    {
        //Add a reward if a bot is killed
        playArea.RemoveKilled(killObject);
        AddReward(1f);
    }

    private void ExecuteAlways()
    {
        GameObject reKilledBot = Instantiate<GameObject>(Rekilled);
        reKilledBot.transform.parent = transform.parent;
        reKilledBot.transform.position = destination.transform.position;
        Destroy(reKilledBot,5f);

        AddReward(1f);

        if (playArea.TotalAlive<=0)
        {
            EndEpisode();
        }
    }

    private void Update()
    {   // If the ROBOT falls from the plane give a negative Reward
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
            AddReward(-.5f);
        }

    }
}
