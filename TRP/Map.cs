using System;
using System.Collections.Generic;
using System.Text;

namespace TRP
{
    public enum TileType {battle}
    public enum LayerType {Desert, Forest}

    class Map
    {
        public Layer[] Layers = new Layer[10];
    }

    class Layer
    {
        public LayerType Type { get; set; }
        public Tile[] Tiles = new Tile[8];
    }

    class Tile
    {
        public TileType Type { get; set; }
        public string Name { get; set; }
    }
}
