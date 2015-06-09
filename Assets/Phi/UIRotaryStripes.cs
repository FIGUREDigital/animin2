   using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
// Make the script also execute in edit mode.
[ExecuteInEditMode()]
public class UIRotaryStripes : MonoBehaviour
{
    [Serializable]
    public class Stripe
    {
        public float sizeRatio;
        public int subDivisions;
        public Color32[] colours = new Color32[2];
        public float[] radius = new float[1];
    }
    [Serializable]
    public class StripesDef
    {
        public Material material;
        public Stripe[] stripes = new Stripe[2];
        public int divisions = 10;
        public float startAngle = 0;
        public float endAngle = 360;
    }

    public StripesDef def = new StripesDef();

    Mesh mesh;
    MeshRenderer meshRenderer;
    Bounds bounds = new Bounds();
    
    void OnEnable()
    {
        CreateMesh();
    }

    protected void CreateMesh()
	{
		if (mesh != null)
		{
            UpdateMesh();
            return;
        }
		mesh = new Mesh();
        mesh.name = name;
		

		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		if (!meshFilter)
		{
			gameObject.AddComponent<MeshFilter>();
			meshFilter = gameObject.GetComponent<MeshFilter>();
		}

		meshFilter.sharedMesh = mesh;
        mesh.hideFlags = HideFlags.DontSave;

		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			gameObject.AddComponent<MeshRenderer>();
			meshRenderer = gameObject.GetComponent<MeshRenderer>();
		}


        UpdateMesh();
	}

    int[] indices;
    Color32[] colors32;
    Vector3[] vertices;
    public void UpdateMesh()
    {
        if (meshRenderer == null) return;
        float maxRadius = 0;
        meshRenderer.material = def.material;
        meshRenderer.enabled = enabled;
        int numTris = 0;
        int numVerts = 0;

        float normaliseRatios = 0;

        for (int s = 0; s < def.stripes.Length; s++)
        {
            Stripe stripe = def.stripes[s];
            normaliseRatios += stripe.sizeRatio;
            if (stripe.subDivisions > 0)
            {
                int prevVerts = 0;
                for (int r = 0; r < stripe.radius.Length; r++)
                {
                    int verts = 0;
                    int polys = 0;
                    if (stripe.radius[r] == 0)
                    {
                        verts = 1;
                    }
                    else
                    {
                        verts = def.divisions * stripe.subDivisions * def.divisions;
                    }
                    if (r > 0 && stripe.radius[r-1] != stripe.radius[r])
                    {
                        if (prevVerts > 1)
                        {
                            polys = (verts - 1) * 2 * stripe.subDivisions * def.divisions;
                        }
                        else
                        {
                            polys = (verts - 1) * stripe.subDivisions * def.divisions;
                        }
                    }
                    if (stripe.radius[r] > maxRadius)
                    {
                        maxRadius = stripe.radius[r];
                    }
                    prevVerts = verts;
                    numTris += polys;
                    numVerts += verts;
                }
            }
        }
        
        if (numVerts == 0)
        {
            meshRenderer.enabled = false;
            return;
        }

        if (vertices == null || vertices.Length != numVerts)
        {
            vertices = new Vector3[numVerts];
            colors32 = new Color32[numVerts];
        }

        if (indices == null || indices.Length != numVerts * 3)
        {
            indices = new int[numVerts * 3];
        }
        
        float ratioTally = 0;
        float da = (def.endAngle - def.startAngle) / def.divisions;
        float aratio = 0;
        int v = 0;
        int i = 0;
        for (int s = 0; s < def.stripes.Length; s++)
        {
            Stripe stripe = def.stripes[s];
            int centerIndex = v;
            if (stripe.radius[0] == 0)
            {
                vertices[v] = Vector3.zero;
                colors32[v] = stripe.colours[0];
                v++;
            }
            int vertsPerSpoke = stripe.radius.Length;
            if (stripe.radius[0] == 0)
            {
                vertsPerSpoke--;
            }
            float a = def.startAngle + (ratioTally / normaliseRatios) * da;
            ratioTally += stripe.sizeRatio;
            float a2 = def.startAngle + (ratioTally / normaliseRatios) * da;
            aratio += stripe.sizeRatio / normaliseRatios;
            for (int d = 0; d < def.divisions; d++)
            {
                float aa = d * (def.endAngle - def.startAngle) / def.divisions; 
                float aa2 = aa + a2;                                            // end angle for division
                aa += a;                                                        // start angle for division
                for (int sd = 0; sd <= stripe.subDivisions; sd++)
                {
                    float ang = (aa + ((aa2 - aa) * sd / stripe.subDivisions)) * Mathf.PI / 180; ;
                    float prevRadius = 0;
                    for (int r = 0; r < stripe.radius.Length; r++)
                    {
                        float radius = stripe.radius[r];
                        if (radius != 0)
                        {
                            // Add vert
                            vertices[v] = new Vector3(Mathf.Sin(ang) * radius, Mathf.Cos(ang) * radius, 0);
                            colors32[v++] = stripe.colours[r];
                        }

                        if (sd > 0 && r > 0 && prevRadius != radius)
                        {
                            if (prevRadius == 0)
                            {
                                indices[i++] = centerIndex;
                                indices[i++] = v - 1 - vertsPerSpoke;
                                indices[i++] = v - 1;
                            }
                            else
                            {                                
                                indices[i++] = v - 2  - vertsPerSpoke;
                                indices[i++] = v - 1  - vertsPerSpoke;
                                indices[i++] = v - 2;
                                        
                                indices[i++] = v - 1  - vertsPerSpoke;
                                indices[i++] = v - 1;
                                indices[i++] = v - 2;
                            }
                        }
                        prevRadius = radius;
                    }
                }
            }
        }
        	
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.colors32 = colors32;
		mesh.SetTriangles(indices, 0);

        meshRenderer.enabled = true;

        bounds.extents = new Vector3(maxRadius, maxRadius, 0.0f);
		bounds.center = Vector3.zero;
		mesh.bounds = bounds;
    }
}
