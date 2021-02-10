using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Monetizacao : MonoBehaviour, IUnityAdsListener
{
    string GooglePlay_ID = "3798365";
    bool GameTest = false;
    string myPlacementId = "rewardedVideo";

    [SerializeField] Jogador jogador;
    [SerializeField] GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID, GameTest);
    }

  public void Display_AD()
    {
        Advertisement.Show();
    }

    public void Display_VideoAD()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.


            Debug.LogWarning("voce conseguiu um premio por ter visto o video");

            gameManager.resetJogador();
            jogador.ResetJogador();
            
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.LogWarning("voce foi punido por pular o video");
            gameManager.resetJogador();
            jogador.ResetJogador();
            
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
           
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
