using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class Ads : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //[SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId; // This will remain null for unsupported platforms

    public bool ads_for_hearts;
    public bool ads_for_coins;

    
    private void Start()
    {
        ads_for_hearts = false;
        ads_for_coins = false;
        
    }

    public static Ads Instance;
    void Awake()
    {
        Instance = this;
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
    _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
    _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
       // _showAdButton.interactable = false;
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            //_showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        MusicManager.instance.audioSource_music.mute = true;

        // Disable the button:
        //_showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {

            Debug.Log("Unity Ads Rewarded Ad Completed");

            StartCoroutine(giveReward());
            //MusicManager.instance.audioSource_music.mute = false;
            
            // Grant a reward.
        }
    }

    [SerializeField] GameObject _3xanimPrefab;
    IEnumerator giveReward()
    {
        if (ads_for_hearts)
        {
            HeartRefillSystem.instance.currentHearts += 1;
            HeartRefillSystem.instance.SaveData();
            HeartRefillSystem.instance.EarnAheart_screen.SetActive(true);
            MusicManager.instance.PlayClip(8);
            ads_for_hearts = false;
        }
        else if (ads_for_coins)
        {
            int will_get_coins = CoinsManager.instance.will_get_coins;
            int max = will_get_coins * 3;
            GameObject temp = Instantiate(_3xanimPrefab, CanvasManager.instance.transform);
            Destroy(temp, 1.5f);
            MusicManager.instance.PlayClip(9);
            yield return new WaitForSeconds(1.5f);
            for (int i = will_get_coins; i <max; i++)
            {
                CoinsManager.instance.coins_get_txt.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                yield return new WaitForSeconds(.01f);

                
                will_get_coins++;
                CoinsManager.instance.coins_get_txt.text = "+" + will_get_coins.ToString();
               //yield return null;
            }


            CoinsManager.instance.coins_get_txt.transform.localScale = new Vector3(1f, 1f, 1f);

        }
        
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
       // _showAdButton.onClick.RemoveAllListeners();
    }
}
