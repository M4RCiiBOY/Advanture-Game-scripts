using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

public class MapBuilder : MonoBehaviour
{

    private const string MapFileExtension = ".map";
    private const string MapFileFilter = "Map files (.map)|*map";
    private const string XMLElementHeight = "Height";
    private const string XMLElementWidth = "Width";
    private const string XMLElementPosX = "X";
    private const string XMLElementPosY = "y";
    private const string XMLElementTiles = "Tiles";
    private const string XMLElementObjects = "Objects";
    private const string XMLElementType = "Type";

    Map map;
    Map objectMap;
    public string MapName;
    public int mapSizeInTiles = 9;
    public float tileSizeModif = 1;

    public GameObject House;
    public GameObject[] mobs;
    public GameObject boss;

    public GameObject[] forest;


    void Start()
    {
        //Clear();
        //ReadXMLFile();
        //BuildMap();
    }

    public void Reload()
    {
        Clear();
        ReadXMLFile();
        BuildMap();
        if (GetComponent<RuntimeTileChange>() != null)
        {
            GetComponent<RuntimeTileChange>().Starting();
        }
    }

    public void Clear()
    {
        try
        {
            ArrayList mapChildren = new ArrayList();
            foreach (Transform child in transform)
            {
                mapChildren.Add(child.gameObject);
            }
            foreach (GameObject child in mapChildren)
            {
                Destroy(child);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            throw;
        }
        Debug.Log("Map cleared");
    }

    private void BuildMap()
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                switch (map[x, y].Assetpack)
                {
                    case MapTile.pack.CSTM:
                        CSTMBuild(x, y, false);
                        break;
                    case MapTile.pack.FRST:
                        FRSTBuild(x, y, false);
                        break;
                    case MapTile.pack.VILG:
                        VILGBuild(x, y, false);
                        break;
                    case MapTile.pack.DNGN:
                        DNGNBuild(x, y, false);
                        break;
                }
                switch (objectMap[x, y].Assetpack)
                {
                    case MapTile.pack.CSTM:
                        CSTMBuild(x, y, true);
                        break;
                    case MapTile.pack.FRST:
                        FRSTBuild(x, y, true);
                        break;
                    case MapTile.pack.VILG:
                        VILGBuild(x, y, true);
                        break;
                    case MapTile.pack.DNGN:
                        DNGNBuild(x, y, true);
                        break;
                }
            }
        }
    }

    private void DNGNBuild(int x, int y, bool objectLayer)
    {
        throw new NotImplementedException();

    }

    private void VILGBuild(int x, int y, bool objectLayer)
    {
        throw new NotImplementedException();
    }

    private void FRSTBuild(int x, int y, bool objectLayer)
    {
        if (objectLayer)
        {
            Instantiation(forest[int.Parse(objectMap[x, y].Type)], x, y, objectLayer);
        }
        else
        {
            Instantiation(forest[int.Parse(map[x, y].Type)], x, y, objectLayer);
        }
    }

    private void CSTMBuild(int x, int y, bool objectLayer)
    {
        switch (objectMap[x, y].Type) //The cases are stil called P1, P2 etc since these types were created for a previous version, but since changing them would require changing the level editor I'm gonna use them as they are
        {
            case "P1":
                Instantiation(House, x, y, objectLayer, 5.2f);
                break;
            case "P2":
                GameObject bos = Instantiate(boss, this.transform.position /*+ (new Vector3(x, -y, 0) * tileSizeModif)*/, Quaternion.identity);
                bos.transform.SetParent(this.gameObject.transform);
                bos.transform.localScale = new Vector3(8, 8, 8);
                bos.transform.position += new Vector3(x, -y, 0);
                break;
            case "Enemy":
                GameObject mob = Instantiate(mobs[UnityEngine.Random.Range(0, mobs.Length)], this.transform.position /*+ (new Vector3(x, -y, 0) * tileSizeModif)*/, Quaternion.identity);
                mob.transform.SetParent(this.gameObject.transform);
                mob.transform.localScale = new Vector3(8, 8, 8);
                mob.transform.position += new Vector3(x, -y, 0);
                break;
            default:
                break;
        }
    }





    //case "sign":
    //                        GameObject obj = Instantiation(sign, x, y, true);
    //obj.GetComponent<Effect>().id = objectMap[x, y].Id;
    //                        obj.GetComponent<Effect>().connection = objectMap[x, y].Connection;
    //                        break;
    //                    case "pressure":
    //                        obj = Instantiation(pressure, x, y, true);
    //obj.GetComponent<Switch>().id = objectMap[x, y].Id;
    //                        obj.GetComponent<Switch>().connection = objectMap[x, y].Connection;
    //                        break;

    private GameObject Instantiation(GameObject type, float x, float y, bool objectLayer)
    {
        GameObject obj = Instantiate(type, this.transform.position /*+ (new Vector3(x, -y, 0) * tileSizeModif)*/, Quaternion.identity);
        obj.transform.SetParent(this.gameObject.transform);
        obj.transform.localScale = new Vector3(tileSizeModif, tileSizeModif, tileSizeModif);
        obj.transform.position += new Vector3(x, -y, 0);
        if (objectLayer)
        {
            try
            {
                obj.GetComponent<SpriteRenderer>().sortingOrder = (int)(y - (this.gameObject.transform.position.y));
            }
            catch
            {
                SpriteRenderer[] tmpList = obj.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < tmpList.Length; i++)
                {
                    tmpList[i].sortingOrder = (int)(y - (this.gameObject.transform.position.y));
                }
            }
        }
        else
        {
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        }
        return obj;
    }

    private GameObject Instantiation(GameObject type, float x, float y, bool objectLayer, float scale)
    {
        GameObject obj = Instantiate(type, this.transform.position /*+ (new Vector3(x, -y, 0) * tileSizeModif)*/, Quaternion.identity);
        obj.transform.SetParent(this.gameObject.transform);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.transform.position += new Vector3(x, -y, 0);
        if (objectLayer)
        {
            try
            {
                obj.GetComponent<SpriteRenderer>().sortingOrder = (int)(y - (this.gameObject.transform.position.y));
            }
            catch
            {
                SpriteRenderer[] tmpList = obj.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < tmpList.Length; i++)
                {
                    tmpList[i].sortingOrder = (int)(y - (this.gameObject.transform.position.y));
                }
            }
        }
        else
        {
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        }
        return obj;
    }


    private void ReadXMLFile()
    {
        try
        {
            XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/MapXML/" + MapName + ".map");
            reader.Read();
            reader.ReadToFollowing(XMLElementWidth);
            reader.ReadStartElement();
            int width = reader.ReadContentAsInt();
            reader.ReadToFollowing(XMLElementHeight);
            reader.ReadStartElement();
            int height = reader.ReadContentAsInt();
            this.map = new Map(width, height);
            reader.ReadToFollowing(XMLElementTiles);
            for (int i = 0; i < width * height; i++)
            {
                reader.ReadToFollowing(XMLElementPosX);
                reader.ReadStartElement();
                var x = reader.ReadContentAsInt();
                reader.ReadToFollowing(XMLElementPosY);
                reader.ReadStartElement();
                var y = reader.ReadContentAsInt();
                reader.ReadToFollowing(XMLElementType);
                reader.ReadStartElement();
                string tmp = reader.ReadContentAsString();
                var pack = tmp.Substring(0, 4);
                var type = tmp.Substring(5);

                var mapTile = new MapTile(x, y, type, pack);
                this.map[x, y] = mapTile;
            }
            this.objectMap = new Map(width, height);
            reader.ReadToFollowing(XMLElementObjects);
            for (int i = 0; i < width * height; i++)
            {
                reader.ReadToFollowing(XMLElementPosX);
                reader.ReadStartElement();
                var x = reader.ReadContentAsInt();
                reader.ReadToFollowing(XMLElementPosY);
                reader.ReadStartElement();
                var y = reader.ReadContentAsInt();
                reader.ReadToFollowing(XMLElementType);
                reader.ReadStartElement();
                string tmp = reader.ReadContentAsString();
                var pack = tmp.Substring(0, 4);
                var type = tmp.Substring(5);
                reader.ReadToFollowing("Id");
                reader.ReadStartElement();
                var id = reader.ReadContentAsInt();
                reader.ReadToFollowing("Connection");
                reader.ReadStartElement();
                var connection = reader.ReadContentAsString();

                var mapObjects = new MapTile(x, y, type, pack, id, connection);
                this.objectMap[x, y] = mapObjects;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            throw;
        }

    }



    void Update()
    {

    }
}
