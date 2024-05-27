using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    
    [SerializeField] Transform target;
    Ray lastRay;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MoveToCursor();
        }
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        //this.GetComponent<NavMeshAgent>().destination = target.position;
    }

    private void MoveToCursor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit) {
            this.GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}
