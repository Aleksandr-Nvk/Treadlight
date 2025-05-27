using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button PauseButton;
    
    [SerializeField] private Toggle SoundToggle;
    [SerializeField] private Sprite SoundOnSprite;
    [SerializeField] private Sprite SoundOffSprite;
    
    [SerializeField] private Button InfoButton;
    
    [SerializeField] private CanvasGroup GameOverMenu;
    [SerializeField] private CanvasGroup PauseMenu;

    [SerializeField] private TMP_Text ScoreCounterText;
    [SerializeField] private TMP_Text HighScoreText;

    private const float AnimationsDuration = 0.25f;
    private const float UITransparencyAlpha = 0.75f;

    public void HidePlayButton() => HideElement(PlayButton);
    public void ShowPlayButton() => ShowElement(PlayButton);
    
    public void HidePauseButton() => HideElement(PauseButton);
    public void ShowPauseButton() => ShowElement(PauseButton);

    public void ShowGameOverMenu() => ShowMenu(GameOverMenu);
    public void HideGameOverMenu() => HideMenu(GameOverMenu);

    public void ShowPauseMenu() => ShowMenu(PauseMenu);
    public void HidePauseMenu() => HideMenu(PauseMenu);

    public void ShowSoundToggle() => ShowElement(SoundToggle);
    public void HideSoundToggle() => HideElement(SoundToggle);

    public void ShowInfoButton() => ShowElement(InfoButton);
    public void HideInfoButton() => HideElement(InfoButton);
    
    public void ToggleSoundSprite() => SwapToggleSprite(SoundToggle, SoundOnSprite, SoundOffSprite);

    public void ShowHighestScoreText() => ShowText(HighScoreText);
    public void HideHighestScoreText() => HideText(HighScoreText);

    public void ShowScoreCounter() => ShowText(ScoreCounterText);
    public void HideScoreCounter() => HideText(ScoreCounterText);

    public void SetScoreText(int score) => ScoreCounterText.text = score.ToString();

    private void SwapToggleSprite(Toggle toggle, Sprite onSprite, Sprite offSprite)
    {
        var buttonTransform = toggle.GetComponent<RectTransform>();

        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(buttonTransform.DOScale(1f, AnimationsDuration).SetEase(Ease.OutBack));
        toggle.image.sprite = toggle.isOn ? onSprite : offSprite;
    }

    private void HideText(TMP_Text text)
    {
        if (!text.gameObject.activeSelf) return;
        
        var textTransform = text.GetComponent<RectTransform>();;
        var seq = DOTween.Sequence();
        seq.Append(textTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(textTransform.DOScale(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(text.DOFade(0f, AnimationsDuration));
        seq.OnComplete(() => text.gameObject.SetActive(false));
    }
    
    private void ShowText(TMP_Text text)
    {
        if (text.gameObject.activeSelf) return;

        text.gameObject.SetActive(true);
        var textTransform = text.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(textTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(text.DOFade(UITransparencyAlpha, AnimationsDuration));
        seq.Append(textTransform.DOScale(1f, AnimationsDuration).SetEase(Ease.OutBack));
    }
    
    private void HideElement(Selectable element)
    {
        if (!element.gameObject.activeSelf) return;

        element.interactable = false;
        
        var elementTransform = element.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(elementTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(elementTransform.DOScale(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(element.image.DOFade(0f, AnimationsDuration));
        seq.OnComplete(() => element.gameObject.SetActive(false));
    }
    
    private void ShowElement(Selectable element)
    {
        if (element.gameObject.activeSelf) return;
        
        element.gameObject.SetActive(true);
        
        var elementTransform = element.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(elementTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(element.image.DOFade(UITransparencyAlpha, AnimationsDuration));
        seq.Append(elementTransform.DOScale(1f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.OnComplete(() => element.interactable = true);
    }
    
    private void HideMenu(CanvasGroup menu)
    {
        if (!menu.gameObject.activeSelf) return;
        
        menu.interactable = false;
        
        var menuTransform = menu.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(menuTransform.DOAnchorPosY(Screen.height / 50f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(menuTransform.DOAnchorPosY(-Screen.height / 2f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(menu.DOFade(0f, AnimationsDuration).SetEase(Ease.InExpo));
        seq.OnComplete(() => menu.gameObject.SetActive(false));
    }
    
    private void ShowMenu(CanvasGroup menu)
    {
        if (menu.gameObject.activeSelf) return;
        
        menu.gameObject.SetActive(true);
        
        var menuTransform = menu.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(menuTransform.DOAnchorPosY(Screen.height / 50f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(menu.DOFade(UITransparencyAlpha, AnimationsDuration).SetEase(Ease.OutExpo));
        seq.Append(menuTransform.DOAnchorPosY(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.OnComplete(() => menu.interactable = true);
    }
}
