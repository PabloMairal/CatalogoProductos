﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        public Articulo()
        {

        }

        public int Id { get; set; }
        public string Codigo { get; set; }

        public string Nombre {  get; set; }

        public string Descripcion { get; set; }

        public Marca Marca { get; set; }

        public Categoria Categoria { get; set; }
        
        public List<string> Imagenes = new List<string>();
        public decimal Precio { get; set; }

        
    }
}
  