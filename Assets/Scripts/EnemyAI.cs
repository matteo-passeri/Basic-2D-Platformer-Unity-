﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
    // What to chase
    public Transform target;

    // How many times each second we will update our path
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    // the calculated path
    public Path path;

    // The AI's speed per second
    public float speed = 300f;
    public ForceMode2D forceMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            // search for player
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            return;
        }
        // Start a new path to the target position, and return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            // search for player
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            yield return false;
        }
        else
        {

            // Start a new path to the target position, and return the result to the OnPathComplete method
            seeker.StartPath(transform.position, target.position, OnPathComplete);

            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }
    }

        public void OnPathComplete(Path p)
        {
            // Debug.Log("We have a path...we have an error? " + p.error);
            
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        void FixedUpdate()
        {
            if (target == null)
            {
                // search for player
                if (!searchingForPlayer)
                {
                    searchingForPlayer = true;
                    StartCoroutine(SearchPlayer());
                }
                return;
            }

            // TODO: Always look at player

            if (path == null)
            {
                return;
            }

            // if currentWaypoint is as long as the total path (we reached the end of the path)
            if (currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                {
                    return;
                }

                // Debug.Log("End of path reached");

                pathIsEnded = true;
                return;
            }
            pathIsEnded = false;

            // Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;

            // Move the AI
            rb.AddForce(dir, forceMode);

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

            if (dist < nextWaypointDistance)
            {
                currentWaypoint++;
                return;
            }
        }

        IEnumerator SearchPlayer()
        {
            GameObject searchResultPlayerTarget = GameObject.FindGameObjectWithTag("Player");
            if (searchResultPlayerTarget != null)
            {
                target = searchResultPlayerTarget.transform;
                searchingForPlayer = false;
                StartCoroutine(UpdatePath());
                yield return false;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SearchPlayer());
            }
        }

    }
