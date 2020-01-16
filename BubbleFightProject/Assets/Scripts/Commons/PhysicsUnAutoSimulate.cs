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

    public override void MyUpdate()
    {
        if (Time.deltaTime > 0.0f) Physics.Simulate(Time.deltaTime);
    }
}
