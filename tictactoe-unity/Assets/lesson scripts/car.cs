using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : vehicle
{
    public car() : base()
    {
        Debug.Log(message:"the car is instatioated");
    }

    public car(string VehicleName) : Base(VehicleName)
    {
        Debug.Log(message:"a car with name", +vehicle=VehicleName+ "is instationated");
    }
    //dry
}
