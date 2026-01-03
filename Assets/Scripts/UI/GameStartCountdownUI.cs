using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI countDownText;

    const string NUMBER_POPUP = "NumberPopup";

    Animator animator;
    int previousCountdownNumber;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void Update()
    {
        var countDownNumber = (int)Math.Ceiling(GameManager.Instance.GetGameStartCountdown());
        if (previousCountdownNumber != countDownNumber)
        {
            // Turn on animator
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
            previousCountdownNumber = countDownNumber;
        }

        countDownText.text = countDownNumber.ToString();
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameStartCountDownActive())
            Show();
        else
            Hide();
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
