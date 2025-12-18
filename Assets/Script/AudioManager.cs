using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public bool isMute;
    public AudioSource audioSource;
    public Image volumeImage;
    public Sprite mute, unMute;

    public Button volumeBtn;

    void Start()
    {
        volumeBtn.onClick.AddListener(VolumeControls);
    }
    public void VolumeControls()
    {
        isMute = !isMute;
        if (isMute)
        {
            volumeBtn.transform.localScale = Vector3.zero;
            volumeBtn.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutFlash);
            audioSource.volume = 0f;
            volumeImage.sprite = mute;
        }
        else
        {
            volumeBtn.transform.localScale = Vector3.zero;
            volumeBtn.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutFlash);
            audioSource.volume = 1f;
            volumeImage.sprite = unMute;
        }
    }
}
