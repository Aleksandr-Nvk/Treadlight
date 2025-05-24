using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
 
    private const float AnimationsDuration = 0.25f;

    public void HidePlayButton() => HideButton(PlayButton);

    private void HideButton(Button button)
    {
        var buttonTransform = button.GetComponent<RectTransform>();
        button.interactable = false;
        var seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOScale(1.15f, AnimationsDuration / 2).SetEase(Ease.OutBack));
        seq.Append(buttonTransform.DOScale(0f, AnimationsDuration).SetEase(Ease.InBack));
        seq.Join(button.image.DOFade(0f, AnimationsDuration));
        seq.OnComplete(() =>
        {
            button.gameObject.SetActive(false);
        });
    }
}
