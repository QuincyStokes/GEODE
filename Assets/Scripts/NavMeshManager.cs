using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using Unity.VisualScripting;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
   private NavMeshSurface Surface2D;
   public static NavMeshManager instance;
    
    void Start()
    {
        instance = this;
        Surface2D = GetComponent<NavMeshSurface>();
        if (Surface2D != null) 
        {
            Surface2D.BuildNavMeshAsync();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Surface2D != null)
        {
            //Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        }
    }

    public void UpdateNevMesh() {
        if (Surface2D != null)
        {
            Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        }
    }
}
