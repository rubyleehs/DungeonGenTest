using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class DungeonDecoPlacer : MonoBehaviour
    {
        public static Deco deco;
        public Deco I_deco;
        public Transform decoParent;
        private Vector2 cellSize;

        private List<Transform> decoList;

        public void Init(Vector2 _cellSize)
        {
            deco = I_deco;
            cellSize = _cellSize;
            if (decoList == null) decoList = new List<Transform>();
            else ClearDecos();
        }

        public void ClearDecos()
        {
            for (int i = 0; i < decoList.Count; i++)
            {
                Destroy(decoList[i].gameObject);
            }
            decoList.Clear();
        }

        public void PlaceDecos(ref Cell[,] cells)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    switch (cells[x, y].genID)
                    {
                        case 1:
                            PlaceTorch(x, y, cells[x, y].visualMetaID);
                            PlaceChandelier(x, y, cells[x, y].visualMetaID);
                            break;
                        case 2:
                            PlaceArch(x, y, cells[x, y].visualMetaID);
                            break;
                        default: break;
                    }
                }
            }
        }

        private void PlaceArch(int x, int y, int cellMetaID)//deco type placed is random aka decoID is random
        {
            if (Random.Range(0f, 1f) > deco.archChance) return;
            Sprite s = null;
            switch (cellMetaID)
            {
                case 6:
                    s = deco.decoArch[Random.Range(0, deco.decoArch.Length)].metaSprites[0];
                    break;
                case 9:
                    s = deco.decoArch[Random.Range(0, deco.decoArch.Length)].metaSprites[1];
                    break;
                default: break;
            }

            if (s != null)
            {
                SpriteRenderer sr = Instantiate(deco.decoGO, new Vector2(x, y +0.5f) * cellSize, Quaternion.identity, decoParent).GetComponent<SpriteRenderer>();
                sr.transform.localScale *= cellSize;
                sr.sprite = s;
                decoList.Add(sr.transform);
            }
        }

        private void PlaceTorch(int x, int y, int cellMetaID)//deco type placed is random aka decoID is random
        {
            if (Random.Range(0f, 1f) > deco.torchChance) return;
            Sprite s = null;
            switch (cellMetaID)
            {
                case 14:
                    s = deco.decoTorch[Random.Range(0, deco.decoTorch.Length)].metaSprites[0];
                    break;
                case 13:
                    s = deco.decoTorch[Random.Range(0, deco.decoTorch.Length)].metaSprites[1];
                    break;
                case 12:
                    s = deco.decoTorch[Random.Range(0, deco.decoTorch.Length)].metaSprites[2];
                    break;
                case 10:
                    s = deco.decoTorch[Random.Range(0, deco.decoTorch.Length)].metaSprites[3];
                    break;

                default: break;
            }

            if (s != null)
            {
                SpriteRenderer sr = Instantiate(deco.decoLightGO, new Vector2(x, y+0.5f) * cellSize, Quaternion.identity, decoParent).GetComponent<SpriteRenderer>();
                sr.transform.localScale *= cellSize;
                sr.sprite = s;
                decoList.Add(sr.transform);
            }
        }


        private void PlaceChandelier(int x, int y, int cellMetaID)//deco type placed is random aka decoID is random
        {
            if (Random.Range(0f, 1f) > deco.chandelierChance) return;
            Debug.Log(x + " , " + y);
            Sprite s = null;
            switch (cellMetaID)
            {
                case 15:
                    s = deco.decoChandelier[Random.Range(0, deco.decoChandelier.Length)].metaSprites[0];
                    break;

                default: break;
            }

            if (s != null)
            {
                SpriteRenderer sr = Instantiate(deco.decoChandelierGO, new Vector2(x, y+0.5f) * cellSize, Quaternion.identity, decoParent).GetComponent<SpriteRenderer>();
                sr.transform.localScale *= cellSize;
                sr.sprite = s;
                decoList.Add(sr.transform);
            }
        }
    }

[System.Serializable]
    public struct Deco
    {
        public GameObject decoGO;
        public GameObject decoLightGO;
        public GameObject decoChandelierGO;

        [Range(0, 1)]
        public float archChance;
        [Range(0, 1)]
        public float torchChance;
        [Range(0, 1)]
        public float chandelierChance;

        //0 right, 1 up, 2 left,3 down
        public TileSprites[] decoArch; 
        public TileSprites[] decoTorch;
        public TileSprites[] decoChandelier;
    }
}
