using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    [SerializeField] Image reloadImage;

    public void ChangeIcon(Sprite spr)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = spr;
    }

    public void Reload(float duration)
    {
        reloadImage.fillAmount = 1;
        StartCoroutine(ReloadCoroutine(duration));
    }

    IEnumerator ReloadCoroutine(float duration)
    {
        float fill = 1;
        while (fill > 0)
        {
            fill -= Time.deltaTime / duration;
            reloadImage.fillAmount = fill;

            yield return null;
        }
        reloadImage.fillAmount = 0;
    }
    
}
