using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeByTilePlacer : MonoBehaviour
{
    [SerializeField] private float _treeWidth = 1f;
    [SerializeField] private GameObject[] _treePrefabs;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _treeTile;
    [SerializeField] private Grid _grid;

    [SerializeField] private float _maxSize = 0.8f;
    [SerializeField] private float _minSize = 1.3f;
    [SerializeField] private bool _drawTiles = false;
    
    private readonly List<GameObject> _trees = new();
    private Transform _treesParent;

    [Button]
    public void Place()
    {
        UpdateTrees();
        
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                TileBase tile = _tilemap.GetTile(new Vector3Int(x, y, 0));

                if (tile == null)
                    continue;

                if (tile == _treeTile)
                {
                    if (TryGetFreePlaceInsideTile(new Vector3Int(x, y, 0), out Vector3 position))
                    {
                        GameObject tree = CreateRandomTree();
                        tree.transform.position = position;
                        tree.transform.parent = _treesParent;
                    }
                }
            }
        }
        
        Debug.Log($"Trees count: {_trees.Count}");
    }
    
    
    [Button]
    public void Clean()
    {
        UpdateTrees();
        _trees.ForEach(DestroyImmediate);
    }

    public void OnDrawGizmos()
    {
        if (!_drawTiles) return;
        Gizmos.color = Color.blue;
        
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                TileBase tile = _tilemap.GetTile(new Vector3Int(x, y, 0));

                if (tile == null)
                    continue;

                if (tile == _treeTile)
                    Gizmos.DrawWireCube(new Vector3(x, y) + transform.position + _grid.cellSize/2, new Vector3(_grid.cellSize.x, _grid.cellSize.y, 1));
            }
        }
    }

    private GameObject CreateRandomTree()
    {
        int index = Random.Range(0, _treePrefabs.Length);
        GameObject tree = Instantiate(_treePrefabs[index]);
        tree.transform.Rotate(Vector3.forward, Random.Range(0, 360));
        tree.transform.localScale *= Random.Range(_minSize, _maxSize);
        _trees.Add(tree);
        return tree;
    }
    
    private void UpdateTrees()
    {
        UpdateTreesParent();
        _trees.Clear();
        for (int i = 0; i < _treesParent.childCount; i++)
            _trees.Add(_treesParent.GetChild(i).gameObject);
    }
    
    private bool TryGetFreePlaceInsideTile(Vector3Int tilePos, out Vector3 position)
    {
        int tries = 10;
        for (int i = 0; i < tries; i++)
        {
            float x = Random.Range(0f, _grid.cellSize.x) + tilePos.x - _grid.cellSize.x / 2;
            float y = Random.Range(0f, _grid.cellSize.y) + tilePos.y - _grid.cellSize.y / 2;
            position = new Vector3(x, y) + transform.position + _grid.cellSize/2;
            bool isFreePoint = true;
            foreach (var tree in _trees)
            {
                float distance = (tree.transform.position - position).sqrMagnitude;
                if (distance * distance <= _treeWidth)
                    isFreePoint = false;
            }
            if (isFreePoint)
                return true;
        }
        position = default;
        return false;
    }

    private void UpdateTreesParent()
    {
        _treesParent = transform.Find("TreesParent");
        if (_treesParent == null)
        {
            _treesParent = new GameObject("TreesParent").transform;
            _treesParent.parent = transform;
        }
    }
}