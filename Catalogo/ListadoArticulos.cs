﻿using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Dominio;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Datos;

namespace Catalogo {
    public partial class ListadoArticulos : Form
    {
        ArticuloNegocio articuloNegocio = new ArticuloNegocio();
        List<Articulo> Articulos = new List<Articulo>();
        public ListadoArticulos()
        {
            InitializeComponent();

        }

        public EventHandler OnEditorOpen;
        public EventHandler OnDetailOpen;

        private void _OnEditorOpen(CustomEventArgs e)
        {
            if (OnEditorOpen != null)
            {
                OnEditorOpen(this, e);
            }
        }
        private void _OnDetailOpen(CustomEventArgs e)
        {
            if (OnDetailOpen != null)
            {
                OnDetailOpen(this, e);
            }
        }
        private void RecargarLista(List<Articulo> data = null)
        {
            try
            {
                dgvArticulos.DataSource = null;
                if (data == null)
                {
                    Articulos = articuloNegocio.ListarArticulos();
                    dgvArticulos.DataSource = Articulos;
                }
                else
                {
                    dgvArticulos.DataSource = data;
                }
                FormatearColumnaPrecios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());      
            }

        }
        private void ListadoArticulos_Load(object sender, EventArgs e)
        {
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            _OnEditorOpen(new CustomEventArgs { Articulo = null });
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            _OnDetailOpen(new CustomEventArgs { Articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem });
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            _OnEditorOpen(new CustomEventArgs { Articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem });

        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtBuscador.Text;

            if (filtro.Length >= 2)
            {
                filtro = filtro.ToUpper();
                listaFiltrada = Articulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro) 
                || x.Categoria.Nombre.ToUpper().Contains(filtro) 
                || x.Marca.Nombre.ToUpper().Contains(filtro)
                || x.Codigo.ToUpper().Contains(filtro)
                || x.Descripcion.ToUpper().Contains(filtro));
            }
            else
            {
                listaFiltrada = Articulos;
            }

            RecargarLista(listaFiltrada);
        }



        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                if (cboCampo.SelectedItem == null || cboCriterio.SelectedItem == null || txtFiltroAvanzado.Text == "")
                {
                    MessageBox.Show("Debe completar todos los campos");
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                Articulos = articuloNegocio.filtrar(campo, criterio, filtro);
                RecargarLista(Articulos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                RecargarLista();
            }

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCampo.SelectedItem == null)
            {
                return;
            }
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Codigo")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Marca")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Categoria")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
        }

        private void ListadoArticulos_Enter(object sender, EventArgs e)
        {
            RecargarLista();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            {
                if (dgvArticulos.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un articulo");
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea eliminar el articulo?", "Eliminar", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                try
                {
                    Articulo articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    articuloNegocio.EliminarArticulo(articulo.Id);
                    RecargarLista();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void FormatearColumnaPrecios()
        {
            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                if (row.Cells["Precio"].Value != null && row.Cells["Precio"].Value != DBNull.Value)
                {
                    decimal valorDecimal = Convert.ToDecimal(row.Cells["Precio"].Value);
                    row.Cells["Precio"].Value = valorDecimal.ToString("0.00");
                }
            }
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            if (cboCampo.SelectedItem == null && cboCriterio.SelectedItem == null && txtFiltroAvanzado.Text == "")
            {
                return;
            }
            cboCampo.SelectedItem = null;
            cboCriterio.SelectedItem = null;
            txtFiltroAvanzado.Text = "";
            RecargarLista();
        }
    }
}
