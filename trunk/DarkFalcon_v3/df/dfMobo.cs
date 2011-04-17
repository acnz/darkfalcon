using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMobo : dfCom
    {
        public int mem { get; set; }
        public int spcie2 { get; set; }
        public int spcie1 { get; set; }
        public int spci { get; set; }
        public int svga { get; set; }
        public int ssata { get; set; }
        public int side { get; set; }
        public dfMobo(string IDDoComponete, string NomeDoComponente, string TipoDoComponente, float PrecoDoComponente, string TagsDoComponente)
            :base(IDDoComponete, NomeDoComponente, TipoDoComponente, PrecoDoComponente, TagsDoComponente)
        {
        }
        public dfMobo()
        {
        }
        public dfMobo(bool nulo)
            : base("Motherboard", "#0 #0 #0 #0 #0 #0 #0")
        {
            mem = Convert.ToInt32(Tags.qtd[0]);
            spcie2 = Convert.ToInt32(Tags.qtd[1]);
            spcie1 = Convert.ToInt32(Tags.qtd[2]);
            spci = Convert.ToInt32(Tags.qtd[3]);
            svga = Convert.ToInt32(Tags.qtd[4]);
            ssata = Convert.ToInt32(Tags.qtd[5]);
            side = Convert.ToInt32(Tags.qtd[6]);
        }
    }
}

