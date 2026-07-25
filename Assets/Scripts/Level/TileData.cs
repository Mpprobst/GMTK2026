using System.Collections.Generic;
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
    ROUGH_NW 
};

public enum DIRECTIONS
{
    NORTH, SOUTH, EAST, WEST, NORTH_EAST, SOUTH_EAST, SOUTH_WEST, NORTH_WEST
};

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
public class TileData : ScriptableObject
{
    public static Dictionary<DIRECTIONS, Vector3> DirectionVectors = new Dictionary<DIRECTIONS, Vector3>()
    {
        { DIRECTIONS.NORTH, Vector3.forward },
        { DIRECTIONS.SOUTH, Vector3.back },
        { DIRECTIONS.EAST, Vector3.right },
        { DIRECTIONS.WEST, Vector3.left },
        { DIRECTIONS.NORTH_EAST, new Vector3(1, 0, 1) },
        { DIRECTIONS.SOUTH_EAST, new Vector3(1, 0, -1) },
        { DIRECTIONS.SOUTH_WEST, new Vector3(-1, 0, -1) },
        { DIRECTIONS.NORTH_WEST, new Vector3(-1, 0, 1) },
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


}
