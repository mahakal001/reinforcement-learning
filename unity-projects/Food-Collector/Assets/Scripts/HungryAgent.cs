using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HungryAgent : Agent
{
    public GameObject Food;
    Rigidbody rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        Food.transform.localPosition = new Vector3(Random.value * 6, 0.5f, Random.value * 6);
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(Random.value * 6,0.5f, Random.value * 6);


        

    }

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    base.CollectObservations(sensor);


    //    sensor.AddObservation(Food.transform.localPosition);
    //    sensor.AddObservation(this.transform.localPosition);
        

    //    //agent velocity
    //    sensor.AddObservation(rBody.velocity.x);
    //    sensor.AddObservation(rBody.velocity.z);

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            AddReward(1.0f);
            Debug.Log("Food Collected");

            Debug.Log("Agent Position:- " + this.transform.localPosition.ToString());
            Debug.Log("Food Position:- " + Food.transform.position.ToString());
            EndEpisode();
            
        } else if(collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1.0f / MaxStep);
            Debug.Log("Hitted Wall");
        }
    }

    public float AgentSpeed = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        base.OnActionReceived(actionBuffers);

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var act = actionBuffers.DiscreteActions;
        var action = act[0];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }

        this.transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        rBody.AddForce(dirToGo *AgentSpeed,ForceMode.VelocityChange);

        AddReward(-1.0f/MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }
}