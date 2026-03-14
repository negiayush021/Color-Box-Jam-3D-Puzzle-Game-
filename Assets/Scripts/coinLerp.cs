using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinLerp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        
        StartCoroutine(Lerp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //[SerializeField] RectTransform target;
    IEnumerator Lerp()
    {
        print("coin");
        yield return new WaitForSeconds(.5f);
        print("finish");
        if (Ads.Instance.ads_for_coins)
        {
            CoinsManager.instance.AddCoins(9);
            
        }
        else
        {
            CoinsManager.instance.AddCoins(3);
        }


        if (CanvasManager.instance.canVibrate)
        {
            GameManager.Instance.LightImpactHaptic();
        }
        MusicManager.instance.PlayClip(0);
        Vector3 imagnary_point = GameManager.Instance.final.position + new Vector3(0,GameManager.Instance.final.anchoredPosition.y -1500,0);
        if (CanvasManager.instance.canVibrate)
        {
            GameManager.Instance.LightImpactHaptic();
        }
        float time = 0;
        while (time < .5f)
        {
            transform.position = Vector3.Lerp(transform.position , imagnary_point, time/1f);
            imagnary_point = Vector3.Lerp(imagnary_point, GameManager.Instance.final.position + new Vector3(0, GameManager.Instance.final.anchoredPosition.y +80f, 0), time / .8f);
            time += Time.deltaTime;
            yield return null;
        }
        GameObject glow = Instantiate(GameManager.Instance.glow_prefab, GameManager.Instance.coinstorage);
        Destroy(glow, .3f);
        
        transform.position = GameManager.Instance.final.position + new Vector3(0, GameManager.Instance.final.anchoredPosition.y +80f, 0);
        
        
    }
}
