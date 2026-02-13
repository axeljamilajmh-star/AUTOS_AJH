using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmDetalleAuto : Form
    {
        private readonly AutoController _controller;
        private readonly Auto _auto;
        private Panel panelPrincipal;
        private TabControl tabControl;
        private PictureBox[] pictureBoxes;

        public FrmDetalleAuto(AutoController controller, Auto auto)
        {
            _controller = controller;
            _auto = auto;
            InitializeComponent();
            CargarDatosAuto();
        }

        public FrmDetalleAuto(AutoController controller, string codigoAuto)
        {
            _controller = controller;
            _auto = _controller.BuscarAutoPorCodigo(codigoAuto);
            InitializeComponent();
            if (_auto != null)
            {
                CargarDatosAuto();
            }
            else
            {
                MessageBox.Show("Auto no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Detalle de Vehículo - AUTOS AJH";
            this.Size = new Size(900, 700);
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

            // Header con título
            Panel panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "🔍 DETALLE DE VEHÍCULO",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            Button btnCerrar = new Button
            {
                Text = "✕ Cerrar",
                Dock = DockStyle.Right,
                Size = new Size(100, 30),
                Margin = new Padding(0, 25, 0, 25),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderColor = Color.White;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 86, 179);
            btnCerrar.Click += (s, e) => this.Close();

            panelHeader.Controls.Add(btnCerrar);
            panelHeader.Controls.Add(lblTitulo);

            // TabControl para organizar la información
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(15, 10),
                Font = new Font("Segoe UI", 9f)
            };

            // Pestaña 1: Información General
            TabPage tabInfoGeneral = new TabPage("📋 Información General");
            CrearTabInfoGeneral(tabInfoGeneral);

            // Pestaña 2: Especificaciones Técnicas
            TabPage tabEspecificaciones = new TabPage("⚙️ Especificaciones Técnicas");
            CrearTabEspecificaciones(tabEspecificaciones);

            // Pestaña 3: Galería de Fotos
            TabPage tabFotos = new TabPage("🖼️ Galería de Fotos");
            CrearTabFotos(tabFotos);

            tabControl.TabPages.Add(tabInfoGeneral);
            tabControl.TabPages.Add(tabEspecificaciones);
            tabControl.TabPages.Add(tabFotos);

            panelPrincipal.Controls.Add(tabControl);
            panelPrincipal.Controls.Add(panelHeader);
            this.Controls.Add(panelPrincipal);
        }

        private void CrearTabInfoGeneral(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;
            tabPage.Padding = new Padding(20);

            // Panel izquierdo - Información básica
            Panel panelIzquierdo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 400,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Información principal del vehículo
            Label lblTituloAuto = new Label
            {
                Text = $"{_auto.Marca} {_auto.Modelo} {_auto.Ano}",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(10, 15),
                Size = new Size(350, 35)
            };

            Label lblCodigo = new Label
            {
                Text = $"Código: {_auto.Codigo}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(10, 60),
                Size = new Size(350, 20)
            };

            Label lblPrecio = new Label
            {
                Text = $"Precio: ${_auto.Precio:N2}",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(10, 90),
                Size = new Size(350, 25)
            };

            Label lblColor = new Label
            {
                Text = $"Color: {_auto.Color}",
                Font = new Font("Segoe UI", 11f),
                Location = new Point(10, 125),
                Size = new Size(350, 20)
            };

            Label lblTipo = new Label
            {
                Text = $"Tipo: {_auto.Tipo}",
                Font = new Font("Segoe UI", 11f),
                Location = new Point(10, 150),
                Size = new Size(350, 20)
            };

            Label lblDisponible = new Label
            {
                Text = $"Estado: {(_auto.Disponible ? "✅ DISPONIBLE" : "❌ VENDIDO")}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = _auto.Disponible ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69),
                Location = new Point(10, 175),
                Size = new Size(350, 20)
            };

            // Panel derecho - Descripción
            Panel panelDerecho = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 15, 15, 15)
            };

            Label lblDescTitulo = new Label
            {
                Text = "Descripción:",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(10, 15),
                Size = new Size(400, 25)
            };

            TextBox txtDescripcion = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(420, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = string.IsNullOrEmpty(_auto.Descripcion) ?
                    "No hay descripción disponible para este vehículo." : _auto.Descripcion,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10f)
            };

            panelIzquierdo.Controls.AddRange(new Control[] {
                lblTituloAuto, lblCodigo, lblPrecio, lblColor, lblTipo, lblDisponible
            });

            panelDerecho.Controls.AddRange(new Control[] {
                lblDescTitulo, txtDescripcion
            });

            tabPage.Controls.Add(panelDerecho);
            tabPage.Controls.Add(panelIzquierdo);
        }

        private void CrearTabEspecificaciones(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;
            tabPage.Padding = new Padding(20);

            // Crear tabla de especificaciones
            TableLayoutPanel tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White
            };

            // Configurar columnas
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            // Agregar filas y especificaciones
            AgregarFilaEspecificacion(tableLayout, "Motor / Cilindraje:", $"{_auto.Cilindraje} cc", 0);
            AgregarFilaEspecificacion(tableLayout, "Transmisión:", _auto.Transmision, 1);
            AgregarFilaEspecificacion(tableLayout, "Combustible:", _auto.Combustible, 2);
            AgregarFilaEspecificacion(tableLayout, "Número de Puertas:", _auto.Puertas.ToString(), 3);
            AgregarFilaEspecificacion(tableLayout, "Capacidad de Pasajeros:", _auto.Pasajeros.ToString(), 4);
            AgregarFilaEspecificacion(tableLayout, "Año del Modelo:", _auto.Ano.ToString(), 5);
            AgregarFilaEspecificacion(tableLayout, "Marca:", _auto.Marca, 6);
            AgregarFilaEspecificacion(tableLayout, "Modelo:", _auto.Modelo, 7);

            tabPage.Controls.Add(tableLayout);
        }

        private void AgregarFilaEspecificacion(TableLayoutPanel table, string titulo, string valor, int fila)
        {
            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 10f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            table.Controls.Add(lblTitulo, 0, fila);
            table.Controls.Add(lblValor, 1, fila);
        }

        private void CrearTabFotos(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;
            tabPage.Padding = new Padding(20);

            Label lblTituloFotos = new Label
            {
                Text = "Galería de Fotos (Máximo 6 imágenes)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(10, 15),
                Size = new Size(400, 25)
            };

            // Panel para las fotos
            Panel panelFotos = new Panel
            {
                Location = new Point(10, 50),
                Size = new Size(840, 500),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            // Crear PictureBoxes para hasta 6 fotos
            pictureBoxes = new PictureBox[6];
            int xPos = 20;
            int yPos = 20;

            for (int i = 0; i < 6; i++)
            {
                pictureBoxes[i] = new PictureBox
                {
                    Location = new Point(xPos, yPos),
                    Size = new Size(250, 180),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White
                };

                // Cargar imagen de placeholder o imagen real si existe
                if (_auto.Fotos != null && i < _auto.Fotos.Count && !string.IsNullOrEmpty(_auto.Fotos[i]))
                {
                    // Aquí cargarías la imagen real desde la ruta
                    pictureBoxes[i].Image = CrearImagenPlaceholder($"Foto {i + 1}");
                }
                else
                {
                    pictureBoxes[i].Image = CrearImagenPlaceholder("Sin imagen");
                }

                // Etiqueta para el número de foto
                Label lblNumeroFoto = new Label
                {
                    Text = $"Foto {i + 1}",
                    Location = new Point(xPos, yPos + 185),
                    Size = new Size(250, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8f)
                };

                panelFotos.Controls.Add(pictureBoxes[i]);
                panelFotos.Controls.Add(lblNumeroFoto);

                // Organizar en grid 2x3
                xPos += 270;
                if ((i + 1) % 3 == 0)
                {
                    xPos = 20;
                    yPos += 220;
                }
            }

            // Botón para agregar fotos (solo si el auto está disponible)
            if (_auto.Disponible)
            {
                Button btnAgregarFotos = new Button
                {
                    Text = "➕ Agregar Fotos",
                    Location = new Point(10, 560),
                    Size = new Size(120, 35),
                    BackColor = Color.FromArgb(0, 123, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnAgregarFotos.Click += (s, e) => MessageBox.Show("Funcionalidad para agregar fotos - Próximamente", "AUTOS AJH");

                tabPage.Controls.Add(btnAgregarFotos);
            }

            tabPage.Controls.Add(lblTituloFotos);
            tabPage.Controls.Add(panelFotos);
        }

        private Bitmap CrearImagenPlaceholder(string texto)
        {
            // Crear una imagen placeholder con texto
            Bitmap bmp = new Bitmap(250, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 14, FontStyle.Bold))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(texto, font, Brushes.DarkGray, new RectangleF(0, 0, 250, 180), sf);
                }
                g.DrawRectangle(Pens.Gray, 0, 0, 249, 179);
            }
            return bmp;
        }

        private void CargarDatosAuto()
        {
            this.Text = $"Detalle: {_auto.Marca} {_auto.Modelo} - AUTOS AJH";
        }
    }
}