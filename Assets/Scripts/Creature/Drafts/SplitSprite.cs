#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using BlueGravity.Utility;
using TricksAndTreatsOrThreats;
using UnityEngine;
using UnityEditor;

public static class SplitSpriteUtility
{
    [MenuItem("TTT/Database/Assign Icon")]
    public static void AssignToIcon()
    {
        var item = Selection.objects.OfType<DatabaseItem>().FirstOrDefault();
        var sprite = Selection.objects.OfType<Sprite>().FirstOrDefault();
        if (!item || !sprite) return;

        var itemPath = AssetDatabase.GetAssetPath(item);
        var dir = Path.GetDirectoryName(itemPath);
        var savePath = Path.Combine(dir!, $"{item.name.ToLower()} icon.png");

        CopySpriteToFile(sprite, savePath);
        AssetDatabase.ImportAsset(savePath, ImportAssetOptions.ForceSynchronousImport);

        var created = AssetDatabase.LoadAssetAtPath<Sprite>(savePath);
        if (!created) throw new Exception("Failed to imported sprite");

        item.ReflectionSet("icon", created);
        EditorUtility.SetDirty(item);
        AssetDatabase.SaveAssets();
        
        Debug.Log("icon set");
    }

    [MenuItem("Drafts/Unpack Sprite")]
    public static void CopySelectedSpritesToFile()
    {
        foreach (var sprite in Selection.objects.OfType<Sprite>())
            CopySpriteToFile(sprite);
        AssetDatabase.Refresh();
    }

    public static void CopySpriteToFile(Sprite sprite, string savePath = null)
    {
        var x = (int)sprite.rect.x;
        var y = (int)sprite.rect.y;
        var w = (int)sprite.rect.width;
        var h = (int)sprite.rect.height;
        var pixels = sprite.texture.GetPixels(x, y, w, h);
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.SetPixels(pixels);

        var assetPath = AssetDatabase.GetAssetPath(sprite);
        var dir = Path.GetDirectoryName(assetPath);
        savePath ??= Path.Combine(dir!, $"{sprite.name}.png");
        File.WriteAllBytes(savePath, tex.EncodeToPNG());
    }
}
#endif