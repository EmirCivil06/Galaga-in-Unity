using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CustomCollider : MonoBehaviour
{
    public float width = 2f;  // full width of top bar
    public float barHeight = 0.5f;  // height of top bar
    public float stemWidth = 0.5f;  // width of stem
    public float stemHeight = 1f;  // height of stem, downward from bar

    void Awake()
    {
        CreateTShape();
    }

    void CreateTShape()
    {
        var poly = GetComponent<PolygonCollider2D>();

        // define the points of a T shape (CW or CCW)
        // coordinate origin is at GameObject center
        // You may want to adjust offset, pivot accordingly

        float halfWidth = width * 0.5f;
        float halfStemWidth = stemWidth * 0.5f;

        Vector2[] points = new Vector2[8];

        // Points in local space, e.g.:

        // Top bar left
        points[0] = new Vector2(-halfWidth, stemHeight);
        // Top bar right
        points[1] = new Vector2(halfWidth, stemHeight);
        // Top bar bottom right
        points[2] = new Vector2(halfWidth, stemHeight - barHeight);
        // Stem right top
        points[3] = new Vector2(halfStemWidth, stemHeight - barHeight);
        // Stem right bottom
        points[4] = new Vector2(halfStemWidth, -stemHeight);
        // Stem left bottom
        points[5] = new Vector2(-halfStemWidth, -stemHeight);
        // Stem left top
        points[6] = new Vector2(-halfStemWidth, stemHeight - barHeight);
        // Top bar bottom left
        points[7] = new Vector2(-halfWidth, stemHeight - barHeight);

        poly.pathCount = 1;
        poly.SetPath(0, points);
    }
}
