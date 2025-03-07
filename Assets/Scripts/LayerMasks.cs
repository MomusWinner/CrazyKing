using UnityEngine;

public static class LayerMasks
{
    public static LayerMask Default { get; private set; }
    public static LayerMask King { get; private set; }
    public static LayerMask Enemy { get; private set; }
    public static LayerMask NeutralEntity { get; private set; }
    public static LayerMask Entity => King | Enemy | NeutralEntity;
    public static LayerMask HighBarrier { get; private set; }
    public static LayerMask LowBarrier { get; private set; }
    public static LayerMask Barrier => HighBarrier | LowBarrier;

    static LayerMasks()
    {
        Default = LayerMask.GetMask("Default");
        King = LayerMask.GetMask("King");
        Enemy = LayerMask.GetMask("Enemy");
        NeutralEntity = LayerMask.GetMask("NeutralEntity");
        HighBarrier = LayerMask.GetMask("HighBarrier");
        LowBarrier = LayerMask.GetMask("LowBarrier");
    }
    
}