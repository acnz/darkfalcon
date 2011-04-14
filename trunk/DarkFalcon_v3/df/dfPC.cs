using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon.df
{
    public class dfPC
    {
        #region Fields
        private dfMobo _mobo;
        private dfCom _pro;
        private dfCom _gab;
        private dfMem _mem;
        private dfCom _fonte;
        private dfCom _hd;
        private dfCom _monitor;
        private dfCom _video;
        private dfCom _som;
        private dfCom _tec;
        private dfCom _mou;
     #endregion
        #region Publicos
        public dfMobo Motherboard
        {
            get { return _mobo; }
            set {
                if (value.Tipo == "Motherboard")
                    _mobo = value;
                else
                    _mobo = new dfMobo(true);
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
                    _pro = new dfCom("Processador");
            }
        }
        public dfMem Memoria
        {
            get { return _mem; }
            set
            {
                    _mem = value;
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
                    _fonte = new dfCom("Fonte");
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
                    _hd = new dfCom("HD");
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
                    _monitor = new dfCom("Monitor");
            }
        }
        public dfCom Video
        {
            get { return _video; }
            set
            {
                if (value.Tipo == "P.Video")
                    _video = value;
                else
                    _video = new dfCom("P.Video");
            }
        }
        public dfCom Gabinete
        {
            get { return _gab; }
            set
            {
                if (value.Tipo == "Gabinete")
                    _gab = value;
                else
                    _gab = new dfCom("Gabinete");
            }
        }
        //public dfCom Peri1
        //{
        //    get { return _p1; }
        //    set
        //    {
        //        if (!Tperi(value.Tipo))
        //            _p1 = value;
        //        else
        //            _p1 = new dfCom(true);
        //    }
        //}

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
        public dfPC(bool init)
        {
            if (init)
            {
                _mobo = new dfMobo(true);
            
                _pro = new dfCom("Processador");
            
                //_mem = new dfCom("Memoria");
            
                _fonte = new dfCom("Fonte");
        
                _hd = new dfCom("HD");
        
                _monitor = new dfCom("Monitor");
        
                _video = new dfCom("P.Video");
        
                _gab =new dfCom("Gabinete");
            }
            else
            {

            }
        }
        #endregion
        #region Funcs
        public List<dfIOb> getACom()
        {
            List<dfIOb> l = new List<dfIOb>();
             l.Add(_mobo);
        l.Add(_pro);
         l.Add(_mem);
        l.Add(_fonte);
        l.Add(_hd);
        l.Add(_monitor);
         l.Add(_video);
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
