using System.IO;
using UnityEditor;
using UnityEngine;

public static class PrefabImageExporter
{
    [MenuItem("Tools/Export Prefab Previews")]
    public static void ExportAllPrefabPreviews()
    {
        string outputDirectory = "Assets/Sprites/tanks";
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        // Tìm tất cả Prefab trong thư mục chứa tank
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/tanks" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            // Lấy preview (có nền xám)
            Texture2D preview = AssetPreview.GetAssetPreview(prefab);

            // Đợi cho đến khi preview được load đầy đủ
            int wait = 0;
            while (preview == null && wait < 30)
            {
                System.Threading.Thread.Sleep(100);
                preview = AssetPreview.GetAssetPreview(prefab);
                wait++;
            }
            if (preview == null) continue;

            // Bước xử lý để biến nền xám thành trong suốt
            Color[] pixels = preview.GetPixels();
            // Giả sử pixel (0,0) là màu nền, thường là xám
            Color bgColor = preview.GetPixel(0, 0);
            for (int i = 0; i < pixels.Length; i++)
            {
                // So sánh gần đúng để tránh sai sót floating-point
                if (pixels[i].r == bgColor.r &&
                    pixels[i].g == bgColor.g &&
                    pixels[i].b == bgColor.b)
                {
                    pixels[i].a = 0f;
                }
            }

            // Tạo texture mới với kênh alpha
            Texture2D transparentTex = new Texture2D(preview.width, preview.height, TextureFormat.RGBA32, false);
            transparentTex.SetPixels(pixels);
            transparentTex.Apply();

            // Lưu ra PNG
            byte[] bytes = transparentTex.EncodeToPNG();
            string outPath = $"{outputDirectory}/{prefab.name}.png";
            File.WriteAllBytes(outPath, bytes);
            Debug.Log($"Saved transparent preview for {prefab.name} → {outPath}");
        }

        AssetDatabase.Refresh();
    }
}
