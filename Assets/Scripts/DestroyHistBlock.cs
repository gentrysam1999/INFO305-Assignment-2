using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHistBlock : MonoBehaviour
{
    float size = 0.0f;
    float prevSize = 0.0f;

    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        size = this.gameObject.transform.localScale.y;
        if(size == prevSize){
            count++;
            
        }else{
            count = 0;
        }
        if(count>100){
            Destroy(gameObject);
        }else{
            prevSize = size;
        }
        
    }
}
