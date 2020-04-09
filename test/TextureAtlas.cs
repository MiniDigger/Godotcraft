using System;
using System.Collections.Generic;
using Godot;
using File = System.IO.File;

namespace Godotcraft.test {
public class TextureAtlas {
	public static readonly TextureAtlas instance = new TextureAtlas();

	public Texture atlas { get; }

	public TextureAtlas() {
		atlas = createAtlas();
	}

	private Texture createAtlas() {
		string texturePath = "user://mcassets/assets/minecraft/textures/block/";
		Directory textureDir = new Directory();
		textureDir.Open(texturePath);

		List<string> images = getImages(textureDir);
		GD.Print($"found {images.Count} images");

		int pixelWidth = 16;
		int pixelHeight = 16;
		int atlasWidth = Mathf.CeilToInt((Mathf.Sqrt(images.Count) + 1) * pixelWidth);
		int atlasHeight = Mathf.CeilToInt((Mathf.Sqrt(images.Count) + 1) * pixelHeight);

		Image texture = new Image();
		texture.Create(atlasWidth, atlasHeight, false, Image.Format.Rgba8);
		int count = 0;

		for (int x = 0; x < atlasWidth / pixelHeight; x++) {
			for (int y = 0; y < atlasHeight / pixelHeight; y++) {
				if (count >= images.Count) {
					goto end;
				}

				Image img = new Image();
				img.Load(texturePath + images[count]);

				for (var i = 0; i < pixelWidth; i++) {
					for (var j = 0; j < pixelHeight; j++) {
						texture.Lock();
						img.Lock();
						texture.SetPixel(x * pixelWidth + i, y * pixelHeight + j, img.GetPixel(i, j));
					}
				}

				float startX = x * pixelWidth;
				float startY = y * pixelHeight;
				float perPixelRatioX = 1f / atlasWidth;
				float perPixelRatioY = 1f / atlasHeight;
				startX *= perPixelRatioX;
				startY *= perPixelRatioY;
				float endX = startX + (perPixelRatioX * pixelWidth);
				float endY = startY + (perPixelRatioY * pixelHeight);
				// float endX = startX + pixelWidth;
				// float endY = startY + pixelHeight;

				new UVMap(images[count].Replace(".png", ""), new[] {
					new Vector2(startX, startY),
					new Vector2(startX, endY),
					new Vector2(endX, startY),
					new Vector2(endX, endY),
				}).register();

				count++;
			}
		}

		end:

		ImageTexture imageTexture = new ImageTexture();
		imageTexture.CreateFromImage(texture);
		imageTexture.Flags &= ~(uint)Texture.FlagsEnum.Filter;

		texture.SavePng("user://test.png");

		// return new AtlasTexture {Atlas = imageTexture, Region = new Rect2(0,0,atlasWidth, atlasHeight)};
		return imageTexture;
	}

	private List<string> getImages(Directory textureDir) {
		List<string> images = new List<string>();
		textureDir.ListDirBegin(true, true);
		while (true) {
			string file = textureDir.GetNext();
			if (file.Equals("")) {
				break;
			}
			else if (file.EndsWith(".png")) {
				images.Add(file);
			}
		}

		textureDir.ListDirEnd();

		return images;
	}
}
}