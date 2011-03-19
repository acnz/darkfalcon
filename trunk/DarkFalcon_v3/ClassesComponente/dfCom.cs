using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkFalcon_v3
{
    public class dfCom
    {
        #region Fields
        private string _id;
        private string _nome;
        private string _tipo;
        private float _preco;
        public string _image2d;
        public string _image3d;
        #endregion
        #region Publicos
        public string ID
        {
            get{return _id;}
            set{_id=value;}
        }
        public string Nome        {
            get{return _nome;}
            set{_nome=value;}
        }
        public string Tipo        {
            get{return _tipo;}
            set{_tipo=value;}
        }
        public float Preco        {
            get{return _preco;}
            set{_preco=value;}
        }
        public string LocalImagem2D  {
            get { return (_image2d+"//"+_id); }
            /// <summary>
            /// Digite somente o local(o nome da imagem é incluido automaticamente)
            /// </summary>
            set { _image2d = value + "//" + _id; }
        }
        public string LocalImagem3D
        {
            get { return (_image3d + "//" + _id); }
            /// <summary>
            /// Digite somente o local(o nome da imagem é incluido automaticamente)
            /// </summary>
            set { _image3d = value + "//" + _id; }
        }
        #endregion
        #region Inicializadores

        public dfCom(string IDDoComponete ,string NomeDoComponente, string TipoDoComponente, float PrecoDoComponente)
        {
            ID = IDDoComponete;
            Nome = NomeDoComponente;
            Tipo = TipoDoComponente;
            Preco = PrecoDoComponente;

           _image2d = "Textures//" + Tipo + "//" + ID;

            _image3d = "Models//" + Tipo + "//" + ID;
        }
        public dfCom()
        {
 
        }
        #endregion
    }
}
