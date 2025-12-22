using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class MoveDecoration : MonoBehaviour
{
    [Header("移動後の座標差分")]
    public float moveX = 1f;
    public float moveY = 1f;

    [Header("移動開始までの待機時間 (秒)")]
    public float delaySeconds = 1f;
    public float waitSeconds = 1f;

    [Header("移動にかける時間 (秒)")]
    public float duration = 0.5f;

    [Header("初期位置に戻る時間 (秒)")]
    public float returnDuration = 0.5f;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.localPosition; // ← 初期位置記録
        MoveAsync().Forget();
    }

    private async UniTask MoveAsync()
    {
        // 開始前待機
        await UniTask.Delay((int)(delaySeconds * 1000));

        // 移動先
        Vector3 targetPos = initialPos + new Vector3(moveX, moveY, 0f);

        // 本移動
        await transform.DOLocalMove(targetPos, duration)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();

        await UniTask.Delay((int)(waitSeconds * 1000));

        // 初期位置へ戻る
        await transform.DOLocalMove(initialPos, returnDuration)
            .SetEase(Ease.InOutCubic)
            .AsyncWaitForCompletion();
    }
}
