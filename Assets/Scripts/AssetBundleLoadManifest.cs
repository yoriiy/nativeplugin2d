﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AssetBundleのマニフェストファイルを読み込んで
/// 依存ファイルを取得するため
/// </summary>
public class AssetBundleLoadManifest : AssetBundleLoadAsset {

	public AssetBundleLoadManifest (string bundleName, string assetName, System.Type type) : base (bundleName, assetName, type)
    {

    }

    public override bool LoadRequest()
    {
        base.LoadRequest();

        if(_request != null && _request.isDone)
        {
            AssetBundleManager.AssetBundleManifestObject = GetAsset<AssetBundleManifest>();
            return false;
        }else
        {
            return true;
        }
    }
}
