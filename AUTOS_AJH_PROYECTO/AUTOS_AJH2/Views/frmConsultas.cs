using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmConsultas : Form
    {
        private readonly AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelConsultas;
        private Panel panelResultados;

        // Controles de resultados
        private Label lblResultado1, lblResultado2, lblResultado3;
        private Label lblDetalle1, lblDetalle2, lblDetalle3;
        private Button btnVerDetalle1, btnVerDetalle2, btnVerDetalle3;

        // Almacenar resultados
        private Auto autoMasAntiguo, autoMayorCilindraje, autoPrecioMasBajo;

        public FrmConsultas(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
            EjecutarConsultas();
        }

        private void InitializeComponent()
        {
            this.Text = "Consultas Especiales - AUTOS AJH";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            CrearHeader();
            CrearPanelConsultas();
            CrearPanelResultados();

            panelPrincipal.Controls.Add(panelResultados);
            panelPrincipal.Controls.Add(panelConsultas);
            panelPrincipal.Controls.Add(panelHeader);

            this.Controls.Add(panelPrincipal);
        }

        private void CrearHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(32, 201, 151), // Verde azulado para consultas
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "🔍 CONSULTAS ESPECIALES",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            // Botón Actualizar
            Button btnActualizar = new Button
            {
                Text = "🔄 Actualizar",
                Dock = DockStyle.Right,
                Size = new Size(100, 35),
                Margin = new Padding(0, 22, 0, 22),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnActualizar.FlatAppearance.BorderColor = Color.White;
            btnActualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 141, 101);
            btnActualizar.Click += (s, e) => EjecutarConsultas();

            panelHeader.Controls.Add(btnActualizar);
            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearPanelConsultas()
        {
            panelConsultas = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };

            Label lblInstrucciones = new Label
            {
                Text = "Estas consultas especiales analizan todo el inventario disponible " +
                                      "para proporcionarte información valiosa sobre los vehículos en stock.",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(20, 20),
                Size = new Size(700, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblInfo = new Label
            {
                Text = "💡 Haz clic en 'Ver Detalle' para obtener información completa del vehículo.",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, 70),
                Size = new Size(700, 20)
            };

            panelConsultas.Controls.Add(lblInstrucciones);
            panelConsultas.Controls.Add(lblInfo);
        }

        private void CrearPanelResultados()
        {
            panelResultados = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // ===== CONSULTA 1: VEHÍCULO MÁS ANTIGUO =====
            Panel panelConsulta1 = CrearPanelConsulta(
                "📅 VEHÍCULO MÁS ANTIGUO",
                "Muestra el vehículo con el año de fabricación más antiguo en el inventario.",
                Color.FromArgb(13, 110, 253), // Azul
                20
            );

            lblResultado1 = new Label
            {
                Text = "Ejecutando consulta...",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(30, 80),
                Size = new Size(500, 25)
            };
            panelConsulta1.Controls.Add(lblResultado1);

            lblDetalle1 = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(30, 105),
                Size = new Size(500, 40)
            };
            panelConsulta1.Controls.Add(lblDetalle1);

            btnVerDetalle1 = new Button
            {
                Text = "👁️ Ver Detalle",
                Size = new Size(100, 30),
                Location = new Point(550, 85),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnVerDetalle1.Click += (s, e) => VerDetalleAuto(autoMasAntiguo);
            btnVerDetalle1.Enabled = false;
            panelConsulta1.Controls.Add(btnVerDetalle1);

            // ===== CONSULTA 2: VEHÍCULO CON MAYOR CILINDRAJE =====
            Panel panelConsulta2 = CrearPanelConsulta(
                "⚡ VEHÍCULO CON MAYOR CILINDRAJE",
                "Identifica el vehículo con el motor de mayor capacidad en el inventario.",
                Color.FromArgb(214, 51, 132), // Rosa
                200
            );

            lblResultado2 = new Label
            {
                Text = "Ejecutando consulta...",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(30, 80),
                Size = new Size(500, 25)
            };
            panelConsulta2.Controls.Add(lblResultado2);

            lblDetalle2 = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(30, 105),
                Size = new Size(500, 40)
            };
            panelConsulta2.Controls.Add(lblDetalle2);

            btnVerDetalle2 = new Button
            {
                Text = "👁️ Ver Detalle",
                Size = new Size(100, 30),
                Location = new Point(550, 85),
                BackColor = Color.FromArgb(214, 51, 132),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnVerDetalle2.Click += (s, e) => VerDetalleAuto(autoMayorCilindraje);
            btnVerDetalle2.Enabled = false;
            panelConsulta2.Controls.Add(btnVerDetalle2);

            // ===== CONSULTA 3: VEHÍCULO CON PRECIO MÁS BAJO =====
            Panel panelConsulta3 = CrearPanelConsulta(
                "💰 VEHÍCULO CON PRECIO MÁS BAJO",
                "Encuentra el vehículo más económico actualmente en el inventario.",
                Color.FromArgb(25, 135, 84), // Verde
                380
            );

            lblResultado3 = new Label
            {
                Text = "Ejecutando consulta...",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(30, 80),
                Size = new Size(500, 25)
            };
            panelConsulta3.Controls.Add(lblResultado3);

            lblDetalle3 = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(30, 105),
                Size = new Size(500, 40)
            };
            panelConsulta3.Controls.Add(lblDetalle3);

            btnVerDetalle3 = new Button
            {
                Text = "👁️ Ver Detalle",
                Size = new Size(100, 30),
                Location = new Point(550, 85),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnVerDetalle3.Click += (s, e) => VerDetalleAuto(autoPrecioMasBajo);
            btnVerDetalle3.Enabled = false;
            panelConsulta3.Controls.Add(btnVerDetalle3);

            panelResultados.Controls.Add(panelConsulta1);
            panelResultados.Controls.Add(panelConsulta2);
            panelResultados.Controls.Add(panelConsulta3);
        }

        private Panel CrearPanelConsulta(string titulo, string descripcion, Color color, int top)
        {
            Panel panel = new Panel
            {
                Location = new Point(0, top),
                Size = new Size(720, 160),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Header del panel
            Panel panelHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(720, 50),
                BackColor = color,
                Padding = new Padding(15, 0, 15, 0)
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            panelHeader.Controls.Add(lblTitulo);
            panel.Controls.Add(panelHeader);

            // Descripción
            Label lblDescripcion = new Label
            {
                Text = descripcion,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(30, 55),
                Size = new Size(650, 20)
            };

            panel.Controls.Add(lblDescripcion);

            return panel;
        }

        private void EjecutarConsultas()
        {
            try
            {
                // Ejecutar las tres consultas
                autoMasAntiguo = _controller.ObtenerAutoMasAntiguo();
                autoMayorCilindraje = _controller.ObtenerAutoMayorCilindraje();
                autoPrecioMasBajo = _controller.ObtenerAutoPrecioMasBajo();

                // Actualizar interfaz con los resultados
                ActualizarResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ejecutar las consultas: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ActualizarResultados()
        {
            // ===== CONSULTA 1: VEHÍCULO MÁS ANTIGUO =====
            if (autoMasAntiguo != null)
            {
                lblResultado1.Text = $"{autoMasAntiguo.Marca} {autoMasAntiguo.Modelo} {autoMasAntiguo.Ano}";
                lblDetalle1.Text = $"Código: {autoMasAntiguo.Codigo} | Precio: ${autoMasAntiguo.Precio:N2} | Color: {autoMasAntiguo.Color}";
                btnVerDetalle1.Enabled = true;
            }
            else
            {
                lblResultado1.Text = "No hay vehículos disponibles";
                lblDetalle1.Text = "No se encontraron vehículos en el inventario";
                btnVerDetalle1.Enabled = false;
            }

            // ===== CONSULTA 2: VEHÍCULO CON MAYOR CILINDRAJE =====
            if (autoMayorCilindraje != null)
            {
                lblResultado2.Text = $"{autoMayorCilindraje.Marca} {autoMayorCilindraje.Modelo} - {autoMayorCilindraje.Cilindraje} cc";
                lblDetalle2.Text = $"Código: {autoMayorCilindraje.Codigo} | Precio: ${autoMayorCilindraje.Precio:N2} | Tipo: {autoMayorCilindraje.Tipo}";
                btnVerDetalle2.Enabled = true;
            }
            else
            {
                lblResultado2.Text = "No hay vehículos disponibles";
                lblDetalle2.Text = "No se encontraron vehículos en el inventario";
                btnVerDetalle2.Enabled = false;
            }

            // ===== CONSULTA 3: VEHÍCULO CON PRECIO MÁS BAJO =====
            if (autoPrecioMasBajo != null)
            {
                lblResultado3.Text = $"{autoPrecioMasBajo.Marca} {autoPrecioMasBajo.Modelo} - ${autoPrecioMasBajo.Precio:N2}";
                lblDetalle3.Text = $"Código: {autoPrecioMasBajo.Codigo} | Año: {autoPrecioMasBajo.Ano} | Color: {autoPrecioMasBajo.Color}";
                btnVerDetalle3.Enabled = true;
            }
            else
            {
                lblResultado3.Text = "No hay vehículos disponibles";
                lblDetalle3.Text = "No se encontraron vehículos en el inventario";
                btnVerDetalle3.Enabled = false;
            }
        }

        private void VerDetalleAuto(Auto auto)
        {
            if (auto != null)
            {
                var detalleForm = new FrmDetalleAuto(_controller, auto);
                detalleForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay información disponible para este vehículo.",
                              "Información No Disponible",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
        }
    }
}