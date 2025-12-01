using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public enum MotionType
{
    RiseMotion,         // 旧 PlayUpMotion
    SideWalkMotion,     // 旧 SideWalk
    LoopCircleMotion,   // 旧 SimpleCircle
    SnakeBackwardMotion // ★ 追加（蛇行しながら後退）
}

public class CardManager : MonoBehaviour
{
    [Header("Card Setting")]
    [SerializeField] private GameObject card;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject insideCard;
    [SerializeField] private GameObject charactorInCard;
    [SerializeField] private GameObject worldInCard;
    [SerializeField] private GameObject text;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;

    [Header("Animation Setting")]
    [SerializeField] private MotionType insideCardMotion = MotionType.RiseMotion;

    // -----------------------------------------------------------------------
    // ▼ Path Settings (Optional)
    // Inspectorで値が設定されている場合、コード内のデフォルト値ではなくこちらが使用されます。
    // -----------------------------------------------------------------------
    [Header("Path Overrides (Optional)")]
    [Tooltip("LoopCircleMotionの前半パス。空の場合はデフォルト値が使われます。")]
    [SerializeField] private Vector3[] pathFirstHalfSettings;

    [Tooltip("LoopCircleMotionの後半パス。空の場合はデフォルト値が使われます。")]
    [SerializeField] private Vector3[] pathSecondHalfSettings;

    [Tooltip("SnakeBackwardMotionの蛇行オフセット値（開始位置からの相対座標）。空の場合はデフォルト値が使われます。")]
    [SerializeField] private Vector3[] snakePathOffsets;
    // -----------------------------------------------------------------------

    private bool _flag = false;

    // 初期値保存
    private Vector3 cardInitPos, cardInitScale;
    private Quaternion cardInitRot;

    private Vector3 frameInitPos;
    private Quaternion frameInitRot;

    private Vector3 insideInitPos, insideInitScale;
    private Quaternion insideInitRot;

    private Vector3 textInitPos;
    private Quaternion textInitRot;

    private Vector3 charInitPos;
    private Vector3 worldInitPos;


    private void Start()
    {
        // RunSequenceAsync(this.gameObject).Forget();
    }

    public void StartAnimation(GameObject window2)
    {
        window2.SetActive(false);
        RunSequenceAsync(window2).Forget();
    }

    private async UniTask RunSequenceAsync(GameObject window2)
    {
        if (_flag) return;
        _flag = true;

        SaveInitialTransforms();

        try
        {
            await UniTask.Delay(3000);

            // --- ① カードを Z+1 へ ---
            await UniTask.WhenAll(
                card.transform.DOLocalMove(cardInitPos + new Vector3(0, 0, 1f), 0.6f)
                    .SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion()
                    .AsUniTask(),

                card.transform.DOScale(cardInitScale * 1.05f, 0.6f)
                    .SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion()
                    .AsUniTask()
            );

            // --- ② 停止 ---
            await UniTask.Delay(500);

            // --- ③ frame, text, inside ---
            await UniTask.WhenAll(
                frame.transform.DOLocalMove(frameInitPos + new Vector3(0, 0, -2.4f), 0.7f)
                    .SetEase(Ease.OutQuad)
                    .AsyncWaitForCompletion()
                    .AsUniTask(),

                text.transform.DOLocalMove(textInitPos + new Vector3(0, 0, -3f), 0.7f)
                    .SetEase(Ease.OutQuad)
                    .AsyncWaitForCompletion()
                    .AsUniTask(),

                insideCard.transform.DOLocalMove(insideInitPos + new Vector3(0, 0, -1.5f), 1f)
                    .SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion()
                    .AsUniTask(),

                insideCard.transform.DOScale(insideInitScale * 1.02f, 1f)
                    .SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion()
                    .AsUniTask()
            );

            // --- ④ insideCard motion ---
            switch (insideCardMotion)
            {
                case MotionType.RiseMotion:
                    await PlayRiseMotion();
                    break;
                case MotionType.SideWalkMotion:
                    await PlaySideWalkMotion();
                    break;
                case MotionType.LoopCircleMotion:
                    await PlayLoopCircleMotion();
                    break;
                case MotionType.SnakeBackwardMotion:
                    await PlaySnakeBackwardMotion();
                    break;
            }

            // --- ⑤ 初期位置へ戻す ---
            float r = 3f;
            await UniTask.WhenAll(
                card.transform.DOLocalMove(cardInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                card.transform.DOScale(cardInitScale, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                card.transform.DOLocalRotateQuaternion(cardInitRot, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),

                frame.transform.DOLocalMove(frameInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                frame.transform.DOLocalRotateQuaternion(frameInitRot, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),

                text.transform.DOLocalMove(textInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                text.transform.DOLocalRotateQuaternion(textInitRot, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                charactorInCard.transform.DOLocalMove(charInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                worldInCard.transform.DOLocalMove(worldInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),

                insideCard.transform.DOLocalMove(insideInitPos, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                insideCard.transform.DOScale(insideInitScale, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask(),
                insideCard.transform.DOLocalRotateQuaternion(insideInitRot, r).SetEase(Ease.InOutQuad).AsyncWaitForCompletion().AsUniTask()
            );

            await UniTask.Delay(2000);
            this.gameObject.SetActive(false);
            if (window2 != null) window2.SetActive(true);
        }
        finally
        {
            _flag = false;
        }
    }

    // -------------------------------------------------
    // ▼ BGM
    // -------------------------------------------------
    private void PlayMotionBGM()
    {
        if (bgmSource == null) return;

        // 多重再生を防ぐため、一度止めてから再生
        bgmSource.Stop();
        bgmSource.Play();
    }

    private async UniTask FadeOutAndStopBGM(float duration = 1f)
    {
        if (bgmSource == null) return;

        float startVolume = bgmSource.volume;

        await bgmSource.DOFade(0f, duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();

        bgmSource.Stop();
        bgmSource.volume = startVolume;   // 次回の再生のため元に戻す
    }


    // -------------------------------------------------
    // ▼ insideCard Motion
    // -------------------------------------------------

    // PlayLoopCircleMotion — 元の動作に合わせた修正版
    private async UniTask PlayLoopCircleMotion()
    {
        Transform t = insideCard.transform;

        // -------------------------------------------------------
        // パスの決定: Inspector設定があればそれを使い、なければデフォルト値を使う
        // -------------------------------------------------------
        Vector3[] pathFirstHalf;
        if (pathFirstHalfSettings != null && pathFirstHalfSettings.Length > 0)
        {
            pathFirstHalf = pathFirstHalfSettings;
        }
        else
        {
            pathFirstHalf = new[]
            {
                new Vector3(0f, 0.3f, -1f),
                new Vector3(0.1f, 0.3f, 0.2f),
                new Vector3(0f, 0f, -0.8f),
            };
        }

        Vector3[] pathSecondHalf;
        if (pathSecondHalfSettings != null && pathSecondHalfSettings.Length > 0)
        {
            pathSecondHalf = pathSecondHalfSettings;
        }
        else
        {
            pathSecondHalf = new[]
            {
                new Vector3(-0.5f, -0.3f, 0f),
                new Vector3(0f, -0.8f, -1f),
                new Vector3(0.4f, -0.6f, -1.7f),
                new Vector3(0f, 0f, 0f),
            };
        }
        // -------------------------------------------------------


        PlayMotionBGM();

        // ① 直線移動（あなたの元コードと同じ位置・時間）
        await t.DOLocalMove(new Vector3(-0.5f, 0.3f, -0.8f), 1f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion()
            .AsUniTask();

        // ② pathFirstHalf（10秒, CatmullRom, InOutSine）
        await t.DOLocalPath(pathFirstHalf, 12f, PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .SetOptions(false)
            .AsyncWaitForCompletion()
            .AsUniTask();

        // ③ pathSecondHalf（15秒, CatmullRom, OutCubic, closePath:true）
        await t.DOLocalPath(pathSecondHalf, 14f, PathType.CatmullRom)
            .SetEase(Ease.OutCubic)
            .SetOptions(false) // 元コード通り
            .AsyncWaitForCompletion()
            .AsUniTask();

        // ★ Motion 完了 → フェードアウト
        await FadeOutAndStopBGM(1f);
    }



    // 旧 SideWalk
    private async UniTask PlaySideWalkMotion()
    {
        Transform t = insideCard.transform;

        PlayMotionBGM();

        await t.DOLocalMove(new Vector3(-0.2f, 0, -1.0f), 2f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();
        await t.DOLocalMove(new Vector3(-1.0f, 0, 0), 3f)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
        await t.DOLocalMove(new Vector3(-4.0f, 0, 0), 15f)
            .SetEase(Ease.InOutQuad)
            .AsyncWaitForCompletion();
        await t.DOLocalMove(new Vector3(-1.5f, 0, 4f), 5f)
            .SetEase(Ease.InQuad)
            .AsyncWaitForCompletion();

        // ★ Motion 完了 → フェードアウト
        await FadeOutAndStopBGM(1f);
    }


    // 旧 PlayUpMotion
    private async UniTask PlayRiseMotion()
    {
        Transform t = worldInCard.transform;

        PlayMotionBGM();

        await t.DOLocalMove(new Vector3(0, -2f, -6.25f), 15f)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
        await t.DOLocalMove(new Vector3(0, -1.2f, -4.5f), 5f)
            .SetEase(Ease.InOutQuad)
            .AsyncWaitForCompletion();

        // ★ Motion 完了 → フェードアウト
        await FadeOutAndStopBGM(1f);
    }


    // ★ 新規：蛇行しながら後退する
    private async UniTask PlaySnakeBackwardMotion()
    {
        Transform t = insideCard.transform;

        // ① まず 0.5 秒で Z を -1 だけ動かす (元コードでコメントアウトされていたが基点計算用に使用)
        Vector3 firstPos = t.localPosition + new Vector3(0f, 0f, 0f);

        // -------------------------------------------------------
        // パスの決定: Inspector設定があればそれをオフセットとして使い、なければデフォルト値を使う
        // -------------------------------------------------------
        Vector3[] snakePath;

        if (snakePathOffsets != null && snakePathOffsets.Length > 0)
        {
            // Inspectorで指定されたオフセットを firstPos に加算してパスを作成
            snakePath = new Vector3[snakePathOffsets.Length];
            for (int i = 0; i < snakePathOffsets.Length; i++)
            {
                snakePath[i] = firstPos + snakePathOffsets[i];
            }
        }
        else
        {
            // デフォルトのオフセット
            snakePath = new[]
            {
                firstPos + new Vector3( 0.3f, 0.1f, 0.2f),
                firstPos + new Vector3(-0.3f, 0.2f, 0.4f),
                firstPos + new Vector3( 0.3f, 0.3f, 0.6f),
                firstPos + new Vector3(-0.3f, 0.5f, 0.8f),
                firstPos + new Vector3( 0f,   0.5f, 1f),
            };
        }
        // -------------------------------------------------------

        PlayMotionBGM();

        await t.DOLocalPath(snakePath, 22f, PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .SetOptions(false)
            .AsyncWaitForCompletion()
            .AsUniTask();

        // ★ Motion 完了 → フェードアウト
        await FadeOutAndStopBGM(1f);
    }



    // -----------------------------------------
    // ▼ Helper
    // -----------------------------------------
    private void SaveInitialTransforms()
    {
        cardInitPos = card.transform.localPosition;
        cardInitRot = card.transform.localRotation;
        cardInitScale = card.transform.localScale;

        frameInitPos = frame.transform.localPosition;
        frameInitRot = frame.transform.localRotation;

        insideInitPos = insideCard.transform.localPosition;
        insideInitRot = insideCard.transform.localRotation;
        insideInitScale = insideCard.transform.localScale;

        textInitPos = text.transform.localPosition;
        textInitRot = text.transform.localRotation;

        charInitPos = charactorInCard.transform.localPosition;
        worldInitPos = worldInCard.transform.localPosition;
    }
}