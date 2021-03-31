using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointIntime
{
   public Vector3 position;
   public Quaternion rotation ;
   public PointIntime( Vector3 _position , Quaternion _rotation){
       position = _position;
       rotation = _rotation;
   }
}
