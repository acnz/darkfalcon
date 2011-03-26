using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon_v3.ClassesComponente
{
    class dfPC
    {
        #region Fields 
        private dfCom _mobo;
        private dfCom _pro;
        private dfCom _mem;
        private dfCom _fonte;
        private dfCom _hd;
        private dfCom _monitor;
        private dfCom _video;
        private dfCom _p1;
        private dfCom _p2;
        private dfCom _p3;
        private dfCom _p4;
        private dfCom _p5;
     #endregion
        #region Publicos
        public dfCom Motherboard
        {
            get { return _mobo; }
            set {
                if (value.Tipo == "Motherboard")
                    _mobo = value;
                else
                    _mobo = new dfCom(true, "Motherboard");
                }
        }
        public dfCom Processador
        {
            get { return _pro; }
            set
            {
                if (value.Tipo == "Processador")
                    _pro = value;
                else
                    _pro = new dfCom(true, "Processador");
            }
        }
        public dfCom Memoria
        {
            get { return _mem; }
            set
            {
                if (value.Tipo == "Memoria")
                    _mem = value;
                else
                    _mem = new dfCom(true, "Memoria");
            }
        }
        public dfCom Fonte
        {
            get { return _fonte; }
            set
            {
                if (value.Tipo == "Fonte")
                    _fonte = value;
                else
                    _fonte = new dfCom(true, "Fonte");
            }
        }
        public dfCom HD
        {
            get { return _hd; }
            set
            {
                if (value.Tipo == "HD")
                    _hd = value;
                else
                    _hd = new dfCom(true, "HD");
            }
        }
        public dfCom Monitor
        {
            get { return _monitor; }
            set
            {
                if (value.Tipo == "Monitor")
                    _monitor = value;
                else
                    _monitor = new dfCom(true, "Monitor");
            }
        }
        public dfCom Video
        {
            get { return _video; }
            set
            {
                if (value.Tipo == "Video")
                    _video = value;
                else
                    _video = new dfCom(true, "Video");
            }
        }
        public dfCom Peri1
        {
            get { return _p1; }
            set
            {
                if (!Tperi(value.Tipo))
                    _p1 = value;
                else
                    _p1 = new dfCom(true);
            }
        }
        public dfCom Peri2
        {
            get { return _p2; }
            set
            {
                if (!Tperi(value.Tipo))
                    _p2 = value;
                else
                    _p2 = new dfCom(true);
            }
        }
        public dfCom Peri3
        {
            get { return _p3; }
            set
            {
                if (!Tperi(value.Tipo))
                    _p3 = value;
                else
                    _p3 = new dfCom(true);
            }
        }
        public dfCom Peri4
        {
            get { return _p4; }
            set
            {
                if (!Tperi(value.Tipo))
                    _p4 = value;
                else
                    _p4 = new dfCom(true);
            }
        }
        public dfCom Peri5
        {
            get { return _p5; }
            set
            {
                if (!Tperi(value.Tipo))
                    _p5 = value;
                else
                    _p5 = new dfCom(true);
            }
        }

        private bool Tperi(string p)
        {
            String[] t = {"Motherboard","Processador","Memoria","Fonte","HD","Monitor","Video"};
            return t.Contains(p);
        }
        #endregion
        #region Construtres
        public dfPC()
        {
        }
        public dfPC(bool n)
        {

        }
        #endregion
        #region Funcs
        public List<dfCom> getACom()
        {
            List<dfCom> l = new List<dfCom>();
             l.Add(_mobo);
        l.Add(_pro);
         l.Add(_mem);
        l.Add(_fonte);
        l.Add(_hd);
        l.Add(_monitor);
         l.Add(_video);
        l.Add(_p1);
         l.Add(_p2);
        l.Add( _p3);
         l.Add(_p4);
         l.Add(_p5);
         return l;
        }
        public List<dfCom> getVCom()
        {
            List<dfCom> l = new List<dfCom>();
            foreach (dfCom c in getACom())
                if (c != null)
                   l.Add(c);
            return l;
        }
        #endregion 
    }
}
