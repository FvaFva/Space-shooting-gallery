using System;

public interface IRechargeable
{
    public event Action<float> RechargeProgressChanged;
}