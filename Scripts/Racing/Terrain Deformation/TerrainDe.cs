using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDe : MonoBehaviour
    {
        public bool Deform;
        public float deep;
        public float deepMultiplier=1;
            public Vector2 offset;
            public int TerrainHeight=15;
            public Vector2 TerrainSize;
     public int forwardMultiply;
    public Vector2Int position_WB;
    public Vector2Int position_WF;
    private float[,] Height;
    
    public TerrainData TerrainDataDirt;
    public Terrain TerrainSelect;
    private float heightafter_Rear = 0f;
    private float heightafter_Front = 0f;
    public GameObject Bike;
    private float bikepower;

    public float LerpFinal_Rear;
    // Start is called before the first frame update
    void Start()
    {
        TerrainSelect = this.GetComponent<Terrain>();
        Bike = GameObject.FindGameObjectWithTag("Bike");
     StartCoroutine("UpdateTerrain");
    }

     
 IEnumerator UpdateTerrain() {
     for(;;) {
         // execute block of code here
        TerrainDataDirt.SyncHeightmap();
        Debug.Log("Updated Lod");
       
        
         yield return new WaitForSeconds(5f);
     }
 }
    // Update is called once per frame

    void Update() {

        
//Debug.Log(TerrainDataDirt.heightmapResolution);

        if(Bike!=null){
        position_WB = new Vector2Int(
            Mathf.RoundToInt((Bike.GetComponent<Ride>().BackWheel.transform.position.x-(Bike.transform.forward.x*offset.x))*TerrainDataDirt.heightmapResolution/TerrainSize.x),
            Mathf.RoundToInt((Bike.GetComponent<Ride>().BackWheel.transform.position.z-(Bike.transform.forward.z*offset.y))*TerrainDataDirt.heightmapResolution/TerrainSize.y));
      
             position_WF = new Vector2Int(
            Mathf.RoundToInt((Bike.GetComponent<Ride>().FrontWheel.transform.position.x)*TerrainDataDirt.heightmapResolution/TerrainSize.x),
            Mathf.RoundToInt((Bike.GetComponent<Ride>().FrontWheel.transform.position.z)*TerrainDataDirt.heightmapResolution/TerrainSize.y));
        }else{
            position_WB = new Vector2Int(0,0);
            position_WF = new Vector2Int(0,0);
        }

        float[,] Height_rear = new float[1,1];
        float[,] Height_front = new float[1,1];
         bikepower = Bike.GetComponent<Ride>().Wheel_f.rpm / Bike.GetComponent<Ride>().WheelSize - Bike.GetComponent<Rigidbody> ().velocity.magnitude;
       
        
        float terraincurrentheight_rear = TerrainDataDirt.GetHeight(position_WB.x,position_WB.y)/TerrainHeight;
        float terraincurrentheight_front = TerrainDataDirt.GetHeight(position_WF.x,position_WF.y)/TerrainHeight;
/*
        if(terraincurrentheight-deep < heightafter){
        heightafter=terraincurrentheight;
        }else{

            heightafter = terraincurrentheight-0.1f;
        }
*/
//rear
if((Bike.GetComponent<Ride>().SpringPosition.y/TerrainHeight)-(deep*deepMultiplier) < (Bike.GetComponent<Ride>().BackWheel.transform.position.y/TerrainHeight)-0.0008f 
&& (Bike.GetComponent<Ride>().SpringPosition.y/TerrainHeight)-(deep*deepMultiplier) > (Bike.GetComponent<Ride>().BackWheel.transform.position.y/TerrainHeight)-0.2f){
  heightafter_Rear = (Bike.GetComponent<Ride>().BackWheel.transform.position.y/TerrainHeight)-((deep*deepMultiplier)+(bikepower/900000));
//Debug.Log((Bike.GetComponent<Ride>().SpringPosition/TerrainHeight)-deep+" TH " + (Bike.GetComponent<Ride>().BackWheel.transform.position.y/TerrainHeight) + " BK" +"Bkpower=" + bikepower+"(Deforming)");
}else{
    heightafter_Rear = terraincurrentheight_rear;
  //  Debug.Log((Bike.GetComponent<Ride>().SpringPosition/TerrainHeight)-deep+" TH " + (Bike.GetComponent<Ride>().BackWheel.transform.position.y/TerrainHeight) + " BK" +"Bkpower=" + bikepower+"(Nop)");
}

       //front
if((Bike.GetComponent<Ride>().SpringPosition.x/TerrainHeight)-(deep*deepMultiplier) < (Bike.GetComponent<Ride>().FrontWheel.transform.position.y/TerrainHeight)-0.0008f 
&& (Bike.GetComponent<Ride>().SpringPosition.x/TerrainHeight)-(deep*deepMultiplier) > (Bike.GetComponent<Ride>().FrontWheel.transform.position.y/TerrainHeight)-0.5f){
  heightafter_Front = (Bike.GetComponent<Ride>().FrontWheel.transform.position.y/TerrainHeight)-(((deep*1.3f)*deepMultiplier));
}else{
    heightafter_Front = terraincurrentheight_front;
  
}


      
      

        //Rear    
        
        LerpFinal_Rear = Mathf.Lerp(terraincurrentheight_rear,heightafter_Rear,Time.deltaTime*7);
//     Debug.Log(bikepower);
     Height_rear[0,0] = heightafter_Rear;
    //Height[0,0] =heightafter+(deep/2);
   // Height_rear[1,0]= heightafter_Rear;
    //Height_rear[2,0] = heightafter_Rear;
    //Height_rear[0,1]= heightafter_Rear;
    //Height_rear[0,2] = heightafter_Rear;
   // Height_rear[1,1] = LerpFinal_Rear;
  //  Height_rear[1,2]= heightafter_Rear;
  //  Height_rear[2,1]= heightafter_Rear;
  //  Height_rear[2,2] = heightafter_Rear;

       
 //front
 
  Height_front[0,0] = heightafter_Front;


//Debug.Log(bikepower);

//rear
//if(bikepower>40f || bikepower<-12f || Bike.GetComponent<Ride>().SideSlipping){



if(Deform && Bike.GetComponent<Ride>().RearWheelTouchGround() == true && Bike.GetComponent<TireWhear_test>().Dirt==true ){
      TerrainDataDirt.SetHeightsDelayLOD(position_WB.x,position_WB.y,Height_rear);
}else{
    LerpFinal_Rear=terraincurrentheight_rear;
}
//front
   if(Deform && Bike.GetComponent<Ride>().FrontWheelTouchGround() == true && Bike.GetComponent<TireWhear_test>().Dirt==true ){
     TerrainDataDirt.SetHeightsDelayLOD(position_WF.x,position_WF.y,Height_front);
}   


     
    //}
    }
     
}
