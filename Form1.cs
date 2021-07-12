using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace RayTracingFirst
{
    public partial class Form1 : Form
    {
        public Bitmap canvas;
        Graphics g;
        Scene scene;
        Color BACKGROUND_COLOR = Color.White;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            scene = new Scene();
            
            scene.addSphere(new Sphere(new Vector3(7, -1, 0), 1,  Color.Red, 25, 0f));
            scene.addSphere(new Sphere(new Vector3(-5, 0, 7), 2, Color.Black, 25, 0.9f));
            AddDodecahedron(new Vector3(2, 1, 11), 4f, Color.Red, 0.5f, 1.6180339887f*2);
            AddDodecahedron(new Vector3(-2, -1.25f, 5), 1f, Color.Orange, 0, 1.6180339887f/2);
            scene.addSphere(new Sphere(new Vector3(-1, 0, 7), 1, Color.Green, 25, 0.4f));
            AddCube(new Vector3(-1, -1.5f, 7), 0.5f, Color.Magenta, 0.5f);
            AddCube(new Vector3(4, -0.75f, 7), 1.25f, Color.Blue, 0.6f);

            Color roomColor = Color.LightBlue;
            float envReflective = 0.1f;
            int specTri = 2000;
            scene.addTriangle(new Triangle(new Vector3(-10, -2, 14), new Vector3(10, -2, 14), new Vector3(-10, -2, -6), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(-10, -2, -6), new Vector3(10, -2, 14), new Vector3(10, -2, -6), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(-10, -2, 14), new Vector3(-10, 8, 14), new Vector3(10, -2, 14), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(10, -2, 14), new Vector3(-10, 8, 14), new Vector3(10, 8, 14), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(-10, -2, -6), new Vector3(-10, 8, -6), new Vector3(-10, -2, 14), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(-10, -2, 14), new Vector3(-10, 8, -6), new Vector3(-10, 8, 14), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(10, -2, -6), new Vector3(10, -2, 14), new Vector3(10, 8, -6), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle(new Vector3(10, -2, 14), new Vector3(10, 8, 14), new Vector3(10, 8, -6), roomColor, specTri, envReflective));

            scene.addTriangle(new Triangle( new Vector3(-10, 8, -6), new Vector3(-10, -2, -6),new Vector3(10, -2, -6), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle( new Vector3(-10, 8, -6),new Vector3(10, -2, -6), new Vector3(10, 8, -6), roomColor, specTri, envReflective));
            scene.addTriangle(new Triangle( new Vector3(10, 8, 14),new Vector3(-10, 8, 14), new Vector3(-10, 8, -6), roomColor, 25, envReflective));
            scene.addTriangle(new Triangle( new Vector3(10, 8, 14), new Vector3(-10, 8, -6),new Vector3(10, 8, -6), roomColor, 25, envReflective));
            scene.addLight(new AmbientLight(0.2));
            scene.addLight(new PointLight(new Vector3(2, 1, 0), 0.6));
            scene.addLight(new DirectionalLight(new Vector3(1, 4, 4), 0.2));


            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            int Cw = pictureBox1.Width;
            int Ch = pictureBox1.Height;
            int Vw = 2;
            int Vh = 1;
            double d = 1;
            var a = Cw / 2;
            var b = Ch / 2;
            Vector3 O = new Vector3(0f, 0f, 0f);

            for (int x = - Cw / 2; x < Cw / 2; x++)
            {

                for (int y = -Ch / 2 + 1; y <= Ch / 2; y++)
                {

                    ViewportPixel D = ViewportPixel.CanvasToViewPort(x + a, b - y, d, Cw, Ch, Vw, Vh);
                    D = new ViewportPixel(D.vector.X - (float)Vw / 2, (float)Vh / 2 - D.vector.Y, D.vector.Z);
                    Color color = TraceRay(O, D, 1, 1000, 5);
                    canvas.SetPixel(x + a, b - y, color);

                }

            }

            g.DrawImage(canvas, 0, 0);
        }

        private void AddDodecahedron(Vector3 center, float size, Color color, float refl, float fi)
        {

            float half = size / 2;
            float b = half / (fi + half);
            float a = b * fi;

            float[,] points1 = { { half + a, 0, -a }, { half + a, 0, a }, { half, -half, half }, { a, -(half + a), 0 }, { half, -half, -half } };
            AddPentagon(new Vector3(center.X + points1[0, 0], center.Y + points1[0, 1], center.Z + points1[0, 2]),
                new Vector3(center.X + points1[1, 0], center.Y + points1[1, 1], center.Z + points1[1, 2]),
                new Vector3(center.X + points1[2, 0], center.Y + points1[2, 1], center.Z + points1[2, 2]),
                new Vector3(center.X + points1[3, 0], center.Y + points1[3, 1], center.Z + points1[3, 2]),
                new Vector3(center.X + points1[4, 0], center.Y + points1[4, 1], center.Z + points1[4, 2]), color, refl);


            float[,] points2 = { { -a, -(half + a), 0 }, { -half, -half, half }, { -(half + a), 0, a }, { -(half + a), 0, -a }, { -half, -half, -half } };
            AddPentagon(new Vector3(center.X + points2[0, 0], center.Y + points2[0, 1], center.Z + points2[0, 2]),
                new Vector3(center.X + points2[1, 0], center.Y + points2[1, 1], center.Z + points2[1, 2]),
                new Vector3(center.X + points2[2, 0], center.Y + points2[2, 1], center.Z + points2[2, 2]),
                new Vector3(center.X + points2[3, 0], center.Y + points2[3, 1], center.Z + points2[3, 2]),
                new Vector3(center.X + points2[4, 0], center.Y + points2[4, 1], center.Z + points2[4, 2]), color, refl);

            float[,] points3 = { { -(half + a), 0, a }, { -(half + a), 0, -a }, { -half, half, -half }, { -a, half + a, 0 }, { -half, half, half } };
            AddPentagon(new Vector3(center.X + points3[0, 0], center.Y + points3[0, 1], center.Z + points3[0, 2]),
                new Vector3(center.X + points3[1, 0], center.Y + points3[1, 1], center.Z + points3[1, 2]),
                new Vector3(center.X + points3[2, 0], center.Y + points3[2, 1], center.Z + points3[2, 2]),
                new Vector3(center.X + points3[3, 0], center.Y + points3[3, 1], center.Z + points3[3, 2]),
                new Vector3(center.X + points3[4, 0], center.Y + points3[4, 1], center.Z + points3[4, 2]), color, refl);

            float[,] points4 = { { half + a, 0, a }, { half + a, 0, -a }, { half, half, -half }, { a, half + a, 0 }, { half, half, half } };
            AddPentagon(new Vector3(center.X + points4[0, 0], center.Y + points4[0, 1], center.Z + points4[0, 2]),
                new Vector3(center.X + points4[1, 0], center.Y + points4[1, 1], center.Z + points4[1, 2]),
                new Vector3(center.X + points4[2, 0], center.Y + points4[2, 1], center.Z + points4[2, 2]),
                new Vector3(center.X + points4[3, 0], center.Y + points4[3, 1], center.Z + points4[3, 2]),
                new Vector3(center.X + points4[4, 0], center.Y + points4[4, 1], center.Z + points4[4, 2]), color, refl);
            
            float[,] points5 = { { a, half + a, 0 }, { -a, half + a, 0 }, { -half, half, half }, { 0, a, half + a }, { half, half, half } };
            AddPentagon(new Vector3(center.X + points5[0, 0], center.Y + points5[0, 1], center.Z + points5[0, 2]),
                new Vector3(center.X + points5[1, 0], center.Y + points5[1, 1], center.Z + points5[1, 2]),
                new Vector3(center.X + points5[2, 0], center.Y + points5[2, 1], center.Z + points5[2, 2]),
                new Vector3(center.X + points5[3, 0], center.Y + points5[3, 1], center.Z + points5[3, 2]),
                new Vector3(center.X + points5[4, 0], center.Y + points5[4, 1], center.Z + points5[4, 2]), color, refl);

            float[,] points6 = { { a, half + a, 0 }, { -a, half + a, 0 }, { -half, half, -half }, { 0, a, -(half + a) }, { half, half, -half } };
            AddPentagon(new Vector3(center.X + points6[0, 0], center.Y + points6[0, 1], center.Z + points6[0, 2]),
                new Vector3(center.X + points6[1, 0], center.Y + points6[1, 1], center.Z + points6[1, 2]),
                new Vector3(center.X + points6[2, 0], center.Y + points6[2, 1], center.Z + points6[2, 2]),
                new Vector3(center.X + points6[3, 0], center.Y + points6[3, 1], center.Z + points6[3, 2]),
                new Vector3(center.X + points6[4, 0], center.Y + points6[4, 1], center.Z + points6[4, 2]), color, refl);

            float[,] points7 = { { a, -(half + a), 0 }, { -a, -(half + a), 0 }, { -half, -half, -half }, { 0, -a, -(half + a) }, { half, -half, -half } };
            AddPentagon(new Vector3(center.X + points7[0, 0], center.Y + points7[0, 1], center.Z + points7[0, 2]),
                new Vector3(center.X + points7[1, 0], center.Y + points7[1, 1], center.Z + points7[1, 2]),
                new Vector3(center.X + points7[2, 0], center.Y + points7[2, 1], center.Z + points7[2, 2]),
                new Vector3(center.X + points7[3, 0], center.Y + points7[3, 1], center.Z + points7[3, 2]),
                new Vector3(center.X + points7[4, 0], center.Y + points7[4, 1], center.Z + points7[4, 2]), color, refl);

            float[,] points8 = { { a, -(half + a), 0 }, { -a, -(half + a), 0 }, { -half, -half, half }, { 0, -a, half + a }, { half, -half, half } };
            AddPentagon(new Vector3(center.X + points8[0, 0], center.Y + points8[0, 1], center.Z + points8[0, 2]),
                new Vector3(center.X + points8[1, 0], center.Y + points8[1, 1], center.Z + points8[1, 2]),
                new Vector3(center.X + points8[2, 0], center.Y + points8[2, 1], center.Z + points8[2, 2]),
                new Vector3(center.X + points8[3, 0], center.Y + points8[3, 1], center.Z + points8[3, 2]),
                new Vector3(center.X + points8[4, 0], center.Y + points8[4, 1], center.Z + points8[4, 2]), color, refl);

            float[,] points9 = { { -half, -half, half }, { 0, -a, half + a }, { 0, a, half + a }, { -half, half, half }, { -(half+a), 0, a } };
            AddPentagon(new Vector3(center.X + points9[0, 0], center.Y + points9[0, 1], center.Z + points9[0, 2]),
                new Vector3(center.X + points9[1, 0], center.Y + points9[1, 1], center.Z + points9[1, 2]),
                new Vector3(center.X + points9[2, 0], center.Y + points9[2, 1], center.Z + points9[2, 2]),
                new Vector3(center.X + points9[3, 0], center.Y + points9[3, 1], center.Z + points9[3, 2]),
                new Vector3(center.X + points9[4, 0], center.Y + points9[4, 1], center.Z + points9[4, 2]), color, refl);

            float[,] points10 = { { 0, -a, half + a }, { half, -half, half }, { half + a, 0, a }, { half, half, half }, { 0, a, half + a } };
            AddPentagon(new Vector3(center.X + points10[0, 0], center.Y + points10[0, 1], center.Z + points10[0, 2]),
                new Vector3(center.X + points10[1, 0], center.Y + points10[1, 1], center.Z + points10[1, 2]),
                new Vector3(center.X + points10[2, 0], center.Y + points10[2, 1], center.Z + points10[2, 2]),
                new Vector3(center.X + points10[3, 0], center.Y + points10[3, 1], center.Z + points10[3, 2]),
                new Vector3(center.X + points10[4, 0], center.Y + points10[4, 1], center.Z + points10[4, 2]), color, refl);

            float[,] points11 = { { 0, -a, -(half + a) }, { half, -half, -half }, { half + a, 0, -a }, { half, half, -half }, { 0, a, -(half + a) } };
            AddPentagon(new Vector3(center.X + points11[0, 0], center.Y + points11[0, 1], center.Z + points11[0, 2]),
                new Vector3(center.X + points11[1, 0], center.Y + points11[1, 1], center.Z + points11[1, 2]),
                new Vector3(center.X + points11[2, 0], center.Y + points11[2, 1], center.Z + points11[2, 2]),
                new Vector3(center.X + points11[3, 0], center.Y + points11[3, 1], center.Z + points11[3, 2]),
                new Vector3(center.X + points11[4, 0], center.Y + points11[4, 1], center.Z + points11[4, 2]), color, refl);

            float[,] points12 = { { -half, -half, -half }, { 0, -a, -(half + a) }, { 0, a, -(half + a) }, { -half, half, -half }, { -(half + a), 0, -a } };
            AddPentagon(new Vector3(center.X + points12[0, 0], center.Y + points12[0, 1], center.Z + points12[0, 2]),
                new Vector3(center.X + points12[1, 0], center.Y + points12[1, 1], center.Z + points12[1, 2]),
                new Vector3(center.X + points12[2, 0], center.Y + points12[2, 1], center.Z + points12[2, 2]),
                new Vector3(center.X + points12[3, 0], center.Y + points12[3, 1], center.Z + points12[3, 2]),
                new Vector3(center.X + points12[4, 0], center.Y + points12[4, 1], center.Z + points12[4, 2]), color, refl);
        }

        private void AddPentagon(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color color, float refl)
        {
            scene.addTriangle(new Triangle( p2, p1, p0, color, 1000, refl));
            scene.addTriangle(new Triangle(p0, p1, p2, color, 1000, refl));
            scene.addTriangle(new Triangle(p4, p2, p0, color, 1000, refl));
            scene.addTriangle(new Triangle(p0, p2, p4, color, 1000, refl));
            scene.addTriangle(new Triangle(p4, p3, p2, color, 1000, refl));
            scene.addTriangle(new Triangle(p2, p3, p4, color, 1000, refl));
        }

        private void AddCube(Vector3 center, float size, Color color, float refl)
        {
            int n = 8;
            float[,] points = { { 1, 1, 1}, {-1, 1, 1},{-1,-1, 1 },{1, -1, 1 },
                { 1, 1, -1}, {-1, 1, -1},{-1,-1, -1 },{1, -1, -1 } };

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    points[i, j] *= size;
                }
            }

            AddRect(new Vector3(center.X + points[0, 0], center.Y + points[0, 1], center.Z + points[0, 2]), 
                new Vector3(center.X + points[1, 0], center.Y + points[1, 1], center.Z + points[1, 2]),
                new Vector3(center.X + points[2, 0], center.Y + points[2, 1], center.Z + points[2, 2]), 
                new Vector3(center.X + points[3, 0], center.Y + points[3, 1], center.Z + points[3, 2]), color, refl);

            AddRect(new Vector3(center.X + points[4, 0], center.Y + points[4, 1], center.Z + points[4, 2]),
                new Vector3(center.X + points[5, 0], center.Y + points[5, 1], center.Z + points[5, 2]),
                new Vector3(center.X + points[6, 0], center.Y + points[6, 1], center.Z + points[6, 2]),
                new Vector3(center.X + points[7, 0], center.Y + points[7, 1], center.Z + points[7, 2]), color, refl);

            AddRect(new Vector3(center.X + points[2, 0], center.Y + points[2, 1], center.Z + points[2, 2]),
                new Vector3(center.X + points[3, 0], center.Y + points[3, 1], center.Z + points[3, 2]),
                new Vector3(center.X + points[7, 0], center.Y + points[7, 1], center.Z + points[7, 2]),
                new Vector3(center.X + points[6, 0], center.Y + points[6, 1], center.Z + points[6, 2]), color, refl);

            AddRect(new Vector3(center.X + points[0, 0], center.Y + points[0, 1], center.Z + points[0, 2]),
                new Vector3(center.X + points[1, 0], center.Y + points[1, 1], center.Z + points[1, 2]),
                new Vector3(center.X + points[5, 0], center.Y + points[5, 1], center.Z + points[5, 2]),
                new Vector3(center.X + points[4, 0], center.Y + points[4, 1], center.Z + points[4, 2]), color, refl);

            AddRect(new Vector3(center.X + points[0, 0], center.Y + points[0, 1], center.Z + points[0, 2]),
                new Vector3(center.X + points[4, 0], center.Y + points[4, 1], center.Z + points[4, 2]),
                new Vector3(center.X + points[7, 0], center.Y + points[7, 1], center.Z + points[7, 2]),
                new Vector3(center.X + points[3, 0], center.Y + points[3, 1], center.Z + points[3, 2]), color, refl);

            AddRect(new Vector3(center.X + points[1, 0], center.Y + points[1, 1], center.Z + points[1, 2]),
                new Vector3(center.X + points[5, 0], center.Y + points[5, 1], center.Z + points[5, 2]),
                new Vector3(center.X + points[6, 0], center.Y + points[6, 1], center.Z + points[6, 2]),
                new Vector3(center.X + points[2, 0], center.Y + points[2, 1], center.Z + points[2, 2]), color, refl);
        }

        private void AddRect(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color, float refl)
        {
            scene.addTriangle(new Triangle(p2, p1, p0, color, 1000, refl));
            scene.addTriangle(new Triangle(p0, p1, p2, color, 1000, refl));
            scene.addTriangle(new Triangle(p3, p2, p0, color, 1000, refl));
            scene.addTriangle(new Triangle(p0, p2, p3, color, 1000, refl));
        }

        private Color TraceRay(Vector3 O, ViewportPixel D, double tMin, double tMax, int recursionDepth)
        {
            var temp = ClosestIntersection(O, D, tMin, tMax);
            var closestT = temp.Item2;
            var closetSphere = temp.Item1;

            if (closetSphere == null) return BACKGROUND_COLOR;

            Color resultColor = BACKGROUND_COLOR;

            float r = 0;
            Vector3 P = Vector3.Zero;
            Vector3 N = Vector3.Zero;

            if (closetSphere is Sphere)
            {
                var sphere = (Sphere)closetSphere;

                P = O + new Vector3(D.vector.X * (float)closestT, D.vector.Y * (float)closestT, D.vector.Z * (float)closestT);
                N = P - sphere.center;
                N = N / N.Length();

                var computeLighting = ComputeLighting(P, N, -D.vector, sphere.specular);
                if (computeLighting > 1) computeLighting = 1;
                resultColor = Color.FromArgb((int)(sphere.color.R * computeLighting),
                                                (int)(sphere.color.G * computeLighting),
                                                (int)(sphere.color.B * computeLighting));

                r = sphere.reflective;

            }
            else if (closetSphere is Triangle)
            {
                var triangle = (Triangle)closetSphere;

                P = O + new Vector3(D.vector.X * (float)closestT, D.vector.Y * (float)closestT, D.vector.Z * (float)closestT);
                N = Vector3.Cross(Vector3.Subtract(triangle.v1,triangle.v0), Vector3.Subtract(triangle.v2, triangle.v0));
                N = N / N.Length();

                var computeLighting = ComputeLighting(P, N, -D.vector, triangle.specular);
                if (computeLighting > 1) computeLighting = 1;
                resultColor = Color.FromArgb((int)(triangle.color.R * computeLighting),
                                                (int)(triangle.color.G * computeLighting),
                                                (int)(triangle.color.B * computeLighting));

                r = triangle.reflective;
            }

            if (recursionDepth <= 0 || r <= 0)
                return resultColor;

            var R = ReflectRay(-D.vector, N);
            var reflectedColor = TraceRay(P, new ViewportPixel(R.X, R.Y, R.Z), 0.001, 1000, recursionDepth - 1);

            resultColor = Color.FromArgb((int)(resultColor.R * (1 - r) + reflectedColor.R * r),
                                        (int)(resultColor.G * (1 - r) + reflectedColor.G * r),
                                        (int)(resultColor.B * (1 - r) + reflectedColor.B * r));

            return resultColor;
        }

        private List<double> IntersectRaySphere(Vector3 O, ViewportPixel D, Sphere sphere)
        {
            var vectorD = new Vector3((float) D.vector.X, (float) D.vector.Y, (float) D.vector.Z);
            List<double> result = new List<double>();
            var k1 = Vector3.Dot(vectorD, vectorD);
            var k2 = 2 * Vector3.Dot(O - sphere.center, vectorD);
            var k3 = Vector3.Dot(O - sphere.center, O - sphere.center) - sphere.radius * sphere.radius;

            var discriminant = k2 * k2 - 4 * k1 * k3;
            if (discriminant < 0) return result;

            result.Add((-k2 + Math.Sqrt(discriminant)) / (2 * k1));
            result.Add((-k2 - Math.Sqrt(discriminant)) / (2 * k1));
            return result;
        }

        private double IntersectRayTriangle(Vector3 O, ViewportPixel D, Triangle triangle)
        {
            var vectorD = new Vector3((float)D.vector.X, (float)D.vector.Y, (float)D.vector.Z);
            double result;

            Vector3 v0v1 = Vector3.Subtract(triangle.v1, triangle.v0);
            Vector3 v0v2 = Vector3.Subtract(triangle.v2, triangle.v0);
            Vector3 pvec = Vector3.Cross(D.vector, v0v2);
            float det = Vector3.Dot(v0v1, pvec);

            if (det < 0.000001)
                return -1000;

            float invDet = (float)(1.0 / det);
            Vector3 tvec = Vector3.Subtract(O, triangle.v0);
            float u = Vector3.Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
                return -1000;

            Vector3 qvec = Vector3.Cross(tvec, v0v1);
            float v = Vector3.Dot(D.vector, qvec) * invDet;

            if (v < 0 || u + v > 1)
                return -1000;

            return (Vector3.Dot(v0v2, qvec) * invDet);
        }

        private double ComputeLighting(Vector3 P, Vector3 N, Vector3 V, int s)
        {
            var intensity = 0.0;
            foreach (Light light in scene.lights)
            {
                float tMax;
                if (light.GetLightType() == "Ambient")
                    intensity += light.intensity;
                else
                {
                    var L = new Vector3();
                    if (light.GetLightType() == "Point")
                    {
                        L = ((PointLight)light).position - P;
                        tMax = 1;
                    }
                    else
                    {
                        L = ((DirectionalLight)light).direction;
                        tMax = 1000;
                    }

                    //Проверка тени
                    var shadowTemp = ClosestIntersection(P, new ViewportPixel(L.X, L.Y, L.Z), 0.001, tMax);
                    if (shadowTemp.Item1 != null) continue;

                    //Диффузность
                    var n_dot_l = Vector3.Dot(N, L);
                    if (n_dot_l > 0)
                    {
                        intensity += light.intensity * n_dot_l / (N.Length() * L.Length());
                    }

                    //Блики
                    if (s != -1)
                    {
                        var R = ReflectRay(L, N);
                        var r_dot_v = Vector3.Dot(R, V);
                        if (r_dot_v > 0)
                            intensity += light.intensity * Math.Pow(r_dot_v / (R.Length() * V.Length()), s);
                    }
                }
            }
            return intensity;
        }


        private Tuple<object, double> ClosestIntersection(Vector3 O, ViewportPixel D, double tMin, double tMax)
        {
            var closet_t = tMax;
            object closet_object = null;
            foreach (Sphere sphere in scene.spheres)
            {
                List<double> tList = IntersectRaySphere(O, D, sphere);
                for (var i = 0; i < tList.Count; i++)
                    if (tList[i] >= tMin && tList[i] <= tMax && tList[i] < closet_t)
                    {
                        closet_t = tList[i];
                        closet_object = sphere;
                    }
            }

            foreach (Triangle triangle in scene.triangles)
            {
                double t = IntersectRayTriangle(O, D, triangle);
                if (t >= tMin && t <= tMax && t < closet_t)
                {
                    closet_t = t;
                    closet_object = triangle;
                }
            }

            return new Tuple<object, double>(closet_object, closet_t);
        }

        /*
        private Tuple<Sphere, double> ClosestIntersection(Vector3 O, ViewportPixel D, double tMin, double tMax)
        {
            var closet_t = tMax;
            Sphere closet_sphere = null;
            foreach (Sphere sphere in scene.spheres)
            {
                List<double> tList = IntersectRaySphere(O, D, sphere);
                for (var i = 0; i < tList.Count; i++)
                    if (tList[i] >= tMin && tList[i] <= tMax && tList[i] < closet_t)
                    {
                        closet_t = tList[i];
                        closet_sphere = sphere;
                    }
            }
            return new Tuple<Sphere, double>(closet_sphere, closet_t);
        }*/

        private Vector3 ReflectRay (Vector3 R, Vector3 N)
        {
            return 2 * N * Vector3.Dot(N, R) - R;
        }
    }
}
