using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCBuilder : MonoBehaviour
{
    [Tooltip("Width of the world")]
    [SerializeField] private int Width;
    [Tooltip("Height of the world")]
    [SerializeField] private int Height;

    private WFCNode[,] _grid;

    public List<WFCNode> Nodes = new();

    private List<Vector2Int> _toCollapse = new();

    private Vector2Int[] offsets = new Vector2Int[] {
        new(0, 2),//top
        new(0, -2),//bottom
        new(2, 0),//right
        new(-2, 0)//left
    };

    private void Start() {
        _grid = new WFCNode[Width, Height];

        CollapseWorld();
    }

    private void CollapseWorld()
    {
        _toCollapse.Clear();

        _toCollapse.Add(new(Width / 2, Height / 2));

        while (_toCollapse.Count > 0)
        {
            int x = _toCollapse[0].x;
            int y = _toCollapse[0].y;

            List<WFCNode> potentialNodes = new(Nodes);

            for (int i = 0; i < offsets.Length; i++)
            {
                Vector2Int neighbor = new(x + offsets[i].x, y + offsets[i].y);

                if (IsInsideGrid(neighbor))
                {
                    WFCNode neighborNode = _grid[neighbor.x, neighbor.y];

                    if (neighborNode != null)
                    {
                        switch (i)
                        {
                            case 0://top
                                WhittleNodes(potentialNodes, neighborNode.Bottom.CompatibleNodes);
                                break;
                            case 1://bottom
                                WhittleNodes(potentialNodes, neighborNode.Top.CompatibleNodes);
                                break;
                            case 2://right
                                WhittleNodes(potentialNodes, neighborNode.Left.CompatibleNodes);
                                break;
                            case 3://left
                                WhittleNodes(potentialNodes, neighborNode.Right.CompatibleNodes);
                                break;
                        }
                    }
                    else
                    {
                        if (!_toCollapse.Contains(neighbor)) _toCollapse.Add(neighbor);
                    }
                }
            }

            if (potentialNodes.Count < 1)
            {
                _grid[x, y] = Nodes[0];
                Debug.LogWarning($"Attempted to collapse wave on {x}, {y} but found no compatible nodes.");
            }
            else
            {
                //could give nodes weight depending on which nodes would be more frequent in the map
                _grid[x, y] = potentialNodes[Random.Range(0, potentialNodes.Count)];
            }

            GameObject newNode = Instantiate(_grid[x, y].Prefab, new Vector3(x, y, 0f), Quaternion.identity);

            _toCollapse.RemoveAt(0);
        }
    }

    private void WhittleNodes(List<WFCNode> potentialNodes, List<WFCNode> validNodes)
    {
        for (int i = potentialNodes.Count - 1; i > -1; i--)
        {
            if (!validNodes.Contains(potentialNodes[i]))
            {
                potentialNodes.RemoveAt(i);
            }
        }
    }

    private bool IsInsideGrid(Vector2Int v2int)
    {
        if (v2int.x > -1 && v2int.x < Width && v2int.y > -1 && v2int.y < Height)
            return true;
        else
            return false;
    }
}
