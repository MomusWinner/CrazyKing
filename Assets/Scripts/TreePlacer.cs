using System.Collections.Generic;
using UnityEngine;
using TriInspector;


public class TreePlacer : MonoBehaviour
{
    [SerializeField]private float _treeWidth;
    [SerializeField] private GameObject[] _treePrefabs;
    [SerializeField] private float _width;
    [SerializeField] private float _height;
    
    [SerializeField] private float _maxSize;
    [SerializeField] private float _minSize;
    
    private readonly List<GameObject> _trees = new();
    
    [Button]
    public void Place()
    {
        UpdateTrees();
        while (TryGetFreePlace(out Vector3 freePosition))
        {
            GameObject tree = CreateRandomTree();
            tree.transform.position = freePosition;
            tree.transform.parent = transform;
        }
    }

    private void UpdateTrees()
    {
        _trees.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            _trees.Add(transform.GetChild(i).gameObject);
        }
    }

    [Button]
    public void Clean()
    {
        UpdateTrees();
        _trees.ForEach(DestroyImmediate);
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

    private bool TryGetFreePlace(out Vector3 position)
    {
        int tries = 200;
        for (int i = 0; i < tries; i++)
        {
            float x = Random.Range(0f, _width) + transform.position.x - _width / 2;
            float y = Random.Range(0f, _height) + transform.position.y - _height / 2;
            position = new Vector3(x, y);
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_width, _height, 1));
    }
}