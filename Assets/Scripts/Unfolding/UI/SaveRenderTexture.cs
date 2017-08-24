using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRenderTexture : MonoBehaviour {

    public MeshGenerator meshGenerator;
    public RenderTexture oldRenderTexture;
    string path;
    private string path1 = "Assets/Resources/Unfolding/_Results/Level";
    private string path2 = ".png";

    public void SaveRT()
    {
        //Application.CaptureScreenshot("Screenshot.png");
        RenderTexture.active = oldRenderTexture;
        Texture2D tex = new Texture2D(oldRenderTexture.width, oldRenderTexture.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        var bytes = tex.EncodeToPNG();

        path = path1 + meshGenerator.CurrentLevel + path2;

        System.IO.File.WriteAllBytes(path, bytes);
        Destroy(tex);
    }
}
