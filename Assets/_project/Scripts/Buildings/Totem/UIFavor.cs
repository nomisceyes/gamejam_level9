using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFavor : MonoBehaviour
{
    public Totem Totem;
    public Slider FavorSlider;
    public float FillingSpeed = 15f;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        Totem.OnFavorChange += UpdateAmount;
        
        FavorSlider.maxValue = Totem.MaxFavor;
        FavorSlider.SetValueWithoutNotify(Totem.CurrentFavor);
    }

    private void UpdateAmount()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SmoothFill());
    }

    private IEnumerator SmoothFill()
    {
        while (Mathf.Approximately(FavorSlider.value, Totem.CurrentFavor) == false)
        {
            FavorSlider.value = Mathf.MoveTowards(FavorSlider.value, Totem.CurrentFavor, FillingSpeed * Time.deltaTime);

            yield return null;
        }
    }
}