using UnityEngine;
using System.Collections.Generic;

public class RoadNode : MonoBehaviour
{
    public List<RoadNode> neighbors = new List<RoadNode>();
    public Vector3 Position => transform.position;

}
