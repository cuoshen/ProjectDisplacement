using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectDisplacement
{
    class FocalPointSymmetryConstraint : Constraint
    {
        private List<GameObject> group;
        private GameObject focalPoint;
        private Road road;
        float w;
        
        public FocalPointSymmetryConstraint(List<GameObject> group, GameObject focalPoint, float stiffness)
        {
            this.group = group;
            this.focalPoint = focalPoint;
            float totalMass = 0f;
            foreach (GameObject g in group)
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
                return 0.5f * Vector3.SqrMagnitude(ComputeCenterOfMass(group, w) - ProjectToRoad(obj.transform.position, road));
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
                return obj.GetComponent<Rigidbody>().mass * w * (ComputeCenterOfMass(group, w) - ProjectToRoad(obj.transform.position, road));
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
