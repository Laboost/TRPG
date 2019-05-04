using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    public enum TileType {Battle,Boss}
    public enum LayerType {Desert, Forest}

    class Map
    {
        public Layer[] Layers = new Layer[10];
        public Layer CurrentLayer { get; private set; }
        public Tile CurrentTile { get; private set; }

        public void InitMap()
        {
            CurrentLayer = Layers[0];
            CurrentTile = CurrentLayer.Tiles[0];
        }
        public void MoveForward()
        {
            if (CurrentTile.Type == TileType.Boss) // if tile is last in layer == boss
            {
                CurrentLayer = Layers[CurrentLayer.Num + 1];
                CurrentTile = CurrentLayer.Tiles[0];
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
        public Tile[] Tiles = new Tile[8];
    }

    class Tile
    {
        public int Num { get; set; }
        public TileType Type { get; set; }
        public string Name { get; set; }
    }
}
