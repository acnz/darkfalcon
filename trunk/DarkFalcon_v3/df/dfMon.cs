using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfMon : dfIOb
    {
        dfCom[] _mons;
        public dfMon(int Qtd)
        {
            _mons = new dfCom[Qtd];
            for(int i = 0; i<Qtd;i++){
                add(new dfCom("Monitor"));
            }
        }
        public dfCom[] Mons
        {
            get { return _mons; }
        }
        public string add(dfCom m)
        {
            if (m.Tipo == "Monitor")
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _mons.ToList())
                    tl.Add(d);
                tl.Add(m);
                if (tl.Count <= _mons.Count())
                {
                    _mons = tl.ToArray();
                    return ("ok");
                }
                else
                {
                    return ("Não há mais saidas de vídeo Disponíveis!(Max: " + _mons.Count() + ")");
                }
            }
            else
            {
                return (m.Nome + " não é uma Monitor!");
            }
        }

    }
}
