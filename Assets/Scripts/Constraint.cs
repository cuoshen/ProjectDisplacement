using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDisplacement
{
    /// <summary>
    /// Represent a scalar-valued function that takes a vector in R^3 as a parameter
    /// </summary>
    abstract class Constraint
    {
        private const float DELTA = 0.001f;
        /// <summary>
        /// M >= 1 determines the rate at which stiffness approaches 0
        /// </summary>
        private const float M = 2f;
        private float initialStiffness;
        public float InitialStiffness
        {
            get
            {
                return initialStiffness;
            }
            set
            {
                initialStiffness = value;
                CurrentStiffness = value;
            }
        }
        public float CurrentStiffness
        {
            get; private set;
        }
        public bool IsEquality
        {
            get;
            protected set;
        }

        public void UpdateStiffness(int iteration)
        {
            CurrentStiffness = Mathf.Pow(1 - (1 - InitialStiffness), M / iteration);
        }

        public abstract float EvaluateAt(GameObject obj);
        protected virtual float? EvaluateAt(Vector3 p)
        {
            return null;
        }

        public virtual Vector3 GradientAt(GameObject obj)
        {
            Vector3 p = obj.transform.position;
            return new Vector3(
                PartialDerivative(0, p), PartialDerivative(1, p), PartialDerivative(2, p)
                );
        }

        /// <summary>
        /// Take partial derivative with respect to x, y or z
        /// </summary>
        private float PartialDerivative(int index, Vector3 p)
        {
            Vector3 pShifted = p;
            pShifted[index] += DELTA;
            return (EvaluateAt(pShifted).GetValueOrDefault() - EvaluateAt(p).GetValueOrDefault()) / DELTA;
        }


        protected Vector3 ComputeCenterOfMass(List<GameObject> group, float inverseTotalWeight)
        {
            Vector3 weightedSum = new Vector3();
            foreach (GameObject g in group)
            {
                weightedSum += g.GetComponent<Rigidbody>().mass * g.transform.position;
            }
            return inverseTotalWeight * weightedSum;
        }

        public struct Road
        {
            public GameObject StartingPoint;
            public Vector3 Direction;
        }

        protected Vector3 ProjectToRoad(Vector3 pi, Road road)
        {
            Vector3 pij = pi - road.StartingPoint.transform.position;
            return road.StartingPoint.transform.position + Vector3.Project(pij, road.Direction);
        }
    }
}