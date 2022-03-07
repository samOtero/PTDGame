using System;

public interface IHasLife
{
    void registerToLifeChangeEvent(Func<float, int> listener);
    void unregisterToLifeChangeEvent(Func<float, int> listener);

    float getLifePercent();
}
