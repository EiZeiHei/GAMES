using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CubeModel : MonoBehaviour
{
    public bool SVD = false;

    float dt = 0.003f;
    float mass = 1;
    float stiffness_0 = 20000.0f;
    float stiffness_1 = 5000.0f;
    float damp = 0.999f;

    int[] Tet;
    int tet_number;         //The number of tetrahedra

    Vector3[] Force;
    Vector3[] V;
    Vector3[] X;
    int number;             //The number of vertices

    Matrix4x4[] inv_Dm;

    //For Laplacian smoothing.
    Vector3[] V_sum;
    int[] V_num;

    SVD svd = new SVD();

    // Start is called before the first frame update
    void Start()
    {
        // FILO IO: Read the house model from files.
        // The model is from Jonathan Schewchuk's Stellar lib.
        {
            string fileContent = File.ReadAllText("Assets/Deform/cube1k.ele");
            string[] Strings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            tet_number = int.Parse(Strings[0]);
            Tet = new int[tet_number * 4];

            for (int tet = 0; tet < tet_number; tet++)
            {
                Tet[tet * 4 + 0] = int.Parse(Strings[tet * 5 + 4]) - 1;
                Tet[tet * 4 + 1] = int.Parse(Strings[tet * 5 + 5]) - 1;
                Tet[tet * 4 + 2] = int.Parse(Strings[tet * 5 + 6]) - 1;
                Tet[tet * 4 + 3] = int.Parse(Strings[tet * 5 + 7]) - 1;
            }
        }
        {
            string fileContent = File.ReadAllText("Assets/Deform/cube1k.node");
            string[] Strings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            number = int.Parse(Strings[0]);
            X = new Vector3[number];
            for (int i = 0; i < number; i++)
            {
                X[i].x = float.Parse(Strings[i * 5 + 5]) * 0.4f;
                X[i].y = float.Parse(Strings[i * 5 + 6]) * 0.4f;
                X[i].z = float.Parse(Strings[i * 5 + 7]) * 0.4f;
            }
            //Centralize the model.
            Vector3 center = Vector3.zero;
            for (int i = 0; i < number; i++) center += X[i];
            center = center / number;
            for (int i = 0; i < number; i++)
            {
                X[i] -= center;
                float temp = X[i].y;
                X[i].y = X[i].z;
                X[i].z = temp;
            }
        }
        /*tet_number=1;
        Tet = new int[tet_number*4];
        Tet[0]=0;
        Tet[1]=1;
        Tet[2]=2;
        Tet[3]=3;

        number=4;
        X = new Vector3[number];
        V = new Vector3[number];
        Force = new Vector3[number];
        X[0]= new Vector3(0, 0, 0);
        X[1]= new Vector3(1, 0, 0);
        X[2]= new Vector3(0, 1, 0);
        X[3]= new Vector3(0, 0, 1);*/


        //Create triangle mesh.
        Vector3[] vertices = new Vector3[tet_number * 12];
        int vertex_number = 0;
        for (int tet = 0; tet < tet_number; tet++)
        {
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];

            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
        }

        int[] triangles = new int[tet_number * 12];
        for (int t = 0; t < tet_number * 4; t++)
        {
            triangles[t * 3 + 0] = t * 3 + 0;
            triangles[t * 3 + 1] = t * 3 + 1;
            triangles[t * 3 + 2] = t * 3 + 2;
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        V = new Vector3[number];
        Force = new Vector3[number];
        V_sum = new Vector3[number];
        V_num = new int[number];

        //TODO: Need to allocate and assign inv_Dm
        inv_Dm = new Matrix4x4[tet_number];
        for (int i = 0; i < tet_number; i++)
        {
            inv_Dm[i] = Build_Edge_Matrix(i).inverse;
        }
    }

    Matrix4x4 Build_Edge_Matrix(int tet)
    {
        Matrix4x4 ret = Matrix4x4.zero;
        //TODO: Need to build edge matrix here.
        ret[0, 0] = X[Tet[tet * 4 + 1]].x - X[Tet[tet * 4 + 0]].x;
        ret[1, 0] = X[Tet[tet * 4 + 1]].y - X[Tet[tet * 4 + 0]].y;
        ret[2, 0] = X[Tet[tet * 4 + 1]].z - X[Tet[tet * 4 + 0]].z;
        ret[3, 0] = 0;

        ret[0, 1] = X[Tet[tet * 4 + 2]].x - X[Tet[tet * 4 + 0]].x;
        ret[1, 1] = X[Tet[tet * 4 + 2]].y - X[Tet[tet * 4 + 0]].y;
        ret[2, 1] = X[Tet[tet * 4 + 2]].z - X[Tet[tet * 4 + 0]].z;
        ret[3, 1] = 0;

        ret[0, 2] = X[Tet[tet * 4 + 3]].x - X[Tet[tet * 4 + 0]].x;
        ret[1, 2] = X[Tet[tet * 4 + 3]].y - X[Tet[tet * 4 + 0]].y;
        ret[2, 2] = X[Tet[tet * 4 + 3]].z - X[Tet[tet * 4 + 0]].z;
        ret[3, 2] = 0;

        ret[0, 3] = 0;
        ret[1, 3] = 0;
        ret[2, 3] = 0;
        ret[3, 3] = 1;

        return ret;
    }

    void Smooth()
    {
        for (int i = 0; i < number; i++)
        {
            V_sum[i] = new Vector3(0, 0, 0);
            V_num[i] = 0;
        }

        for (int tet = 0; tet < tet_number; tet++)
        {
            Vector3 sum = V[Tet[tet * 4 + 0]] + V[Tet[tet * 4 + 1]] + V[Tet[tet * 4 + 2]] + V[Tet[tet * 4 + 3]];
            V_sum[Tet[tet * 4 + 0]] += sum;
            V_sum[Tet[tet * 4 + 1]] += sum;
            V_sum[Tet[tet * 4 + 2]] += sum;
            V_sum[Tet[tet * 4 + 3]] += sum;
            V_num[Tet[tet * 4 + 0]] += 4;
            V_num[Tet[tet * 4 + 1]] += 4;
            V_num[Tet[tet * 4 + 2]] += 4;
            V_num[Tet[tet * 4 + 3]] += 4;
        }

        for (int i = 0; i < number; i++)
        {
            V[i] = 0.9f * V[i] + 0.1f * V_sum[i] / V_num[i];
        }
    }

    void _Update()
    {
        // Jump up.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < number; i++)
                V[i].y += 0.2f;
        }

        for (int i = 0; i < number; i++)
        {
            //TODO: Add gravity to Force.
            Force[i] = new Vector3(0, -9.81f * mass, 0);
        }

        for (int tet = 0; tet < tet_number; tet++)
        {
            //TODO: Deformation Gradient
            Matrix4x4 edge = Build_Edge_Matrix(tet);
            Matrix4x4 F = edge * inv_Dm[tet];
            Matrix4x4 tmp = Matrix4x4.zero;

            if (!SVD)
            {
                //TODO: Green Strain
                Matrix4x4 G = F.transpose * F;
                G[0, 0] -= 1;
                G[1, 1] -= 1;
                G[2, 2] -= 1;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        G[i, j] *= 0.5f;
                    }
                }

                //TODO: Second PK Stress
                Matrix4x4 S = Matrix4x4.zero;
                float trace = G[0, 0] + G[1, 1] + G[2, 2];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        S[i, j] = 2 * stiffness_1 * G[i, j];
                    }
                }
                S[0, 0] += stiffness_0 * trace;
                S[1, 1] += stiffness_0 * trace;
                S[2, 2] += stiffness_0 * trace;

                //Matrix4x4 S = Add_Matrix(Prod_Matrix(G, 2.0f * stiffness_0), Prod_Matrix(Matrix4x4.identity, stiffness_1 * trace));
                Matrix4x4 P = F * S;

                //TODO: Elastic Force
                tmp = P * inv_Dm[tet].transpose;
            }
            else
            {
                Matrix4x4 U = Matrix4x4.identity;
                Matrix4x4 D = Matrix4x4.identity;
                Matrix4x4 V = Matrix4x4.identity;
                svd.svd(F, ref U, ref D, ref V);

                Matrix4x4 P = Matrix4x4.identity;
                float I = D[0, 0] * D[0, 0] + D[1, 1] * D[1, 1] + D[2, 2] * D[2, 2];
                float J = D[0, 0] * D[1, 1] * D[2, 2];
                float II = D[0, 0] * D[0, 0] * D[0, 0] * D[0, 0] + D[1, 1] * D[1, 1] * D[1, 1] * D[1, 1] + D[2, 2] * D[2, 2] * D[2, 2] * D[2, 2];
                float III = J * J;
                float dEdI = stiffness_0 * (I - 3) * 0.25f - stiffness_1 * 0.5f;
                float dEdII = stiffness_1 * 0.25f;
                float dEdIII = 0;

                P[0, 0] = 2 * dEdI * D[0, 0] + 4 * dEdII * D[0, 0] * D[0, 0] * D[0, 0] + 2 * dEdIII * III / D[0, 0];
                P[1, 1] = 2 * dEdI * D[1, 1] + 4 * dEdII * D[1, 1] * D[1, 1] * D[1, 1] + 2 * dEdIII * III / D[1, 1];
                P[2, 2] = 2 * dEdI * D[2, 2] + 4 * dEdII * D[2, 2] * D[2, 2] * D[2, 2] + 2 * dEdIII * III / D[2, 2];

                tmp = U * P * V.transpose * inv_Dm[tet].transpose;
            }

            float k = -1 / (inv_Dm[tet].determinant * 6);

            Force[Tet[tet * 4 + 0]].x -= k * (tmp[0, 0] + tmp[0, 1] + tmp[0, 2]);
            Force[Tet[tet * 4 + 0]].y -= k * (tmp[1, 0] + tmp[1, 1] + tmp[1, 2]);
            Force[Tet[tet * 4 + 0]].z -= k * (tmp[2, 0] + tmp[2, 1] + tmp[2, 2]);
            Force[Tet[tet * 4 + 1]].x += k * tmp[0, 0];
            Force[Tet[tet * 4 + 1]].y += k * tmp[1, 0];
            Force[Tet[tet * 4 + 1]].z += k * tmp[2, 0];
            Force[Tet[tet * 4 + 2]].x += k * tmp[0, 1];
            Force[Tet[tet * 4 + 2]].y += k * tmp[1, 1];
            Force[Tet[tet * 4 + 2]].z += k * tmp[2, 1];
            Force[Tet[tet * 4 + 3]].x += k * tmp[0, 2];
            Force[Tet[tet * 4 + 3]].y += k * tmp[1, 2];
            Force[Tet[tet * 4 + 3]].z += k * tmp[2, 2];
        }

        //Laplacian smoothing.
        Smooth();

        for (int i = 0; i < number; i++)
        {
            //TODO: Update X and V here.
            V[i] = damp * (V[i] + Force[i] / mass * dt);
            X[i] = X[i] + V[i] * dt;

            //TODO: (Particle) collision with floor.
            if (X[i].y < -3.0f)
            {
                V[i].x = 0;
                V[i].z = 0;
                V[i].y += (-3.0f - X[i].y) / dt;
                X[i].y = -3.0f;
            }

            //Collision with Sphere
            GameObject sphere = GameObject.Find("Sphere");
            Vector3 c = sphere.transform.position;
            float r = 2.7f;
            for (int j = 0; j < X.Length; j++)
            {
                Vector3 d = X[i] - c;
                if (d.magnitude < r)
                {
                    V[i] += (c + r * d.normalized - X[i]) / dt;
                    X[i] = c + r * d.normalized;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int l = 0; l < 10; l++)
            _Update();

        // Dump the vertex array for rendering.
        Vector3[] vertices = new Vector3[tet_number * 12];
        int vertex_number = 0;
        for (int tet = 0; tet < tet_number; tet++)
        {
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 0]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 1]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 2]];
            vertices[vertex_number++] = X[Tet[tet * 4 + 3]];
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
