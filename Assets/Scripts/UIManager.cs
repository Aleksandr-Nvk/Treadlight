using DG.Tweening;
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
    
    private const float AnimationsDuration = 0.25f;

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
    
    private void SwapToggleSprite(Toggle toggle, Sprite onSprite, Sprite offSprite)
    {
        var buttonTransform = toggle.GetComponent<RectTransform>();

        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(buttonTransform.DOScale(1f, AnimationsDuration).SetEase(Ease.OutBack));
        toggle.image.sprite = toggle.isOn ? onSprite : offSprite;
    }
    
    private void HideElement(Selectable button)
    {
        button.interactable = false;
        
        var buttonTransform = button.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(buttonTransform.DOScale(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(button.image.DOFade(0f, AnimationsDuration));
        seq.OnComplete(() => button.gameObject.SetActive(false));
    }
    
    private void ShowElement(Selectable button)
    {
        button.gameObject.SetActive(true);
        
        var buttonTransform = button.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(button.image.DOFade(0.8f, AnimationsDuration));
        seq.Append(buttonTransform.DOScale(1f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.OnComplete(() => button.interactable = true);
    }
    
    private void HideMenu(CanvasGroup menu)
    {
        menu.interactable = false;
        
        var menuTransform = menu.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(menuTransform.DOAnchorPosY(Screen.height / 50f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(menuTransform.DOAnchorPosY(-Screen.height / 2f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(menu.DOFade(0f, AnimationsDuration).SetEase(Ease.InExpo));
        seq.OnComplete(() => menuTransform.gameObject.SetActive(false));
    }
    
    private void ShowMenu(CanvasGroup menu)
    {
        menu.gameObject.SetActive(true);
        
        var menuTransform = menu.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(menuTransform.DOAnchorPosY(Screen.height / 50f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(menu.DOFade(0.9f, AnimationsDuration).SetEase(Ease.OutExpo));
        seq.Append(menuTransform.DOAnchorPosY(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.OnComplete(() => menu.interactable = true);
    }
}
