using UnityEngine;

public static class TextureGenerator {		

	// Height Map Colors
	private static Color DeepColor = new Color(15/255f, 30/255f, 80/255f, 1);
	private static Color ShallowColor = new Color(15/255f, 40/255f, 90/255f, 1);
	private static Color RiverColor = new Color(30/255f, 120/255f, 200/255f, 1);
	private static Color SandColor = new Color(198 / 255f, 190 / 255f, 31 / 255f, 1);
	private static Color GrassColor = new Color(50 / 255f, 220 / 255f, 20 / 255f, 1);
	private static Color ForestColor = new Color(16 / 255f, 160 / 255f, 0, 1);
	private static Color RockColor = new Color(0.5f, 0.5f, 0.5f, 1);            
	private static Color SnowColor = new Color(1, 1, 1, 1);

	private static Color IceWater = new Color (210/255f, 255/255f, 252/255f, 1);
	private static Color ColdWater = new Color (119/255f, 156/255f, 213/255f, 1);
	private static Color RiverWater = new Color (65/255f, 110/255f, 179/255f, 1);

	// Height Map Colors
	private static Color Coldest = new Color(0, 1, 1, 1);
	private static Color Colder = new Color(170/255f, 1, 1, 1);
	private static Color Cold = new Color(0, 229/255f, 133/255f, 1);
	private static Color Warm = new Color(1, 1, 100/255f, 1);
	private static Color Warmer = new Color(1, 100/255f, 0, 1);
	private static Color Warmest = new Color(241/255f, 12/255f, 0, 1);

	//Moisture map
	private static Color Dryest = new Color(255/255f, 139/255f, 17/255f, 1);
	private static Color Dryer = new Color(245/255f, 245/255f, 23/255f, 1);
	private static Color Dry = new Color(80/255f, 255/255f, 0/255f, 1);
	private static Color Wet = new Color(85/255f, 255/255f, 255/255f, 1);
	private static Color Wetter = new Color(20/255f, 70/255f, 255/255f, 1);
	private static Color Wettest = new Color(0/255f, 0/255f, 100/255f, 1);

	//biome map
	private static Color Ice = Color.white;
	private static Color Desert = new Color(238/255f, 218/255f, 130/255f, 1);
	private static Color Savanna = new Color(177/255f, 209/255f, 110/255f, 1);
	private static Color TropicalRainforest = new Color(66/255f, 123/255f, 25/255f, 1);
	private static Color Tundra = new Color(96/255f, 131/255f, 112/255f, 1);
	private static Color TemperateRainforest = new Color(29/255f, 73/255f, 40/255f, 1);
	private static Color Grassland = new Color(164/255f, 225/255f, 99/255f, 1);
	private static Color SeasonalForest = new Color(73/255f, 100/255f, 35/255f, 1);
	private static Color BorealForest = new Color(95/255f, 115/255f, 62/255f, 1);
	private static Color Woodland = new Color(139/255f, 175/255f, 90/255f, 1);
    

    public static Texture2D CalculateNormalMap(Texture2D source, float strength)
    {
        Texture2D result;
        float xLeft, xRight;
        float yUp, yDown;
        float yDelta, xDelta;
        var pixels = new Color[source.width * source.height];
        strength = Mathf.Clamp(strength, 0.0F, 10.0F);        
        result = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
        
        for (int by = 0; by < result.height; by++)
        {
            for (int bx = 0; bx < result.width; bx++)
            {
                xLeft = source.GetPixel(bx - 1, by).grayscale * strength;
                xRight = source.GetPixel(bx + 1, by).grayscale * strength;
                yUp = source.GetPixel(bx, by - 1).grayscale * strength;
                yDown = source.GetPixel(bx, by + 1).grayscale * strength;
                xDelta = ((xLeft - xRight) + 1) * 0.5f;
                yDelta = ((yUp - yDown) + 1) * 0.5f;

                pixels[bx + by * source.width] = new Color(xDelta, yDelta, 1.0f, yDelta);
            }
        }

        result.SetPixels(pixels);
        result.wrapMode = TextureWrapMode.Clamp;
        result.Apply();
        return result;
    }

    public static Texture2D GetCloud1Texture(int width, int height, Tile[,] tiles)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				if (tiles[x,y].Cloud1Value > 0.45f)
					pixels[x + y * width] = Color.Lerp(new Color(1f, 1f, 1f, 0), Color.white, tiles[x,y].Cloud1Value);
				else
					pixels[x + y * width] = new Color(0,0,0,0);
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}
    
    public static Texture2D GetCloud2Texture(int width, int height, Tile[,] tiles)
    {
        var texture = new Texture2D(width, height);
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (tiles[x, y].Cloud2Value > 0.5f)
                    pixels[x + y * width] = Color.Lerp(new Color(1f, 1f, 1f, 0), Color.white, tiles[x, y].Cloud2Value);
                else
                    pixels[x + y * width] = new Color(0, 0, 0, 0);
            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    public static Texture2D GetBiomePalette(){
		
		var texture = new Texture2D(128, 128);
		var pixels = new Color[128 * 128];
		
		for (var x = 0; x < 128; x++)
		{
			for (var y = 0; y < 128; y++)
			{				
				if (x < 10)
					pixels[x + y * 128] = Ice;
				else if (x < 20)
					pixels[x + y * 128] = Desert;
				else if (x < 30)
					pixels[x + y * 128] = Savanna;
				else if (x < 40)
					pixels[x + y * 128] = TropicalRainforest;
				else if (x < 50)
					pixels[x + y * 128] = Tundra;
				else if (x < 60)
					pixels[x + y * 128] = TemperateRainforest;
				else if (x < 70)
					pixels[x + y * 128] = Grassland;
				else if (x < 80)
					pixels[x + y * 128] = SeasonalForest;
				else if (x < 90)
					pixels[x + y * 128] = BorealForest;
				else if (x < 100)
					pixels[x + y * 128] = Woodland;
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
		
	}
		
	public static Texture2D GetBumpMap(int width, int height, Tile[,] tiles)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				switch (tiles[x,y].HeightType)
				{
				case HeightType.DeepWater:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				case HeightType.ShallowWater:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				case HeightType.Sand:
					pixels[x + y * width] = new Color(0.3f, 0.3f, 0.3f, 1);
					break;
				case HeightType.Grass:
					pixels[x + y * width] = new Color(0.45f, 0.45f, 0.45f, 1);
					break;
				case HeightType.Forest:
					pixels[x + y * width] = new Color(0.6f, 0.6f, 0.6f, 1);
					break;
				case HeightType.Rock:
					pixels[x + y * width] = new Color(0.75f, 0.75f, 0.75f, 1);
					break;
				case HeightType.Snow:
					pixels[x + y * width] = new Color(1, 1, 1, 1);
					break;
				case HeightType.River:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				}

				if (!tiles[x,y].Collidable)
				{
					pixels[x + y * width] = Color.Lerp(Color.white, Color.black, tiles[x,y].HeightValue * 2);
				}
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}
    
	public static Texture2D GetHeightMapTexture(int width, int height, Tile[,] tiles)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];

		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				switch (tiles[x,y].HeightType)
				{
				case HeightType.DeepWater:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				case HeightType.ShallowWater:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				case HeightType.Sand:
					pixels[x + y * width] = new Color(0.3f, 0.3f, 0.3f, 1);
					break;
				case HeightType.Grass:
					pixels[x + y * width] = new Color(0.45f, 0.45f, 0.45f, 1);
					break;
				case HeightType.Forest:
					pixels[x + y * width] = new Color(0.6f, 0.6f, 0.6f, 1);
					break;
				case HeightType.Rock:
					pixels[x + y * width] = new Color(0.75f, 0.75f, 0.75f, 1);
					break;
				case HeightType.Snow:
					pixels[x + y * width] = new Color(1, 1, 1, 1);
					break;
				case HeightType.River:
					pixels[x + y * width] = new Color(0, 0, 0, 1);
					break;
				}

//				pixels[x + y * width] = Color.Lerp(Color.black, Color.white, tiles[x,y].HeightValue);
//
//				//darken the color if a edge tile
//				if ((int)tiles[x,y].HeightType > 2 && tiles[x,y].Bitmask != 15)
//					pixels[x + y * width] = Color.Lerp(pixels[x + y * width], Color.black, 0.4f);
//
//				if (tiles[x,y].Color != Color.black)
//					pixels[x + y * width] = tiles[x,y].Color;
//				else if ((int)tiles[x,y].HeightType > 2)
//					pixels[x + y * width] = Color.white;
//				else
//					pixels[x + y * width] = Color.black;
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}

	public static Texture2D GetHeatMapTexture(int width, int height, Tile[,] tiles)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				switch (tiles[x,y].HeatType)
				{
				case HeatType.Coldest:
					pixels[x + y * width] = Coldest;
					break;
				case HeatType.Colder:
					pixels[x + y * width] = Colder;
					break;
				case HeatType.Cold:
					pixels[x + y * width] = Cold;
					break;
				case HeatType.Warm:
					pixels[x + y * width] = Warm;
					break;
				case HeatType.Warmer:
					pixels[x + y * width] = Warmer;
					break;
				case HeatType.Warmest:
					pixels[x + y * width] = Warmest;
					break;
				}
				
				//darken the color if a edge tile
				if ((int)tiles[x,y].HeightType > 2 && tiles[x,y].Bitmask != 15)
					pixels[x + y * width] = Color.Lerp(pixels[x + y * width], Color.black, 0.4f);
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}

	public static Texture2D GetMoistureMapTexture(int width, int height, Tile[,] tiles)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				Tile t = tiles[x,y];
				
				if (t.MoistureType == MoistureType.Dryest)           
					pixels[x + y * width] = Dryest;
				else if (t.MoistureType == MoistureType.Dryer)          
					pixels[x + y * width] = Dryer;
				else if (t.MoistureType == MoistureType.Dry)          
					pixels[x + y * width] = Dry;
				else if (t.MoistureType == MoistureType.Wet)          
					pixels[x + y * width] = Wet; 
				else if (t.MoistureType == MoistureType.Wetter)          
					pixels[x + y * width] = Wetter; 
				else      
					pixels[x + y * width] = Wettest; 
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}

	public static Texture2D GetBiomeMapTexture(int width, int height, Tile[,] tiles, float coldest, float colder, float cold)
	{
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				BiomeType value = tiles[x, y].BiomeType;
				
				switch(value){
				case BiomeType.Ice:
					pixels[x + y * width] = Ice;
					break;
				case BiomeType.BorealForest:
					pixels[x + y * width] = BorealForest;
					break;
				case BiomeType.Desert:
					pixels[x + y * width] = Desert;
					break;
				case BiomeType.Grassland:
					pixels[x + y * width] = Grassland;
					break;
				case BiomeType.SeasonalForest:
					pixels[x + y * width] = SeasonalForest;
					break;
				case BiomeType.Tundra:
					pixels[x + y * width] = Tundra;
					break;
				case BiomeType.Savanna:
					pixels[x + y * width] = Savanna;
					break;
				case BiomeType.TemperateRainforest:
					pixels[x + y * width] = TemperateRainforest;
					break;
				case BiomeType.TropicalRainforest:
					pixels[x + y * width] = TropicalRainforest;
					break;
				case BiomeType.Woodland:
					pixels[x + y * width] = Woodland;
					break;							
				}
				
				// Water tiles
				if (tiles[x,y].HeightType == HeightType.DeepWater) {
					pixels[x + y * width] = DeepColor;
				}
				else if (tiles[x,y].HeightType == HeightType.ShallowWater) {
					pixels[x + y * width] = ShallowColor;
				}

				// draw rivers
				if (tiles[x,y].HeightType ==  HeightType.River)
				{
					float heatValue = tiles[x,y].HeatValue;		

					if (tiles[x,y].HeatType == HeatType.Coldest)
						pixels[x + y * width] = Color.Lerp (IceWater, ColdWater, (heatValue) / (coldest));
					else if (tiles[x,y].HeatType == HeatType.Colder)
						pixels[x + y * width] = Color.Lerp (ColdWater, RiverWater, (heatValue - coldest) / (colder - coldest));
					else if (tiles[x,y].HeatType == HeatType.Cold)
						pixels[x + y * width] = Color.Lerp (RiverWater, ShallowColor, (heatValue - colder) / (cold - colder));
					else
						pixels[x + y * width] = ShallowColor;
				}


				// add a outline
				if (tiles[x,y].HeightType >= HeightType.Shore && tiles[x,y].HeightType != HeightType.River)
				{
					if (tiles[x,y].BiomeBitmask != 15)
						pixels[x + y * width] = Color.Lerp (pixels[x + y * width], Color.black, 0.35f);
				}
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}
}
