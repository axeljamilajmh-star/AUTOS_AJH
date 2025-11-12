using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmHistorialVentas : Form
    {
        private readonly AutoController _controller;
        private DataGridView dgvVentas;
        private Panel panelHeader;
        private Panel panelControles;
        private Panel panelEstadisticas;
        private Label lblEstadisticas;

        public FrmHistorialVentas(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "Historial de Ventas - AUTOS AJH";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

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

            CrearHeader();
            CrearPanelEstadisticas();
            CrearPanelControles();
            CrearDataGridView();

            panelPrincipal.Controls.Add(dgvVentas);
            panelPrincipal.Controls.Add(panelControles);
            panelPrincipal.Controls.Add(panelEstadisticas);
            panelPrincipal.Controls.Add(panelHeader);

            this.Controls.Add(panelPrincipal);
        }

        private void CrearHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(111, 66, 193), // Morado para historial
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "📈 HISTORIAL DE VENTAS",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearPanelEstadisticas()
        {
            panelEstadisticas = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20, 10, 20, 10)
            };

            lblEstadisticas = new Label
            {
                Text = "Cargando estadísticas...",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            panelEstadisticas.Controls.Add(lblEstadisticas);
        }

        private void CrearPanelControles()
        {
            panelControles = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(15, 10, 15, 10)
            };

            // Botón Actualizar
            Button btnActualizar = new Button
            {
                Text = "🔄 Actualizar",
                Size = new Size(100, 30),
                Location = new Point(15, 10),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnActualizar.Click += (s, e) => CargarDatos();

            // Botón Exportar
            Button btnExportar = new Button
            {
                Text = "📊 Exportar Reporte",
                Size = new Size(120, 30),
                Location = new Point(125, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White
            };
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnExportar.Click += (s, e) => ExportarReporte();

            // Botón Limpiar Historial
            Button btnLimpiar = new Button
            {
                Text = "🗑️ Limpiar Historial",
                Size = new Size(120, 30),
                Location = new Point(255, 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLimpiar.Click += (s, e) => LimpiarHistorial();

            panelControles.Controls.AddRange(new Control[] { btnActualizar, btnExportar, btnLimpiar });
        }

        private void CrearDataGridView()
        {
            dgvVentas = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            ConfigureColumns();

            // Estilizar encabezados
            dgvVentas.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.FromArgb(111, 66, 193),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            // Alternar colores de filas
            dgvVentas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private void ConfigureColumns()
        {
            dgvVentas.Columns.Clear();

            // Columna ID (acortada)
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID VENTA",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Font = new Font("Consolas", 8f),
                    ForeColor = Color.FromArgb(108, 117, 125)
                }
            });

            // Columna Fecha
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaVenta",
                HeaderText = "FECHA Y HORA",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "dd/MM/yyyy HH:mm",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // Columna Código Auto
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CodigoAuto",
                HeaderText = "CÓDIGO AUTO",
                Width = 100
            });

            // Columna Cliente
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Cliente",
                HeaderText = "CLIENTE",
                Width = 150
            });

            // Columna Vendedor
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Vendedor",
                HeaderText = "VENDEDOR",
                Width = 120
            });

            // Columna Precio Venta
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PrecioVenta",
                HeaderText = "PRECIO ORIGINAL",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.FromArgb(33, 37, 41)
                }
            });

            // Columna Descuento
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DescuentoAplicado",
                HeaderText = "DESCUENTO",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.FromArgb(220, 53, 69)
                }
            });

            // Columna Precio Final
            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PrecioFinal",
                HeaderText = "PRECIO FINAL",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.FromArgb(40, 167, 69),
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                }
            });
        }

        private void CargarDatos()
        {
            try
            {
                var ventas = _controller.Ventas;

                // Ordenar por fecha más reciente primero
                ventas = ventas.OrderByDescending(v => v.FechaVenta).ToList();

                dgvVentas.DataSource = ventas;
                ActualizarEstadisticas(ventas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial de ventas: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ActualizarEstadisticas(System.Collections.Generic.List<Venta> ventas)
        {
            if (ventas.Count == 0)
            {
                lblEstadisticas.Text = "📊 No hay ventas registradas en el sistema.";
                return;
            }

            decimal totalVentas = ventas.Sum(v => v.PrecioFinal);
            decimal totalDescuentos = ventas.Sum(v => v.DescuentoAplicado);
            decimal promedioVenta = ventas.Average(v => v.PrecioFinal);
            int totalVentasCount = ventas.Count;

            // Encontrar la venta más grande
            var ventaMasGrande = ventas.OrderByDescending(v => v.PrecioFinal).FirstOrDefault();

            lblEstadisticas.Text = $"📊 ESTADÍSTICAS: " +
                                 $"Ventas Totales: {totalVentasCount} | " +
                                 $"Ingresos: ${totalVentas:N2} | " +
                                 $"Descuentos: ${totalDescuentos:N2} | " +
                                 $"Promedio: ${promedioVenta:N2} | " +
                                 $"Venta Más Grande: ${ventaMasGrande?.PrecioFinal:N2}";
        }

        private void ExportarReporte()
        {
            if (_controller.Ventas.Count == 0)
            {
                MessageBox.Show("No hay ventas para exportar.",
                              "Exportar Reporte",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                return;
            }

            try
            {
                string reporte = GenerarReporteTexto();

                // Mostrar el reporte en un MessageBox grande
                using (var form = new Form())
                {
                    form.Text = "📊 REPORTE DE VENTAS - AUTOS AJH";
                    form.Size = new Size(800, 600);
                    form.StartPosition = FormStartPosition.CenterScreen;

                    TextBox txtReporte = new TextBox
                    {
                        Multiline = true,
                        Dock = DockStyle.Fill,
                        Text = reporte,
                        Font = new Font("Consolas", 9f),
                        ScrollBars = ScrollBars.Both,
                        ReadOnly = true
                    };

                    Button btnCerrar = new Button
                    {
                        Text = "Cerrar",
                        Dock = DockStyle.Bottom,
                        Height = 40
                    };
                    btnCerrar.Click += (s, e) => form.Close();

                    form.Controls.Add(txtReporte);
                    form.Controls.Add(btnCerrar);
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private string GenerarReporteTexto()
        {
            var ventas = _controller.Ventas.OrderByDescending(v => v.FechaVenta).ToList();
            decimal totalVentas = ventas.Sum(v => v.PrecioFinal);
            decimal totalDescuentos = ventas.Sum(v => v.DescuentoAplicado);

            string reporte = "=".PadRight(80, '=') + "\n";
            reporte += "📊 REPORTE DE VENTAS - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            reporte += $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            reporte += $"Total de ventas registradas: {ventas.Count}\n";
            reporte += $"Ingresos totales: ${totalVentas:N2}\n";
            reporte += $"Descuentos totales aplicados: ${totalDescuentos:N2}\n";
            reporte += $"Ingreso neto: ${totalVentas:N2}\n\n";

            reporte += "-".PadRight(80, '-') + "\n";
            reporte += "DETALLE DE VENTAS:\n";
            reporte += "-".PadRight(80, '-') + "\n\n";

            reporte += "FECHA".PadRight(16) + " | " +
                      "CLIENTE".PadRight(20) + " | " +
                      "VENDEDOR".PadRight(15) + " | " +
                      "CÓDIGO".PadRight(10) + " | " +
                      "PRECIO FINAL".PadRight(12) + " | " +
                      "DESCUENTO".PadRight(10) + "\n";
            reporte += "-".PadRight(80, '-') + "\n";

            foreach (var venta in ventas)
            {
                reporte += $"{venta.FechaVenta:dd/MM/yyyy HH:mm}".PadRight(16) + " | " +
                          $"{venta.Cliente}".PadRight(20) + " | " +
                          $"{venta.Vendedor}".PadRight(15) + " | " +
                          $"{venta.CodigoAuto}".PadRight(10) + " | " +
                          $"${venta.PrecioFinal:N2}".PadRight(12) + " | " +
                          $"${venta.DescuentoAplicado:N2}".PadRight(10) + "\n";
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private void LimpiarHistorial()
        {
            if (_controller.Ventas.Count == 0)
            {
                MessageBox.Show("El historial de ventas ya está vacío.",
                              "Limpiar Historial",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro de que desea limpiar todo el historial de ventas?\n\n" +
                $"Se eliminarán {_controller.Ventas.Count} registros de ventas.\n" +
                $"Esta acción NO se puede deshacer.\n\n" +
                $"Los vehículos vendidos seguirán marcados como no disponibles.",
                "⚠️ CONFIRMAR LIMPIEZA DE HISTORIAL",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    // Limpiar el historial de ventas en el controlador
                    _controller.Ventas.Clear();
                    CargarDatos();

                    MessageBox.Show("✅ Historial de ventas limpiado exitosamente.",
                                  "Historial Limpiado",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al limpiar el historial: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }
    }
}