using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayArea : MonoBehaviour
{
    public RobotAgent robotAgent;
    public GameObject goal; //We dont have a code for Goal
    public Kill killPrefab;
    private List<GameObject> killList;
    public TextMeshPro cumulativeReward;


    //Reset function of the PlayArea 
    public void ResetArea()
    {
        RemoveAllenemies();
        PutRobot();
        PutGoal();
        GenerateEnemies(15, .5f);
    }

    private void GenerateEnemies(int num, float botspeed)
    {
        for (int i=0;i<num;i++)
        {
            GameObject botObject = Instantiate<GameObject>(killPrefab.gameObject);
            botObject.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 2f, 15f); //let it spawn everywhere
            botObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f); // bot can face any direction
            botObject.transform.SetParent(transform);
            killList.Add(botObject);
            botObject.GetComponent<Kill>().botSpeed = botspeed;
        }
       
    }

    private void PutGoal()
    {
        Rigidbody rigidbody = robotAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        goal.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0, 9f) + Vector3.up * .5f;
        goal.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f); //only rotate along the y axis

    }

    private void PutRobot()
    {
        Rigidbody rigidbody = robotAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        //robotAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0, 9f) + Vector3.up ;
        //robotAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f); //only rotate along the y axis
        robotAgent.transform.localPosition = new Vector3(0, 1f, 0);
        robotAgent.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);

    }

    private void RemoveAllenemies()
    {
        if (killList != null)
        {
            for (int i =0; i<killList.Count;i++)
            {
              if (killList[i]!=null)
                {
                    Destroy(killList[i]);
                }
            }
        }
        killList = new List<GameObject>();
    }


    public void RemoveKilled(GameObject killedBot)
    {   
        // remove the enemy bot if it is killed by the robot
        killList.Remove(killedBot);
        Destroy(killedBot);
    }

    public int TotalAlive
    {
        get { return killList.Count; }
    }


    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius;
        float angle;
        radius = UnityEngine.Random.Range(minRadius, maxRadius);
        angle = UnityEngine.Random.Range(minAngle, maxAngle);
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    private void Start()
    {
        ResetArea();
    }
    private void Update()
    {
        // Update the cumulative reward text
        cumulativeReward.text = robotAgent.GetCumulativeReward().ToString("0.00");
    }
}