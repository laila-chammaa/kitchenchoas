using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]
    GameObject counterWithProgress;

    [SerializeField]
    Image barImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barImage.fillAmount = 0;
        Hide();

        var iHasProgress = counterWithProgress.GetComponent<IHasProgress>();
        if (iHasProgress == null)
        {
            Debug.LogWarning("This game object does not have need a progress bar.");
            return;
        }

        iHasProgress.OnProgressChanged += OnProgressChanged;
    }

    void OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        if (e.progressNormalized is 0 or 1)
            Hide();
        else
            Show();

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
