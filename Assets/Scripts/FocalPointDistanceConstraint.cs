using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectDisplacement
{
    class FocalPointDistanceConstraint : Constraint
    {
        private GameObject focalPoint;
        private struct SurroundingConstraint
        {
            public GameObject obj;
            public PairwiseDistanceConstraint constraintToFocal;
        }
        private List<SurroundingConstraint> surroundings = new List<SurroundingConstraint>();

        public FocalPointDistanceConstraint(GameObject focalPoint, List<GameObject> surroundingObjects, float stiffness, float distance)
        {
            this.focalPoint = focalPoint;
            IsEquality = true;
            InitialStiffness = stiffness;
            // Foreach surrounding objects, we create a pairwise distance constraint to focal point
            foreach(GameObject g in surroundingObjects)
            {
                SurroundingConstraint valuePair = new SurroundingConstraint();
                valuePair.obj = g;
                valuePair.constraintToFocal = new PairwiseDistanceConstraint(focalPoint, g, stiffness, distance);
                surroundings.Add(valuePair);
            }
        }

        public override float EvaluateAt(GameObject obj)
        {
            foreach (SurroundingConstraint valuePair in surroundings)
            {
                if (obj == valuePair.obj)
                {
                    return valuePair.constraintToFocal.EvaluateAt(obj);
                }
            }
            // The obj is either the focal point or not involved at all
            return 0f;
        }

        public override Vector3 GradientAt(GameObject obj)
        {
            foreach(SurroundingConstraint valuePair in surroundings)
            {
                if(obj == valuePair.obj)
                {
                    return valuePair.constraintToFocal.GradientAt(obj);
                }
            }
            // The obj is either the focal point or not involved at all
            return Vector3.zero;
        }
    }
}
