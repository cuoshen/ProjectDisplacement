using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectDisplacement
{
    class HeatPointConstraint : Constraint
    {
        private List<GameObject> group;
        private GameObject desirableHeatPoint;
        /// <summary>
        /// w = 1/M, where M is the total mass of the group
        /// </summary>
        private float w;

        public HeatPointConstraint(List<GameObject> group, GameObject desirableHeatPoint, float stiffness)
        {
            this.group = group;
            this.desirableHeatPoint = desirableHeatPoint;
            float totalMass = 0f;
            foreach(GameObject g in group)
            {
                totalMass += g.GetComponent<Rigidbody>().mass;
            }
            w = 1 / totalMass;
            InitialStiffness = stiffness;
            IsEquality = true;
        }

        public override float EvaluateAt(GameObject obj)
        {
            if (group.Contains(obj))
            {
                return 0.5f * Vector3.SqrMagnitude(ComputeCenterOfMass(group, w) - desirableHeatPoint.transform.position);
            }
            else
            {
                return 0f;
            }
        }


        public override Vector3 GradientAt(GameObject obj)
        {
            if (group.Contains(obj))
            {
                return obj.GetComponent<Rigidbody>().mass * w * (ComputeCenterOfMass(group, w) - desirableHeatPoint.transform.position);
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
