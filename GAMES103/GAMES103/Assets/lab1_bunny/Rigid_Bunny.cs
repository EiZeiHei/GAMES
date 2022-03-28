using UnityEngine;
using System.Collections;

public class Rigid_Bunny : MonoBehaviour 
{
	bool launched = false;
	
	float dt = 0.015f;

	Vector3 v = new Vector3(0, 0, 0);	// velocity
	Vector3 w = new Vector3(0, 0, 0);	// angular velocity
	
	float mass;							// mass
	Matrix4x4 I_ref;					// reference inertia

	float linear_decay	= 0.999f;		// for damping
	float angular_decay	= 0.98f;
	float muN 		= 0.5f;     // for collision
	float muT = 0.1f;
	Vector3 g = new Vector3(0, -9.81f, 0);	//gravity

	// Use this for initialization
	void Start () 
	{		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;

		float m=1;
		mass=0;
		for (int i=0; i<vertices.Length; i++) 
		{
			mass += m;
			float diag=m*vertices[i].sqrMagnitude;
			I_ref[0, 0]+=diag;
			I_ref[1, 1]+=diag;
			I_ref[2, 2]+=diag;
			I_ref[0, 0]-=m*vertices[i][0]*vertices[i][0];
			I_ref[0, 1]-=m*vertices[i][0]*vertices[i][1];
			I_ref[0, 2]-=m*vertices[i][0]*vertices[i][2];
			I_ref[1, 0]-=m*vertices[i][1]*vertices[i][0];
			I_ref[1, 1]-=m*vertices[i][1]*vertices[i][1];
			I_ref[1, 2]-=m*vertices[i][1]*vertices[i][2];
			I_ref[2, 0]-=m*vertices[i][2]*vertices[i][0];
			I_ref[2, 1]-=m*vertices[i][2]*vertices[i][1];
			I_ref[2, 2]-=m*vertices[i][2]*vertices[i][2];
		}
		I_ref [3, 3] = 1;
	}
	
	Matrix4x4 Get_Cross_Matrix(Vector3 a)
	{
		//Get the cross product matrix of vector a
		Matrix4x4 A = Matrix4x4.zero;
		A [0, 0] = 0; 
		A [0, 1] = -a [2]; 
		A [0, 2] = a [1]; 
		A [1, 0] = a [2]; 
		A [1, 1] = 0; 
		A [1, 2] = -a [0]; 
		A [2, 0] = -a [1]; 
		A [2, 1] = a [0]; 
		A [2, 2] = 0; 
		A [3, 3] = 1;
		return A;
	}

	Matrix4x4 Add_Matrix(Matrix4x4 A, Matrix4x4 B)
	{
		// Get the sum of matrices A and B
		Matrix4x4 C = Matrix4x4.zero;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				C[i, j] = A[i, j] + B[i, j];
			}
		}
		return C;
	}

	Matrix4x4 Prod_Matrix(Matrix4x4 A, float a)
	{
		// Get the component-wise product of a matrix with a scalar
		Matrix4x4 B = Matrix4x4.zero;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				B[i, j] = A[i, j] * a;
			}
		}
		return B;
	}

	void Collision_Handler(Vector3 P, Vector3 N, float dt)
	{
        Matrix4x4 R = Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1));
        Vector3 x = transform.position;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Vector3 va = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 Ra = new Vector3(0.0f, 0.0f, 0.0f);
        int num = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            //Collision detection
            Vector3 Rr = R * vertices[i];
            Vector3 xi = x + Rr;
            float Phi = Vector3.Dot((xi - P), N);

            if (Phi < 0)
            {
				Vector3 tmp = Vector3.Cross(w, Rr);
                Vector3 vi = v + tmp;
                if (Vector3.Dot(vi, N) < 0)
                {
                    va += vi;
                    Ra += Rr;
                    num++;
                }
            }
        }
        if (num > 0)
        {
            va /= num;
            Ra /= num;
            if (va.magnitude < 4.0f * dt)
            {
				muN = 0;
            }

			Matrix4x4 I_now = R * I_ref * R.transpose;

			//Compute the wanted van
			Vector3 vN = Vector3.Dot(va, N) * N;
            Vector3 vT = va - vN;
            float a = Mathf.Max(1 - (muT * (1 + muN) * vN.magnitude / vT.magnitude), 0);

            vN = - muN * vN;
            vT = a * vT;
            Vector3 van = vN + vT;

            //Compute the impulse j
            Matrix4x4 I = Matrix4x4.identity;
            Matrix4x4 Rrx = Get_Cross_Matrix(Ra);

            Matrix4x4 K = Add_Matrix(Prod_Matrix(I, 1.0f / mass), Prod_Matrix(Rrx * I_now.inverse * Rrx, -1));
            Vector3 j = K.inverse * (van - va);

            //Update v and w
            v = v + 1 / mass * j;
            Vector3 tmp = I_now.inverse * Vector3.Cross(Ra, j);
            w = w + tmp;
        }
    }


    // Update is called once per frame
    void Update () 
	{
		if(Input.GetKey("r"))
		{
			transform.position = new Vector3 (0, 0.6f, 0);
			muN = 0.5f;
			launched=false;
		}
		if(Input.GetKey("l"))
		{
			v = new Vector3 (3, 2, 0);
			launched=true;
		}

        if (launched)
        {
			// Part I: Update velocities
			v += dt * g;
			v *= linear_decay;
			w *= angular_decay;

			// Part II: Collision Handler
			Collision_Handler(new Vector3(0, 0.01f, 0), new Vector3(0, 1, 0), dt);
			Collision_Handler(new Vector3(2, 0, 0), new Vector3(-1, 0, 0), dt);

			// Part III: Update position & orientation
			//Update linear status
			Vector3 x = transform.position;
			x += dt * v;
			//Update angular status
			Quaternion q = transform.rotation;
			Quaternion tmp = new Quaternion(w.x, w.y, w.z, 0);
			tmp = tmp * q;
			q.x += 0.5f * dt * tmp.x;
			q.y += 0.5f * dt * tmp.y;
			q.z += 0.5f * dt * tmp.z;
			q.w += 0.5f * dt * tmp.w;

			// Part IV: Assign to the bunny object
			transform.position = x;
			transform.rotation = q;
		}
	}
}
