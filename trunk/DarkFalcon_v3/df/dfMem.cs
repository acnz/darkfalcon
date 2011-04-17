using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMem : dfIOb
    {
        dfCom[] _mems;
        public dfMem(int Qtd)
        {
            _mems = new dfCom[Qtd];
            for (int i = 0; i < Qtd; i++)
            {
                add(new dfCom("Memoria"));
            }
        }
        public dfCom[] Mems
        {
            get { return _mems; }
        }
        public string add(dfCom m)
        {
            if (m.Tipo == "Memoria")
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _mems.ToList())
                    tl.Add(d);
                tl.Add(m);
                if (tl.Count <= _mems.Count())
                {
                    _mems = tl.ToArray();
                    return ("ok");
                }
                else
                {
                    return ("Não há mais slots de memória Disponíveis!(Max: " + _mems.Count() + ")");
                }
            }
            else
            {
                return (m.Nome + " não é uma Memória!");
            }
        }

    }
}
