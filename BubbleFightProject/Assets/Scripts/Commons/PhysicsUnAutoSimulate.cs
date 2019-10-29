using UnityEngine;

/// <summary>
/// 物理演算を自動で実行しない
/// </summary>
public class PhysicsUnAutoSimulate : Singleton<PhysicsUnAutoSimulate>
{
    void Awake()
    {
        Physics.autoSimulation = false;

    }
    void LateUpdate()
    {
        Physics.Simulate(Time.deltaTime);
    }
}