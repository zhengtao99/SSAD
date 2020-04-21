using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Multiplayer_Scripts
{
    class InvitationController : MonoBehaviour
    {
        public GameObject btnDecline;
        public GameObject btnAccept;
        public void Hide()
        {
            btnDecline.SetActive(false);
            btnAccept.SetActive(false);
        }
        public void Show()
        {
            btnDecline.SetActive(true);
            btnAccept.SetActive(true);
        }
    }
  
}
