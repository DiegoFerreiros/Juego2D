using UnityEngine;

public class Movement1 : MonoBehaviour
{

    /*

    Doble collider:
        Capsule para colisionar
        Box como sensor para detectar el suerlo y q flote en algunos sitios

    Variable de control para poder atacar y que no se cancele al instante con otra animacion

    

    */

    // state:
    // 1: idle 
    // 2: run 
    // 3: slide
    // 4: jump
    // 5: jump -> fall 
    // 6: fall -> debería activarse despues de la transicion de salto a caida 
    //            o cuando velocidad Y es negativa (así siempre q se caiga, sin necesidad de saltar, se activa)
    // 7: attack
    // 8: wall slide -> intentar lo del wall jump
    // 9: death
    // 10: turn around si me da tiempo


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
