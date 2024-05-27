using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    const int LEFT_MOUSE_BUTTON = 0;
    const int RIGHT_MOUSE_BUTTON = 1;
    const int MIDDLE_MOUSE_BUTTON = 2;

    [SerializeField] Transform target;
    Ray lastRay;

    void Update() {
        if (Input.GetMouseButton(RIGHT_MOUSE_BUTTON)) {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor() {
        NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) {
            Debug.LogError("No NavMeshAgent found");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (!hasHit) {
            return;
        }
        navMeshAgent.destination = hit.point;
    }

    private void UpdateAnimator() {
        NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) {
            Debug.LogError("No NavMeshAgent found");
            return;
        }
        Animator animator = this.GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("No Animator found");
            return;
        }

        Vector3 globalVelocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }
}
