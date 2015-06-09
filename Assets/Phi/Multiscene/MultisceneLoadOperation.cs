using UnityEngine;
using System.Collections;
namespace Phi
{

    public class MultisceneLoadOperation
    {
        MultisceneLoadOperation(string path)
        {
            this.path = path;
        }

        private string path;
        /*
        public Coroutine WaitTillLoaded
        {
            get
            {
                return MultisceneLoadManager.WaitTillLoaded(this);
            }
        }*/
    }
}
