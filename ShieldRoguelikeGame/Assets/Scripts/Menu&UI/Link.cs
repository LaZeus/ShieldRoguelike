using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour 
{

	public void GoToPatreon()
	{
    #if !UNITY_EDITOR
		openWindow("https://www.patreon.com/LaZeus");
    #endif
    }

    public void GoToTwitter()
	{
    #if !UNITY_EDITOR
		openWindow("https://twitter.com/TheLaZeus");
    #endif
    }

    public void GoToClinkTwitter()
	{
    #if !UNITY_EDITOR
		openWindow("https://twitter.com/ClinkmakesGames");
    #endif
    }

    public void GoToDiscord()
	{
    #if !UNITY_EDITOR
		openWindow("https://discord.gg/ceqfCKb");
    #endif
    }



    [DllImport("__Internal")]
	private static extern void openWindow(string url);

}