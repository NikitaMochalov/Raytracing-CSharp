using System.Collections.Generic;

namespace RayTracingFirst
{
    internal class Scene
    {
        public List<Sphere> spheres = new List<Sphere>();
        public List<Light> lights = new List<Light>();
        public List<Triangle> triangles = new List<Triangle>();

        public Scene()
        {
        }

        public void addSphere(Sphere sphere)
        {
            spheres.Add(sphere);
        }

        public void addLight(Light light)
        {
            lights.Add(light);
        }

        public void addTriangle(Triangle triangle)
        {
            triangles.Add(triangle);
        }
    }
}