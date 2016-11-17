using UnityEngine;

public static class PreviewMaker {
    public static Texture2D takeScreenshot(Camera mainCamera)
    {
        int resWidthN = Screen.width;
        int resHeightN = Screen.height;
        RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);

        mainCamera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(resWidthN, resHeightN, TextureFormat.RGB24, false);

        mainCamera.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);

        mainCamera.targetTexture = null;

        RenderTexture.active = null;

        screenShot.Apply();

        return screenShot;
    }
}
