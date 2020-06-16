using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDisplacement
{
    class LoadTestConstraint : MonoBehaviour
    {
        [SerializeField]
        private LayoutOptimizer optimizer;

        public enum ConstraintType
        {
            PAIRWISEDISTANCE,
            FOCALPT,
            TRAFFICLANE
        }
        [SerializeField]
        private ConstraintType t;

        // Pairwise distance constr
        public GameObject obj1;
        public GameObject obj2;

        // Focal point constr
        public GameObject focal;
        public List<GameObject> surroundings = new List<GameObject>();

        // Traffic lane constr
        public GameObject startingPoint;
        public Vector3 roadDirection;


        public float distance;
        public float stiffness;
        // Start is called before the first frame update
        void Start()
        {
            switch (t)
            {
                case ConstraintType.PAIRWISEDISTANCE:
                    optimizer.Constraints.Add(new PairwiseDistanceConstraint(obj1, obj2, stiffness, distance));
                    break;
                case ConstraintType.FOCALPT:
                    optimizer.Constraints.Add(new FocalPointDistanceConstraint(focal, surroundings, stiffness, distance));
                    break;
                case ConstraintType.TRAFFICLANE:
                    Constraint.Road road = new Constraint.Road();
                    road.StartingPoint = startingPoint;
                    road.Direction = roadDirection;
                    optimizer.Constraints.Add(new TrafficLaneConstraint(road, stiffness, distance));
                    break;
            }
        }
    }
}