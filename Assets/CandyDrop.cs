using UnityEngine;

public class CandyDrop : MonoBehaviour
{
    public Sprite[] candySprites;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    Rigidbody2D rigidbody2D;
    readonly float candyDropFallSpeed = 0.1f;

    public object CandyDropAnswer;
    TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        var randomIndex = Random.Range(0, candySprites.Length);

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = candySprites[randomIndex];

        capsuleCollider2D = gameObject.AddComponent<CapsuleCollider2D>();
        capsuleCollider2D.size = new Vector2(1.0f, 1.0f);
        capsuleCollider2D.isTrigger = true;

        rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = true;

        // set up text mesh
        var textMeshParent = new GameObject();

        textMesh = textMeshParent.AddComponent<TextMesh>();
        textMesh.font = LevelManager.instance.DefaultFont;
        textMesh.fontSize = 300;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.text = CandyDropAnswer.ToString();
        textMesh.transform.localScale = new Vector3(0.025f, 0.025f, 1);

        textMeshParent.transform.SetParent(gameObject.transform);
        textMeshParent.transform.localPosition = new Vector3(0.0f, -0.96f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime;

        if (textMesh != null && string.IsNullOrEmpty(textMesh.text))
            textMesh.text = CandyDropAnswer.ToString();

        if (Vector2.Distance(transform.position, Camera.main.transform.position) > 15)
            Destroy(gameObject);
    }
}
