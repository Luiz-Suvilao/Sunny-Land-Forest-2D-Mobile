using UnityEngine;

namespace Projeto.Scripts
{
    public class IaPlataform : MonoBehaviour
    {
        public Transform plataform, pontoA, pontoB;
        public Vector3 pontoDestino;
        public GameObject player;

        public float plataformVelocity; 
        void Start()
        {
            plataform.position = pontoA.position;
            pontoDestino = pontoB.position;
        }
        
        void Update()
        {
            if (plataform.position == pontoA.position)
            {
                pontoDestino = pontoB.position;
            }

            if (plataform.position == pontoB.position)
            {
                pontoDestino = pontoA.position;
            }

            plataform.position = Vector3.MoveTowards(plataform.position, pontoDestino, plataformVelocity);
        }
    }
}
