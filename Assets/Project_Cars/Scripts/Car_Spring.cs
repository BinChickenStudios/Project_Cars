[System.Serializable]
public class Car_Spring 
{
    public float restLength = .8f;                                           // the springs length at rest
    public float travelLength = .2f;                                         // the amount the spring can stretch (expand and contract)
    public float stiffness = 60000f;                                         // the stiffness of the spring (higher = more rigid) (if this is too low, it wont lift the car, if its too high it will make the car fly)
    public float damper = 4000f;                                             // the damper power of the spring (higher = more smoother) (if this is too low, the car will bounce, if its too high, it will make the car feel floaty)
    public float minForce = -2000f;                                          // the minimum amount of force that can be applied to the car (the more you lower this, the faster the car will fall, too high will make the car fly)
    public float maxForce = 10000f;                                          // the maximum amount of force that can be applied to the car (the higher you raise this, the higher the car will bounce when force is applied)

    public float minLength { get { return restLength - travelLength; } }     // the minimum length the spring can contract to
    public float maxLength { get { return restLength + travelLength; } }     // the maximum length the spring can expand to

}
