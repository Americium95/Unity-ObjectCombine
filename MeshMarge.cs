using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMarge : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Merge();

    }
	
    //
    public void Merge()
    {
        List<GameObject> RandObject=new List<GameObject>();
        List<Material> RandMat=new List<Material>();
        List< List<CombineInstance>> Cmbnis = new List<List<CombineInstance>>();


        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);


        int i = 0;

        while (i < meshFilters.Length)
        {
            if(!RandMat.Contains(meshFilters[i].GetComponent<Renderer>().sharedMaterial))
            {
                GameObject obj = new GameObject();
                obj.AddComponent<MeshRenderer>();
                obj.GetComponent<MeshRenderer>().material = meshFilters[i].GetComponent<Renderer>().sharedMaterial;
                obj.AddComponent<MeshFilter>();
                RandObject.Add(obj);
                RandMat.Add(meshFilters[i].GetComponent<Renderer>().sharedMaterial);
                Cmbnis.Add(new List<CombineInstance>());
            }
            
            if (gameObject != meshFilters[i].gameObject)
            {
                CombineInstance ncb = new CombineInstance();
                meshFilters[i].gameObject.transform.parent = transform;
                ncb.mesh = meshFilters[i].sharedMesh;
                Matrix4x4 tmp = Matrix4x4.identity;
                tmp.SetTRS(meshFilters[i].transform.position, meshFilters[i].transform.rotation, meshFilters[i].transform.lossyScale);

                ncb.transform = tmp;
                Cmbnis[RandMat.IndexOf(meshFilters[i].GetComponent<Renderer>().sharedMaterial)].Add(ncb);
            }

            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        for (int j = 0; j < RandObject.Count; j++)
        {
            RandObject[j].GetComponent<MeshFilter>().mesh.CombineMeshes(Cmbnis[j].ToArray());
            RandObject[j].transform.parent = transform;
        }
        transform.gameObject.SetActive(true);
    }
}
