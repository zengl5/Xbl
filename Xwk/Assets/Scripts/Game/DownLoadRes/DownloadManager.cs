using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using System;
using System.IO;
public class DownloadManager : MonoBehaviour {
    C_HttpDownloader httpDownloader_bundle;
    C_HttpDownloader httpDownloader_mainifest;

    string url = "https://testpinyinsrc.youban.com/spriteres/sprite";
    string ur2 = "https://testpinyinsrc.youban.com/spriteres/sprite2.manifest";
    string localPath;

    // Use this for initialization
    void Start () {
        //bundle下载
        localPath = LocalPath.LocalPackagingResources;
        httpDownloader_bundle = new C_HttpDownloader();
        httpDownloader_bundle.DownloadFile(url, localPath, Finish);

        //manifest下载
        httpDownloader_mainifest = new C_HttpDownloader();
        httpDownloader_mainifest.DownloadFile(ur2, localPath, Finish);
    }


    void Finish()
    {
        Debug.LogError("Finish");
    }
  
	// Update is called once per frame
	void Update () {
	
	}
}

