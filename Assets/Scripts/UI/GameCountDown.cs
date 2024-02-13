using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCountDown : MonoBehaviour
{
    public bool IsCountDownOff;
    
    [SerializeField] private TextMeshProUGUI mainTexter;

    private Transform _transform;
    private SoundUI sound;


    private void Start()
    {
        IsCountDownOff = false;
        sound = SoundUI.Instance;
        _transform = mainTexter.transform;
        mainTexter.gameObject.SetActive(false);
    }

    public void StartCountDown()
    {
        StartCoroutine(play());
    }
    private IEnumerator play()
    {
        mainTexter.gameObject.SetActive(true);
        mainTexter.fontSize = 500;

        for (int i = 3; i > 0; i--)
        {
            mainTexter.text = i.ToString();
            _transform.localScale = Vector3.one * 0.1f;
            _transform.DORotate(new Vector3(0, 0, 2160), 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutSine);
            _transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);

            yield return new WaitForSeconds(0.5f);
            _transform.eulerAngles = Vector3.zero;
            sound.PlayUISound(SoundsUI.beep_tick);
            _transform.DOShakeScale(0.2f, 0.6f, 60).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.5f);            
        }

        mainTexter.fontSize = 200;
        mainTexter.text = Globals.Language.GOOOOO;//"œŒ≈’¿À»!";
        sound.PlayUISound(SoundsUI.beep_out);
        _transform.localScale = Vector3.one * 0.1f;
        _transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);
        _transform.DOShakeScale(0.2f, 0.6f, 60).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.7f);

        IsCountDownOff = true;

        _transform.DOScale(Vector3.one * 0.1f, 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);
        mainTexter.gameObject.SetActive(false);
    }
}
