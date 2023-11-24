using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using System.IO;
using Dummiesman;
using Oculus.Interaction;
using UnityEngine.UIElements;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator Instance = null;

    private Pose SpawnPose;

    void Awake()
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
    public void CreateObject(SelectableAction action)
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
            case SelectableAction.Close:
                break;
            case SelectableAction.Sphere:
                go =  PresentationObjectPool.Instance.Get(0, position);
                go.transform.rotation = rotation;
                break;
            case SelectableAction.Cube:
                go = PresentationObjectPool.Instance.Get(1, position);
                go.transform.rotation = rotation;
                break;
            case SelectableAction.Cylinder:
                go = PresentationObjectPool.Instance.Get(2, position);
                go.transform.rotation = rotation;
                break;
            case SelectableAction.Plane:
                go = PresentationObjectPool.Instance.Get(3, position);
                go.transform.rotation = rotation;
                break;
            case SelectableAction.Text:
                go = PresentationObjectPool.Instance.Get(4, position);
                go.transform.rotation = rotation;
                break;
            case SelectableAction.ImportImage:
	            LoadImageFile();
                break;
            case SelectableAction.ImportModel:
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
			
			GameObject importObject = PresentationObjectPool.Instance.Get(6, SpawnPose.position);
			importObject.transform.rotation = SpawnPose.rotation;
			GameObject element = importObject.GetComponentInChildren<Grabbable>().gameObject;

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

			GameObject imageObject = PresentationObjectPool.Instance.Get(5, SpawnPose.position);
			imageObject.transform.rotation = SpawnPose.rotation;
			
			Texture2D loadedTexture = new Texture2D(1, 1);
			loadedTexture.LoadImage(bytes);
			imageObject.GetComponentInChildren<Image>().image = loadedTexture;
		}
	}
}
