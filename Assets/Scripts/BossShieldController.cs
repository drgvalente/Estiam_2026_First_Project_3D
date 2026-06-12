using UnityEngine;

public class BossShieldController : MonoBehaviour
{
    private Material shieldMaterial;

    // Array que guarda os impactos: x,y,z = posição mundial, w = intensidade (0 a 1)
    private Vector4[] impacts = new Vector4[4];
    private int nextImpactIndex = 0;

    public float fadeSpeed = 2f; // Quão rápido o impacto desaparece

    void Start()
    {
        // Pega o material instanciado desse renderer (não altera o material original)
        shieldMaterial = GetComponent<Renderer>().material;

        // Inicializa os impactos com intensidade 0 (invisíveis)
        for (int i = 0; i < impacts.Length; i++)
        {
            impacts[i] = new Vector4(0, 0, 0, 0);
        }
    }

    void Update()
    {
        bool changed = false;

        // Desfade (diminui a intensidade) dos impactos ao longo do tempo
        for (int i = 0; i < impacts.Length; i++)
        {
            if (impacts[i].w > 0)
            {
                impacts[i].w -= Time.deltaTime * fadeSpeed;

                // Garante que não fique negativo
                if (impacts[i].w < 0) impacts[i].w = 0;

                changed = true;
            }
        }

        // Se algo mudou, atualiza o shader para não gastar processamento à toa
        if (changed)
        {
            shieldMaterial.SetVectorArray("_Impacts", impacts);
        }
    }

    // Função pública para a Bala chamar quando acertar o escudo
    public void RegisterHit(Vector3 hitPoint)
    {
        // Manda a posição mundial direto, sem converter!
        impacts[nextImpactIndex] = new Vector4(hitPoint.x, hitPoint.y, hitPoint.z, 1f);

        // Atualiza o shader imediatamente
        shieldMaterial.SetVectorArray("_Impacts", impacts);

        // Passa para o próximo índice (volta pra 0 quando chega em 4)
        nextImpactIndex = (nextImpactIndex + 1) % impacts.Length;
    }
}