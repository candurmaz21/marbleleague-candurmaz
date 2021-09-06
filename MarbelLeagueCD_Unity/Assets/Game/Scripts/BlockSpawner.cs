using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject blockObject;
    void Start()
    {
        //Spawn blocks on empty places
        CheckCollision();
    }
    void CheckCollision()
    {
        Vector3 tempPos = Vector3.zero;
        for(int i= -195; i<200; i+=3)
        {
            for(int z= -20; z>-400; z-=5)
            {
                tempPos =new Vector3(i,z,15.05f);
                Collider[] hitCollider = Physics.OverlapBox(tempPos,blockObject.transform.localScale,Quaternion.identity);
                if(hitCollider.Length>0)
                {
                    continue;
                }
                else
                {
                    Instantiate(blockObject,tempPos,Quaternion.identity);
                }
            }
        }
    }
}
