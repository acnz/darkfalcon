using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMobo : dfCom
    {
        public dfMobo(string IDDoComponete, string NomeDoComponente, string TipoDoComponente, float PrecoDoComponente, string TagsDoComponente)
            :base(IDDoComponete, NomeDoComponente, TipoDoComponente, PrecoDoComponente, TagsDoComponente)
        {
        }
        public dfMobo()
        {
        }
        public dfMobo(bool nulo)
            :base ("Motherboard")
        {
        }
    }
}

