using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;
    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;
    private bool isSquashing = false;
    public float squashTimer = 0.1f;
    public float squashDuration = 0.5f;
    public float squashFactor = 0.2f;
    private Vector3 originalScale;

    public void TriggerSquashEffect()
    {
        if (!isSquashing)
        {
            isSquashing = true;
            squashTimer = 0f;
        }
    }

    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh= MeshClone;
        renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
        originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;
        if (isSquashing)
        {
            squashTimer += Time.fixedDeltaTime;
            float squashProgress = squashTimer / squashDuration;

            float squashAmount = Mathf.Sin(squashProgress * Mathf.PI);
            float squashY = Mathf.Lerp(1f, squashFactor, squashAmount * squashAmount);
            float squashX = Mathf.Lerp(1f, 1.2f, squashAmount * squashAmount);

            transform.localScale = new Vector3(squashX * originalScale.x, squashY * originalScale.y, originalScale.z);

            if (squashTimer >= squashDuration)
            {
                transform.localScale = originalScale;
                isSquashing = false;
            }
        }

        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(OriginalMesh.vertices[jv[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }

        MeshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;
        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < 0.001f)
                Position = target;
        }
    }
}
