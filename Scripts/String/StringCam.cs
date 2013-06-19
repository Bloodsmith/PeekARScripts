#if !UNITY_EDITOR
#if UNITY_IPHONE
#define UNITY_MOBILE
#elif UNITY_ANDROID
#define UNITY_MOBILE
#endif
#endif

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

#if UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0
#error Unity 3.4.x and older is not supported by this version of String. Please upgrade Unity.
#endif

public class StringCam : MonoBehaviour
{	
	[StructLayout(LayoutKind.Sequential)]
	public struct MarkerInfo
	{
		public Quaternion rotation;
		public Vector3 position;
		Vector3 _color;
		
		public uint imageID;
		public uint uniqueInstanceID;

		public Color color
		{
			get
			{
				return new Color(_color.x, _color.y, _color.z);
			}
		}
	}
	
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct DeviceInfo
	{
		const int stringLength = 100;
		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = stringLength)]
		public string name;
		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = stringLength)]
		public string id;
		
		[MarshalAs(UnmanagedType.I1)]
		public bool isAvailable;
	}
	
#if UNITY_EDITOR
	const string DLL_NAME = "String";
#elif UNITY_IPHONE
	const string DLL_NAME = "__Internal";
#elif UNITY_ANDROID
	const string DLL_NAME = "String";
#endif

#if UNITY_EDITOR
	class DesktopWrapper
	{
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
	    public static extern bool InitTracker([MarshalAs(UnmanagedType.LPStr)]string deviceId, ref uint width, ref uint height, float verticalFOV);
	
		[DllImport(DLL_NAME)]
		public static extern void DeinitTracker();
	
		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		public static extern int LoadMarkerImage([MarshalAs(UnmanagedType.LPStr)]string fileName);
	
		[DllImport(DLL_NAME)]
		public static extern bool ProcessFrame(uint textureId, uint debugTextureId);
	
		[DllImport(DLL_NAME)]
		public static extern uint GetDataQuaternionBased([Out]MarkerInfo[] markerInfo, uint maxMarkerCount);
	
		[DllImport(DLL_NAME)]
		public static extern bool IsNewFrameReady();
	
		[DllImport(DLL_NAME)]
		public static extern uint EnumerateDevices([Out]DeviceInfo[] deviceInfo, uint maxDeviceCount);
	
		[DllImport(DLL_NAME)]
		public static extern uint GetInterfaceVersion();
	}
#elif UNITY_MOBILE
	class MobileWrapper
	{	
		[DllImport(DLL_NAME)]
		public static extern void String_EnableAR(bool enable);

	    [DllImport(DLL_NAME)]
	    public static extern bool String_Process();

		[DllImport(DLL_NAME)]
	    public static extern bool String_Render();

		[DllImport(DLL_NAME)]
		public static extern uint String_GetData([Out]MarkerInfo[] markerInfo, uint maxMarkerCount);

		[DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
		public static extern int String_LoadMarkerImage([In, MarshalAs(UnmanagedType.LPStr)]string fileName);
		
		[DllImport(DLL_NAME)]
		public static extern void String_UnloadMarkerImages();
	
		[DllImport(DLL_NAME)]
		public static extern void String_GetProjectionMatrix(out Matrix4x4 matrix);

		[DllImport(DLL_NAME)]
		public static extern void String_SetClippingPlanes(float near, float far);

        [DllImport(DLL_NAME)]
		public static extern void String_SetVideoTextureNames(uint first, uint second);

	    [DllImport(DLL_NAME)]
		public static extern bool String_GetCurrentVideoTexture(out uint textureName, out Matrix4x4 viewToVideoTextureTransform);
		
	    [DllImport(DLL_NAME)]
		public static extern void String_EnableWatermark(bool enable);		

		// This is a dummy function to make sure you're linking against 
		// a compatible version of the String for Unity library.
		[DllImport(DLL_NAME)]
		public static extern void String_Mobile_Library_Interface_Version_5();
	}
#endif
	
	// For ensuring on mobile that we process each frame only once
	int lastFrameProcessed;
	
	// To prevent duplicate instantiation
	static bool wasInstantiated = false;
	
	// Marker related
	const uint maxMarkerCount = 20;
	MarkerInfo[] markerInfo   = new MarkerInfo[maxMarkerCount];
	uint markerCount          = 0;
	
	// Only used on platforms that support video textures
	Texture2D[] videoTextures;
	
	// Editor preview use only
	public string previewCamName;
	public float previewCamApproxVerticalFOV = 36.3f;
#if UNITY_EDITOR
	uint previewWidth  = 640;
	uint previewHeight = 480;
	Material videoMaterial;
	Mesh videoPlaneMesh;
	GameObject videoPlaneObject;
#endif

	protected void Start()
	{
		if (wasInstantiated)
			throw new System.InvalidOperationException("StringCam was already instantiated. Only one instance of StringCam may be attached to a game object at any given time.");
		
		if (camera == null)
			throw new System.InvalidOperationException("StringCam component was attached to a GameObject with no Camera component. Please add a Camera component.");
		
		wasInstantiated = true;
		
#if UNITY_MOBILE
		camera.clearFlags = CameraClearFlags.Nothing; 
		camera.rect = new Rect(0, 0, 1, 1);
		MobileWrapper.String_EnableAR(true);
		MobileWrapper.String_UnloadMarkerImages();
		MobileWrapper.String_SetClippingPlanes(camera.nearClipPlane, camera.farClipPlane);
#if UNITY_IPHONE
		videoTextures = new Texture2D[] {new Texture2D(1, 1, TextureFormat.ARGB32, false), new Texture2D(1, 1, TextureFormat.ARGB32, false)};
		videoTextures[0].wrapMode = TextureWrapMode.Clamp;
		videoTextures[1].wrapMode = TextureWrapMode.Clamp;
		MobileWrapper.String_SetVideoTextureNames((uint)videoTextures[0].GetNativeTextureID(), (uint)videoTextures[1].GetNativeTextureID());
#endif	
#else
		try
		{
			InitializePreviewPlugin(previewCamName);
		}
		catch 
		{
			Debug.LogWarning("Couldn't initialize String preview plugin. " +
				"If you're *not* running Unity Pro, your editor doesn't support plugins, and you can safely ignore this message. You will still be able to deploy to iOS. " +
				"If you *are* running Unity Pro, please make sure you've added String.bundle from the SDK to your project and that you have a working camera attached to your computer.");
		}
#endif
	}
	
	void OnDestroy()
	{
#if UNITY_MOBILE
		MobileWrapper.String_EnableAR(false);
#else
		DesktopWrapper.DeinitTracker();
#endif
		wasInstantiated = false;
	}
	
	void OnPreRender()
	{
#if UNITY_MOBILE
		MobileWrapper.String_Render();
#endif
	}

	protected void Update()
	{
#if UNITY_MOBILE
		ProcessIfNecessary();
#else
		if (DesktopWrapper.IsNewFrameReady())
		{
			DesktopWrapper.ProcessFrame((uint)videoTextures[0].GetNativeTextureID(), 0);
			markerCount = DesktopWrapper.GetDataQuaternionBased(markerInfo, maxMarkerCount);
			videoPlaneObject.active = true;
		}
#endif
	}

#if UNITY_EDITOR
	void CreateVideoMaterial() 
	{
        videoMaterial = new Material(
			"Shader \"VideoFrameShader\" {" +
			"Properties { _MainTex (\"Base (RGB)\", 2D) = \"white\" {} }" +
            "SubShader { Pass {" +
            "    Blend Off" +
            "    ZTest Always ZWrite Off Cull Off Lighting Off Fog { Mode Off }" +
			"    SetTexture[_MainTex] { combine texture }" +
			"} } }" );
        videoMaterial.hideFlags = HideFlags.HideAndDontSave;
        videoMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		
		videoTextures = new Texture2D[] {new Texture2D(1, 1, TextureFormat.ARGB32, false)};
		videoTextures[0].wrapMode = TextureWrapMode.Clamp;
		videoMaterial.SetTexture("_MainTex", videoTextures[0]);
		videoMaterial.renderQueue = 0;
	}
	
	void CreateVideoMesh()
	{
		videoPlaneMesh = new Mesh();
		
		videoPlaneMesh.vertices = new Vector3[] {
			new Vector3(-1, -1, 0),
			new Vector3(1, -1, 0),
			new Vector3(-1, 1, 0),
			new Vector3(1, 1, 0)};
		
		videoPlaneMesh.uv = new Vector2[] {
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(0, 0),
			new Vector2(1, 0)};
		
		videoPlaneMesh.SetTriangles(new int[] {0, 1, 2, 1, 3, 2}, 0);
	}
	
	void InitializePreviewPlugin(string preferredDeviceName)
	{
		// Test library compatibility
		if (DesktopWrapper.GetInterfaceVersion() != 3)
		{
			Debug.LogError("You appear to be using incompatible versions of StringCam.cs and String.bundle; Please make sure you're using the latest versions of both.");
			
			return;
		}

		// Enumerate devices
		uint maxDeviceCount = 10;
		DeviceInfo[] deviceInfo = new DeviceInfo[maxDeviceCount];
		
		uint deviceCount = DesktopWrapper.EnumerateDevices(deviceInfo, maxDeviceCount);
		
		if (deviceCount > 0)
		{
			uint i;
			
			for (i = 0; i < deviceCount; i++)
			{
				Debug.Log("Found camera \"" + deviceInfo[i].name + "\" (" + (deviceInfo[i].isAvailable ? "available for use.)" : "not available for use.)"));
			}
		
			for (i = 0; i < deviceCount; i++)
			{
				if (deviceInfo[i].name == preferredDeviceName) break;
			}
			
			if (i < deviceCount)
			{
				Debug.Log("Capturing video from preferred device \"" + deviceInfo[i].name + "\".");
			}
			else
			{
				i = 0;

				if (preferredDeviceName != null)
				{
					Debug.Log("Preferred device was not found. Using \"" + deviceInfo[i].name + "\".");
				}
				else
				{
					Debug.Log("Capturing video from device \"" + deviceInfo[i].name + "\".");
				}
			}
			
			if (DesktopWrapper.InitTracker(deviceInfo[i].id, ref previewWidth, ref previewHeight, previewCamApproxVerticalFOV))
			{
				CreateVideoMaterial();
				CreateVideoMesh();

				float scale = camera.farClipPlane * 0.99f;
				
				float verticalScale = scale * Mathf.Tan(previewCamApproxVerticalFOV * Mathf.PI / 360f);
				
				videoPlaneObject = new GameObject("Video Plane", new Type[] {typeof(MeshRenderer), typeof(MeshFilter)});
				videoPlaneObject.hideFlags = HideFlags.HideAndDontSave;
				videoPlaneObject.active = false;
				
				videoPlaneObject.renderer.material = videoMaterial;
				
				MeshFilter meshFilter = (MeshFilter)videoPlaneObject.GetComponent(typeof(MeshFilter));
				meshFilter.sharedMesh = videoPlaneMesh;
				
				videoPlaneObject.transform.parent = camera.transform;
				videoPlaneObject.transform.localPosition = new Vector3(0, 0, scale);
				videoPlaneObject.transform.localRotation = Quaternion.identity;
				videoPlaneObject.transform.localScale = new Vector3(verticalScale * (float)previewWidth / previewHeight, verticalScale, 1);
	
				camera.clearFlags = CameraClearFlags.SolidColor;
				camera.fieldOfView = previewCamApproxVerticalFOV;
				camera.rect = new Rect(0, 0, 1, 1);
			}
			else
			{
				Debug.Log("Failed to initialize String preview plugin.");
			}
		}
		else
		{
			Debug.LogError("No devices suitable for video capture were detected.");
		}
	}
#endif
	
	protected virtual void ProcessIfNecessary()
	{
	    if (lastFrameProcessed == Time.frameCount) return;

#if UNITY_MOBILE
	    MobileWrapper.String_Process();
		markerCount = MobileWrapper.String_GetData(markerInfo, maxMarkerCount);

		Matrix4x4 newProjMatrix;
		MobileWrapper.String_GetProjectionMatrix(out newProjMatrix);
		camera.projectionMatrix = newProjMatrix;
#endif
	    lastFrameProcessed = Time.frameCount;
	}
	
	
	
	public int LoadMarkerImage(string fileName)
	{
#if UNITY_EDITOR
		string path = Application.dataPath + "/StreamingAssets/" + fileName;
			
		int id = DesktopWrapper.LoadMarkerImage(path);
		
		if (id < 0)
		{
			Debug.LogWarning("Failed to load marker image \"" + path + "\"");
		}
		else
		{
			Debug.Log("Loaded marker image \"" + path + "\"");
		}
		
		return id;
#elif UNITY_ANDROID
			return MobileWrapper.String_LoadMarkerImage(fileName);
#elif UNITY_IPHONE
			return MobileWrapper.String_LoadMarkerImage("Data/Raw/" + fileName);
#endif
	}
	
	public uint GetDetectedMarkerCount()
	{
	    ProcessIfNecessary();
	
	    return markerCount;
	}
	
	public MarkerInfo GetDetectedMarkerInfo(uint markerIndex)
	{
		ProcessIfNecessary();
		
		if (markerIndex < 0 || markerIndex > markerCount) throw new System.ArgumentException("Marker index out of range.");
		
		return markerInfo[markerIndex];
	}
	
	public Texture2D GetCurrentVideoTexture(out Matrix4x4 viewToVideoTextureTransform)
	{
#if UNITY_EDITOR
		viewToVideoTextureTransform = Matrix4x4.TRS(
			new Vector3(0.5f, 0.5f, 0), Quaternion.identity, 
			new Vector3(0.5f * ((float)Screen.width / Screen.height) * (float)previewHeight / previewWidth, -0.5f, 0)) * camera.projectionMatrix;
			
		return videoTextures[0];
#elif UNITY_IPHONE
		ProcessIfNecessary();
		
		uint textureName = 0;

		if(MobileWrapper.String_GetCurrentVideoTexture(out textureName, out viewToVideoTextureTransform))
		{
			if (textureName == (uint)videoTextures[0].GetNativeTextureID())
				return videoTextures[0];
			else if (textureName == (uint)videoTextures[1].GetNativeTextureID())
				return videoTextures[1];
		}
		return null;
#elif UNITY_ANDROID
		viewToVideoTextureTransform = Matrix4x4.identity;
		
		Debug.LogError("Video textures are currently not available on Android.");
		return null;	
#endif
	}

	public void EnableWatermark(bool enable)
	{
#if UNITY_MOBILE
		MobileWrapper.String_EnableWatermark(enable);
#endif
	}
}
