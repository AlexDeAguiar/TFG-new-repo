using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using Unity.Netcode;
using VivoxUnity;

public class VivoxPlayer : MonoBehaviour {
	public static VivoxPlayer Instance;

    private VivoxVoiceManager _vvm;
    IChannelSession _chan;
    private int PermissionAskedCount;
    [SerializeField] public string VoiceChannelName; //nombre del canal

    private Transform mainCam; //position of our Main Camera

    // Start is called before the first frame update
    void Start() {
		Instance = this;

		_vvm = VivoxVoiceManager.Instance;
        _vvm.OnUserLoggedInEvent  += OnUserLoggedIn;
        _vvm.OnUserLoggedOutEvent += OnUserLoggedOut;

        mainCam = GameObject.Find("MainCamera").transform;
    }

    public void SignIntoVivox () {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
    bool IsAndroid12AndUp() {
        // android12VersionCode is hardcoded because it might not be available in all versions of Android SDK
        const int android12VersionCode = 31;
        AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
        int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

        return buildSdkVersion >= android12VersionCode;
    }

    string GetBluetoothConnectPermissionCode() {
        if (IsAndroid12AndUp()) {
            // UnityEngine.Android.Permission does not contain the BLUETOOTH_CONNECT permission, fetch it from Android
            AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
            string permissionCode = manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");

            return permissionCode;
        }
        return "";
    }
#endif

        bool IsMicPermissionGranted() {
            bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp()) {
            // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
            return isGranted;
        }

        void AskForPermissions() {
            string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp()) {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
            PermissionAskedCount++;
            Permission.RequestUserPermission(permissionCode);
        }

        bool IsPermissionsDenied()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp()) {
            return PermissionAskedCount == 2;
        }
#endif
            return PermissionAskedCount == 1;
        }

		Debug.Log("Called vivoxPlayer.SignIntoVivox()");

        //Actual code runs from here
        if (IsMicPermissionGranted()) {
			Debug.Log("1st IF");
			_vvm.Login(transform.name.ToString());
        }
        else {
            if (IsPermissionsDenied()) {
				Debug.Log("2nd IF");
				PermissionAskedCount = 0;
                _vvm.Login(transform.name.ToString());
            }
            else {
				Debug.Log("3rd IF");
				AskForPermissions();
            }
        }
    }

    void OnUserLoggedIn () {
        if (_vvm.LoginState == VivoxUnity.LoginState.LoggedIn) {
            Debug.Log("Successfully connected to Vivox");
            Debug.Log("Joining voice channel: " + VoiceChannelName);
            _vvm.JoinChannel(VoiceChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly);

            var cid = new Channel(VoiceChannelName, ChannelType.Positional);
            _chan = _vvm.LoginSession.GetChannelSession(cid);
        }
        else {
            Debug.Log("Cannot sign into Vivox, check your credentials and token settings");
        }
    }

    void OnUserLoggedOut() {
        Debug.Log("Disconnecting from voice channel " + VoiceChannelName);
        _vvm.DisconnectAllChannels();
        Debug.Log("Disconnecting from Vivox");
        _vvm.Logout();  
    }

    // Update is called once per frame
    void Update(){
        float _nextUpdate = 0;

        if (_chan == null) { return; }
        
        if (_chan.ChannelState.ToString() == "Connected"){
            if (Time.time > _nextUpdate) {
                _chan.Set3DPosition(mainCam.position, mainCam.position, mainCam.forward, mainCam.up);
                _nextUpdate += 0.5f;
            }
        }
    }
}