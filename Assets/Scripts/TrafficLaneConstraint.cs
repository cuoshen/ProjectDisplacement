using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectDisplacement
{
    class TrafficLaneConstraint : Constraint
    {
        
        private Road road;
        private float distance;

        public TrafficLaneConstraint(Road road, float stiffness, float distance)
        {
            this.road = road;
            InitialStiffness = stiffness;
            this.distance = distance;
            IsEquality = false;
        }

        public override float EvaluateAt(GameObject obj)
        {
            if(obj != road.StartingPoint)
            {
                return Vector3.Distance(obj.transform.position, ProjectToRoad(obj.transform.position, road)) - distance;
            }
            else
            {
                return 0f;
            }
        }

        public override Vector3 GradientAt(GameObject obj)
        {
            if (obj != road.StartingPoint)
            {
                return Vector3.Normalize(obj.transform.position - ProjectToRoad(obj.transform.position, road));
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
