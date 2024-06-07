using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {

    public class PatrolPath : MonoBehaviour {

        [SerializeField] Color color = Color.red;

        private void OnDrawGizmos() {
            Gizmos.color = color;
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i);
                Gizmos.DrawSphere(GetWaypoint(i), 0.1f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int i) {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i) {
            return transform.GetChild(i).position;
        }
    }
}
