using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
        public class dfPlaca : dfIOb
        {
            dfCom[] _pcie2;
            dfCom[] _pcie1;
            dfCom[] _pci;
            dfCom[] _vga;

            public int nMon
            {
                get
                {
                    int i=0;
                    dfCom current =  new dfCom("?","?","P.Video",0.0f,"#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "P.Video")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.qtd[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }
            public int nSom
            {
                get
                {
                    int i = 0;
                    dfCom current = new dfCom("?", "?", "P.Som", 0.0f, "#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "P.Som")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.compat[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }
            public int nRede
            {
                get
                {
                    int i = 0;
                    dfCom current = new dfCom("?", "?", "P.Rede", 0.0f, "#0");
                    foreach (dfCom d in getAllP())
                    {
                        if (d.Tipo == "P.Rede")
                        {
                            if (current == d)
                            {
                                i += int.Parse(d.Tags.qtd[0]);
                            }
                            else
                            {
                                if (int.Parse(d.Tags.qtd[0]) >= int.Parse(current.Tags.qtd[0]))
                                {
                                    current = d;
                                    i = 0;
                                    i += int.Parse(current.Tags.qtd[0]);
                                }
                            }
                        }
                    }
                    return i;
                }
            }

            private List<dfCom> getAllP()
            {
                List<dfCom> tl = new List<dfCom>();
                foreach (dfCom d in _pcie2.ToList())
                    tl.Add(d);
                foreach (dfCom d in _pcie1.ToList())
                    tl.Add(d);
                foreach (dfCom d in _pci.ToList())
                    tl.Add(d);
                foreach (dfCom d in _vga.ToList())
                    tl.Add(d);

                return tl;
            }
            public dfPlaca(int Qtdp2, int Qtdp1, int Qtdp, int Qtdv)
            {
                _pcie2 = new dfCom[Qtdp2];
                _pcie1 = new dfCom[Qtdp1];
                _pci = new dfCom[Qtdp];
                _vga = new dfCom[Qtdv];
                for (int i = 0; i < Qtdp2; i++)
                {
                   _pcie2[i]=new dfCom("P.");
                }
                for (int i = 0; i < Qtdp1; i++)
                {
                    _pcie1[i] = new dfCom("P.");
                }
                for (int i = 0; i < Qtdp; i++)
                {
                    _pci[i] = new dfCom("P.");
                }
                for (int i = 0; i < Qtdv; i++)
                {
                    _vga[i] = new dfCom("P.");
                }
            }

            public dfCom[] Pcie2
            {
                get { return _pcie2; }
            }
            public dfCom[] Pcie1
            {
                get { return _pcie1; }
            }
            public dfCom[] Pci
            {
                get { return _pci; }
            }
            public dfCom[] Vga
            {
                get { return _vga; }
            }
            public string add(dfCom m)
            {
                if (m.Tipo.Contains("P."))
                {
                    string ret="";
                    foreach (string t in m.Tags.compat)
                    {
                        if (t == "pcie2")
                        {
                            List<dfCom> tl = new List<dfCom>();
                            foreach (dfCom d in _pcie2.ToList())
                                tl.Add(d);
                            tl.Add(m);
                            if (tl.Count <= _pcie2.Count())
                            {
                                _pcie2 = tl.ToArray();
                                ret = "ok";
                            }
                            else
                            {
                                ret = "Não há slots PciExpress 2.0 Disponíveis!(Max: " + _pcie2.Count() + ")";
                            }
                        }
                        else
                            if (t == "pcie1")
                            {
                                List<dfCom> tl = new List<dfCom>();
                                foreach (dfCom d in _pcie1.ToList())
                                    tl.Add(d);
                                tl.Add(m);
                                if (tl.Count <= _pcie1.Count())
                                {
                                    _pcie1 = tl.ToArray();
                                    ret = "ok";
                                }
                                else
                                {
                                    ret = "Não há slots PciExpress Disponíveis!(Max: " + _pcie1.Count() + ")";
                                }
                            }
                            else
                                if (t == "pci")
                                {
                                    List<dfCom> tl = new List<dfCom>();
                                    foreach (dfCom d in _pci.ToList())
                                        tl.Add(d);
                                    tl.Add(m);
                                    if (tl.Count <= _pci.Count())
                                    {
                                        _pcie1 = tl.ToArray();
                                        ret = "ok";
                                    }
                                    else
                                    {
                                        ret = "Não há slots Pci Disponíveis!(Max: " + _pci.Count() + ")";
                                    }
                                }
                                else
                                    if (t == "vga")
                                    {
                                        List<dfCom> tl = new List<dfCom>();
                                        foreach (dfCom d in _vga.ToList())
                                            tl.Add(d);
                                        tl.Add(m);
                                        if (tl.Count <= _vga.Count())
                                        {
                                            _pcie1 = tl.ToArray();
                                            ret = "ok";
                                        }
                                        else
                                        {
                                          ret = "Não há slots Vga Disponíveis!(Max: " + _vga.Count() + ")";
                                        }
                                    }
                                    else
                                    {
                                        ret = "A Motherboard não possui o tipo de slot necessário!";
                                    }
                      
                    }
                    return ret;
                }
                else
                {
                    return (m.Nome + " não é uma Placa!");
                }
            }

        }
    
}
