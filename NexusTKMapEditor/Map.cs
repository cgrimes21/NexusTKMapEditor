using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace NexusTKMapEditor
{
    public class Map
    {
        public string Name { get; set; }
        public Size Size { get; set; }
        public bool IsModified { get; set; }
        public bool IsEditable { get; set; }
        private Tile[,] mapData;
        private Bitmap[,] mapCache;
        private bool showTiles = true;
        private bool showObjects = true;
        private int totalPassable;
        private int totalNotPassable;

        public Map(string mapPath)
        {
            Name = Path.GetFileNameWithoutExtension(mapPath);
            IsEditable = false;
            
            bool tileCompressed = Path.GetExtension(mapPath).Equals(".cmp");

            FileStream mapFileStream = File.Open(mapPath, FileMode.Open);

            BinaryReader reader = new BinaryReader(mapFileStream);
            try
            {
                //CMP has an extra 'CMAP' header in the first 4 bytes
                if (tileCompressed)
                {
                    string header = new string(reader.ReadChars(4));
                    if (!header.Equals("CMAP"))
                    {
                        reader.Close();
                        mapFileStream.Close();
                        throw new Exception("CMAP header missing, cannot parse cmp file");
                    }
                }

                var sx = reader.ReadUInt16();
                var sy = reader.ReadUInt16();

                if (!tileCompressed)
                {
                    //MAP format uses big endian, manually read as such
                    var sxBytes = BitConverter.GetBytes(sx);
                    var syBytes = BitConverter.GetBytes(sy);
                    sx = BitConverter.ToUInt16(new[] {sxBytes[1], sxBytes[0]}, 0);
                    sy = BitConverter.ToUInt16(new[] {syBytes[1], syBytes[0]}, 0);
                }

                CreateEmptyMap(sx, sy);

                //If we are reading a CMP,change the stream under the reader to Deflate now
                if (tileCompressed)
                {
                    reader = new BinaryReader(new InflaterInputStream(mapFileStream));
                }

                //Reset passability before we load
                totalPassable = totalNotPassable = 0;
                for (int y = 0; y < sy; y++)
                {
                    for (int x = 0; x < sx; x++)
                    {
                        var tileNumber = reader.ReadUInt16();
                        // Passible is true when == 0 in the data
                        var passableVal = reader.ReadUInt16();
                        var passable = !Convert.ToBoolean(passableVal);
                        var objectNumber = reader.ReadUInt16();
                        if (!tileCompressed)
                        {
                            //MAP format uses big endian, manually read as such
                            var tileBtyes = BitConverter.GetBytes(tileNumber);
                            var objectBytes = BitConverter.GetBytes(objectNumber);
                            tileNumber = BitConverter.ToUInt16(new[] {tileBtyes[1], tileBtyes[0]}, 0);
                            objectNumber = BitConverter.ToUInt16(new[] {objectBytes[1], objectBytes[0]}, 0);
                        }
                        mapData[x, y] = new Tile(tileNumber, passable, objectNumber);
                        totalPassable += passable ? 1 : 0;
                        totalNotPassable += passable ? 0 : 1;
                    }
                }

                IsModified = false;
            }
            finally
            {
                reader.Close();
                mapFileStream.Close();                
            }


        }

        public void Save(string mapPath)
        {
            bool tileCompressed = Path.GetExtension(mapPath).Equals(".cmp");
            
            if (File.Exists(mapPath)) File.Delete(mapPath);

            FileStream mapFileStream = File.Create(mapPath);
            BinaryWriter writer = new BinaryWriter(mapFileStream);

            //CMP has an extra 'CMAP' header in the first 4 bytes
            if (tileCompressed)
            {
                writer.Write("CMAP".ToCharArray());
            }
            


            //If we are writing a CMP, flush and change the stream under the writer to Deflate now
            if (tileCompressed)
            {
                writer.Write(Convert.ToInt16(Size.Width));
                writer.Write(Convert.ToInt16(Size.Height));
                writer.Flush();
                writer = new BinaryWriter(new DeflaterOutputStream(mapFileStream));
            }
            else
            {
                // MAP format has big endian which we need to do manually
                var widthBytes = BitConverter.GetBytes(Convert.ToInt16(Size.Width));
                var heightBytes = BitConverter.GetBytes(Convert.ToInt16(Size.Height));
                writer.Write(widthBytes[1]);
                writer.Write(widthBytes[0]);
                writer.Write(heightBytes[1]);
                writer.Write(heightBytes[0]);
            }


            for (int y = 0; y < Size.Height; y++)
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    if (tileCompressed)
                    {
                        writer.Write((short) ((this[x, y] != null) ? this[x, y].TileNumber : 0));
                        // Passible is true when == 0 in the data
                        writer.Write(Convert.ToInt16((this[x, y] == null) || !this[x, y].Passable));
                        writer.Write((short) ((this[x, y] != null) ? this[x, y].ObjectNumber : 0));
                    }
                    else
                    {
                        // MAP format has big endian which we need to do manually
                        var tileBytes = BitConverter.GetBytes(this[x, y].TileNumber);
                        var objectBytes = BitConverter.GetBytes(this[x, y].ObjectNumber);
                        writer.Write(tileBytes[1]);
                        writer.Write(tileBytes[0]);
                        // Passible is true when == 0 in the data
                        writer.Write(Convert.ToInt16((this[x, y] == null) || !this[x, y].Passable));
                        writer.Write(objectBytes[1]);
                        writer.Write(objectBytes[0]);
                    }
                }
            }

            writer.Close();
            mapFileStream.Close();
            Name = Path.GetFileNameWithoutExtension(mapPath);
            IsModified = false;
        }

        public Tile this[int x, int y]
        {
            get => mapData[x, y];
            set
            {
                if (!IsEditable) return;
                mapData[x, y] = value;
                mapCache[x, y] = null;
                IsModified = true;
            }
        }

        public Map(int width, int height)
        {
            CreateEmptyMap(width, height);
            IsEditable = true;
        }
        private void CreateEmptyMap(int width, int height)
        {
            Size = new Size(width, height);
            mapData = new Tile[width,height];
            mapCache = new Bitmap[width,height];
            totalPassable = 0;
            totalNotPassable = width * height;
        }

        public void ResizeMap(Size newSize)
        {
            var oldSize = Size;
            var oldData = mapData;
            var oldCache = mapCache;
            Size = newSize;
            IsModified = true;
            mapData = new Tile[Size.Width, Size.Height];
            mapCache = new Bitmap[Size.Width, Size.Height];
            // Copy columns up to the size of the smaller width of the two arrays. A row is the size of the smaller height of the two arrays
            // We will either let the end fill with null or we are pruning
            for (int x = 0; x < Math.Min(oldSize.Width, newSize.Width); x++)
            {
                var startIndexOld = oldSize.Height * x;
                var startIndexNew = Size.Height * x;
                Array.Copy(oldData, startIndexOld, mapData, startIndexNew, Math.Min(oldSize.Height, Size.Height));
                Array.Copy(oldCache, startIndexOld, mapCache, startIndexNew, Math.Min(oldSize.Height, Size.Height));
            }
            
            RecalculateTotalPassable();
        }

        public void ClearCache()
        {
            mapCache = new Bitmap[Size.Width, Size.Height];
        }

        public Bitmap GetFullyRenderedTile(int x, int y, int sizeModifier, bool forceRenderEmpty, bool currentShowTiles, bool currentShowObjects)
        {
            var cachedTile = mapCache[x, y];
            if (cachedTile?.Size.Width == sizeModifier && showTiles == currentShowTiles && showObjects == currentShowObjects)
                return cachedTile;
            showTiles = currentShowTiles;
            showObjects = currentShowObjects;
            var tileBitmap = !showTiles ? null : this[x, y]?.RenderTile();
            var objectBitmap = GetObjectBitmap(x, y);
            if (forceRenderEmpty)
            {
                Bitmap bitmapClear = new Bitmap(sizeModifier, sizeModifier);
                Graphics gClear = Graphics.FromImage(bitmapClear);
                gClear.Clear(Color.DarkGreen);

                if (objectBitmap == null)
                {
                    if (tileBitmap == null) objectBitmap = bitmapClear;
                    if (tileBitmap != null) objectBitmap = new Bitmap(sizeModifier, sizeModifier);
                }
                if (tileBitmap == null) tileBitmap = bitmapClear;
                gClear.Dispose();
                //bitmapClear.Dispose();
            }

            // Only tile
            if (showTiles && tileBitmap != null && (!showObjects || objectBitmap == null))
            {
                objectBitmap?.Dispose();
                mapCache[x, y] = tileBitmap;
            }

            // Only object
            else if (showObjects && objectBitmap != null && (!showTiles || tileBitmap == null))
            {
                var renderedBitmap = ImageRenderer.Singleton.GetFilledObjectBitmap(objectBitmap, x, y);
                tileBitmap?.Dispose();
                objectBitmap.Dispose();
                mapCache[x, y] = renderedBitmap;
            }

            // Both
            else if (showTiles && showObjects && tileBitmap != null && objectBitmap != null)
            {
                var renderedBitmap = ImageRenderer.Singleton.GetCombinedBitmap(tileBitmap, objectBitmap);
                tileBitmap.Dispose();
                objectBitmap.Dispose();
                mapCache[x, y] = renderedBitmap;
            }

            return mapCache[x, y];
        }

        private Bitmap GetObjectBitmap(int x, int y)
        {
            Tile[] tilesWithPossibleObjects = new Tile[12]; 
            for (int i = 0; i < 12; i++)
            {
                if ((i + y) >= Size.Height) break;
                tilesWithPossibleObjects[i] = this[x, y + i];
            }
            
            return ImageRenderer.Singleton.RenderObjects(tilesWithPossibleObjects);
        }

        public Bitmap GetRenderedMap(bool currentShowTiles, bool currentShowObjects)
        {

            showTiles = currentShowTiles;
            showObjects = currentShowObjects;
            
            var sizeModifier = ImageRenderer.Singleton.sizeModifier;
            
            var returnImage = new Bitmap(Size.Width * sizeModifier, Size.Height * sizeModifier);
            var graphics = Graphics.FromImage(returnImage);
            graphics.Clear(Color.DarkGreen);
            
            if (showTiles || showObjects)
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    for (int y = 0; y < Size.Height; y++)
                    {
                        var xPos = x * sizeModifier;
                        var yPos = y * sizeModifier;
                        var renderedTile = GetFullyRenderedTile(x, y, sizeModifier, false, showTiles, showObjects);
                        if (renderedTile != null)
                        {
                            graphics.DrawImage(renderedTile, xPos, yPos);
                        }
                        else
                            graphics.FillRectangle(Brushes.DarkGreen, xPos, yPos, sizeModifier, sizeModifier);
                    }
                }
            }

            graphics.Dispose();
            return returnImage;
        }

        private void RecalculateTotalPassable()
        {
            totalPassable = 0;
            totalNotPassable = 0;
            foreach (var tile in mapData)
            {
                if (tile != null)
                {
                    totalPassable += tile.Passable ? 1 : 0;
                    totalNotPassable += tile.Passable ? 0 : 1;
                }
            }
        }

        public bool IsMorePassable()
        {
            return totalPassable > totalNotPassable;
        }

        public void SetAllPass(bool enabled)
        {
            foreach (var tile in mapData)
            {
                tile.Passable = enabled;
            }

            totalPassable = enabled ? Size.Width * Size.Height : 0;
            totalNotPassable = enabled ? 0 : Size.Width * Size.Height;
            IsModified = true;
        }
    }
}
