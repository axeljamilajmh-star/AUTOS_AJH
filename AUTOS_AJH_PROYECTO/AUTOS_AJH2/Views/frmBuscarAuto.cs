using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmBuscarAuto : Form
    {
        private readonly AutoController _controller;
        private TextBox txtCodigo;
        private Button btnBuscar, btnCancelar;
        private Label lblTitulo, lblInstrucciones;

        public FrmBuscarAuto(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "🔍 Buscar Auto por Código - AUTOS AJH";
            this.Size = new Size(450, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // Panel principal
            Panel panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Título
            lblTitulo = new Label
            {
                Text = "BUSCAR VEHÍCULO POR CÓDIGO",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Instrucciones
            lblInstrucciones = new Label
            {
                Text = "Ingrese el código del vehículo que desea buscar:",
                Font = new Font("Segoe UI", 10f),
                Location = new Point(20, 60),
                Size = new Size(400, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Campo de código
            txtCodigo = new TextBox
            {
                Location = new Point(20, 95),
                Size = new Size(390, 25),
                Font = new Font("Segoe UI", 12f),
                TextAlign = HorizontalAlignment.Center,
                MaxLength = 20,

                // Placeholder simulado
                Text = "Ej: TOY001, HON002...",
                ForeColor = Color.Gray
            };
            txtCodigo.GotFocus += (s, e) =>
            {
                if (txtCodigo.Text == "Ej: TOY001, HON002...")
                {
                    txtCodigo.Text = "";
                    txtCodigo.ForeColor = Color.Black;
                }
            };
            txtCodigo.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    txtCodigo.Text = "Ej: TOY001, HON002...";
                    txtCodigo.ForeColor = Color.Gray;
                }
            };

            // Panel de botones
            Panel panelBotones = new Panel
            {
                Location = new Point(20, 140),
                Size = new Size(390, 50)
            };

            // Botón Buscar
            btnBuscar = new Button
            {
                Text = "🔍 BUSCAR",
                Size = new Size(120, 40),
                Location = new Point(140, 5),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.Click += BtnBuscar_Click;

            // Botón Cancelar
            btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Size = new Size(120, 40),
                Location = new Point(270, 5),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();

            panelBotones.Controls.AddRange(new Control[] { btnBuscar, btnCancelar });

            panelPrincipal.Controls.AddRange(new Control[] {
                lblTitulo, lblInstrucciones, txtCodigo, panelBotones
            });

            this.Controls.Add(panelPrincipal);
            this.AcceptButton = btnBuscar;
            this.CancelButton = btnCancelar;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();

            // Si es el texto de placeholder, limpiarlo
            if (codigo == "Ej: TOY001, HON002...")
                codigo = "";

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarError("❌ Por favor ingrese un código para buscar.");
                return;
            }

            // Buscar el vehículo
            var auto = _controller.BuscarAutoPorCodigo(codigo.ToUpper());

            if (auto != null)
            {
                // Vehículo encontrado - abrir formulario de detalle
                var detalleForm = new FrmDetalleAuto(_controller, auto);
                detalleForm.ShowDialog();
                this.Close(); // Cerrar el formulario de búsqueda
            }
            else
            {
                // Vehículo no encontrado
                MostrarError($"❌ No se encontró ningún vehículo con el código: {codigo}\n\n" +
                           "Códigos disponibles:\n" +
                           "• TOY001 - Toyota Corolla\n" +
                           "• HON002 - Honda CR-V\n" +
                           "• FOR003 - Ford Mustang\n" +
                           "• CHE004 - Chevrolet Onix");
            }
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Búsqueda - AUTOS AJH",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCodigo.Focus();
            txtCodigo.SelectAll();
        }

        // Propiedad para obtener el código ingresado (opcional)
        public string CodigoIngresado
        {
            get
            {
                string codigo = txtCodigo.Text.Trim();
                return (codigo == "Ej: TOY001, HON002...") ? "" : codigo;
            }
        }
    }
}