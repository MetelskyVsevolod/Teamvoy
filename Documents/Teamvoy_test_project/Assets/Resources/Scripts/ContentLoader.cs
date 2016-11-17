using UnityEngine;
using System.Collections;

public class ContentLoader : MonoBehaviour
{
    public bool isLoading = false;

    private ListManager listManager;

    void Awake()
    {
        listManager = GetComponent<ListManager>();
        StartCoroutine(readAssetBundle());
    }

    public IEnumerator readAssetBundle()
    {
        isLoading = true;

        string[] formats = new string[] { "*.anim", "*.gmobj" };

        string[] files = new string[1];

        WWW www = new WWW(files[0]);

        for (int j = 0; j < formats.Length; j++)
        {
            files = System.IO.Directory.GetFiles(Application.dataPath.Replace("/Assets", "") + "/AssetBundles/", formats[j]);

            for (int i = 0; i < files.Length; i++)
            {
                www = new WWW("file:" + files[i]);
                yield return www;

                if (formats[j] == "*.gmobj")
                    listManager.animatedObjects = www.assetBundle.LoadAllAssets<GameObject>();
                else if (formats[j] == "*.anim")
                    listManager.animatedObjects = www.assetBundle.LoadAllAssets<GameObject>();
            }
        }

        listManager.spawnAnimatedObjects();
        listManager.showListOfAnimatedObjects();

        www.assetBundle.Unload(false);
        www.Dispose();

        isLoading = false;
        yield return new WaitForEndOfFrame();
    }
}
