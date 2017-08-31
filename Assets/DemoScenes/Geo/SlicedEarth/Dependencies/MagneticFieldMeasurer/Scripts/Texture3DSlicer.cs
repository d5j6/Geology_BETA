using UnityEngine;
using System.Collections;

public class Texture3DSlicer : MonoBehaviour {

    //public Texture2D sourceTexture;

    void Awake()
    {
        int depth = 8;
        Texture3D tex3d = new Texture3D(32, 32, depth, TextureFormat.Alpha8, false);
        Color[] colsSame = new Color[32 * 32];
        Color[] cols = new Color[32 * 32 * depth];
        int preMaxCount = 32 * 32 * (depth-1);
        int maxCount = 32 * 32 * depth;
        int minCount = 32 * 32;
        int counter = 0;
        for (counter = 0; counter < minCount; counter++)
        {
            colsSame[counter] = new Color(0f, 0f, 0f, Random.Range(0f, 1f));
        }

        for (int i = 0; i < counter; i++)
        {
            cols[i] = colsSame[i];
        }

        for (; counter < preMaxCount; counter++)
        {
            cols[counter] = new Color(0f, 0f, 0f, Random.Range(0f, 1f));
        }

        for (int i = 0; counter < maxCount; counter++, i++)
        {
            cols[counter] = colsSame[i];
        }

        /*for (int y = 0; y < tex3d.height; y++)
        {
            for (int x = 0; x < tex3d.width; x++)
            {
                cols[(tex3d.depth - 3) * x * y + x * y + x] = cols[x * y + x];
            }
        }

        for (int y = 0; y < tex3d.height; y++)
        {
            for (int x = 0; x < tex3d.width; x++)
            {
                cols[(tex3d.depth - 2) * x * y + x * y + x] = cols[x * y + x];
            }
        }*/

        /*for (int y = 0; y < tex3d.height; y++)
        {
            for (int x = 0; x < tex3d.width; x++)
            {
                cols[(tex3d.depth - 1) * x * y + x * y + x] = cols[x * y + x];
            }
        }*/

        tex3d.SetPixels(cols);
        tex3d.Apply();

        float offset = 1.0f / depth;
        offset /= 2f;

        GetComponent<Renderer>().material.SetTexture("_NoiseTex", tex3d);
        GetComponent<Renderer>().material.SetFloat("_NumOfNoiseSamples", tex3d.depth);
        GetComponent<Renderer>().material.SetFloat("_coeff1", offset);
        GetComponent<Renderer>().material.SetFloat("_coeff2", 1.0f - offset);
    }
}