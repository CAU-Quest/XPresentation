using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using System.IO;
using Dummiesman;
using Oculus.Interaction;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator Instance = null;
    public Transform objectParent = null;
    private Pose SpawnPose;

    void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void CreateObject(DeployType action)
    {
        Vector3 direction = XRUIManager.Instance.positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        SpawnPose = XRUIManager.Instance.GetPlayerSightPose();
        Vector3 position = SpawnPose.position;
        Quaternion rotation = SpawnPose.rotation;
        GameObject go;
        switch (action)
        {
            case DeployType.Sphere:
	            if (objectParent) go = PresentationObjectPool.Instance.Get(0, position, objectParent);
                else go = PresentationObjectPool.Instance.Get(0, position);
                go.transform.rotation = rotation;
                break;
            case DeployType.Cube:
	            if (objectParent) go = PresentationObjectPool.Instance.Get(1, position, objectParent);
	            else go = PresentationObjectPool.Instance.Get(1, position);
                go.transform.rotation = rotation;
                break;
            case DeployType.Cylinder:
	            if (objectParent) go = PresentationObjectPool.Instance.Get(2, position, objectParent);
	            else go = PresentationObjectPool.Instance.Get(2, position);
                go.transform.rotation = rotation;
                break;
            case DeployType.Plane:
	            if (objectParent) go = PresentationObjectPool.Instance.Get(3, position, objectParent);
	            else go = PresentationObjectPool.Instance.Get(3, position);
                go.transform.rotation = rotation;
                break;
            case DeployType.Text:
	            if (objectParent) go = PresentationObjectPool.Instance.Get(4, position, objectParent);
	            else go = PresentationObjectPool.Instance.Get(4, position);
                go.transform.rotation = rotation;
                break;
            case DeployType.ImportImage:
	            XRUIManager.Instance.fileBrowser.SetActive(true);
	            XRUIManager.Instance.fileBrowser.transform.position = position;
	            XRUIManager.Instance.fileBrowser.transform.rotation = rotation;
	            LoadImageFile();
                break;
            case DeployType.ImportModel:
	            XRUIManager.Instance.fileBrowser.SetActive(true);
	            XRUIManager.Instance.fileBrowser.transform.position = position;
	            XRUIManager.Instance.fileBrowser.transform.rotation = rotation;
	            LoadObjFile();
                break;
        }
    }
    
    
	public GameObject loadedObject;
	
	public void LoadObjFile()
	{
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );
        FileBrowser.SetDefaultFilter( ".obj" );
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
		StartCoroutine( ShowLoadObjDialogCoroutine() );
	}
	
	public void LoadTextureFile()
	{
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );
		FileBrowser.SetDefaultFilter(".png");
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
		StartCoroutine( ShowLoadTextureDialogCoroutine() );
	}
    
	public void LoadImageFile()
	{
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );
		FileBrowser.SetDefaultFilter(".png");
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
		StartCoroutine( ShowLoadImageDialogCoroutine() );
	}

	IEnumerator ShowLoadObjDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );
		
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
				Debug.Log( FileBrowser.Result[i] );

			byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );

			GameObject importObject;
			if (objectParent) importObject = PresentationObjectPool.Instance.Get(6, SpawnPose.position, objectParent);
			else importObject = PresentationObjectPool.Instance.Get(6, SpawnPose.position);
			
			importObject.transform.rotation = SpawnPose.rotation;
			GameObject element = importObject.GetComponentInChildren<Grabbable>().gameObject;
			importObject.GetComponent<SelectObject>().objectPath = FileBrowser.Result[0];

			PresentationObject presentationObject = element.AddComponent<PresentationObject>();
			
			var textStream = new MemoryStream(bytes);
			loadedObject = new OBJLoader().Load(textStream, element.transform);
			
			LoadTextureFile();
		}
	}
	
	IEnumerator ShowLoadTextureDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
				Debug.Log( FileBrowser.Result[i] );

			byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );

			string savePath = destinationPath;
			
			loadedObject.GetComponentInParent<SelectObject>().imagePath = destinationPath;
			Texture2D loadedTexture = new Texture2D(1, 1);
			loadedTexture.LoadImage(bytes);
			loadedObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = loadedTexture;
		}
	}
	
	IEnumerator ShowLoadImageDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
				Debug.Log( FileBrowser.Result[i] );

			byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );

			GameObject imageObject;
			if (objectParent) imageObject = PresentationObjectPool.Instance.Get(5, SpawnPose.position, objectParent);
			else imageObject = PresentationObjectPool.Instance.Get(5, SpawnPose.position);
			imageObject.transform.rotation = SpawnPose.rotation;
			
			imageObject.GetComponent<SelectObject>().imagePath = FileBrowser.Result[0];
			Texture2D loadedTexture = new Texture2D(1, 1);
			loadedTexture.LoadImage(bytes);
			imageObject.GetComponentInChildren<RawImage>().texture = loadedTexture;
			if(objectParent) imageObject.transform.SetParent(objectParent);
		}
	}
}
