using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Multiplayer_Scripts
{
    /// <summary>
    /// This controller class holds the behaviour of the inivitation page.
    /// </summary>
    class InvitationController : MonoBehaviour
    {
        /// <summary>
        /// A variable that holds the decline button game object
        /// </summary>
        public GameObject btnDecline;

        /// <summary>
        /// A variable that holds the accept button game object
        /// </summary>
        public GameObject btnAccept;

        /// <summary>
        /// This method is used to hide the buttons.
        /// </summary>
        public void Hide()
        {
            btnDecline.SetActive(false);
            btnAccept.SetActive(false);
        }

        /// <summary>
        /// This method is used to show the buttons.
        /// </summary>
        public void Show()
        {
            btnDecline.SetActive(true);
            btnAccept.SetActive(true);
        }
    }
  
}
