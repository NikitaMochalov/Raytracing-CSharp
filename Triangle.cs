using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace RayTracingFirst
{
    class Triangle
    {
        public Vector3 v0;
        public Vector3 v1;
        public Vector3 v2;

        public Color color;
        public int specular;
        public float reflective;

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Color color, int specular, float reflective)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;

            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
    }
}
