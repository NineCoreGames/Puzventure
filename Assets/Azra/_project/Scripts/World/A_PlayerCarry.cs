using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

namespace Puzventure.Azra
{
    public class A_PlayerCarry : MonoBehaviour
    {
        // Inspector'da atanacak: oyuncunun kafasýndaki taþýma noktasý
        [SerializeField] private Transform carryPoint;

        // Þu an taþýnan idol (null = eller boþ)
        private A_Cargo carried;

        // Baþka script'ler "elinde bir þey var mý?" diye soracak
        public bool IsCarrying()
        {
            return carried != null;
        }

        // Courier bir trigger'a girdiðinde çalýþýr
        void OnTriggerEnter(Collider other)
        {
            // Who? — dokunduðum þey idol mü?
            A_Cargo cargo = other.GetComponent<A_Cargo>();
            if (cargo == null) return;              // idol deðil ? çýk

            // Eller dolu mu?
            if (IsCarrying()) return;                // dolu ? çýk

            // Bu idol alýnabilir mi? (yeni düþmüþ olabilir)
            if (!cargo.CanBePickedUp()) return;      // hayýr ? çýk

            // Act — al ve kafana koy
            carried = cargo;
            cargo.PickUp(carryPoint);
        }

        // Tehlikeler bunu çaðýracak
        public void DropCargo()
        {
            if (!IsCarrying()) return;
            carried.Drop();
            carried = null;
        }

        // Hedef zone bunu çaðýracak
        public A_Cargo HandOver()
        {
            A_Cargo cargo = carried;
            carried = null;          // eller boþaldý
            return cargo;            // idolu geri ver
        }
    }
}
