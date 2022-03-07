using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public float lifePercent;
    public Transform lifeBarGfx;
    public Color NormalColor;
    public Color NonCatchableColor;
    public Color WeakColor;
    public RawImage lifeBarImg;

    public Canvas gfxContainer;
    public FloatVariable weaknessLevel;
    public bool showNonCatchable;
    // Start is called before the first frame update
    void Start()
    {
        lifePercent = 1.0f;
        setLifePercent(lifePercent);
        var lifeContainer = gameObject.GetComponentInParent<IHasLife>();
        if (lifeContainer != null) lifeContainer.registerToLifeChangeEvent(onLifeChanged);
        else Debug.Log("Lifebar parent doesn't contain IHasLife!");
    }

    public int onLifeChanged(float newPercent) {
        setLifePercent(newPercent);
        return 0;
    }

    private void setLifePercent(float newPercent) {
        lifePercent = newPercent;
        if (lifePercent >= 1) {
            gfxContainer.enabled = false;
            return;
        }
        gfxContainer.enabled = true;
        var newScale = lifeBarGfx.localScale;
        newScale.x = lifePercent;
        lifeBarGfx.localScale = newScale;
        setColor();
    }

    private void setColor() {
        if (lifePercent <= weaknessLevel.Value) {
            lifeBarImg.color = WeakColor;
        }else if (showNonCatchable) {
            lifeBarImg.color = NonCatchableColor;
        }else {
            lifeBarImg.color = NormalColor;
        }
    }
}
