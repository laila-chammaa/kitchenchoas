using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]
    CuttingCounter cuttingCounter;

    [SerializeField]
    Image barImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barImage.fillAmount = 0;
        Hide();
        cuttingCounter.OnCuttingProgressChanged += CuttingCounterOnOnCuttingProgressChanged;
    }

    void CuttingCounterOnOnCuttingProgressChanged(object sender, CuttingCounter.OnCuttingProgressChangedEventArgs e)
    {
        if (e.progressNormalized > 0)
            Show();
        else
            Hide();

        barImage.fillAmount = e.progressNormalized;
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
