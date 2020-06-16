using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectDisplacement
{
    class PairwiseDistanceConstraint : Constraint
    {
        private GameObject object1;
        private GameObject object2;
        private float distance;

        public PairwiseDistanceConstraint(GameObject obj1, GameObject obj2, float stiffness, float distance)
        {
            object1 = obj1;
            object2 = obj2;
            IsEquality = true;
            InitialStiffness = stiffness;
            this.distance = distance;
        }

        public override float EvaluateAt(GameObject obj)
        {
            if(obj == object1 || obj == object2)
            {
                return Vector3.Distance(object1.transform.position, object2.transform.position) - distance;
            }
            else
            {
                return 0;
            }
        }

        public override Vector3 GradientAt(GameObject obj)
        {
            Vector3 p_ij = object1.transform.position - object2.transform.position;
            if(obj == object1)
            {
                return Vector3.Normalize(p_ij);
            }
            else if(obj == object2)
            {
                return -Vector3.Normalize(p_ij);
            }
            else
            {
                // Object not involved in this constraint, the position is unchanged
                return Vector3.zero;
            }
        }
    }
}
