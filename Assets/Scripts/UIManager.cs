using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject window1;
    [SerializeField] GameObject window2;
    [SerializeField] GameObject searchView;
    [SerializeField] GameObject readyView;
    [SerializeField] Texture[] windowTexture;
    [SerializeField] RawImage nowTex;
    [SerializeField] new AudioManager audio;
    private int window1Index;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        window1Index = 1;

        if (windowTexture.Length > 0 && nowTex != null)
        {
            nowTex.texture = windowTexture[0];
        }

        // --- 初期セットアップ ---
        if (window1 != null)
        {
            window1.SetActive(true);
            window1.transform.localScale = Vector3.zero; // 最初は小さく
            
            // Pop up animation
            window1.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        if (window2 != null)
        {
            window2.SetActive(false);
            window2.transform.localScale = Vector3.zero; // 準備
        }
    }

    // Nextボタン: 右から新しい画像が来る (コンテンツは左へハケる)
    public void ClickNextExplanation()
    {
        // 連続クリック防止があればベストだが、簡易実装としてそのまま進める
        
        window1Index++;
        if (window1Index <= windowTexture.Length)
        {
            // アニメーション付きで画像切り替え
            ChangeTextureAnimate(windowTexture[window1Index - 1], true).Forget();
        }
        else
        {
            // 終了演出
            FinishExplanationSequence().Forget();
        }
    }

    // Backボタン: 左から前の画像が来る (コンテンツは右へハケる)
    public void ClickBackExplanation()
    {
        window1Index--;
        if (window1Index < 1) window1Index = 1; // 下限ガード

        // アニメーション付きで画像切り替え (逆方向)
        ChangeTextureAnimate(windowTexture[window1Index - 1], false).Forget();
    }

    private async UniTask ChangeTextureAnimate(Texture nextTex, bool isNext)
    {
        if (nowTex == null) return;

        float moveDist = 500f; // スライド距離
        float duration = 0.25f;
        RectTransform rt = nowTex.rectTransform;

        // 1. 現画像をフェードアウト & 移動
        // Nextなら「左へ」(-moveDist), Backなら「右へ」(+moveDist) 去る
        float endX = isNext ? -moveDist : moveDist;
        
        await UniTask.WhenAll(
            nowTex.DOFade(0f, duration).AsyncWaitForCompletion().AsUniTask(),
            rt.DOAnchorPosX(endX, duration).SetEase(Ease.InQuad).AsyncWaitForCompletion().AsUniTask()
        );

        // 2. テクスチャ差し替え & 位置リセット
        nowTex.texture = nextTex;
        
        // Nextなら「右から」(+moveDist), Backなら「左から」(-moveDist) 登場
        float startX = isNext ? moveDist : -moveDist;
        rt.anchoredPosition = new Vector2(startX, rt.anchoredPosition.y);

        // 3. 新画像をフェードイン & 中央へ
        await UniTask.WhenAll(
            nowTex.DOFade(1f, duration).AsyncWaitForCompletion().AsUniTask(),
            rt.DOAnchorPosX(0f, duration).SetEase(Ease.OutQuad).AsyncWaitForCompletion().AsUniTask()
        );
    }

    private async UniTask FinishExplanationSequence()
    {
        if (window1 != null)
        {
            // Window1 を閉じる演出
            await window1.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).AsyncWaitForCompletion().AsUniTask();
            window1.SetActive(false);
        }

        await UniTask.Delay(500); // 少し間を作る

        if (window2 != null)
        {
            window2.SetActive(true);
            window2.transform.localScale = Vector3.zero;
            
            // Window2 を開く演出
            await window2.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion().AsUniTask();

            // カード出現などのトリガーがあればここで
            audio.PlayBgm(0f);
        }
    }

    public void ShowAfterDelay()
    {
        ShowAfterDelayAsync().Forget();
    }

    private async UniTask ShowAfterDelayAsync()
    {
        await UniTask.Delay(5000);      // 5秒待つ
        if (!searchView.activeSelf && readyView != null)
            readyView.SetActive(true);     // アクティブ化
    }


}
