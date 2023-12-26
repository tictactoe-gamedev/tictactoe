using UnityEngine;

public class Vehicle
{
    private int m_tireCount;
    private int m_MaxSpeed;

//encapsulation

//properties (accessors and mutators
public string Name { get; set; }
public string LicensePlate{ get; set;}

public int tireCount => m_tireCount;

public Vehicle()
{
    Debug.Log(message:"the vehicle is instantiated");
}
{
    m_Name = "null";
    m_licenseplate = "null";
    m_tireCount = 0;
    m_maxSpeed = 0;
}

public Vehicle{string vehicleName, int tireCount , int maxSpeed}
{
    nameof = vehicleName;
    this.tireCount = tireCount;
    m_maxSpeed = maxSpeed;
}

//return overloading 

public void speedUp()
{
    Debug.Log(message:"rnnrrnrnrnr i'm speeding up!");
}

public void speedup(string roadtype);
(
    Debug.Log(message:"rnnn rnnn i'm speeding up on "+roadtype+);
)

}
