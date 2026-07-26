using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum TILE_TYPE { 
    FLAT, 
    ROUGH_CENTER, 
    ROUGH_N, 
    ROUGH_S, 
    ROUGH_E, 
    ROUGH_W, 
    ROUGH_NE, 
    ROUGH_SE, 
    ROUGH_SW, 
    ROUGH_NW,
    OASIS,
    MIRAGE
};

public enum DIRECTIONS
{
    NORTH, SOUTH, EAST, WEST, NORTH_EAST, SOUTH_EAST, SOUTH_WEST, NORTH_WEST
};

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
public class TileData : ScriptableObject
{
    public static Dictionary<DIRECTIONS, int2> DirectionVectors = new Dictionary<DIRECTIONS, int2>()
    {
        { DIRECTIONS.NORTH,      new int2( 0,  1) },
        { DIRECTIONS.SOUTH,      new int2( 0, -1) },
        { DIRECTIONS.EAST,       new int2( 1,  0) },
        { DIRECTIONS.WEST,       new int2(-1,  0) },
        { DIRECTIONS.NORTH_EAST, new int2( 1,  1) },
        { DIRECTIONS.SOUTH_EAST, new int2( 1, -1) },
        { DIRECTIONS.SOUTH_WEST, new int2(-1, -1) },
        { DIRECTIONS.NORTH_WEST, new int2(-1,  1) },
    };
    
    [System.Serializable]
    public class TileConstraint
    {
        public DIRECTIONS direction;
        [Tooltip("Right click to remove tile types")]
        public TILE_TYPE[] allowedTypes;
    }

    public TILE_TYPE tileType;  // determines prefab and rules 
    public TileConstraint[] constraints;
    public GameObject prefab;
}
