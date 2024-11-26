using UnityEngine;
using System.Runtime.InteropServices;

public  static class EnemyBehaviour 
{
    [DllImport("GE_DLL 4")]
    public static extern float AdjustEnemySpeed(float playerHealth, float baseSpeed);
}
