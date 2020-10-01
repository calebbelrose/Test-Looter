using UnityEngine;
using System.Collections;

public class ResourceLevel : MonoBehaviour
{

    //Our renderer that'll make the top of the water visible
    LineRenderer Body;

    //Our physics arrays
    float[] xpositions;
    float[] ypositions;
    float[] targetYpositions;
    float[] velocities;
    float[] accelerations;

    //Our meshes and colliders
    GameObject[] meshobjects;
    GameObject[] colliders;
    Mesh[] meshes;

    //The material we're using for the top of the water
    public Material mat;

    //The GameObject we're using for a mesh
    public GameObject watermesh;

    //All our constants
    const float springconstant = 0.001f;
    const float damping = 0.04f;
    const float spread = 0.05f;
    const float z = -1f;

    //The properties of our water
    float baseheight = 0f;
    float left = -128f;
    float width = 256f;
    float bottom = -128f;
    float radiusSquared;
    float splashLocations = 0f;
    public float Top = 128f;


    void Start()
    {
        radiusSquared = (width / 2f);
        radiusSquared *= radiusSquared;
        //Spawning our water
        SpawnResource();
    }

    public void SetLevel(float level)
    {
        Top = width * level + bottom;
        splashLocations = Mathf.Sqrt(radiusSquared - Top * Top);
        
    }

    public void Splash(float xpos, float velocity)
    {
        //If the position is within the bounds of the water:
        if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length - 1])
        {
            //Offset the x position to be the distance from the left side
            xpos -= xpositions[0];

            //Find which spring we're touching
            int index = Mathf.RoundToInt((xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));

            //Add the velocity of the falling object to the spring
            velocities[index] += velocity;

            //Set the correct position of the particle system.
            Vector3 position = new Vector3(xpositions[index], ypositions[index] - 0.35f, 5);

            //This line aims the splash towards the middle. Only use for small bodies of water:
            Quaternion rotation = Quaternion.LookRotation(new Vector3(xpositions[Mathf.FloorToInt(xpositions.Length / 2)], baseheight + 8, 5) - position);
        }
    }

    public void SpawnResource()
    {
        //Calculating the number of edges and nodes we have
        float width = -left * 2;
        int edgecount = Mathf.RoundToInt(width) * 5;
        int nodecount = edgecount + 1;

        //Add our line renderer and set it up:
        Body = gameObject.AddComponent<LineRenderer>();
        Body.material = mat;
        Body.material.renderQueue = 1000;
        Body.positionCount = nodecount;
        Body.startWidth = 0.1f;
        Body.endWidth = 0.1f;

        //Declare our physics arrays
        xpositions = new float[nodecount];
        ypositions = new float[nodecount];
        targetYpositions = new float[nodecount];
        velocities = new float[nodecount];
        accelerations = new float[nodecount];

        //Declare our mesh arrays
        meshobjects = new GameObject[edgecount];
        meshes = new Mesh[edgecount];
        colliders = new GameObject[edgecount];

        //For each node, set the line renderer and our physics arrays
        for (int i = 0; i < nodecount; i++)
        {
            
            xpositions[i] = left + width * i / edgecount;
            ypositions[i] = Mathf.Sqrt(radiusSquared - xpositions[i] * xpositions[i]);
            targetYpositions[i] = Mathf.Sqrt(radiusSquared - xpositions[i] * xpositions[i]);
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
            accelerations[i] = 0;
            velocities[i] = 0;
        }

        //Setting the meshes now:
        for (int i = 0; i < edgecount; i++)
        {
            //Make the mesh
            meshes[i] = new Mesh();

            //Create the corners of the mesh
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], -ypositions[i], z);
            Vertices[3] = new Vector3(xpositions[i + 1], -ypositions[i + 1], z);

            //Set the UVs of the texture
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);

            //Set where the triangles should be.
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            //Add all this data to the mesh.
            meshes[i].vertices = Vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;

            //Create a holder for the mesh, set it to be the manager's child
            meshobjects[i] = Instantiate(watermesh, Vector3.zero, Quaternion.identity) as GameObject;
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.SetParent(transform, false);
        }
    }

    //Same as the code from in the meshes before, set the new mesh positions
    void UpdateMeshes()
    {
        for (int i = 0; i < meshes.Length; i++)
        {

            Vector3[] Vertices = new Vector3[4];
            if (xpositions[i] >= -splashLocations && xpositions[i] <= splashLocations)
            {

                Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
                Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
                Vertices[2] = new Vector3(xpositions[i], -targetYpositions[i], z);
                Vertices[3] = new Vector3(xpositions[i + 1], -targetYpositions[i + 1], z);
            }
            else if (Top >= 0)
            {
                Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
                Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
                Vertices[2] = new Vector3(xpositions[i], -targetYpositions[i], z);
                Vertices[3] = new Vector3(xpositions[i + 1], -targetYpositions[i + 1], z);
            }
            else
            {
                Vertices[0] = new Vector3(xpositions[i], Top, z);
                Vertices[1] = new Vector3(xpositions[i + 1], Top, z);
                Vertices[2] = new Vector3(xpositions[i], Top, z);
                Vertices[3] = new Vector3(xpositions[i + 1], Top, z);
            }

            meshes[i].vertices = Vertices;
        }
    }

    //Called regularly by Unity
    void FixedUpdate()
    {
        Splash(Random.Range(-splashLocations, splashLocations), Random.Range(0, splashLocations/20f));
        //Here we use the Euler method to handle all the physics of our springs:
        for (int i = 0; i < xpositions.Length; i++)
        {
            float force;
            if (xpositions[i] > -splashLocations && xpositions[i] < splashLocations)
                force = springconstant * (ypositions[i] - Top) + velocities[i] * damping;
            else
                force = springconstant * (ypositions[i] - targetYpositions[i]) + velocities[i] * damping;

            accelerations[i] = -force;
            ypositions[i] += velocities[i];
            velocities[i] += accelerations[i];
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
        }

        //Now we store the difference in heights:
        float[] leftDeltas = new float[xpositions.Length];
        float[] rightDeltas = new float[xpositions.Length];

        //We make 8 small passes for fluidity:
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (xpositions[i] > -splashLocations && xpositions[i] < splashLocations)
                {
                    //We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
                    if (i > 0)
                    {
                        leftDeltas[i] = spread * (ypositions[i] - ypositions[i - 1]);
                        velocities[i - 1] += leftDeltas[i];
                    }
                    if (i < xpositions.Length - 1)
                    {
                        rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                        velocities[i + 1] += rightDeltas[i];
                    }
                }
            }

            //Now we apply a difference in position
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                    ypositions[i - 1] += leftDeltas[i];
                if (i < xpositions.Length - 1)
                    ypositions[i + 1] += rightDeltas[i];
            }
        }
        //Finally we update the meshes to reflect this
        UpdateMeshes();
    }
}
