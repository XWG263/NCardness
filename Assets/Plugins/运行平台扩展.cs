using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

#if (UNITY_IOS && !UNITY_EDITOR)
using System.Runtime.InteropServices;
#endif

public class 运行平台扩展 : MonoBehaviour
{
#if (UNITY_IOS && !UNITY_EDITOR)
    [DllImport ("__Internal")]
    private static extern void requestReview();
#endif

    public static void Request( )
    {
#if (UNITY_IOS && !UNITY_EDITOR)
		Debug.Log("Trying to request the review window.");
	    requestReview();
#endif
    }

    static public void 显示错误提示( string 标题, string 内容 )
    {
        showNativeAlert( 标题, UnityWebRequest.UnEscapeURL( 内容 ) );
    }

    public static void 显示错误提示( Exception 异常 )
    {
        showNativeAlert( 异常.ToString( ), 异常.Message );
    }

#if UNITY_EDITOR
    static public void showNativeAlert( string title, string message )
    {
        UnityEditor.EditorUtility.DisplayDialog( title, message, "OK" );
    }

#elif UNITY_IOS
	[DllImport("__Internal")]
    extern static public void showNativeAlert(string title, string message);
#else
    static public void showNativeAlert( string title, string message )
    {
        AndroidJavaClass jc = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>( "currentActivity" );

        jo.Call( "openAlert", title, message );
    }
#endif

}