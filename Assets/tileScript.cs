using UnityEngine;

public class tileScript : MonoBehaviour
{
    public Material roadMat;
    public Material notRoadMat;
    
    public void makeRoad(Material roadMat) {
        GetComponent<Renderer>().material = roadMat;
    }
}
