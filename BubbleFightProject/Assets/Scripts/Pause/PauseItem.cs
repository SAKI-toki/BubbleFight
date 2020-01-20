using UnityEngine;

public class PauseItem : MonoBehaviour
{
    [SerializeField]
    public PauseItem up = null, down = null, right = null, left = null;

    public delegate void FunctionType();
    FunctionType func = null;


    public void SetFuntion(FunctionType f)
    {
        func = f;
    }
    public void Execute()
    {
        func();
    }
}
