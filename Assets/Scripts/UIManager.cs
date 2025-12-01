using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject window1;
    [SerializeField] GameObject window2;
    [SerializeField] Texture[] windowTexture;
    [SerializeField] RawImage nowTex;
    [SerializeField] new AudioManager audio;
    [SerializeField] GameObject card;
    [SerializeField] GameObject mainCamera;
    private int window1Index;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        window1Index = 1;
        nowTex.texture = windowTexture[0];
        window1.SetActive(true);
        window2.SetActive(false);
    }

    public void ClickNextExplanation()
    {
        window1Index++;
        if (window1Index <= windowTexture.Length)
        {
            nowTex.texture = windowTexture[window1Index - 1];
        }
        else
        {
            DG.Tweening.DOVirtual.DelayedCall(1.5f, () =>
            {
                window1.SetActive(false);
                window2.SetActive(true);
                audio.PlayBgm(0f);
            });
        }
    }

    public void ClickBackExplanation()
    {
        window1Index--;
        if (window1Index > 1)
        {
            nowTex.texture = windowTexture[window1Index - 1];
        }
        else
        {
            window1Index = 1;
            nowTex.texture = windowTexture[window1Index - 1];
        }
    }


    public void HideFrameWhenCardIsChild(GameObject aimFrame)
    {
        if (card.transform.IsChildOf(mainCamera.transform))
        {
            aimFrame.gameObject.SetActive(false);
        }
        else
        {
            aimFrame.gameObject.SetActive(true);
        }
    }
}
