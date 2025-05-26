using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button SettingsButton;
    
    [SerializeField] private CanvasGroup GameOverMenu;
    [SerializeField] private CanvasGroup PauseMenu;
    
    private const float AnimationsDuration = 0.25f;

    public void HidePlayButton() => HideButton(PlayButton);
    public void ShowPlayButton() => ShowButton(PlayButton);
    
    public void HidePauseButton() => HideButton(PauseButton);
    public void ShowPauseButton() => ShowButton(PauseButton);

	public void HideSettingsButton() => HideButton(SettingsButton);
    public void ShowSettingsButton() => ShowButton(SettingsButton);

    public void ShowGameOverMenu() => ShowMenu(GameOverMenu);
    public void HideGameOverMenu() => HideMenu(GameOverMenu);

    public void ShowPauseMenu() => ShowMenu(PauseMenu);
    public void HidePauseMenu() => HideMenu(PauseMenu);

    private void HideButton(Button button)
    {
        button.interactable = false;
        
        var buttonTransform = button.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Append(buttonTransform.DOScale(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.Join(button.image.DOFade(0f, AnimationsDuration));
        seq.OnComplete(() => button.gameObject.SetActive(false));
    }
    
    private void ShowButton(Button button)
    {
        button.gameObject.SetActive(true);
        
        var buttonTransform = button.GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2f).SetEase(Ease.OutBack));
        seq.Join(button.image.DOFade(1f, AnimationsDuration));
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
        seq.Join(menu.DOFade(1f, AnimationsDuration).SetEase(Ease.OutExpo));
        seq.Append(menuTransform.DOAnchorPosY(0f, AnimationsDuration).SetEase(Ease.OutBack));
        seq.OnComplete(() => menu.interactable = true);
    }
}
