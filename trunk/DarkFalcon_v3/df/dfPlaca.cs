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
            public dfPlaca(int Qtdp2, int Qtdp1, int Qtdp, int Qtdv)
            {
                _pcie2 = new dfCom[Qtdp2];
                _pcie1 = new dfCom[Qtdp1];
                _pci = new dfCom[Qtdp];
                _vga = new dfCom[Qtdv];
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
            public string addP(dfCom m)
            {
                if (m.Tipo.Contains("P."))
                {
                    string ret="";
                    foreach (string t in m.Tags.compat)
                    {
                        if (t.Contains("$pcie2x"))
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
                            if (t.Contains("$pcie1x"))
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
                                if (t.Contains("$pcix"))
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
                                    if (t.Contains("$vgax"))
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
