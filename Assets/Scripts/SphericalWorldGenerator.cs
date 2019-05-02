using UnityEngine;
using AccidentalNoise;

public class SphericalWorldGenerator : Generator {
		
	MeshRenderer Sphere;
    MeshRenderer Atmosphere1;
    MeshRenderer Atmosphere2;
    MeshRenderer BumpTexture;
    MeshRenderer PaletteTexture;

    protected ImplicitFractal HeightMap;
	protected ImplicitFractal HeatMap;
	protected ImplicitFractal MoistureMap;
    protected ImplicitFractal Cloud1Map;
    protected ImplicitFractal Cloud2Map;

	protected override void Instantiate()
	{
		base.Instantiate ();
		Sphere = transform.Find("Globe").Find ("Sphere").GetComponent<MeshRenderer> ();
        Atmosphere1 = transform.Find("Globe").Find("Atmosphere1").GetComponent<MeshRenderer>();
        Atmosphere2 = transform.Find("Globe").Find("Atmosphere2").GetComponent<MeshRenderer>();

        BumpTexture = transform.Find("BumpTexture").GetComponent<MeshRenderer>();
        PaletteTexture = transform.Find("PaletteTexture").GetComponent<MeshRenderer>();

        Sphere.transform.GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(4, 0.5f);
        Atmosphere1.transform.GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(4, 0.5f);
        Atmosphere2.transform.GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(4, 0.5f);
    }

	protected override void Generate()
	{
		base.Generate ();

        Texture2D bumpTexture = TextureGenerator.GetBumpMap (Width, Height, Tiles);
		Texture2D normal = TextureGenerator.CalculateNormalMap(bumpTexture, 3);

		Sphere.materials [0].mainTexture = BiomeMapRenderer.materials[0].mainTexture;
		Sphere.GetComponent<MeshRenderer> ().materials [0].SetTexture ("_BumpMap", normal);
		Sphere.GetComponent<MeshRenderer> ().materials [0].SetTexture ("_ParallaxMap", HeightMapRenderer.materials[0].mainTexture);

        Atmosphere1.materials[0].mainTexture = TextureGenerator.GetCloud1Texture(Width, Height, Tiles);
        Atmosphere2.materials [0].mainTexture = TextureGenerator.GetCloud2Texture (Width, Height, Tiles); 

        BumpTexture.materials[0].mainTexture = Atmosphere1.materials[0].mainTexture;
        PaletteTexture.materials[0].mainTexture = Atmosphere2.materials[0].mainTexture;
    }

	protected override void Initialize()
	{
		HeightMap = new ImplicitFractal (FractalType.MULTI, 
		                                 BasisType.SIMPLEX, 
		                                 InterpolationType.QUINTIC, 
		                                 TerrainOctaves, 
		                                 TerrainFrequency, 
		                                 Seed);		
		
		HeatMap = new ImplicitFractal(FractalType.MULTI, 
		                              BasisType.SIMPLEX, 
		                              InterpolationType.QUINTIC, 
		                              HeatOctaves, 
		                              HeatFrequency, 
		                              Seed);
		
		MoistureMap = new ImplicitFractal (FractalType.MULTI, 
		                                   BasisType.SIMPLEX, 
		                                   InterpolationType.QUINTIC, 
		                                   MoistureOctaves, 
		                                   MoistureFrequency, 
		                                   Seed);

        Cloud1Map = new ImplicitFractal(FractalType.BILLOW,
                                        BasisType.SIMPLEX,
                                        InterpolationType.QUINTIC,
                                        4,
                                        1.55f,
                                        Seed);

        Cloud2Map = new ImplicitFractal (FractalType.BILLOW, 
		                                BasisType.SIMPLEX, 
		                                InterpolationType.QUINTIC, 
		                                5, 
		                                1.75f, 
		                                Seed);
	}

	protected override void GetData()
	{
		HeightData = new MapData (Width, Height);
		HeatData = new MapData (Width, Height);
		MoistureData = new MapData (Width, Height);
		Clouds1 = new MapData (Width, Height);
        Clouds2 = new MapData(Width, Height);

        // Define our map area in latitude/longitude
        float southLatBound = -180;
		float northLatBound = 180;
		float westLonBound = -90;
		float eastLonBound = 90; 
		
		float lonExtent = eastLonBound - westLonBound;
		float latExtent = northLatBound - southLatBound;
		
		float xDelta = lonExtent / (float)Width;
		float yDelta = latExtent / (float)Height;
		
		float curLon = westLonBound;
		float curLat = southLatBound;
		
        // Loop through each tile using its lat/long coordinates
		for (var x = 0; x < Width; x++) {
			
			curLon = westLonBound;
			
			for (var y = 0; y < Height; y++) {
				
				float x1 = 0, y1 = 0, z1 = 0;
				
                // Convert this lat/lon to x/y/z
				LatLonToXYZ (curLat, curLon, ref x1, ref y1, ref z1);

                // Heat data
				float sphereValue = (float)HeatMap.Get (x1, y1, z1);					
				if (sphereValue > HeatData.Max)
					HeatData.Max = sphereValue;
				if (sphereValue < HeatData.Min)
					HeatData.Min = sphereValue;				
				HeatData.Data [x, y] = sphereValue;
				
				float coldness = Mathf.Abs (curLon) / 90f;
				float heat = 1 - Mathf.Abs (curLon) / 90f;				
				HeatData.Data [x, y] += heat;
				HeatData.Data [x, y] -= coldness;
				
                // Height Data
				float heightValue = (float)HeightMap.Get (x1, y1, z1);
				if (heightValue > HeightData.Max)
					HeightData.Max = heightValue;
				if (heightValue < HeightData.Min)
					HeightData.Min = heightValue;				
				HeightData.Data [x, y] = heightValue;
				
				// Moisture Data
				float moistureValue = (float)MoistureMap.Get (x1, y1, z1);
				if (moistureValue > MoistureData.Max)
					MoistureData.Max = moistureValue;
				if (moistureValue < MoistureData.Min)
					MoistureData.Min = moistureValue;				
				MoistureData.Data [x, y] = moistureValue;

                // Cloud Data
				Clouds1.Data[x,y] = (float)Cloud1Map.Get (x1, y1, z1);
				if (Clouds1.Data[x,y] > Clouds1.Max)
					Clouds1.Max = Clouds1.Data[x,y];
				if (Clouds1.Data[x,y] < Clouds1.Min)
					Clouds1.Min = Clouds1.Data[x,y];

                Clouds2.Data[x, y] = (float)Cloud2Map.Get(x1, y1, z1);
                if (Clouds2.Data[x, y] > Clouds2.Max)
                    Clouds2.Max = Clouds2.Data[x, y];
                if (Clouds2.Data[x, y] < Clouds2.Min)
                    Clouds2.Min = Clouds2.Data[x, y];

                curLon += xDelta;
			}			
			curLat += yDelta;
		}
	}
    
	// Convert Lat/Long coordinates to x/y/z for spherical mapping
	void LatLonToXYZ(float lat, float lon, ref float x, ref float y, ref float z)
	{
        float r = Mathf.Cos(Mathf.Deg2Rad * lon);
        x = r * Mathf.Cos(Mathf.Deg2Rad * lat);
        y = Mathf.Sin(Mathf.Deg2Rad * lon);
        z = r * Mathf.Sin(Mathf.Deg2Rad * lat);
    }
    
	protected override Tile GetTop(Tile t)
	{
		if (t.Y - 1 > 0)
			return Tiles [t.X, t.Y - 1];
		else 
			return null;
	}
	protected override Tile GetBottom(Tile t)
	{
		if (t.Y + 1 < Height)
			return Tiles [t.X, t.Y + 1];
		else
			return null;
	}
	protected override Tile GetLeft(Tile t)
	{
		return Tiles [MathHelper.Mod(t.X - 1, Width), t.Y];
	}
	protected override Tile GetRight(Tile t)
	{
		return Tiles [MathHelper.Mod (t.X + 1, Width), t.Y];
	}

}
