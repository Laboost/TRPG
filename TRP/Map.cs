using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    public enum TileType {Boss, Battle, Shop} //first element is only picked for last tiles.
    public enum LayerType {Desert, Forest, Snowy}

    class Map
    {
        public Layer[] Layers;
        public string LayerName { get; set; }
        public Layer CurrentLayer { get; private set; }
        public Tile CurrentTile { get; private set; }

        public Map(int numOfMapLayers)
        {
            Layers = new Layer[numOfMapLayers];
        }
        public void InitMap()
        {
            CurrentLayer = Layers[0];
            CurrentTile = CurrentLayer.Tiles[0];
            LayerName = " Layer " + CurrentLayer.Num;
        }
        public void MoveForward()
        {
            if (CurrentTile.Type == TileType.Boss) // if tile is last in layer == boss
            {
                CurrentLayer = Layers[CurrentLayer.Num];
                CurrentTile = CurrentLayer.Tiles[0];
                int layerNum = CurrentLayer.Num;
                LayerName = " Layer " + layerNum;
            }
            else
            {
                CurrentTile = CurrentLayer.Tiles[CurrentTile.Num + 1];
            }
        }
    }

    class Layer
    {
        public int Num { get; set; }
        public LayerType Type { get; set; }
        public Tile[] Tiles = new Tile[Program.NUMBER_OF_TILES_IN_LAYER];
    }

    class Tile
    {
        public int Num { get; set; }
        public TileType Type { get; set; }
        public string Name { get; set; }
        public bool EventIsDone = false;
    }
}
